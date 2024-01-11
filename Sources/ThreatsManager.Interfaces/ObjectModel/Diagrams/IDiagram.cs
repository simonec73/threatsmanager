using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the Diagrams.
    /// </summary>
    public interface IDiagram : IIdentity, IThreatModelChild, IPropertiesContainer, 
        IEntityShapesContainer, IGroupShapesContainer, ILinksContainer, ISourceInfo
    {
        /// <summary>
        /// Order of the diagram.
        /// </summary>
        /// <remarks>The lowest values indicate higher priority in the sequence.</remarks>
        int Order { get; set; }

        /// <summary>
        /// Dpi used for the diagram, expressed as percentage.
        /// </summary>
        /// <remarks>If the screen is 96dpi (or 100%), Dpi equals 100.<para/>
        /// If the screen is instead 192dpi (or 200%), Dpi equals 200.<para/>
        /// If it is missing, the default DPI for the system is used, which could be the value stored in a property for legacy threat models.</remarks>
        int? Dpi { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Diagram and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Diagram.</returns>
        IDiagram Clone(IDiagramsContainer container);
    }
}