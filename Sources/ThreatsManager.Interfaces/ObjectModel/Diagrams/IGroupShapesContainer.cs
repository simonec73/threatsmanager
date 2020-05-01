using System;
using System.Collections.Generic;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the containers of shapes.
    /// </summary>
    public interface IGroupShapesContainer
    {
        /// <summary>
        /// Event raised when a Group Shape is added to the Container.
        /// </summary>
        event Action<IGroupShapesContainer, IGroupShape> GroupShapeAdded;

        /// <summary>
        /// Event raised when a Group Shape is removed from the Container.
        /// </summary>
        event Action<IGroupShapesContainer, IGroup> GroupShapeRemoved;

        /// <summary>
        /// Enumeration of Shapes associated to Groups.
        /// </summary>
        IEnumerable<IGroupShape> Groups { get; }

        /// <summary>
        /// Get a shape associated to a Group.
        /// </summary>
        /// <param name="group">Group whose shape is to be found.</param>
        /// <returns>Shape associated to the group if found, null otherwise.</returns>
        IGroupShape GetShape(IGroup group);
 
        /// <summary>
        /// Get a shape associated to a Group.
        /// </summary>
        /// <param name="groupId">Identifier of the group whose shape is to be found.</param>
        /// <returns>Shape associated to the group if found, null otherwise.</returns>
        IGroupShape GetGroupShape(Guid groupId);

        /// <summary>
        /// Adds the Group Shape passed as argument to the container.
        /// </summary>
        /// <param name="groupShape">Group Shape to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IGroupShape groupShape);

        /// <summary>
        /// Adds a Shape associated to a group.
        /// </summary>
        /// <param name="group">Group to be associated to the new shape.</param>
        /// <param name="position">Position of the shape.</param>
        /// <param name="size">Size of the shape.</param>
        /// <returns>New shape.</returns>
        IGroupShape AddShape(IGroup group, PointF position, SizeF size);

        /// <summary>
        /// Adds a Shape associated to a group.
        /// </summary>
        /// <param name="groupId">Identifier of the group to be associated to the new shape.</param>
        /// <param name="position">Position of the shape.</param>
        /// <param name="size">Size of the shape.</param>
        /// <returns>New shape.</returns>
        IGroupShape AddGroupShape(Guid groupId, PointF position, SizeF size);

        /// <summary>
        /// Removes a Shape associated to a group.
        /// </summary>
        /// <param name="groupId">Identifier of the Group to be removed.</param>
        /// <returns>True if the Shape has been successfully removed, otherwise false.</returns>
        bool RemoveGroupShape(Guid groupId);

        /// <summary>
        /// Removes a Shape associated to a group.
        /// </summary>
        /// <param name="group">Group to be removed.</param>
        /// <returns>True if the Shape has been successfully removed, otherwise false.</returns>
        bool RemoveShape(IGroup group);

        /// <summary>
        /// Removes a Shape associated to a group.
        /// </summary>
        /// <param name="groupShape">Shape associated to the Group to be removed.</param>
        /// <returns>True if the Shape has been successfully removed, otherwise false.</returns>
        bool RemoveShape(IGroupShape groupShape);
    }
}