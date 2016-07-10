using System.Xml;

namespace DocSite.Pages
{
    /// <summary>
    /// Class representing the content of a table cell.
    /// </summary>
    public  class TableData
    {
        /// <summary>
        /// The link data. If defined, this can be used to make a hyperlink.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Link"/>.</value>
        public string Link { get; set; }

        /// <summary>
        /// The content of the table data. This will be rendered and visible to the user.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Content"/>.</value>
        public XmlNode Content { get; set; }
    }
}