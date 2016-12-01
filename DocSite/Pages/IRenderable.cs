using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Renderers;

namespace DocSite.Pages
{
    /// <summary>
    /// An interface that allows an object to be rendered by an <see cref="IRenderer"/>.
    /// </summary>
    /// <seealso cref="Page"/>
    /// <seealso cref="ISection"/>
    /// <seealso cref="Section"/>
    /// <seealso cref="TableSection"/>
    /// <seealso cref="DefinitionsSection"/>
    /// <seealso cref="IRenderer"/>
    public interface IRenderable
    {
        /// <summary>
        /// Render the <see cref="IRenderable"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="IRenderer"/> to use for the rendering operation.</param>
        /// <returns><see cref="String"/> - The rendered string.</returns>
        string RenderWith(IRenderer renderer);
    }
}
