using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using DocSite.Renderers;
using DocSite.Xml;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    /// <summary>
    /// <see cref="IDocModel"/> that represents a full documentation site.
    /// </summary>
    /// <seealso cref="IDocModel"/>
    public class DocSiteModel : IDocModel
    {
        /// <summary>
        /// The name of the assembly the documentation site is for.
        /// </summary>
        /// <value>Gets the <see cref="AssemblyName"/></value>
        public string AssemblyName { get; }

        /// <summary>
        /// A collection of <see cref="DocNamespace"/> objects in the documentation.
        /// </summary>
        /// <value>Gets the <see cref="Namespaces"/></value>
        public IEnumerable<DocNamespace> Namespaces { get; }

        /// <summary>
        /// A dictionary mapping member ids to <see cref="IDocModel"/>
        /// </summary>
        /// <value>Gets the <see cref="MembersDictionary"/></value>
        public IDictionary<string, IDocModel> MembersDictionary { get; }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public MemberDetails MemberDetails { get; }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public IDocModel Parent { get;  }

        /// <summary>
        /// Create a <see cref="DocSiteModel"/> from a <see cref="DocXmlModel"/>
        /// </summary>
        /// <param name="xmlModel">The <see cref="DocXmlModel"/> to create the <see cref="DocSiteModel"/> from.</param>
        public DocSiteModel(DocXmlModel xmlModel)
        {
            AssemblyName = xmlModel.Assembly.Name;
            var namespaces = new List<string>();
            var typesByParent = xmlModel.Members.Where(m => m.Type == MemberType.Type).GroupBy(m => m.ParentMember);
            foreach (var parentTypeMapping in typesByParent)
            {
                if (!xmlModel.Members.Any(m => m.FullName == parentTypeMapping.Key))
                    namespaces.Add(parentTypeMapping.Key);
            }
            Namespaces = namespaces.Distinct().Select(n => new DocNamespace(new MemberDetails {Id = $"N:{n}"}, xmlModel.Members));

            MembersDictionary = new Dictionary<string, IDocModel>();
            AddMembersToDictionary(MembersDictionary);
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary)
        {
            if (membersDictionary == null) throw new ArgumentNullException(nameof(membersDictionary));
            foreach (var ns in Namespaces)
            {
                ns.AddMembersToDictionary(MembersDictionary);
            }
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public Page BuildPage(DocSiteModel context)
        {
            return new Page
            {
                Name = "index.html",
                AssemblyName = AssemblyName,
                Title = AssemblyName,
                Sections = new[]
                {
                    new TableSection
                    {
                        Title = "Namespaces",
                        Headers = new []{"Namespace"},
                        Rows = Namespaces.Select(n => n.GetTableRow())
                    }
                }
            };
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        /// <exception cref="NotImplementedException">Not implemented on this type.</exception>
        public TableRow GetTableRow()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Build all pages in the <see cref="DocSiteModel"/>.
        /// </summary>
        /// <returns><see cref="IEnumerable{Page}"/> - The collection of pages in the documentation.</returns>
        public IEnumerable<Page> BuildPages()
        {
            foreach (var member in MembersDictionary)
            {
                yield return member.Value.BuildPage(this);
            }
            yield return BuildPage(this);
        }

        
    }
}
