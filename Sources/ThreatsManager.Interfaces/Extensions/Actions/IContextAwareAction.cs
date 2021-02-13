using System.Drawing;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Action that can be executed on an object and is typically represented as a context menu item.
    /// </summary>
    [ExtensionDescription("Context Aware Action")]
    public interface IContextAwareAction : IExtension
    {
        /// <summary>
        /// Scope of the Action.
        /// </summary>
        Scope Scope { get; }

        /// <summary>
        /// Label to use for the Action.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Group of Actions the current one belongs to.
        /// </summary>
        string Group { get; }

        /// <summary>
        /// Icon to use for the action.
        /// </summary>
        Bitmap Icon { get; }

        /// <summary>
        /// Small image to use for the action.
        /// </summary>
        Bitmap SmallIcon { get; }

        /// <summary>
        /// Shortcut to use for the action.
        /// </summary>
        Shortcut Shortcut { get; }

        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="item">Object on which the Action must be executed.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        bool Execute(object item);

        /// <summary>
        /// Checks if the Action should be visible for the current object.
        /// </summary>
        /// <param name="item">Current object.</param>
        /// <returns>True if the Action is visible, false otherwise.</returns>
        bool IsVisible(object item);
    }
}