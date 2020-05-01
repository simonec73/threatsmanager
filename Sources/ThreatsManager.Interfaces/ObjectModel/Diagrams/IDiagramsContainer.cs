using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the containers of Diagrams.
    /// </summary>
    public interface IDiagramsContainer
    {
        /// <summary>
        /// Event raised when an Entity Shape is added to a Diagram.
        /// </summary>
        event Action<IEntityShapesContainer, IEntityShape> EntityShapeAdded;

        /// <summary>
        /// Event raised when an Entity Shape is removed from a Diagram.
        /// </summary>
        event Action<IEntityShapesContainer, IEntity> EntityShapeRemoved;

        /// <summary>
        /// Event raised when a Group Shape is added to a Diagram.
        /// </summary>
        event Action<IGroupShapesContainer, IGroupShape> GroupShapeAdded;

        /// <summary>
        /// Event raised when a Group Shape is removed from a Diagram.
        /// </summary>
        event Action<IGroupShapesContainer, IGroup> GroupShapeRemoved;

        /// <summary>
        /// Event raised when a Link is added to a Diagram.
        /// </summary>
        event Action<ILinksContainer, ILink> LinkAdded;

        /// <summary>
        /// Event raised when a Link is removed from a Diagram.
        /// </summary>
        event Action<ILinksContainer, IDataFlow> LinkRemoved;

        /// <summary>
        /// Enumeration of the Diagrams.
        /// </summary>
        IEnumerable<IDiagram> Diagrams { get; }

        /// <summary>
        /// Get an enumeration of Diagrams by name.
        /// </summary>
        /// <param name="name">Name of the Diagram.</param>
        /// <returns>Enumeration of all the Diagrams with the specified name.</returns>
        IEnumerable<IDiagram> GetDiagrams(string name);

        /// <summary>
        /// Get a Diagram by ID.
        /// </summary>
        /// <param name="id">Identifier of the Diagram.</param>
        /// <returns>Object representing the Diagram, if found.</returns>
        IDiagram GetDiagram(Guid id);

        /// <summary>
        /// Adds the Diagram passed as argument to the container.
        /// </summary>
        /// <param name="diagram">Diagram to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IDiagram diagram);

        /// <summary>
        /// Add a Diagram, assigning the name automatically.
        /// </summary>
        /// <returns>New instance of the Diagram.</returns>
        IDiagram AddDiagram();

        /// <summary>
        /// Add a Diagram.
        /// </summary>
        /// <param name="name">Name of the Diagram.</param>
        /// <returns>New instance of the Diagram.</returns>
        /// <remarks>The Name of the Diagram can be changed.</remarks>
        IDiagram AddDiagram(string name);

        /// <summary>
        /// Remove the Diagram whose ID is passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Diagram.</param>
        /// <returns>True if the Diagram has been found and removed, false otherwise.</returns>
        bool RemoveDiagram(Guid id);
    }
}