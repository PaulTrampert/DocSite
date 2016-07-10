using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Renderers;

namespace DocSite.Pages
{
    public class TableSection : IRenderable
    {
        public string Title { get; set; }

        public int Order { get; set; }

        public IEnumerable<string> Headers { get; set; }

        public IEnumerable<TableRow> Rows { get; set; }

        public string RenderWith(IRenderer renderer)
        {
            return renderer.RenderTableSection(this);
        }
    }
}
