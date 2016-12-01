using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocSite.Pages
{
    /// <summary>
    /// Interface representing a section in a <see cref="Page"/>
    /// </summary>
    public interface ISection : IRenderable
    {
        /// <summary>
        /// The title of the section
        /// </summary>
        /// <value>Gets/Sets the <see cref="Title"/></value>
        string Title { get; set; }
        /// <summary>
        /// The order in which the section should appear.
        /// </summary>
        /// <value>Gets/Sets the order in which the section should appear.</value>
        int Order { get; set; }
    }
}
