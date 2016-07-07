using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocSite.Xml
{
    [XmlRoot("assembly")]
    public class AssemblyDetails
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
