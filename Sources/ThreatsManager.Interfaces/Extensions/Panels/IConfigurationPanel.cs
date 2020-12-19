using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel to show Extension-specific configurations.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    [ExtensionDescription("Configuration Panel")]
    public interface IConfigurationPanel<T> : IExtension, IPanel<T>
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
    }
}
