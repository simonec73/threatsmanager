using System;
using System.Collections.Generic;
using System.Text;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// A row which is empty and should not be shown.
    /// </summary>
    public class EmptyRow : ItemRow
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="label">Label of the Row.</param>
        public EmptyRow(string label) : base(label)
        {
        }

        /// <summary>
        /// Flag specifying if the row should be visible or not.
        /// </summary>
        public override bool Visible => false;
    }
}
