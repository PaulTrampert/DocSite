using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DocSite.Renderers;

namespace DocSite.Pages
{
    public interface IRenderable
    {
        int Order { get; set; }
        string RenderWith(IRenderer renderer);
    }
}
