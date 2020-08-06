using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the Diagrams.
    /// </summary>
    public interface IDiagram : IIdentity, IThreatModelChild, IPropertiesContainer, 
        IEntityShapesContainer, IGroupShapesContainer, ILinksContainer, IDirty
    {
        /// <summary>
        /// Order of the diagram.
        /// </summary>
        /// <remarks>The lowest values indicate higher priority in the sequence.</remarks>
        int Order { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Diagram and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Diagram.</returns>
        IDiagram Clone(IDiagramsContainer container);
    }
}