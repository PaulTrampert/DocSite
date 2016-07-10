using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Renderers;

namespace DocSite.Pages
{
    public class Page : IRenderable
    {
        public string Name { get; set; }

        public string AssemblyName { get; set; }

        public string Title { get; set; }

        public IEnumerable<IRenderable> Sections { get; set; }

        public int Order { get; set; }

        public string RenderWith(IRenderer renderer)
        {
            Sections = Sections.OrderBy(s => s.Order);
            return renderer.RenderPage(this);
        }
    }
}
