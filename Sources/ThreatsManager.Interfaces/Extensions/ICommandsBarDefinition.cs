using System.Collections.Generic;

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
        /// Enumeration of the various buttons to be added to the Bar.
        /// </summary>
        IEnumerable<IActionDefinition> Commands { get; }
    }
}