using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using DocSite.Renderers;

namespace DocSite.Pages
{
    /// <summary>
    /// Class representing a basic Title/Body section.
    /// </summary>
    /// <seealso cref="ISection"/>
    /// <seealso cref="IRenderable"/>
    public class Section : ISection
    {
        /// <summary>
        /// Inherited from <see cref="ISection"/>.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Inherited from <see cref="ISection"/>.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The <see cref="XmlNode"/>s making up the body of the <see cref="Section"/>.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Body"/></value>
        public IEnumerable<XmlNode> Body { get; set; }

        /// <summary>
        /// Inherited from <see cref="IRenderable"/>
        /// </summary>
        public string RenderWith(IRenderer renderer)
        {
            return renderer.RenderSection(this);
        }
    }
}
