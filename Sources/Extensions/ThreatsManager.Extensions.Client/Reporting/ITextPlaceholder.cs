using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Placeholder manager for Reporting, to replace the placeholder with text.
    /// </summary>
    public interface ITextPlaceholder : IPlaceholder
    {
        /// <summary>
        /// Get the text to be used to replace the placeholder.
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Text to use for replacement.</returns>
        string GetText(IThreatModel model);
    }
}
