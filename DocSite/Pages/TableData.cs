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
        /// Text content for the table data. Only used if XmlContent is null.
        /// </summary>
        /// <value>Gets/Sets the <see cref="TextContent"/></value>
        public string TextContent { get; set; }

        /// <summary>
        /// The content of the table data. This will be rendered and visible to the user.
        /// </summary>
        /// <value>Gets/Sets the <see cref="XmlContent"/>.</value>
        public XmlNode XmlContent { get; set; }
    }
}