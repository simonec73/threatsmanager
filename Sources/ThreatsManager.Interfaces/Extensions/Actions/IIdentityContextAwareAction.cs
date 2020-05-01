using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Specialized Context Aware Action for objects implementing interface IIdentity.
    /// </summary>
    public interface IIdentityContextAwareAction : IContextAwareAction
    {
        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="identity">Object on which the Action must be executed.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        bool Execute(IIdentity identity);
    }
}