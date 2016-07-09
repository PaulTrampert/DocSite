using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace DocSite.Renderers
{
    public class HtmlRenderer : IRenderer
    {
        private ITemplateFinder _templateFinder;
        public string RenderElement(XmlElement element)
        {
            var template = _templateFinder.FindTemplate(element.Name);
            return "";
        }
    }

    internal interface ITemplateFinder
    {
        string FindTemplate(string name);
    }
}
