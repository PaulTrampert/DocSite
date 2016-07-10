using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using DocSite.Pages;

namespace DocSite.Renderers
{
    /// <summary>
    /// Interface to be implemented by classes intending to render <see cref="IRenderable"/> classes.
    /// </summary>
    /// <seealso cref="HtmlRenderer"/>
    /// <seealso cref="IRenderable"/>
    public interface IRenderer
    {
        /// <summary>
        /// Render a collection of <see cref="XmlNode"/>.
        /// </summary>
        /// <param name="nodes">The collection of nodes to render.</param>
        /// <returns><see cref="String"/> - Returns the rendered string.</returns>
        string RenderNodes(IEnumerable<XmlNode> nodes);

        /// <summary>
        /// Render a single <see cref="XmlNode"/>.
        /// </summary>
        /// <param name="node">The node to render.</param>
        /// <returns><see cref="String"/> - Returns the rendered string.</returns>
        string RenderNode(XmlNode node);

        /// <summary>
        /// Render a <see cref="Page"/>.
        /// </summary>
        /// <param name="page">The <see cref="Page"/> to render.</param>
        /// <returns><see cref="String"/> - Returns the rendered string.</returns>
        string RenderPage(Page page);

        /// <summary>
        /// Render a <see cref="Section"/>
        /// </summary>
        /// <param name="section">The <see cref="Section"/> to render.</param>
        /// <returns><see cref="String"/> - Returns the rendered string.</returns>
        string RenderSection(Section section);

        /// <summary>
        /// Render a <see cref="TableSection"/>.
        /// </summary>
        /// <param name="section">The <see cref="TableSection"/> to render.</param>
        /// <returns><see cref="String"/> - Returns the rendered string.</returns>
        string RenderTableSection(TableSection section);

        /// <summary>
        /// Render a <see cref="DefinitionsSection"/>.
        /// </summary>
        /// <param name="section">The <see cref="DefinitionsSection"/> to render.</param>
        /// <returns><see cref="String"/> - Returns the rendered string.</returns>
        string RenderDefinitionsSection(DefinitionsSection section);
    }
}
