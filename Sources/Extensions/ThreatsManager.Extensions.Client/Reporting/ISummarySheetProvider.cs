using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Provider used to generate additional sheets in the Summary Excel Report.
    /// </summary>
    [ExtensionDescription("Table provider for the Summary Excel Report")]
    public interface ISummarySheetProvider : IExtension
    {
        /// <summary>
        /// Name of the sheet to be created.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Generates the Rows to be used to fill the sheet.
        /// </summary>
        /// <param name="model">Reference Threat Model for sheet generation.</param>
        /// <remarks>The outer enumeration is for the rows.
        /// The inner enumeration contains the fields for each row.
        /// Each row must have the same number of fields,
        /// otherwise the sheet will not be generated.
        /// The first row is used as the Header.</remarks>
        IEnumerable<IEnumerable<string>> GetRows(IThreatModel model);
    }
}
