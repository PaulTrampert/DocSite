﻿using System;
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
            var page = new Page
            {
                AssemblyName = context.AssemblyName,
                Name = Name,
                Title = MemberDetails.LocalName,
                Sections = sections
            };
            AddTypeParams(sections);
            AddSummary(sections);
            AddRemarks(sections);
            AddExample(sections);

            return page;
        }

        private void AddExample(List<IRenderable> sections)
        {
            if (Example != null)
            {
                sections.Add(new Section
                {
                    Title = "Example",
                    Body = Example.ChildNodes.Cast<XmlNode>()
                });
            }
        }

        private void AddRemarks(List<IRenderable> sections)
        {
            if (Remarks != null)
            {
                sections.Add(new Section
                {
                    Title = "Remarks",
                    Body = Remarks.ChildNodes.Cast<XmlNode>()
                });
            }
        }

        private void AddSummary(List<IRenderable> sections)
        {
            if (Summary != null)
            {
                sections.Add(new Section
                {
                    Title = "Summary",
                    Body = Summary.ChildNodes.Cast<XmlNode>()
                });
            }
        }

        private void AddTypeParams(List<IRenderable> sections)
        {
            if (TypeParams.Any())
            {
                sections.Add(new DefinitionsSection()
                {
                    Title = "Type Parameters",
                    Definitions = TypeParams
                });
            }
        }
    }
}