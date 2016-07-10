using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DocSite.Renderers;
using DocSite.Xml;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    public class DocType : IDocModel
    {
        public MemberDetails MemberDetails { get; }
        public IDocModel Parent { get; }

        public string Name => MemberDetails.FullName;

        public XmlElement Summary => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "summary");

        public XmlElement Remarks => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "remarks");

        public XmlElement Example => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "example");

        public IEnumerable<XmlElement> TypeParams => MemberDetails.DocXml.Where(xml => xml.Name == "typeparam");

        public IEnumerable<DocConstructor> Constructors { get; set; }

        public IEnumerable<DocProperty> Properties { get; }

        public IEnumerable<DocMethod> Methods { get; }

        public IEnumerable<DocField> Fields { get; set; }

        public IEnumerable<DocEvent> Events { get; set; }

        public IEnumerable<DocType> Types { get; set; }

        public IEnumerable<XmlElement> SeeAlso => MemberDetails.DocXml.Where(xml => xml.Name == "seealso");

        public DocType(MemberDetails memberDetails, IEnumerable<MemberDetails> otherMembers, IDocModel parent = null)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (otherMembers == null) throw new ArgumentNullException(nameof(otherMembers));
            if (memberDetails.Type != MemberType.Type)
                throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Type}", nameof(memberDetails));
            Parent = parent;
            MemberDetails = memberDetails;
            Constructors =
                otherMembers.Where(
                    m => m.Type == MemberType.Method && m.ParentMember == Name && m.LocalName.StartsWith("#ctor"))
                    .Select(m => new DocConstructor(m, this));
            Properties =
                otherMembers.Where(m => m.Type == MemberType.Property && m.ParentMember == Name)
                    .Select(m => new DocProperty(m, this));
            Methods =
                otherMembers.Where(
                    m => m.Type == MemberType.Method && m.ParentMember == Name && !m.LocalName.StartsWith("#ctor"))
                    .Select(m => new DocMethod(m, this));
            Fields =
                otherMembers.Where(m => m.Type == MemberType.Field && m.ParentMember == Name)
                    .Select(m => new DocField(m, this));
            Events =
                otherMembers.Where(m => m.Type == MemberType.Event && m.ParentMember == Name)
                    .Select(m => new DocEvent(m, this));
            Types =
                otherMembers.Where(m => m.Type == MemberType.Type && m.ParentMember == Name)
                    .Select(m => new DocType(m, otherMembers, this));
        }

        public void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary)
        {
            if (membersDictionary == null) throw new ArgumentNullException(nameof(membersDictionary));
            var members =
                Constructors.Cast<IDocModel>().Union(Properties).Union(Methods).Union(Fields).Union(Events).Union(Types);
            foreach (var member in members)
            {
                member.AddMembersToDictionary(membersDictionary);
            }
            membersDictionary.Add(MemberDetails.Id, this);
        }

        public Page BuildPage(DocSiteModel context)
        {
            var sections = new List<IRenderable>();
            MemberDetails.AddCommonSections(sections);
            AddConstructors(sections);
            AddFields(sections);
            AddProperties(sections);
            AddMethods(sections);
            AddEvents(sections);
            AddTypes(sections);
            return new Page
            {
                AssemblyName = context.AssemblyName,
                Name = Name,
                Title = MemberDetails.LocalName,
                Sections = sections
            };
        }

        private void AddConstructors(IList<IRenderable> sections)
        {
            if (Constructors.Any())
            {
                sections.Add(new TableSection
                {
                    Title = "Constructors",
                    Headers = DocConstructor.GetTableHeaders(),
                    Order = 10,
                    Rows = Constructors.Select(c => c.GetTableRow())
                });
            }
        }

        private void AddFields(IList<IRenderable> sections)
        {
            if (Constructors.Any())
            {
                sections.Add(new TableSection
                {
                    Title = "Fields",
                    Headers = DocField.GetTableHeaders(),
                    Order = 11,
                    Rows = Fields.Select(c => c.GetTableRow())
                });
            }
        }

        private void AddProperties(IList<IRenderable> sections)
        {
            if (Constructors.Any())
            {
                sections.Add(new TableSection
                {
                    Title = "Properties",
                    Headers = DocProperty.GetTableHeaders(),
                    Order = 12,
                    Rows = Properties.Select(c => c.GetTableRow())
                });
            }
        }

        private void AddMethods(IList<IRenderable> sections)
        {
            if (Constructors.Any())
            {
                sections.Add(new TableSection
                {
                    Title = "Methods",
                    Headers = DocMethod.GetTableHeaders(),
                    Order = 13,
                    Rows = Methods.Select(c => c.GetTableRow())
                });
            }
        }

        private void AddEvents(IList<IRenderable> sections)
        {
            if (Constructors.Any())
            {
                sections.Add(new TableSection
                {
                    Title = "Events",
                    Headers = DocEvent.GetTableHeaders(),
                    Order = 14,
                    Rows = Events.Select(c => c.GetTableRow())
                });
            }
        }

        private void AddTypes(IList<IRenderable> sections)
        {
            if (Constructors.Any())
            {
                sections.Add(new TableSection
                {
                    Title = "Types",
                    Headers = DocType.GetTableHeaders(),
                    Order = 15,
                    Rows = Types.Select(c => c.GetTableRow())
                });
            }
        }

        public static IEnumerable<string> GetTableHeaders()
        {
            return new[] {"Name", "Description"};
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
                        Content = new XmlDocument {InnerText = MemberDetails.LocalName}
                    },
                    new TableData
                    {
                        Content = MemberDetails.Summary
                    }
                }
            };
        }
    }
}