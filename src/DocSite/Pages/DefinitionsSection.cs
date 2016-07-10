using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Renderers;
using System.Xml;

namespace DocSite.Pages
{
    public class DefinitionsSection : IRenderable
    {
        public string Title { get; set; }

        public int Order { get; set; }

        public IEnumerable<XmlNode> Definitions { get; set; }

        public string RenderWith(IRenderer renderer)
        {
            return renderer.RenderDefinitionsSection(this);
        }
    }
}
