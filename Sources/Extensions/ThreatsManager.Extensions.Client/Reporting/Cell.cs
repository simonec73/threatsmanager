using System;
using System.Collections.Generic;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// A cell.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value of the cell.</param>
        public Cell(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value of the cell to be used if no LinkIds correspond to an actual object.</param>
        /// <param name="prefix">Prefix to compose the final text.</param>
        /// <param name="suffix">Suffix to compose the final text.</param>
        /// <param name="linkIds">Identifiers of items in a List to which the text should be linked to.</param>
        public Cell(string value, string prefix, string suffix, IEnumerable<Guid> linkIds) : this(value)
        {
            LinkIds = linkIds;
            Prefix = prefix;
            Suffix = suffix;
        }

        /// <summary>
        /// Value of the cell.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Prefix to compose the final text.
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// Suffix to compose the final text.
        /// </summary>
        public string Suffix { get; private set; }

        /// <summary>
        /// Identifier of items in a List to which the content of the cell should be linked to.
        /// </summary>
        /// <remarks>The Ids are listed in priority order: the first which is a match is applied.</remarks>
        public IEnumerable<Guid> LinkIds { get; private set; }
    }
}