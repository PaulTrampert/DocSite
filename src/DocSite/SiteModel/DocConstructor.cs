﻿using System;
using DocSite.Xml;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using DocSite.Renderers;
using DocSite.Pages;

namespace DocSite.SiteModel
{
    /// <summary>
    /// <see cref="IDocModel"/> that represents a Constructor.
    /// </summary>
    /// <seealso cref="IDocModel"/>
    public class DocConstructor : IDocModel
    {
        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public MemberDetails MemberDetails { get; }

        /// <summary>
        /// Inherited from <see cref="IDocModel"/>
        /// </summary>
        public IDocModel Parent { get; }

        private string Title => MemberDetails.LocalName.Replace("#ctor", Parent.MemberDetails.LocalName);

        /// <summary>
        /// Create a new DocConstructor
        /// </summary>
        /// <param name="memberDetails">The <see cref="DocSite.Xml.MemberDetails"/> to create the <see cref="DocConstructor"/> from.</param>
        /// <param name="parent">The parent of the <see cref="DocConstructor"/>. This should be a <see cref="DocType"/>.</param>
        public DocConstructor(MemberDetails memberDetails, IDocModel parent = null)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Method) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Method}", nameof(memberDetails));
            if (!memberDetails.LocalName.StartsWith("#ctor")) throw new ArgumentException($"{nameof(memberDetails)} describes a method", nameof(memberDetails));
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
                Title = Title,
                Sections = sections
            };
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
            return new TableRow
            {
                Columns = new[]
                {
                    new TableData
                    {
                        Link = MemberDetails.FileId,
                        TextContent = Title
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
        public Tree BuildTree(string currentPage)
        {
            return new Tree
            {
                Text = MemberDetails.LocalName,
                Href = MemberDetails.FileId,
                State = new TreeState
                {
                    Selected = currentPage == MemberDetails.FileId
                }
            };
        }
    }
}