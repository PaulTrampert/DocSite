﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DocSite.Renderers;
using DocSite.Xml;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    public class DocMethod : IDocModel
    {
        public MemberDetails MemberDetails { get; }
        public IDocModel Parent { get; }

        public DocMethod(MemberDetails memberDetails, IDocModel parent = null)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Method) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Method}", nameof(memberDetails));
            if (memberDetails.LocalName.StartsWith("#ctor")) throw new ArgumentException($"{nameof(memberDetails)} describes a constructor", nameof(memberDetails));
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
                Title = MemberDetails.LocalName,
                Sections = sections
            };
        }

        public static IEnumerable<string> GetTableHeaders()
        {
            return new[] { "Name", "Description" };
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