using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace DocSite.Pages
{
    public class Section
    {
        public string Title { get; set; }

        public IEnumerable<XmlNode> Body { get; set; }
    }
}
