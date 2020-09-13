using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by Data Flows.
    /// </summary>
    public interface IDataFlow : IIdentity, IThreatModelChild, IPropertiesContainer, IVulnerabilitiesContainer, 
        IThreatEventsContainer, IDirty//, ILockable
    {
        /// <summary>
        /// Identifier of the Source.
        /// </summary>
        Guid SourceId { get; }

        /// <summary>
        /// Identifier of the Target.
        /// </summary>
        Guid TargetId { get; }

        /// <summary>
        /// Source entity.
        /// </summary>
        IEntity Source { get; }

        /// <summary>
        /// Target entity.
        /// </summary>
        IEntity Target { get; }

        /// <summary>
        /// Type of Flow.
        /// </summary>
        FlowType FlowType { get; set; }
 
        /// <summary>
        /// Template used to generate the Flow.
        /// </summary>
        /// <remarks>It returns null if there is no known Template which generated the Flow.</remarks>
        IFlowTemplate Template { get; }

        /// <summary>
        /// Disassociate the Flow from the underlying Template.
        /// </summary>
        void ResetTemplate();

        /// <summary>
        /// Creates a duplicate of the current Data Flow and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Data Flow.</returns>
        IDataFlow Clone(IDataFlowsContainer container);
    }
}