using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocSite.Xml
{
    [XmlRoot("doc")]
    public class DocSiteModel
    {
        [XmlElement("assembly")]
        public AssemblyDetails Assembly { get; set; }

        [XmlArray("members")]
        [XmlArrayItem("member")]
        public List<MemberDetails> Members { get; set; }

        public DocSiteModel()
        {
            Assembly = new AssemblyDetails();
            Members = new List<MemberDetails>();
        }
    }
}
