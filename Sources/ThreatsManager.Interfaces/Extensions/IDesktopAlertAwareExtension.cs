using System;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by the Extensions that can request the UI to show an alert, when required.
    /// </summary>
    public interface IDesktopAlertAwareExtension
    {
        /// <summary>
        /// Event raised when a normal message should be shown.
        /// </summary>
        event Action<string> ShowMessage;

        /// <summary>
        /// Event raised when a warning should be shown.
        /// </summary>
        event Action<string> ShowWarning;
    }
}