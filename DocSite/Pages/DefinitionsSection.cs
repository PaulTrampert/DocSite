using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Renderers;
using System.Xml;

namespace DocSite.Pages
{
    /// <summary>
    /// A renderable section for a list of definitions.
    /// </summary>
    /// <seealso cref="ISection"/>
    /// <seealso cref="IRenderable"/>
    public class DefinitionsSection : ISection
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
        /// The <see cref="XmlNode"/>s that make up the definitions.
        /// </summary>
        /// <value>Gets/Sets the nodes that make up the definitions.</value>
        public IEnumerable<XmlNode> Definitions { get; set; }

        /// <summary>
        /// Inherited from <see cref="IRenderable"/>.
        /// </summary>
        public string RenderWith(IRenderer renderer)
        {
            return renderer.RenderDefinitionsSection(this);
        }
    }
}
