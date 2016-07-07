using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DocSite.Xml
{
    public class ModelBuilder
    {
        public DocSiteModel BuildModelFromXml(string xmlFileName)
        {
            var xmlDoc = new XDocument();
            var result = new DocSiteModel();
            return result;
        }
    }
}
