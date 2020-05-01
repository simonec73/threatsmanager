using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface implemented by container of Severities.
    /// </summary>
    public interface ISeveritiesContainer
    {
        /// <summary>
        /// Event raised when a Severity is created.
        /// </summary>
        /// <returns>The object which has been created as ISeverity.</returns>
        event Action<ISeverity> SeverityCreated;

        /// <summary>
        /// Event raised when a Severity is removed.
        /// </summary>
        /// <returns>The object which has been removed as ISeverity.</returns>
        event Action<ISeverity> SeverityRemoved;

        /// <summary>
        /// Enumeration of Severities.
        /// </summary>
        IEnumerable<ISeverity> Severities { get; }

        /// <summary>
        /// Get the Severity whose identifier is passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Severity.</param>
        /// <returns>Severity object if found, otherwise null.</returns>
        ISeverity GetSeverity(int id);

        /// <summary>
        /// Get the first Severity having its id equal of greater than the one passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Severity.</param>
        /// <returns>Severity object if found, otherwise null.</returns>
        ISeverity GetMappedSeverity(int id);

        /// <summary>
        /// Adds the Severity passed as argument to the container.
        /// </summary>
        /// <param name="severity">Severity to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(ISeverity severity);

        /// <summary>
        /// Add a custom Severity to the container.
        /// </summary>
        /// <param name="id">Identifier of the Severity.</param>
        /// <param name="name">Name of the Severity.</param>
        /// <returns>New Severity if it is not already included in the system, otherwise null.</returns>
        ISeverity AddSeverity(int id, string name);

        /// <summary>
        /// Add a standard Severity to the container.
        /// </summary>
        /// <param name="severity">Default Severity to be created.</param>
        /// <returns>New Severity if it is not already included in the system, otherwise null.</returns>
        ISeverity AddSeverity(DefaultSeverity severity);

        /// <summary>
        /// Remove an existing Severity from the Container.
        /// </summary>
        /// <param name="id">Identifier of the Severity to be removed.</param>
        /// <returns>True if the Severity has been found and removed, otherwise false.</returns>
        /// <remarks>If the severity is in use by Threat Events or Threat Types, it cannot be removed.</remarks>
        bool RemoveSeverity(int id);

        /// <summary>
        /// Initialization of the Standard Severities.
        /// </summary>
        void InitializeStandardSeverities();
    }
}