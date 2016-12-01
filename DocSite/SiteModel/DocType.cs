using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DocSite.Renderers;
using DocSite.Xml;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    /// <summary>
    /// <see cref="IDocModel"/> that represents a Type.
    /// </summary>
    /// <seealso cref="IDocModel"/>
    public class DocType : IDocModel
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
        /// The full name of the type.
        /// </summary>
        /// <value>Gets the full name of the type.</value>
        public string Name => MemberDetails.FullName;

        /// <summary>
        /// Collection of <see cref="DocConstructor"/> for this type.
        /// </summary>
        /// <value>Gets the <see cref="Constructors"/></value>
        public IEnumerable<DocConstructor> Constructors { get; }

        /// <summary>
        /// Collection of <see cref="DocProperty"/> for this type.
        /// </summary>
        /// <value>Gets the <see cref="Properties"/></value>
        public IEnumerable<DocProperty> Properties { get; }

        /// <summary>
        /// Collection of <see cref="DocMethod"/> for this type.
        /// </summary>
        /// <value>Gets the <see cref="Methods"/></value>
        public IEnumerable<DocMethod> Methods { get; }

        /// <summary>
        /// Collection of <see cref="DocField"/> for this type.
        /// </summary>
        /// <value>Gets the <see cref="Fields"/></value>
        public IEnumerable<DocField> Fields { get; }

        /// <summary>
        /// Collection of <see cref="DocEvent"/> for this type.
        /// </summary>
        /// <value>Gets the <see cref="Events"/></value>
        public IEnumerable<DocEvent> Events { get; }

        /// <summary>
        /// Collection of <see cref="DocType"/> for this type.
        /// </summary>
        /// <value>Gets the <see cref="Types"/></value>
        public IEnumerable<DocType> Types { get; }

        /// <summary>
        /// Create a new <see cref="DocType"/>
        /// </summary>
        /// <param name="memberDetails">The <see cref="MemberDetails"/> to create the <see cref="DocType"/> from.</param>
        /// <param name="otherMembers">Collection of other <see cref="DocSite.Xml.MemberDetails"/> that contains the other members to create children from.</param>
        /// <param name="parent">The parent of the <see cref="DocType"/>. This should be a <see cref="DocNamespace"/> or <see cref="DocType"/>.</param>
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

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
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

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public Page BuildPage(DocSiteModel context)
        {
            var sections = new List<ISection>();
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
                Name = MemberDetails.FileId,
                Title = MemberDetails.LocalName,
                Sections = sections
            };
        }

        private void AddConstructors(IList<ISection> sections)
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

        private void AddFields(IList<ISection> sections)
        {
            if (Fields.Any())
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

        private void AddProperties(IList<ISection> sections)
        {
            if (Properties.Any())
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

        private void AddMethods(IList<ISection> sections)
        {
            if (Methods.Any())
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

        private void AddEvents(IList<ISection> sections)
        {
            if (Events.Any())
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

        private void AddTypes(IList<ISection> sections)
        {
            if (Types.Any())
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

        /// <summary>
        /// Gets the table headers for including this type in a table.
        /// </summary>
        /// <returns><see cref="IEnumerable{String}"/> - The collection of table headers.</returns>
        public static IEnumerable<string> GetTableHeaders()
        {
            return new[] {"Name", "Description"};
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public TableRow GetTableRow()
        {
            var document = new XmlDocument();
            return new TableRow
            {
                Columns = new[]
                {
                    new TableData
                    {
                        Link = MemberDetails.FileId,
                        TextContent = MemberDetails.LocalName
                    },
                    new TableData
                    {
                        XmlContent = MemberDetails.Summary
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
            var nodes = Constructors.Select(c => c.BuildTree(currentPage, hrefExtension))
                .Union(Fields.Select(f => f.BuildTree(currentPage, hrefExtension)))
                .Union(Properties.Select(p => p.BuildTree(currentPage, hrefExtension)))
                .Union(Methods.Select(m => m.BuildTree(currentPage, hrefExtension)))
                .Union(Events.Select(m => m.BuildTree(currentPage, hrefExtension)))
                .Union(Types.Select(m => m.BuildTree(currentPage, hrefExtension)));
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
    }
}