using System;
using DocSite.Xml;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace DocSite.SiteModel
{
    public class DocConstructor
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

        public DocConstructor(MemberDetails memberDetails)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Method) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Method}", nameof(memberDetails));
            if (!memberDetails.LocalName.StartsWith("#ctor")) throw new ArgumentException($"{nameof(memberDetails)} describes a method", nameof(memberDetails));
            MemberDetails = memberDetails;
        }
    }
}