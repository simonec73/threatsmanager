using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Specialized Context Aware Action for objects implementing interface IThreatEventMitigation.
    /// </summary>
    public interface IThreatEventMitigationContextAwareAction : IContextAwareAction
    {
        /// <summary>
        /// Action execution.
        /// </summary>
        /// <param name="mitigation">Object on which the Action must be executed.</param>
        /// <returns>True if the action succeeded, false otherwise.</returns>
        bool Execute(IThreatEventMitigation mitigation);
    }
}