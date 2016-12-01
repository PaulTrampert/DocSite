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
    /// <see cref="IDocModel"/> that represents an Namespace.
    /// </summary>
    /// <seealso cref="IDocModel"/>
    public class DocNamespace : IDocModel
    {
        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public MemberDetails MemberDetails { get; }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public IDocModel Parent { get; }

        /// <summary>
        /// The full name of the namespace.
        /// </summary>
        /// <value>Gets the full name of the namespace.</value>
        public string Name => MemberDetails.FullName;

        /// <summary>
        /// A collection of child <see cref="DocType"/> objects.
        /// </summary>
        /// <value>Gets the collection of child <see cref="DocType"/> objects.</value>
        public IEnumerable<DocType> Types { get; }

        /// <summary>
        /// Create a new <see cref="DocEvent"/>
        /// </summary>
        /// <param name="memberDetails">The <see cref="DocSite.Xml.MemberDetails"/> to create the <see cref="DocNamespace"/> from.</param>
        /// <param name="otherMembers">Collection of <see cref="DocSite.Xml.MemberDetails"/> to create children from.</param>
        public DocNamespace(MemberDetails memberDetails, IEnumerable<MemberDetails> otherMembers)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (otherMembers == null) throw new ArgumentNullException(nameof(otherMembers));
            if (memberDetails.Type != MemberType.Namespace) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Namespace}", nameof(memberDetails));
            MemberDetails = memberDetails;
            Types = otherMembers.Where(m => m.Type == MemberType.Type && m.ParentMember == Name).Select(m => new DocType(m, otherMembers, this));
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary)
        {
            if (membersDictionary == null) throw new ArgumentNullException(nameof(membersDictionary));
            foreach (var type in Types)
            {
                type.AddMembersToDictionary(membersDictionary);
            }
            membersDictionary.Add(MemberDetails.Id, this);
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public Page BuildPage(DocSiteModel context)
        {
            var sections = new List<ISection>();
            MemberDetails.AddCommonSections(sections);
            AddTypes(sections);
            return new Page
            {
                AssemblyName = context.AssemblyName,
                Name = MemberDetails.FileId,
                Title = Name,
                Sections = sections
            };
        }

        /// <summary>
        /// Gets the table headers for including this type in a table.
        /// </summary>
        /// <returns><see cref="IEnumerable{String}"/> - The collection of table headers.</returns>
        public static IEnumerable<string> GetTableHeaders()
        {
            return new[] {"Name"};
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public TableRow GetTableRow()
        {
            return new TableRow
            {
                Columns = new[]
                {
                    new TableData
                    {
                        Link = MemberDetails.FileId,
                        TextContent = MemberDetails.FullName
                    }
                }
            };
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="hrefExtension"></param>
        public Tree BuildTree(string currentPage, string hrefExtension)
        {
            var nodes = Types.Select(t => t.BuildTree(currentPage, hrefExtension));
            var href = MemberDetails.FileId + (hrefExtension != null ? $".{hrefExtension}" : "");
            return new Tree
            {
                Text = MemberDetails.LocalName,
                Href = href,
                Nodes = nodes,
                State = new TreeState
                {
                    Expanded = nodes.Any(n => n.AnySelected()),
                    Selected = currentPage == MemberDetails.FileId
                }
            };
        }

        private void AddTypes(IList<ISection> sections)
        {
            if (Types.Any())
            {
                sections.Add(new TableSection
                {
                    Title = "Types",
                    Headers = DocType.GetTableHeaders(),
                    Order = 10,
                    Rows = Types.Select(t => t.GetTableRow())
                });
            }
        }
    }
}
