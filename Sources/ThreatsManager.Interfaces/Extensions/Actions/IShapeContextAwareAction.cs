using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Specialized Context Aware Action for a single shape.
    /// </summary>
    public interface IShapeContextAwareAction : IContextAwareAction
    {
        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="shape">Shape on which the Action must be executed.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        bool Execute(IShape shape);
    }
}