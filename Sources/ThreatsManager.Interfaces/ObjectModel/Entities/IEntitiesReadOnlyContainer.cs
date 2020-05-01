using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Read-only container for Entities.
    /// </summary>
    public interface IEntitiesReadOnlyContainer
    {
        /// <summary>
        /// Enumerate the Entities.
        /// </summary>
        IEnumerable<IEntity> Entities { get; }

        /// <summary>
        /// Gets a specific entity.
        /// </summary>
        /// <param name="id">Identifier of the entity.</param>
        /// <returns>Instance of the entity, if found.</returns>
        IEntity GetEntity(Guid id);

        /// <summary>
        /// Obtains a list of entities by name.
        /// </summary>
        /// <param name="name">Name of the entity.</param>
        /// <returns>Enumeration of the entities found. It may be null if the search returns no entities.</returns>
        IEnumerable<IEntity> GetEntities(string name);

        /// <summary>
        /// Search entities using a filter.
        /// </summary>
        /// <param name="filter">Filter for the name. It is a Regular Expression.</param>
        /// <returns>Enumeration of the entities found. It may be null if the search returns no entities.</returns>
        /// <remarks>The search is case-insensitive.</remarks>
        IEnumerable<IEntity> SearchEntities(string filter);
    }
}