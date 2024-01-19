using System.Drawing;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the Shapes associated to Groups.
    /// </summary>
    public interface IGroupShape : IShape
    {
        /// <summary>
        /// Size of the Group.
        /// </summary>
        SizeF Size { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Group Shape and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Group Shape.</returns>
        IGroupShape Clone(IGroupShapesContainer container);
    }
}