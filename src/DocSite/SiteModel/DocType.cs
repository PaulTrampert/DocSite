﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class DocType
    {
        public MemberDetails MemberDetails { get; }

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

        public DocType(MemberDetails memberDetails, IEnumerable<MemberDetails> otherMembers)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (otherMembers == null) throw new ArgumentNullException(nameof(otherMembers));
            if (memberDetails.Type != MemberType.Type) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Type}", nameof(memberDetails));
            MemberDetails = memberDetails;
            Constructors = otherMembers.Where(m => m.Type == MemberType.Method && m.ParentMember == Name && m.LocalName.StartsWith("#ctor")).Select(m => new DocConstructor(m));
            Properties = otherMembers.Where(m => m.Type == MemberType.Property && m.ParentMember == Name).Select(m => new DocProperty(m));
            Methods = otherMembers.Where(m => m.Type == MemberType.Method && m.ParentMember == Name && !m.LocalName.StartsWith("#ctor")).Select(m => new DocMethod(m));
            Fields = otherMembers.Where(m => m.Type == MemberType.Field && m.ParentMember == Name).Select(m => new DocField(m));
            Events = otherMembers.Where(m => m.Type == MemberType.Event && m.ParentMember == Name).Select(m => new DocEvent(m));
            Types = otherMembers.Where(m => m.Type == MemberType.Type && m.ParentMember == Name).Select(m => new DocType(m, otherMembers));
        }
    }
}