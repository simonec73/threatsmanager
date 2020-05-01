using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface defining a containers of Threat Types.
    /// </summary>
    public interface IThreatTypesContainer
    {
        /// <summary>
        /// Enumeration of the defined Threat Types.
        /// </summary>
        IEnumerable<IThreatType> ThreatTypes { get; }

        /// <summary>
        /// Search the Threat Types using a filter.
        /// </summary>
        /// <param name="filter">Filter to be applied.</param>
        /// <returns>Enumeration of Threat Types based on the filter.</returns>
        /// <remarks>The enumeration is ordered by relevancy.</remarks>
        IEnumerable<IThreatType> SearchThreatTypes(string filter);

        /// <summary>
        /// Get a Threat Type from the container, given its Id.
        /// </summary>
        /// <param name="id">Identifier of the Threat Type.</param>
        /// <returns>Searched Threat Type, if found, otherwise null.</returns>
        IThreatType GetThreatType(Guid id);

        /// <summary>
        /// Adds the Threat Type passed as argument to the container.
        /// </summary>
        /// <param name="threatType">Threat Type to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IThreatType threatType);

        /// <summary>
        /// Creates a new Threat Type.
        /// </summary>
        /// <param name="name">Name of the Threat Type.</param>
        /// <param name="severity">Severity of the new Threat Type.</param>
        /// <returns>The new Threat Type.</returns>
        IThreatType AddThreatType(string name, ISeverity severity);

        /// <summary>
        /// Delete an existing Threat Type.
        /// </summary>
        /// <param name="id">Identifier of the Threat Type.</param>
        /// <param name="force">If set, the removal is forced even if the Threat Type is used. It is false by default</param>
        /// <returns>True if the Threat Type has been deleted successfully, otherwise false.</returns>
        bool RemoveThreatType(Guid id, bool force = false);
    }
}