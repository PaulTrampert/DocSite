using System.Collections.Generic;
using System.Linq;

namespace DocSite.Pages
{
    /// <summary>
    /// Represents the documentation tree. Can be used to render a navigation tree.
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// The display text of the tree.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The page to link to with the tree.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// The state of the tree.
        /// </summary>
        public TreeState State { get; set; }

        /// <summary>
        /// Child nodes of the tree.
        /// </summary>
        public IEnumerable<Tree> Nodes { get; set; }

        /// <summary>
        /// Find if this or any child nodes are selected.
        /// </summary>
        /// <returns>True if this or any child nodes are selected, false otherwise.</returns>
        public bool AnySelected()
        {
            return State.Selected || (Nodes == null ? false : Nodes.Any(n => n.AnySelected()));
        }
    }
}