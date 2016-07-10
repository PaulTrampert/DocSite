using System;
using DocSite.Xml;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using DocSite.Renderers;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    public class DocConstructor : IDocModel
    {
        public MemberDetails MemberDetails { get; }
        public IDocModel Parent { get; }

        private string Title => MemberDetails.LocalName.Replace("#ctor", Parent.MemberDetails.LocalName);

        public DocConstructor(MemberDetails memberDetails, IDocModel parent = null)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Method) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Method}", nameof(memberDetails));
            if (!memberDetails.LocalName.StartsWith("#ctor")) throw new ArgumentException($"{nameof(memberDetails)} describes a method", nameof(memberDetails));
            MemberDetails = memberDetails;
            Parent = parent;
        }

        public void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary)
        {
            if (membersDictionary == null) throw new ArgumentNullException(nameof(membersDictionary));
            membersDictionary.Add(MemberDetails.Id, this);
        }

        public Page BuildPage(DocSiteModel context)
        {
            var sections = new List<ISection>();
            MemberDetails.AddCommonSections(sections);
            return new Page
            {
                AssemblyName = context.AssemblyName,
                Name = MemberDetails.FileId,
                Title = Title,
                Sections = sections
            };
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
                        Content = new XmlDocument {InnerText = Title}
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