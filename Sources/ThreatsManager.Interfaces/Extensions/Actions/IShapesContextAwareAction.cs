using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Specialized Context Aware Action for enumerations of shapes.
    /// </summary>
    public interface IShapesContextAwareAction : IContextAwareAction
    {
        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="shapes">Enumeration of shapes on which the Action must be executed.</param>
        /// <param name="links">Enumeration of links on which the Action must be executed.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        bool Execute(IEnumerable<IShape> shapes, IEnumerable<ILink> links);
    }
}