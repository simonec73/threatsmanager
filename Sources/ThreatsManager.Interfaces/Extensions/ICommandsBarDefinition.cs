using System.Collections.Generic;
using System.Drawing;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface that represents the definition of the custom Tab to be merged to the Ribbon.
    /// </summary>
    public interface ICommandsBarDefinition
    {
        /// <summary>
        /// Name of the Bar.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Label for the Bar.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Flag specifying if the Commands Bar can be collapsed.
        /// </summary>
        bool Collapsible { get; }

        /// <summary>
        /// Image to use when the Commands Bar is collapsed.
        /// </summary>
        /// <remarks>If null, the default image is used.</remarks>
        Image CollapsedImage { get; }

        /// <summary>
        /// Enumeration of the various buttons to be added to the Bar.
        /// </summary>
        IEnumerable<IActionDefinition> Commands { get; }
    }
}