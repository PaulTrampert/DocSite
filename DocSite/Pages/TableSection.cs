using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocSite.Renderers;

namespace DocSite.Pages
{
    /// <summary>
    /// Class representing a section defined by a table.
    /// </summary>
    /// <seealso cref="ISection"/>
    /// <seealso cref="IRenderable"/>
    public class TableSection : ISection
    {
        /// <summary>
        /// Inherited from <see cref="ISection"/>
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Inherited from <see cref="ISection"/>
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The column headers of the table.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Headers"/></value>
        public IEnumerable<string> Headers { get; set; }

        /// <summary>
        /// The rows of the table.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Rows"/></value>
        public IEnumerable<TableRow> Rows { get; set; }

        /// <summary>
        /// Inherited from <see cref="IRenderable"/>
        /// </summary>
        public string RenderWith(IRenderer renderer)
        {
            return renderer.RenderTableSection(this);
        }
    }
}
