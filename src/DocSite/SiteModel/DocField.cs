using System;
using DocSite.Xml;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace DocSite.SiteModel
{
    public class DocField
    {
        public MemberDetails MemberDetails { get; }

        public XmlElement Summary => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "summary");

        public XmlElement Example => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "example");

        public XmlElement Permission => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "permission");

        public XmlElement Remarks => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "remarks");

        public XmlElement Returns => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "returns");

        public XmlElement Value => MemberDetails.DocXml.SingleOrDefault(xml => xml.Name == "value");

        public IEnumerable<XmlElement> Exceptions => MemberDetails.DocXml.Where(xml => xml.Name == "exception");

        public IEnumerable<XmlElement> SeeAlso => MemberDetails.DocXml.Where(xml => xml.Name == "seealso");
        public DocField(MemberDetails memberDetails)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Field) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Field}", nameof(memberDetails));
            MemberDetails = memberDetails;
        }
    }
}