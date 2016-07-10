using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Renderers;

namespace DocSite.Pages
{
    /// <summary>
    /// Class representing a page.
    /// </summary>
    /// <seealso cref="IRenderable"/>
    public class Page : IRenderable
    {
        /// <summary>
        /// The name of the page. This should be used as the file name for file based renderers.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Name"/> of the page.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// The AssemblyName for the page.
        /// </summary>
        /// <value>Gets/Sets the <see cref="AssemblyName"/> of the page.</value>
        public string AssemblyName { get; set; }

        /// <summary>
        /// The title of the page.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Title"/> of the page.</value>
        public string Title { get; set; }

        /// <summary>
        /// The page sections.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Sections"/> of the page.</value>
        public IEnumerable<ISection> Sections { get; set; }

        /// <summary>
        /// Inherited from <see cref="IRenderable"/>
        /// </summary>
        public string RenderWith(IRenderer renderer)
        {
            Sections = Sections.OrderBy(s => s.Order);
            return renderer.RenderPage(this);
        }
    }
}
