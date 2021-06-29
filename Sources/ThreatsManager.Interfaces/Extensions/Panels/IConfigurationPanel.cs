using System.Collections.Generic;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel to show Extension-specific configurations.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    public interface IConfigurationPanel<T> : IPanel<T>
    {
        /// <summary>
        /// Label to be used to show the Configuration Panel.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Icon to use for the configuration panel.
        /// </summary>
        /// <remarks>Best if icon is white. Preferred size is 24x24 pixels image or bigger.</remarks>
        Bitmap Icon { get; }

        /// <summary>
        /// Icon to use for the configuration panel when selected.
        /// </summary>
        /// <remarks>Best if icon is black. Use Preferred size is 24x24 pixels image or bigger.</remarks>
        Bitmap SelectedIcon { get; }

        /// <summary>
        /// Get the Configuration objects associated with the Extension.
        /// </summary>
        IEnumerable<ConfigurationData> Configuration { get; }

        /// <summary>
        /// Initialize the Configuration Panel with the folder containing the Extension Configuration files.
        /// </summary>
        /// <param name="model">Current Threat Model.</param>
        void Initialize(IThreatModel model);

        /// <summary>
        /// Apply the current configuration.
        /// </summary>
        /// <remarks>This is typically called after the configuration changes have been confirmed.</remarks>
        void Apply();
    }
}
