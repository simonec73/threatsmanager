using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by containers of Vulnerabilities.
    /// </summary>
    public interface IVulnerabilitiesContainer
    {
        /// <summary>
        /// Event raised when a Vulnerability is added to the Container.
        /// </summary>
        event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityAdded;

        /// <summary>
        /// Event raised when a Vulnerability is removed from the Container.
        /// </summary>
        event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityRemoved;

        /// <summary>
        /// Enumeration of the Vulnerabilities associated to the Container.
        /// </summary>
        IEnumerable<IVulnerability> Vulnerabilities { get; }
        
        /// <summary>
        /// Get a Vulnerability from the container, given its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Vulnerability.</param>
        /// <returns>Searched Vulnerability, if found, otherwise null.</returns>
        IVulnerability GetVulnerability(Guid id);

        /// <summary>
        /// Get a Vulnerability from the container, given the identifier of the corresponding Weakness.
        /// </summary>
        /// <param name="weaknessId">Identifier of the Weakness associated to the Vulnerability.</param>
        /// <returns>Searched Vulnerability, if found, otherwise null.</returns>
        IVulnerability GetVulnerabilityByWeakness(Guid weaknessId);

        /// <summary>
        /// Adds the Vulnerability passed as argument to the container.
        /// </summary>
        /// <param name="vulnerability">Vulnerability to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IVulnerability vulnerability);

        /// <summary>
        /// Creates a new Vulnerability from a Weakness passed as argument.
        /// </summary>
        /// <param name="weakness">Source Weakness.</param>
        /// <returns>The new Vulnerability if there is no event of the same Weakness, otherwise null.</returns>
        IVulnerability AddVulnerability(IWeakness weakness);

        /// <summary>
        /// Delete an existing Vulnerability.
        /// </summary>
        /// <param name="id">Identifier of the Vulnerability.</param>
        /// <returns>True if the Vulnerability has been deleted successfully, otherwise false.</returns>
        bool RemoveVulnerability(Guid id);
    }
}