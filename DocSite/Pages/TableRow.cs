using System.Collections.Generic;

namespace DocSite.Pages
{
    /// <summary>
    /// Class representing a table row.
    /// </summary>
    public class TableRow
    {
        /// <summary>
        /// The collection of <see cref="TableData"/> to be included in the row.
        /// </summary>
        /// <value>Gets/Sets the <see cref="Columns"/></value>
        public IEnumerable<TableData> Columns { get; set; }
    }
}