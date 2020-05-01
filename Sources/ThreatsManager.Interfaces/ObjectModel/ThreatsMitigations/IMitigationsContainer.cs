using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by the containers of Mitigations.
    /// </summary>
    public interface IMitigationsContainer
    {
        /// <summary>
        /// Enumeration of Mitigations.
        /// </summary>
        IEnumerable<IMitigation> Mitigations { get; }

        /// <summary>
        /// Gets a specific mitigation.
        /// </summary>
        /// <param name="id">Identifier of the mitigation.</param>
        /// <returns>Instance of the mitigation, if found.</returns>
        IMitigation GetMitigation(Guid id);

        /// <summary>
        /// Obtains a list of mitigations by name.
        /// </summary>
        /// <param name="name">Name of the mitigation.</param>
        IEnumerable<IMitigation> GetMitigations(string name);

        /// <summary>
        /// Adds the Mitigation passed as argument to the container.
        /// </summary>
        /// <param name="mitigation">Mitigation to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IMitigation mitigation);

        /// <summary>
        /// Adds a mitigation to the container.
        /// </summary>
        /// <param name="name">Name of the mitigation. If null, the name is assigned automatically.</param>
        /// <returns>New mitigation.</returns>
        IMitigation AddMitigation(string name = null);

        /// <summary>
        /// Removes a mitigation from the container.
        /// </summary>
        /// <param name="id">Identifier of the mitigation.</param>
        /// <param name="force">Forces the removal of the Mitigation even if it is in use.</param>
        /// <returns>True if the mitigation has been removed, otherwise false.</returns>
        /// <remarks>It removes the Mitigation, if it has not been already used. Removal could be forced, by setting the flag argument.</remarks>
        bool RemoveMitigation(Guid id, bool force = false);
    }
}