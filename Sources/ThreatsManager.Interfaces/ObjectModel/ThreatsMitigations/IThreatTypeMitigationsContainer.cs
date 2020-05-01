using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by the containers of associations between Threat Types and Mitigations.
    /// </summary>
    public interface IThreatTypeMitigationsContainer
    {
        /// <summary>
        /// Event raised when a Threat Type Mitigation is added to the Container.
        /// </summary>
        event Action<IThreatTypeMitigationsContainer, IThreatTypeMitigation> ThreatTypeMitigationAdded;

        /// <summary>
        /// Event raised when a Threat Type Mitigation is removed from the Container.
        /// </summary>
        event Action<IThreatTypeMitigationsContainer, IThreatTypeMitigation> ThreatTypeMitigationRemoved;

        /// <summary>
        /// Enumeration of the associated mitigations.
        /// </summary>
        IEnumerable<IThreatTypeMitigation> Mitigations { get; }

        /// <summary>
        /// Get a Threat Type Mitigation association identified by the identifier of the Mitigation.
        /// </summary>
        /// <param name="mitigationId">Identifier of the Mitigation.</param>
        /// <returns>Threat Type Mitigation association if found, otherwise null.</returns>
        IThreatTypeMitigation GetMitigation(Guid mitigationId);

        /// <summary>
        /// Adds the Mitigation passed as argument to the container.
        /// </summary>
        /// <param name="mitigation">Mitigation to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IThreatTypeMitigation mitigation);

        /// <summary>
        /// Creates a Threat Type Mitigation association.
        /// </summary>
        /// <param name="mitigation">Mitigation to be associated to the Threat Type.</param>
        /// <param name="strength">Strength of the Mitigation.</param>
        /// <returns>New instance of Threat Type Mitigation association if the Mitigation is not already associated, otherwise null.</returns>
        IThreatTypeMitigation AddMitigation(IMitigation mitigation, IStrength strength);

        /// <summary>
        /// Removes the association with a given Mitigation.
        /// </summary>
        /// <param name="mitigationId">Identifier of the Mitigation to be de-associated.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        bool RemoveMitigation(Guid mitigationId);
    }
}