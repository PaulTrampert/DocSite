using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace DocSite.Renderers
{
    public interface IRenderer
    {
        string RenderElement(XmlElement element);
    }
}
