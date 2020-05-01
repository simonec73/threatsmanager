using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by the containers of associations between Threat Events and Mitigations.
    /// </summary>
    public interface IThreatEventMitigationsContainer
    {
        /// <summary>
        /// Event raised when a Threat Event Mitigation is added to the Container.
        /// </summary>
        event Action<IThreatEventMitigationsContainer, IThreatEventMitigation> ThreatEventMitigationAdded;

        /// <summary>
        /// Event raised when a Threat Event Mitigation is removed from the Container.
        /// </summary>
        event Action<IThreatEventMitigationsContainer, IThreatEventMitigation> ThreatEventMitigationRemoved;

        /// <summary>
        /// Enumeration of the associated mitigations.
        /// </summary>
        IEnumerable<IThreatEventMitigation> Mitigations { get; }

        /// <summary>
        /// Get a Threat Event Mitigation association identified by the identifier of the Mitigation.
        /// </summary>
        /// <param name="mitigationId">Identifier of the Mitigation.</param>
        /// <returns>Threat Event Mitigation association if found, otherwise null.</returns>
        IThreatEventMitigation GetMitigation(Guid mitigationId);
                
        /// <summary>
        /// Adds the Threat Event Mitigation passed as argument to the container.
        /// </summary>
        /// <param name="mitigation">Threat Event Mitigation to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IThreatEventMitigation mitigation);

        /// <summary>
        /// Creates a Threat Event Mitigation association.
        /// </summary>
        /// <param name="mitigation">Mitigation to be associated to the Threat Event.</param>
        /// <param name="strength">Strength of the Mitigation.</param>
        /// <param name="status">Status of the mitigation</param>
        /// <param name="directives">Additional directives for the mitigation, specific to the Threat Event.</param>
        /// <returns>New instance of Threat Event Mitigation association if the Mitigation is not already associated, otherwise null.</returns>
        IThreatEventMitigation AddMitigation(IMitigation mitigation, 
            IStrength strength, MitigationStatus status = MitigationStatus.Proposed, string directives = null);

        /// <summary>
        /// Removes the association with a given Mitigation.
        /// </summary>
        /// <param name="mitigationId">Identifier of the Mitigation to be de-associated.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        /// <remarks>It does not remove the related standard Mitigation, but only the specific Threat Event Mitigation.</remarks>
        bool RemoveMitigation(Guid mitigationId);
    }
}