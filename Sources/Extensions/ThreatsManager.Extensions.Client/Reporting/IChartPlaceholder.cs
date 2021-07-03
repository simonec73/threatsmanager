using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Interface representing a placeholder for Reporting, to replace the placeholder with a Chart.
    /// </summary>
    public interface IChartPlaceholder : IPlaceholder
    {
        /// <summary>
        /// Chart type to be used. A different Chart Type may be used, potentially.
        /// </summary>
        ChartType StandardChartType { get; }

        /// <summary>
        /// Get the items to be used to replace the placeholder.
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Items to be represented in the chart generation.</returns>
        IEnumerable<ChartItem> GetChart(IThreatModel model);
    }
}