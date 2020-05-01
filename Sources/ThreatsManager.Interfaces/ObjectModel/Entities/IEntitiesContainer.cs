using System;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by Container of Entities
    /// </summary>
    public interface IEntitiesContainer : IEntitiesReadOnlyContainer
    {
        /// <summary>
        /// Event raised when a Threat Event is associated to an Entity.
        /// </summary>
        event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAddedToEntity;

        /// <summary>
        /// Event raised when a Threat Event is removed from an Entity.
        /// </summary>
        event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemovedFromEntity;
        
        /// <summary>
        /// Adds the Entity passed as argument to the container.
        /// </summary>
        /// <param name="entity">Entity to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IEntity entity);

        /// <summary>
        /// Adds an entity to the container, assigning the name automatically.
        /// </summary>
        /// <typeparam name="T">Type of the entity to be added. It is the interface derived from IEntity.</typeparam>
        /// <returns>New entity.</returns>
        IEntity AddEntity<T>() where T : IEntity;

        /// <summary>
        /// Adds an entity to the container.
        /// </summary>
        /// <typeparam name="T">Type of the entity to be added. It is the interface derived from IEntity.</typeparam>
        /// <param name="name">Name of the Entity</param>
        /// <returns>New entity.</returns>
        IEntity AddEntity<T>(string name) where T : IEntity;

        /// <summary>
        /// Adds an entity to the container and associating it to the Entity Template passed as argument.
        /// </summary>
        /// <typeparam name="T">Type of the entity to be added. It is the interface derived from IEntity.</typeparam>
        /// <param name="name">Name of the Entity</param>
        /// <param name="template">Template to associate to the Entity.</param>
        /// <returns>New entity.</returns>
        IEntity AddEntity<T>(string name, IEntityTemplate template) where T : IEntity;

        /// <summary>
        /// Removes an entity from the container.
        /// </summary>
        /// <param name="id">Identifier of the entity.</param>
        /// <returns>True if the entity has been removed, otherwise false.</returns>
        /// <remarks>It also removes the Data Flows and the Entity Shapes associated to the Entity.</remarks>
        bool RemoveEntity(Guid id);
    }
}