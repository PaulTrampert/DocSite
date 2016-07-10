using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocSite.Xml
{
    /// <summary>
    /// Root model for a documentation xml document.
    /// </summary>
    [XmlRoot("doc")]
    public class DocXmlModel
    {
        /// <summary>
        /// The <see cref="AssemblyDetails"/>
        /// </summary>
        /// <value>Gets/Sets the <see cref="Assembly"/></value>
        [XmlElement("assembly")]
        public AssemblyDetails Assembly { get; set; }

        /// <summary>
        /// The collection of members that are documented in the xml document.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Members"/></value>
        [XmlArray("members")]
        [XmlArrayItem("member")]
        public List<MemberDetails> Members { get; set; }

        /// <summary>
        /// Create a new, empty <see cref="DocXmlModel"/>
        /// </summary>
        public DocXmlModel()
        {
            Assembly = new AssemblyDetails();
            Members = new List<MemberDetails>();
        }
    }
}
