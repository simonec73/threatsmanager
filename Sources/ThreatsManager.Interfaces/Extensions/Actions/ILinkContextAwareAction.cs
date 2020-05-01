using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Specialized Context Aware Action for a single shape.
    /// </summary>
    public interface ILinkContextAwareAction : IContextAwareAction
    {
        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="link">Link on which the Action must be executed.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        bool Execute(ILink link);
    }
}