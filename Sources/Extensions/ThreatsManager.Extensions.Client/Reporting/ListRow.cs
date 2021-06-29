using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// A row for an List Item containing multiple lines of text.
    /// </summary>
    public class ListRow : ItemRow
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Label of the Row.</param>
        /// <param name="lines">Lines of text to be shown.</param>
        public ListRow(string label, IEnumerable<string> lines) : base(label)
        {
            Lines = lines;
        }

        /// <summary>
        /// Flag specifying if the row should be visible or not.
        /// </summary>
        public override bool Visible => Lines?.Any() ?? false;

        /// <summary>
        /// Lines part of the Row.
        /// </summary>
        public IEnumerable<string> Lines { get; private set; }
    }
}