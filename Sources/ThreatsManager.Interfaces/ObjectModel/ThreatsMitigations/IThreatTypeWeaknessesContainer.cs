using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by the containers of associations between Threat Types and Weaknesses.
    /// </summary>
    public interface IThreatTypeWeaknessesContainer
    {
        /// <summary>
        /// Event raised when a Threat Type Weakness is added to the Container.
        /// </summary>
        event Action<IThreatTypeWeaknessesContainer, IThreatTypeWeakness> ThreatTypeWeaknessAdded;

        /// <summary>
        /// Event raised when a Threat Type Weakness is removed from the Container.
        /// </summary>
        event Action<IThreatTypeWeaknessesContainer, IThreatTypeWeakness> ThreatTypeWeaknessRemoved;

        /// <summary>
        /// Enumeration of the associated Weaknesses.
        /// </summary>
        IEnumerable<IThreatTypeWeakness> Weaknesses { get; }

        /// <summary>
        /// Get a Threat Type Weakness association identified by the identifier of the Weakness.
        /// </summary>
        /// <param name="weaknessId">Identifier of the Weakness.</param>
        /// <returns>Threat Type Weakness association if found, otherwise null.</returns>
        IThreatTypeWeakness GetWeakness(Guid weaknessId);

        /// <summary>
        /// Adds the Weakness passed as argument to the container.
        /// </summary>
        /// <param name="weakness">Weakness to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IThreatTypeWeakness weakness);

        /// <summary>
        /// Creates a Threat Type Weakness association.
        /// </summary>
        /// <param name="weakness">Weakness to be associated to the Threat Type.</param>
        /// <returns>New instance of Threat Type Weakness association if the Mitigation is not already associated, otherwise null.</returns>
        IThreatTypeWeakness AddWeakness(IWeakness weakness);

        /// <summary>
        /// Removes the association with a given Weakness.
        /// </summary>
        /// <param name="weaknessId">Identifier of the Weakness to be de-associated.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        bool RemoveWeakness(Guid weaknessId);
    }
}