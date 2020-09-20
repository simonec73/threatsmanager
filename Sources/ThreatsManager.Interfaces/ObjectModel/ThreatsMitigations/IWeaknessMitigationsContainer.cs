using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by the containers of associations between Weaknesses and Mitigations.
    /// </summary>
    public interface IWeaknessMitigationsContainer
    {
        /// <summary>
        /// Event raised when a Weakness Mitigation is added to the Container.
        /// </summary>
        event Action<IWeaknessMitigationsContainer, IWeaknessMitigation> WeaknessMitigationAdded;

        /// <summary>
        /// Event raised when a Weakness Mitigation is removed from the Container.
        /// </summary>
        event Action<IWeaknessMitigationsContainer, IWeaknessMitigation> WeaknessMitigationRemoved;

        /// <summary>
        /// Enumeration of the associated mitigations.
        /// </summary>
        IEnumerable<IWeaknessMitigation> Mitigations { get; }

        /// <summary>
        /// Get a Weakness Mitigation association identified by the identifier of the Mitigation.
        /// </summary>
        /// <param name="mitigationId">Identifier of the Mitigation.</param>
        /// <returns>Weakness Mitigation association if found, otherwise null.</returns>
        IWeaknessMitigation GetMitigation(Guid mitigationId);

        /// <summary>
        /// Adds the Mitigation passed as argument to the container.
        /// </summary>
        /// <param name="mitigation">Mitigation to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IWeaknessMitigation mitigation);

        /// <summary>
        /// Creates a Weakness Mitigation association.
        /// </summary>
        /// <param name="mitigation">Mitigation to be associated to the Weakness.</param>
        /// <param name="strength">Strength of the Mitigation.</param>
        /// <returns>New instance of Weakness Mitigation association if the Mitigation is not already associated, otherwise null.</returns>
        IWeaknessMitigation AddMitigation(IMitigation mitigation, IStrength strength);

        /// <summary>
        /// Removes the association with a given Mitigation.
        /// </summary>
        /// <param name="mitigationId">Identifier of the Mitigation to be de-associated.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        bool RemoveMitigation(Guid mitigationId);
    }
}