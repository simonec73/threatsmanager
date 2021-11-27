using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Specialized Context Aware Action for objects implementing interface IWeaknessMitigation.
    /// </summary>
    public interface IWeaknessMitigationContextAwareAction : IContextAwareAction
    {
        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="mitigation">Object on which the Action must be executed.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        bool Execute(IWeaknessMitigation mitigation);
    }
}