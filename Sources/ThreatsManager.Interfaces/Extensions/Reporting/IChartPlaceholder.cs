using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Reporting
{
    /// <summary>
    /// Placeholder manager for Reporting, to replace the placeholder with a Chart.
    /// </summary>
    [ExtensionDescription("Reporting Placeholder for Chart")]
    public interface IChartPlaceholder : IPlaceholder, IExtension
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