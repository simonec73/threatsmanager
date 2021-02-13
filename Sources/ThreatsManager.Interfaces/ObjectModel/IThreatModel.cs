using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface which represents a Threat Model.
    /// </summary>
    public interface IThreatModel : IIdentity, IPropertiesContainer, IPropertyFinder, 
        IEntitiesContainer, IGroupsContainer, IDataFlowsContainer, 
        IPropertySchemasContainer, IDiagramsContainer, ISeveritiesContainer, 
        IThreatTypesContainer, IStrengthsContainer, IMitigationsContainer, 
        IThreatActorsContainer, IEntityTemplatesContainer, IFlowTemplatesContainer,
        ITrustBoundaryTemplatesContainer, IThreatEventsContainer, IThreatEventFinder,
        IWeaknessesContainer, IVulnerabilitiesContainer, IVulnerabilityFinder, IDirty
    {
        #region Events.
        /// <summary>
        /// Event raised when an object is created.
        /// </summary>
        /// <returns>The object which has been created as IIdentity.</returns>
        /// <seealso cref="IIdentity"/>
        event Action<IIdentity> ChildCreated;

        /// <summary>
        /// Event raised when an object is removed.
        /// </summary>
        /// <returns>The object which has been removed as IIdentity.</returns>
        /// <seealso cref="IIdentity"/>
        event Action<IIdentity> ChildRemoved;

        /// <summary>
        /// Event raised when a property of an object changes.
        /// </summary>
        /// <returns>The object which has changed as IIdentity, and the name of the information which has changed.</returns>
        /// <seealso cref="IIdentity"/>
        event Action<IIdentity, string> ChildChanged;

        /// <summary>
        /// Event raised when a property of an object is added.
        /// </summary>
        /// <returns>The object as IIdentity, the Type of the Property and the Property itself.</returns>
        /// <seealso cref="IIdentity"/>
        /// <seealso cref="IPropertyType"/>
        /// <seealso cref="IProperty"/>
        event Action<IIdentity, IPropertyType, IProperty> ChildPropertyAdded;

        /// <summary>
        /// Event raised when a property of an object is removed.
        /// </summary>
        /// <returns>The object as IIdentity, the Type of the Property and the Property itself.</returns>
        /// <seealso cref="IIdentity"/>
        /// <seealso cref="IPropertyType"/>
        /// <seealso cref="IProperty"/>
        event Action<IIdentity, IPropertyType, IProperty> ChildPropertyRemoved;

        /// <summary>
        /// Event raised when a property of an object is changed.
        /// </summary>
        /// <returns>The object as IIdentity, the Type of the Property and the Property itself.</returns>
        /// <seealso cref="IIdentity"/>
        /// <seealso cref="IPropertyType"/>
        /// <seealso cref="IProperty"/>
        event Action<IIdentity, IPropertyType, IProperty> ChildPropertyChanged;

        /// <summary>
        /// Event raised when a contributor is added.
        /// </summary>
        event Action<string> ContributorAdded;

        /// <summary>
        /// Event raised when a contributor is removed.
        /// </summary>
        event Action<string> ContributorRemoved;

        /// <summary>
        /// Event raised when a contributor is changed.
        /// </summary>
        event Action<string, string> ContributorChanged;

        /// <summary>
        /// Event raised when an assumption is added.
        /// </summary>
        event Action<string> AssumptionAdded;

        /// <summary>
        /// Event raised when an assumption is removed.
        /// </summary>
        event Action<string> AssumptionRemoved;

        /// <summary>
        /// Event raised when an assumption is changed.
        /// </summary>
        event Action<string, string> AssumptionChanged;

        /// <summary>
        /// Event raised when a dependency is added.
        /// </summary>
        event Action<string> DependencyAdded;

        /// <summary>
        /// Event raised when a dependency is removed.
        /// </summary>
        event Action<string> DependencyRemoved;

        /// <summary>
        /// Event raised when a dependency is changed.
        /// </summary>
        event Action<string, string> DependencyChanged;
        #endregion

        #region Advanced functions to handle Identities.
        /// <summary>
        /// Returns the standard display Type Name for the Identity.
        /// </summary>
        /// <param name="identity">Identity to be evaluated.</param>
        /// <returns>Display Type name for the Identity.</returns>
        string GetIdentityTypeName(IIdentity identity);

        /// <summary>
        /// Returns the standard initial for the Identity.
        /// </summary>
        /// <param name="identity">Identity to be evaluated.</param>
        /// <returns>Initial for the Identity.</returns>
        string GetIdentityTypeInitial(IIdentity identity);

        /// <summary>
        /// Returns the Identity registered within the Threat Model having the given Id.
        /// </summary>
        /// <param name="id">Identifier of the searched Identity.</param>
        /// <returns>Searched Identity if found, otherwise null.</returns>
        IIdentity GetIdentity(Guid id);
        #endregion

        #region Duplication and Merge.
        /// <summary>
        /// Duplicates the Threat Model.
        /// </summary>
        /// <param name="name">Name for the new Threat Model.</param>
        /// <param name="def">Definition of what needs to be included in the duplication.</param>
        /// <returns>The duplicated Threat Model.</returns>
        /// <exception cref="ThreatsManager.Interfaces.Exceptions.InvalidDuplicationDefinitionException">The Duplication Definition is invalid.</exception>
        IThreatModel Duplicate(string name, DuplicationDefinition def);

        /// <summary>
        /// Merge the Threat Model with the <paramref name="source"/> passed as argument,
        /// selecting only the specific items specified in <paramref name="def"/>.
        /// </summary>
        /// <param name="source">Existing Threat Model to be merged into the current one.</param>
        /// <param name="def">Definition of what needs to be included in the merge.</param>
        /// <returns>True if the merge has been performed successfully, otherwise false.</returns>
        /// <remarks>The existing Threat Model is enriched by merging the Threat Model passed as argument.</remarks>
        bool Merge(IThreatModel source, DuplicationDefinition def);
        #endregion

        #region Owner.
        /// <summary>
        /// The owner of the Threat Model.
        /// </summary>
        string Owner { get; set; }
        #endregion

        #region Contributors.
        /// <summary>
        /// The list of the contributors to the Threat Model.
        /// </summary>
        IEnumerable<string> Contributors { get; }

        /// <summary>
        /// Add a contributor to the list.
        /// </summary>
        /// <param name="name">Name of the contributor.</param>
        /// <returns>True if the contributor has been added, otherwise false.</returns>
        /// <remarks>A contributor may not be added, if the name is already in the list.</remarks>
        bool AddContributor(string name);

        /// <summary>
        /// Remove a contributor from the list.
        /// </summary>
        /// <param name="name">Name of the contributor.</param>
        /// <returns>True if the contributor has been successfully removed.</returns>
        /// <remarks>If the contributor is missing from the list, then it returns false.</remarks>
        bool RemoveContributor(string name);

        /// <summary>
        /// Change a contributor in the list.
        /// </summary>
        /// <param name="oldName">Old name of the contributor.</param>
        /// <param name="newName">New name of the contributor.</param>
        /// <returns>True if the contributor has been successfully updated.</returns>
        /// <remarks>If the contributor is missing from the list, then it returns false.</remarks>
        bool ChangeContributor(string oldName, string newName);
        #endregion

        #region Assumptions.
        /// <summary>
        /// The list of the assumptions.
        /// </summary>
        IEnumerable<string> Assumptions { get; }

        /// <summary>
        /// Add an assumption to the list.
        /// </summary>
        /// <param name="text">Description of the assumption.</param>
        /// <returns>True if the assumption has been added, otherwise false.</returns>
        /// <remarks>An assumption may not be added, if it is already in the list.</remarks>
        bool AddAssumption(string text);

        /// <summary>
        /// Remove an assumption from the list.
        /// </summary>
        /// <param name="text">Description of the assumption.</param>
        /// <returns>True if the assumption has been successfully removed.</returns>
        /// <remarks>If the assumption is missing from the list, then it returns false.</remarks>
        bool RemoveAssumption(string text);

        /// <summary>
        /// Change an assumption in the list.
        /// </summary>
        /// <param name="oldText">Old description of the assumption.</param>
        /// <param name="newText">New description of the assumption.</param>
        /// <returns>True if the assumption has been successfully changed.</returns>
        /// <remarks>If the assumption is missing from the list, then it returns false.</remarks>
        bool ChangeAssumption(string oldText, string newText);
        #endregion

        #region Dependencies.
        /// <summary>
        /// The list of external dependencies.
        /// </summary>
        IEnumerable<string> ExternalDependencies { get; }

        /// <summary>
        /// Add a dependency to the list.
        /// </summary>
        /// <param name="text">Description of the dependency.</param>
        /// <returns>True if the dependency has been added, otherwise false.</returns>
        /// <remarks>A dependency may not be added, if it is already in the list.</remarks>
        bool AddDependency(string text);

        /// <summary>
        /// Remove a dependency from the list.
        /// </summary>
        /// <param name="text">Description of the dependency.</param>
        /// <returns>True if the dependency has been successfully removed.</returns>
        /// <remarks>If the dependency is missing from the list, then it returns false.</remarks>
        bool RemoveDependency(string text);

        /// <summary>
        /// Change a dependency in the list.
        /// </summary>
        /// <param name="oldText">Old description of the dependency.</param>
        /// <param name="newText">New description of the dependency.</param>
        /// <returns>True if the dependency has been successfully change.</returns>
        /// <remarks>If the dependency is missing from the list, then it returns false.</remarks>
        bool ChangeDependency(string oldText, string newText);
        #endregion

        #region Advanced Threats & Mitigations properties and functions.
        /// <summary>
        /// Counter of the Threat Types that are associated to at least a Threat Event in the Threat Model.
        /// </summary>
        int AssignedThreatTypes { get; }

        /// <summary>
        /// Counter of the number of Threat Types that are fully Mitigated.
        /// </summary>
        int FullyMitigatedThreatTypes { get; }

        /// <summary>
        /// Counter of the number of Threat Types that are partially Mitigated.
        /// </summary>
        int PartiallyMitigatedThreatTypes { get; }

        /// <summary>
        /// Counter of the number of Threat Types that have not been Mitigated.
        /// </summary>
        int NotMitigatedThreatTypes { get; }

        /// <summary>
        /// Counter of the total number of Threat Events defined in the Threat Model.
        /// </summary>
        int TotalThreatEvents { get; }

        /// <summary>
        /// Counter of the unique assigned mitigations in the Threat Model.
        /// </summary>
        /// <remarks>It includes the Mitigations associated to Threat Events and Vulnerabilities.</remarks>
        int UniqueMitigations { get; }

        /// <summary>
        /// Counter of the number of Threat Events that are fully Mitigated.
        /// </summary>
        int FullyMitigatedThreatEvents { get; }

        /// <summary>
        /// Counter of the number of Threat Events that are partially Mitigated.
        /// </summary>
        int PartiallyMitigatedThreatEvents { get; }

        /// <summary>
        /// Counter of the number of Threat Events that have not been Mitigated.
        /// </summary>
        int NotMitigatedThreatEvents { get; }

        /// <summary>
        /// Count the Threat Events by a specific severity.
        /// </summary>
        /// <param name="severity">Reference severity.</param>
        /// <returns>Number of Threat Events with the specified severity.</returns>
        int CountThreatEvents(ISeverity severity);

        /// <summary>
        /// Count the Threat Events by a specific severity.
        /// </summary>
        /// <param name="severityId">Identified of the reference severity.</param>
        /// <returns>Number of Threat Events with the specified severity.</returns>
        int CountThreatEvents(int severityId);

        /// <summary>
        /// Count the Threat Types associated to Threat Events with a specified maximum severity.
        /// </summary>
        /// <param name="severity">Reference severity.</param>
        /// <returns>Number of Threat Types with the specified maximum severity.</returns>
        int CountThreatEventsByType(ISeverity severity);

        /// <summary>
        /// Count the Threat Types associated to Threat Events with a specified maximum severity.
        /// </summary>
        /// <param name="severityId">Identified of the reference severity.</param>
        /// <returns>Number of Threat Types with the specified maximum severity.</returns>
        int CountThreatEventsByType(int severityId);

        /// <summary>
        /// Count the Mitigations having a specific status.
        /// </summary>
        /// <param name="status">Status.</param>
        /// <returns>Number of Mitigations with the specified status.</returns>
        /// <remarks>The result is related to the single mitigations, applied to Threat Events or Vulnerabilities.
        /// This means that summing up all the values obtained applying the various states,
        /// the resulting value would generally be higher than what returned by <see cref="UniqueMitigations"/>.</remarks>
        int CountMitigationsByStatus(MitigationStatus status);

        /// <summary>
        /// Get the Threat Events, inspecting the whole Threat Model.
        /// </summary>
        /// <returns>Enumeration of the Threat Events defined within the Threat Model.</returns>
        IEnumerable<IThreatEvent> GetThreatEvents();

        /// <summary>
        /// Get the Threat Events that are associated to a specific Threat Type, inspecting the whole Threat Model.
        /// </summary>
        /// <param name="threatType">Reference Threat Type.</param>
        /// <returns>Enumeration of the Threat Events associated to the given Threat Type.</returns>
        IEnumerable<IThreatEvent> GetThreatEvents(IThreatType threatType);

        /// <summary>
        /// Get the list of unique assigned mitigations in the Threat Model.
        /// </summary>
        /// <returns>Enumeration of the Mitigations adopted within the Threat Model,
        /// because associated to at least to a Threat Event or Vulnerability.</returns>
        IEnumerable<IMitigation> GetUniqueMitigations();

        /// <summary>
        /// Get the Threat Type Mitigations, inspecting the whole Threat Model.
        /// </summary>
        /// <returns>Enumeration of the Threat Type Mitigations defined within the Threat Model.</returns>
        IEnumerable<IThreatTypeMitigation> GetThreatTypeMitigations();

        /// <summary>
        /// Get all the Threat Type Mitigations associated to a given Mitigation, inspecting the whole Threat Model.
        /// </summary>
        /// <param name="mitigation">Reference mitigation.</param>
        /// <returns>Enumeration of the associations found.</returns>
        IEnumerable<IThreatTypeMitigation> GetThreatTypeMitigations(IMitigation mitigation);

        /// <summary>
        /// Get the Threat Event Mitigations, inspecting the whole Threat Model.
        /// </summary>
        /// <returns>Enumeration of the Threat Event Mitigations defined within the Threat Model.</returns>
        IEnumerable<IThreatEventMitigation> GetThreatEventMitigations();

        /// <summary>
        /// Get all the Threat Event Mitigations associated to a given Mitigation, inspecting the whole Threat Model.
        /// </summary>
        /// <param name="mitigation">Reference mitigation.</param>
        /// <returns>Enumeration of the associations found.</returns>
        IEnumerable<IThreatEventMitigation> GetThreatEventMitigations(IMitigation mitigation);
        #endregion

        #region Advanced Weaknesses & Mitigations properties and functions.
        /// <summary>
        /// Counter of the Weaknesses that are associated to at least a Vulnerability in the Threat Model.
        /// </summary>
        int AssignedWeaknesses { get; }

        /// <summary>
        /// Counter of the number of Weaknesses that are fully Mitigated.
        /// </summary>
        int FullyMitigatedWeaknesses { get; }

        /// <summary>
        /// Counter of the number of Weaknesses that are partially Mitigated.
        /// </summary>
        int PartiallyMitigatedWeaknesses { get; }

        /// <summary>
        /// Counter of the number of Weaknesses that have not been Mitigated.
        /// </summary>
        int NotMitigatedWeaknesses { get; }

        /// <summary>
        /// Counter of the total number of Vulnerabilities defined in the Threat Model.
        /// </summary>
        int TotalVulnerabilities { get; }

        /// <summary>
        /// Counter of the number of Vulnerabilities that are fully Mitigated.
        /// </summary>
        int FullyMitigatedVulnerabilities { get; }

        /// <summary>
        /// Counter of the number of Vulnerabilities that are partially Mitigated.
        /// </summary>
        int PartiallyMitigatedVulnerabilities { get; }

        /// <summary>
        /// Counter of the number of Vulnerabilities that have not been Mitigated.
        /// </summary>
        int NotMitigatedVulnerabilities { get; }

        /// <summary>
        /// Count the Vulnerabilities by a specific severity.
        /// </summary>
        /// <param name="severity">Reference severity.</param>
        /// <returns>Number of Vulnerabilities with the specified severity.</returns>
        int CountVulnerabilities(ISeverity severity);

        /// <summary>
        /// Count the Vulnerabilities by a specific severity.
        /// </summary>
        /// <param name="severityId">Identified of the reference severity.</param>
        /// <returns>Number of Vulnerabilities with the specified severity.</returns>
        int CountVulnerabilities(int severityId);

        /// <summary>
        /// Count the Weaknesses associated to Vulnerabilities with a specified maximum severity.
        /// </summary>
        /// <param name="severity">Reference severity.</param>
        /// <returns>Number of Weaknesses with the specified maximum severity.</returns>
        int CountVulnerabilitiesByType(ISeverity severity);

        /// <summary>
        /// Count the Weaknesses associated to Vulnerabilities with a specified maximum severity.
        /// </summary>
        /// <param name="severityId">Identified of the reference severity.</param>
        /// <returns>Number of Weaknesses with the specified maximum severity.</returns>
        int CountVulnerabilitiesByType(int severityId);

        /// <summary>
        /// Get the Vulnerabilities, inspecting the whole Threat Model.
        /// </summary>
        /// <returns>Enumeration of the Vulnerabilities defined within the Threat Model.</returns>
        IEnumerable<IVulnerability> GetVulnerabilities();

        /// <summary>
        /// Get the Vulnerabilities that are associated to a specific Weakness, inspecting the whole Threat Model.
        /// </summary>
        /// <param name="weakness">Reference Weakness.</param>
        /// <returns>Enumeration of the Vulnerabilities associated to the given Weakness.</returns>
        IEnumerable<IVulnerability> GetVulnerabilities(IWeakness weakness);

        /// <summary>
        /// Get the Vulnerability Mitigations, inspecting the whole Threat Model.
        /// </summary>
        /// <returns>Enumeration of the Vulnerability Mitigations defined within the Threat Model.</returns>
        IEnumerable<IVulnerabilityMitigation> GetVulnerabilityMitigations();

        /// <summary>
        /// Get all the Vulnerability Mitigations associated to a given Mitigation, inspecting the whole Threat Model.
        /// </summary>
        /// <param name="mitigation">Reference mitigation.</param>
        /// <returns>Enumeration of the associations found.</returns>
        IEnumerable<IVulnerabilityMitigation> GetVulnerabilityMitigations(IMitigation mitigation);
        #endregion
    }
}
