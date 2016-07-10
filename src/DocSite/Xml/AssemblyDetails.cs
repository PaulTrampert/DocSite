using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocSite.Xml
{
    /// <summary>
    /// Represents the assembly details in a documentation xml document.
    /// </summary>
    [XmlRoot("assembly")]
    public class AssemblyDetails
    {
        /// <summary>
        /// The name of the assembly.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Name"/> of the assembly.</value>
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
