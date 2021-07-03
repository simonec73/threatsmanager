using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Definition of a column in a Table.
    /// </summary>
    public class TableColumn
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Label to be shown for the column.</param>
        /// <param name="width">Width of the column.</param>
        public TableColumn([Required] string label, int width)
        {
            Label = label;
            Width = width;
        }

        /// <summary>
        /// Label of the column.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Width of the column.
        /// </summary>
        public int Width { get; private set; }
    }
}