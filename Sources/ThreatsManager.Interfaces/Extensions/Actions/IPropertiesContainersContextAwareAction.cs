using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Specialized Context Aware Action executed on multiple objects implementing interface IPropertiesContainer.
    /// </summary>
    public interface IPropertiesContainersContextAwareAction : IContextAwareAction
    {
        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="containers">Enumeration of the objects on which the Action must be executed.</param>
        /// <returns>True if the action succeeded for all objects, false otherwise.</returns>
        bool Execute(IEnumerable<IPropertiesContainer> containers);
    }
}