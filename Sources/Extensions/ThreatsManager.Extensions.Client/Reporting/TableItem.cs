using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// A table item.
    /// </summary>
    public class TableItem
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Label to use for the Table Item.</param>
        /// <param name="columns">Headers to be shown for the table.</param>
        /// <param name="cells">Cells composing the table.</param>
        public TableItem(string label, IEnumerable<TableColumn> columns, IEnumerable<Cell> cells)
        {
            Label = label;
            Columns = columns;
            Cells = cells;
        }

        /// <summary>
        /// Label to use for the Table Item.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Definition of the Headers composing the Table Item.
        /// </summary>
        public IEnumerable<TableColumn> Columns { get; private set; }

        /// <summary>
        /// Cells composing the Table Item.
        /// </summary>
        public IEnumerable<Cell> Cells { get; private set; }

        /// <summary>
        /// Returns the number of rows defined in the table.
        /// </summary>
        /// <returns></returns>
        public int RowCount => (Columns?.Any() ?? false) ? (Cells?.Count() ?? 0) / Columns.Count() : 0; 

        /// <summary>
        /// Get a cell given its cohordinates.
        /// </summary>
        /// <param name="row">Index of the Row, zero based.</param>
        /// <param name="col">Index of the Column, zero based.</param>
        /// <returns></returns>
        public Cell Get(int row, int col)
        {
            return Cells?.ElementAt(row * Columns.Count() + col);
        }
    }
}