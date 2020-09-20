using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by the containers of associations between Threat Events and Vulnerabilities.
    /// </summary>
    public interface IThreatEventVulnerabilitiesContainer
    {
        /// <summary>
        /// Event raised when a Threat Event Vulnerability is added to the Container.
        /// </summary>
        event Action<IThreatEventVulnerabilitiesContainer, IThreatEventVulnerability> ThreatEventVulnerabilityAdded;

        /// <summary>
        /// Event raised when a Threat Event Vulnerability is removed from the Container.
        /// </summary>
        event Action<IThreatEventVulnerabilitiesContainer, IThreatEventVulnerability> ThreatEventVulnerabilityRemoved;

        /// <summary>
        /// Enumeration of the associations.
        /// </summary>
        IEnumerable<IThreatEventVulnerability> ThreatEventVulnerabilities { get; }

        /// <summary>
        /// Get a Threat Event Vulnerability association identified by the identifier of the Vulnerability.
        /// </summary>
        /// <param name="vulnerabilityId">Identifier of the Vulnerability.</param>
        /// <returns>Threat Event Vulnerability association if found, otherwise null.</returns>
        IThreatEventVulnerability GetThreatEventVulnerability(Guid vulnerabilityId);

        /// <summary>
        /// Adds the Threat Event Vulnerability passed as argument to the container.
        /// </summary>
        /// <param name="association">Threat Event Vulnerability to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IThreatEventVulnerability association);

        /// <summary>
        /// Creates a Threat Event Vulnerability association.
        /// </summary>
        /// <param name="vulnerability">Vulnerability to be associated to the Threat Event.</param>
        /// <returns>New instance of Threat Event Vulnerability association if it is not already associated, otherwise null.</returns>
        IThreatEventVulnerability AddThreatEventVulnerability(IVulnerability vulnerability);

        /// <summary>
        /// Removes the association with a given Vulnerability.
        /// </summary>
        /// <param name="vulnerabilityId">Identifier of the Vulnerability to be de-associated.</param>
        /// <returns>True if the operation succeeds, otherwise false.</returns>
        /// <remarks>It does not remove the related standard Mitigation, but only the specific Threat Event Mitigation.</remarks>
        bool RemoveThreatEventVulnerability(Guid vulnerabilityId);

        /// <summary>
        /// Get the list of the Effective Mitigations for the Threat Event.
        /// </summary>
        /// <returns>List of effective mitigations, calculated considering the associated Vulnerabilities.</returns>
        /// <remarks>A Threat Event is potentially associated to multiple Vulnerabilities.
        /// As a result of this logical association, the Mitigations associated to the Vulnerabilities impact to the Threat Event.
        /// By design, if the Mitigation is associated to multiple Vulnerabilities, the first one with the highest Strength is considered, without any consideration to its Status.
        /// If the Mitigation is associated to both the Threat Event and to Vulnerability, then the one associated to the Threat Event is considered.</remarks>
        IEnumerable<IThreatEventMitigation> GetEffectiveMitigations();
    }
}