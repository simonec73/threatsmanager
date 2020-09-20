using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel handling IDiagrams.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    public interface IShowDiagramPanel<T> : IPanel<T>
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