using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions
{
    /// <summary>
    /// Get filters for the Roadmap.
    /// </summary>
    [ExtensionDescription("Roadmap Filter Provider")]
    public interface IRoadmapFilterProvider : IExtension
    {
        /// <summary>
        /// Returns the filters defined by the Provider for the Threat Model.
        /// </summary>
        /// <param name="model">Threat Model for which the filters must be calculated.</param>
        /// <returns>Enumerations of the available Roadmap Filters.</returns>
        IEnumerable<IRoadmapFilter> GetFilters(IThreatModel model);
    }
}
