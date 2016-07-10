namespace DocSite.Pages
{
    /// <summary>
    /// Representation of the tree state.
    /// </summary>
    public class TreeState
    {
        /// <summary>
        /// True if selected, false otherwise.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// True if expanded, false otherwise.
        /// </summary>
        public bool Expanded { get; set; }
    }
}