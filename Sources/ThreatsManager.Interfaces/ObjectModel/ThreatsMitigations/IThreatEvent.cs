using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by Threat Events, that is Threats that are associated to Entities, Data Flows or the Threat Model as a whole.
    /// </summary>
    public interface IThreatEvent : IIdentity, IThreatModelChild, IPropertiesContainer, 
        IThreatEventScenariosContainer, IThreatEventMitigationsContainer, 
        IThreatEventVulnerabilitiesContainer, IDirty
    {
        /// <summary>
        /// Identifier of the Parent Identity.
        /// </summary>
        /// <remarks>The Parent Identity is the Entity (External Interactor, Process or Data Store) or Data Flow to which the Threat is associated.</remarks>
        Guid ParentId { get; }

        /// <summary>
        /// Parent Identity.
        /// </summary>
        /// <remarks>The Parent Identity is the Entity (External Interactor, Process or Data Store) or Data Flow to which the Threat is associated.</remarks>
        IIdentity Parent { get; }

        /// <summary>
        /// Identifier of the Severity.
        /// </summary>
        int SeverityId { get; }

        /// <summary>
        /// Severity of the Threat Event.
        /// </summary>
        ISeverity Severity { get; set; }

        /// <summary>
        /// Identifier of the Threat Type from which the Threat is derived.
        /// </summary>
        Guid ThreatTypeId { get; }

        /// <summary>
        /// Threat Type from which the Threat is derived.
        /// </summary>
        IThreatType ThreatType { get; }
        
        /// <summary>
        /// Get the Mitigation Level for the Threat.
        /// </summary>
        /// <returns>Mitigation Level, which is obtaining by adding the Strength of all the applied Mitigations and by comparing it to 100, which represents the full mitigation.</returns>
        MitigationLevel GetMitigationLevel();

        /// <summary>
        /// Creates a duplicate of the current Threat Event and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Threat Event.</returns>
        IThreatEvent Clone(IThreatEventsContainer container);
    }
}