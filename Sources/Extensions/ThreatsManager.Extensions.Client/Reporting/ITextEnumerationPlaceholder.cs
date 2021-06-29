using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Placeholder manager for Reporting, to replace the placeholder with an enumeration of strings.
    /// </summary>
    [ExtensionDescription("Reporting Placeholder for Enumeration of Strings")]
    public interface ITextEnumerationPlaceholder : IPlaceholder, IExtension
    {
        /// <summary>
        /// Get the enumeration of strings to be used to replace the placeholder.
        /// </summary>
        /// <param name="model">Threat Model to get the Placeholder from.</param>
        /// <returns>Enumeration of strings to use for replacement.</returns>
        IEnumerable<string> GetTextEnumeration(IThreatModel model);
    }
}