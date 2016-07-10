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
    public class DocNamespace : IDocModel
    {
        public MemberDetails MemberDetails { get; }
        public IDocModel Parent { get; }

        public string Name => MemberDetails.FullName;

        public IEnumerable<DocType> Types { get; }

        public DocNamespace(MemberDetails memberDetails, IEnumerable<MemberDetails> otherMembers)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (otherMembers == null) throw new ArgumentNullException(nameof(otherMembers));
            if (memberDetails.Type != MemberType.Namespace) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Namespace}", nameof(memberDetails));
            MemberDetails = memberDetails;
            Types = otherMembers.Where(m => m.Type == MemberType.Type && m.ParentMember == Name).Select(m => new DocType(m, otherMembers, this));
        }

        public void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary)
        {
            if (membersDictionary == null) throw new ArgumentNullException(nameof(membersDictionary));
            foreach (var type in Types)
            {
                type.AddMembersToDictionary(membersDictionary);
            }
            membersDictionary.Add(MemberDetails.Id, this);
        }

        public Page BuildPage(DocSiteModel context)
        {
            var sections = new List<IRenderable>();
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

        public static IEnumerable<string> GetTableHeaders()
        {
            return new[] {"Name"};
        }

        public TableRow GetTableRow()
        {
            return new TableRow
            {
                Columns = new[]
                {
                    new TableData
                    {
                        Link = MemberDetails.FileId,
                        Content = new XmlDocument {InnerText = MemberDetails.FullName}
                    }
                }
            };
        }

        public void AddTypes(IList<IRenderable> sections)
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
