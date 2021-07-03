using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Interface representing a placeholder for Reporting, to replace the placeholder with an integer.
    /// </summary>
    public interface ICounterPlaceholder : IPlaceholder
    {
        /// <summary>
        /// Get the integer to be used to replace the placeholder.
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Integer to use for replacement.</returns>
        int GetCounter(IThreatModel model);
    }
}