using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocSite.Pages
{
    public class Page
    {
        public string Name { get; set; }

        public string AssemblyName { get; set; }

        public string Title { get; set; }

        public IEnumerable<Section> Sections { get; set; }
    }
}
