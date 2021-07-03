using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// A row for an List Item containing a table.
    /// </summary>
    public class TableRow : ItemRow
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Label of the Row.</param>
        /// <param name="columns">Headers to be shown for the table.</param>
        /// <param name="cells">Cells composing the table.</param>
        public TableRow(string label, IEnumerable<TableColumn> columns, IEnumerable<Cell> cells) : base(label)
        {
            Columns = columns;
            Cells = cells;
        }

        /// <summary>
        /// Flag specifying if the row should be visible or not.
        /// </summary>
        public override bool Visible
        {
            get
            {
                var result = false;

                var cols = Columns?.Count() ?? 0;
                if (cols > 0)
                {
                    var div = Math.DivRem(Cells?.Count() ?? 0, cols, out var rem);
                    result = div > 0 && rem == 0;
                }

                return result;
            }
        }

        /// <summary>
        /// Lines part of the Row.
        /// </summary>
        public IEnumerable<TableColumn> Columns { get; private set; }

        /// <summary>
        /// Cells composing the Row.
        /// </summary>
        public IEnumerable<Cell> Cells { get; private set; }

        public Cell Get(int row, int col)
        {
            return Cells?.ElementAt(row * Columns.Count() + col);
        }
    }
}