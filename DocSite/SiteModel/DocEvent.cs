﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DocSite.Renderers;
using DocSite.Xml;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    /// <summary>
    /// <see cref="IDocModel"/> that represents an Event.
    /// </summary>
    /// <seealso cref="IDocModel"/>
    public class DocEvent : IDocModel
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
        /// Create a new <see cref="DocEvent"/>
        /// </summary>
        /// <param name="memberDetails">The <see cref="DocSite.Xml.MemberDetails"/> to create the <see cref="DocEvent"/> from.</param>
        /// <param name="parent">The parent of the <see cref="DocEvent"/>. This should be a <see cref="DocType"/>.</param>
        public DocEvent(MemberDetails memberDetails, IDocModel parent = null)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Event) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Event}", nameof(memberDetails));
            MemberDetails = memberDetails;
            Parent = parent;
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary)
        {
            if (membersDictionary == null) throw new ArgumentNullException(nameof(membersDictionary));
            membersDictionary.Add(MemberDetails.Id, this);
        }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
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

        /// <summary>
        /// Gets the table headers for including this type in a table.
        /// </summary>
        /// <returns><see cref="IEnumerable{String}"/> - The collection of table headers.</returns>
        public static IEnumerable<string> GetTableHeaders()
        {
            return new[] { "Name", "Description" };
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
        public Tree BuildTree(string currentPage, string hrefExtension)
        {
            var href = MemberDetails.FileId + (hrefExtension != null ? $".{hrefExtension}" : "");
            return new Tree
            {
                Text = MemberDetails.LocalName,
                Href = href,
                State = new TreeState
                {
                    Selected = currentPage == MemberDetails.FileId
                }
            };
        }
    }
}