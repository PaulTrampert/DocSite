using System.Collections.Generic;
using System.Xml;

namespace DocSite.Pages
{
    public class Definition
    {
        public string Term { get; set; }

        public IEnumerable<XmlNode> Body { get; set; }
    }
}