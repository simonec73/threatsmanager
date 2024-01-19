namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the Shapes associated to Entities.
    /// </summary>
    public interface IEntityShape : IShape
    {
        /// <summary>
        /// Creates a duplicate of the current Entity Shape and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Entity Shape.</returns>
        IEntityShape Clone(IEntityShapesContainer container);
    }
}