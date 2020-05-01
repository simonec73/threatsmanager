using System;
using System.Collections.Generic;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the containers of shapes.
    /// </summary>
    public interface IEntityShapesContainer
    {
        /// <summary>
        /// Event raised when an Entity Shape is added to the Container.
        /// </summary>
        event Action<IEntityShapesContainer, IEntityShape> EntityShapeAdded;

        /// <summary>
        /// Event raised when an Entity Shape is removed from the Container.
        /// </summary>
        event Action<IEntityShapesContainer, IEntity> EntityShapeRemoved;

        /// <summary>
        /// Enumeration of Shapes associated to Entities.
        /// </summary>
        IEnumerable<IEntityShape> Entities { get; }

        /// <summary>
        /// Get a shape associated to an Entity.
        /// </summary>
        /// <param name="entity">Entity whose shape is to be found.</param>
        /// <returns>Shape associated to the entity if found, null otherwise.</returns>
        IEntityShape GetShape(IEntity entity);
 
        /// <summary>
        /// Get a shape associated to an Entity.
        /// </summary>
        /// <param name="entityId">Identifier of the entity whose shape is to be found.</param>
        /// <returns>Shape associated to the entity if found, null otherwise.</returns>
        IEntityShape GetEntityShape(Guid entityId);

        /// <summary>
        /// Adds the Entity Shape passed as argument to the container.
        /// </summary>
        /// <param name="entityShape">Entity Shape to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IEntityShape entityShape);

        /// <summary>
        /// Adds a Shape associated to an entity.
        /// </summary>
        /// <param name="entity">Entity to be associated to the new shape.</param>
        /// <param name="position">Position of the shape.</param>
        /// <returns>New shape.</returns>
        IEntityShape AddShape(IEntity entity, PointF position);

        /// <summary>
        /// Adds a Shape associated to an entity.
        /// </summary>
        /// <param name="entityId">Identifier of the entity to be associated to the new shape.</param>
        /// <param name="position">Position of the shape.</param>
        /// <returns>New shape.</returns>
        IEntityShape AddEntityShape(Guid entityId, PointF position);

        /// <summary>
        /// Removes a Shape associated to an entity.
        /// </summary>
        /// <param name="entityId">Identifier of the Entity to be removed.</param>
        /// <returns>True if the Shape has been successfully removed, otherwise false.</returns>
        bool RemoveEntityShape(Guid entityId);

        /// <summary>
        /// Removes a Shape associated to an entity.
        /// </summary>
        /// <param name="entity">Entity to be removed.</param>
        /// <returns>True if the Shape has been successfully removed, otherwise false.</returns>
        bool RemoveShape(IEntity entity);

        /// <summary>
        /// Removes a Shape associated to an entity.
        /// </summary>
        /// <param name="entityShape">Shape associated to the Entity to be removed.</param>
        /// <returns>True if the Shape has been successfully removed, otherwise false.</returns>
        bool RemoveShape(IEntityShape entityShape);
    }
}