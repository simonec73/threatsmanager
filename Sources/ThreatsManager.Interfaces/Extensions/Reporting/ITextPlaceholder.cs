using System;
using System.Text;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Reporting
{
    /// <summary>
    /// Placeholder manager for Reporting, to replace the placeholder with text.
    /// </summary>
    [ExtensionDescription("Reporting Placeholder for Text")]
    public interface ITextPlaceholder : IPlaceholder, IExtension
    {
        /// <summary>
        /// Get the text to be used to replace the placeholder.
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Text to use for replacement.</returns>
        string GetText(IThreatModel model);
    }
}
