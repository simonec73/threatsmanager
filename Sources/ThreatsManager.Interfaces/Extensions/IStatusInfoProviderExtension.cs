using System;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by the extensions that provide information within the Status Bar.
    /// </summary>
    [ExtensionDescription("Status Info Provider")]
    public interface IStatusInfoProviderExtension : IExtension, IDisposable
    {
        /// <summary>
        /// Event raised when there is the need to update the text shown by the Extension.
        /// </summary>
        /// <remarks>The first parameter corresponds to the identifier of the Extension,
        /// while the second is the text to be shown.</remarks>
        event Action<string, string> UpdateInfo;

        /// <summary>
        /// Initialization of the Status Info Provider.
        /// </summary>
        /// <param name="model">Threat Model used as reference for the Status Info Provider.</param>
        void Initialize(IThreatModel model);

        /// <summary>
        /// Get the current status.
        /// </summary>
        string CurrentStatus { get; }

        /// <summary>
        /// Get the description about what the Extension provide.
        /// </summary>
        string Description { get; }
    }
}
