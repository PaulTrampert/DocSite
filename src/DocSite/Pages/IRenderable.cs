using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Renderers;

namespace DocSite.Pages
{
    public interface IRenderable
    {
        string RenderWith(IRenderer renderer);
    }
}
