﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using DocSite.Renderers;

namespace DocSite.Pages
{
    public class Section : IRenderable
    {
        public string Title { get; set; }

        public IEnumerable<XmlNode> Body { get; set; }
        public string RenderWith(IRenderer renderer)
        {
            return renderer.RenderSection(this);
        }
    }
}
