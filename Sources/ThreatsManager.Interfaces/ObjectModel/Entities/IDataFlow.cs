using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by Data Flows.
    /// </summary>
    public interface IDataFlow : IIdentity, IThreatModelChild, IPropertiesContainer, 
        IThreatEventsContainer//, ILockable
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
        /// Type of Data Flow.
        /// </summary>
        FlowType FlowType { get; set; }
 
        /// <summary>
        /// Creates a duplicate of the current Data Flow and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Data Flow.</returns>
        IDataFlow Clone(IDataFlowsContainer container);
    }
}