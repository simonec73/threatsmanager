using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// Extension used to create placeholders.
    /// </summary>
    [ExtensionDescription("Placeholder to be used for Reporting")]
    public interface IPlaceholderFactory : IExtension
    {
        /// <summary>
        /// Placeholder qualifier.
        /// </summary>
        string Qualifier { get; }

        /// <summary>
        /// Creates a placeholder.
        /// </summary>
        /// <param name="parameters">Parameters to be used for the Placeholder.</param>
        /// <returns>The new placeholder instance.</returns>
        IPlaceholder Create(string parameters = null);
    }
}
