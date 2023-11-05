using System.Collections.Generic;
using System.Drawing;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Definition of a Commands Bar, that is a section of the Ribbon.
    /// </summary>
    public class CommandsBarDefinition : ICommandsBarDefinition
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the bar.</param>
        /// <param name="label">Label to show as title of the bar.</param>
        /// <param name="commands">Commands to show in the bar.</param>
        /// <param name="collapsible">Flag specifying if the bar can be collapsed.</param>
        /// <param name="image">Image to use if the bar is collapsed.</param>
        public CommandsBarDefinition([Required] string name, [Required] string label,
            [NotNull] IEnumerable<IActionDefinition> commands, bool collapsible = true,
            Image image = null)
        {
            Name = name;
            Label = label;
            Commands = commands;
            Collapsible = collapsible;
            CollapsedImage = image;
        }

        /// <summary>
        /// Name of the bar.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Label to show as title of the bar.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Commands to show in the bar.
        /// </summary>
        public IEnumerable<IActionDefinition> Commands { get; }

        /// <summary>
        /// Flag specifying if the bar can be collapsed.
        /// </summary>
        public bool Collapsible { get; }

        /// <summary>
        /// Image to use when the bar is collapsed.
        /// </summary>
        /// <remarks>If null, the default image is used.</remarks>
        public Image CollapsedImage { get; } = null;
    }
}
