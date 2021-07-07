using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Interface representing a placeholder for Reporting, to replace the placeholder with a Table.
    /// </summary>
    public interface ITablePlaceholder : IPlaceholder
    {
        /// <summary>
        /// Get the items to be used to replace the placeholder.
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Table to be represented in the Table generation.</returns>
        TableItem GetTable(IThreatModel model);
    }
}