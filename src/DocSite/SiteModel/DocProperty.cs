using System;
using System.Linq;
using System.Xml;
using DocSite.Xml;
using System.Collections.Generic;

namespace DocSite.SiteModel
{
    public class DocProperty : IDocModel
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

        public DocProperty(MemberDetails memberDetails)
        {
            if (memberDetails == null) throw new ArgumentNullException(nameof(memberDetails));
            if (memberDetails.Type != MemberType.Property) throw new ArgumentException($"{nameof(memberDetails)} must be {MemberType.Property}", nameof(memberDetails));
            MemberDetails = memberDetails;
        }

        public void AddMembersToDictionary(IDictionary<string, IDocModel> membersDictionary)
        {
            if (membersDictionary == null) throw new ArgumentNullException(nameof(membersDictionary));
            membersDictionary.Add(MemberDetails.Id, this);
        }
    }
}