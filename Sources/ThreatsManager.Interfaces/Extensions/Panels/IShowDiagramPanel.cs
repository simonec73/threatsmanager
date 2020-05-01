using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling IDiagrams.
    /// </summary>
    public interface IShowDiagramPanel : IPanel
    {
        /// <summary>
        /// Diagram associated to the Panel.
        /// </summary>
        IDiagram Diagram { get; }

        /// <summary>
        /// Method to initialize the Diagram associated to the Panel.
        /// </summary>
        /// <param name="diagram">Diagram to be shown in the Panel.</param>
        void SetDiagram(IDiagram diagram);
    }
}