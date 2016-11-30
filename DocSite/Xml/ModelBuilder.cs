using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DocSite.Xml
{
    /// <summary>
    /// Class for building the <see cref="DocXmlModel"/> from an xml file.
    /// </summary>
    public class ModelBuilder
    {
        /// <summary>
        /// Build a <see cref="DocXmlModel"/> from the specified <paramref name="xmlFileName"/>.
        /// </summary>
        /// <param name="xmlFileName">The file to load the <see cref="DocXmlModel"/> from.</param>
        /// <returns>The constructed <see cref="DocXmlModel"/></returns>
        public DocXmlModel BuildModelFromXml(string xmlFileName)
        {
            var ser = new XmlSerializer(typeof(DocXmlModel));
            using (var reader = File.OpenText(xmlFileName))
            {
                return ser.Deserialize(reader) as DocXmlModel;
            }
        }
    }
}
