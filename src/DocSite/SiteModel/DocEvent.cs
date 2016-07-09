﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DocSite.Xml;

namespace DocSite.SiteModel
{
    public class DocEvent : IDocModel
    {
        public MemberDetails MemberDetails { get; }

        public XmlElement Summary => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "summary");

        public XmlElement Example => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "example");

        public XmlElement Permission => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "permission");

        public XmlElement Remarks => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "remarks");

        public IEnumerable<XmlElement> Params => MemberDetails.DocXml.Where(xml => xml.Name == "param");

        public IEnumerable<XmlElement> TypeParams => MemberDetails.DocXml.Where(xml => xml.Name == "typeparam");

        public IEnumerable<XmlElement> Exceptions => MemberDetails.DocXml.Where(xml => xml.Name == "exception");

        public IEnumerable<XmlElement> SeeAlso => MemberDetails.DocXml.Where(xml => xml.Name == "seealso");

        public DocEvent(MemberDetails memberDetails)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Event) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Event}", nameof(memberDetails));
            MemberDetails = memberDetails;
        }

        public void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary)
        {
            if (membersDictionary == null) throw new ArgumentNullException(nameof(membersDictionary));
            membersDictionary.Add(MemberDetails.Id, this);
        }
    }
}