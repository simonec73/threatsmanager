using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Specialized Context Aware Action executed on multiple objects implementing interface IIdentity.
    /// </summary>
    public interface IIdentitiesContextAwareAction : IContextAwareAction
    {
        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="identities">Enumeration of the objects on which the Action must be executed.</param>
        /// <returns>True if the action succeeded for all objects, false otherwise.</returns>
        bool Execute(IEnumerable<IIdentity> identities);
    }
}