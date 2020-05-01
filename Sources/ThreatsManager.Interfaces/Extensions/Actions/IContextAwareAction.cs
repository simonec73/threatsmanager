using System.Drawing;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Action that can be executed on a 
    /// </summary>
    public interface IContextAwareAction
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
    }
}