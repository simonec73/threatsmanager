using System;
using System.Collections.Generic;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Item to be shown in the List.
    /// </summary>
    public class ListItem
    {
        /// <summary>
        /// Constructor for the List Item.
        /// </summary>
        /// <param name="label">Label to use for the List Item.</param>
        /// <param name="id">Identifier of the object.</param>
        /// <param name="rows">Rows composing the List Item.</param>
        public ListItem([Required] string label, Guid id, IEnumerable<ItemRow> rows)
        {
            Label = label;
            Id = id;
            Rows = rows;
        }

        /// <summary>
        /// Identifier of the Item.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Label of the Item.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Rows composing the List Item.
        /// </summary>
        public IEnumerable<ItemRow> Rows { get; private set; }
    }
}