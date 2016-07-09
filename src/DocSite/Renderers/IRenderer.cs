using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using DocSite.Pages;

namespace DocSite.Renderers
{
    public interface IRenderer
    {
        string RenderNode(XmlNode node);
        string RenderPage(Page page);
        string RenderSection(Section section);
        string RenderTableSection(TableSection section);
    }
}
