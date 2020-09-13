using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface defining a containers of Weaknesses.
    /// </summary>
    public interface IWeaknessesContainer
    {
        /// <summary>
        /// Enumeration of the defined Weaknesses.
        /// </summary>
        IEnumerable<IWeakness> Weaknesses { get; }

        /// <summary>
        /// Search the Weaknesses using a filter.
        /// </summary>
        /// <param name="filter">Filter to be applied.</param>
        /// <returns>Enumeration of Weaknesses based on the filter.</returns>
        /// <remarks>The enumeration is ordered by relevancy.</remarks>
        IEnumerable<IWeakness> SearchWeaknesses(string filter);

        /// <summary>
        /// Get a Weakness from the container, given its Id.
        /// </summary>
        /// <param name="id">Identifier of the Weakness.</param>
        /// <returns>Searched Weakness, if found, otherwise null.</returns>
        IWeakness GetWeakness(Guid id);

        /// <summary>
        /// Adds the Weakness passed as argument to the container.
        /// </summary>
        /// <param name="weakness">Weakness to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IWeakness weakness);

        /// <summary>
        /// Creates a new Weakness.
        /// </summary>
        /// <param name="name">Name of the Weakness.</param>
        /// <param name="severity">Severity of the new Weakness.</param>
        /// <returns>The new Weakness.</returns>
        IWeakness AddWeakness(string name, ISeverity severity);

        /// <summary>
        /// Delete an existing Weakness.
        /// </summary>
        /// <param name="id">Identifier of the Weakness.</param>
        /// <param name="force">If set, the removal is forced even if the Weakness is used. It is false by default</param>
        /// <returns>True if the Weakness has been deleted successfully, otherwise false.</returns>
        bool RemoveWeakness(Guid id, bool force = false);
    }
}