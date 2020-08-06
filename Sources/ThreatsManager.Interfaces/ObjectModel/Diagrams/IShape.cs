using System;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the Shapes.
    /// </summary>
    /// <remarks>It should not be used directly, but only through the derived IEntityShape and IGroupShape interfaces.</remarks>
    public interface IShape : IPropertiesContainer, IDirty
    {
        /// <summary>
        /// Identifier of the associated Entity or Group.
        /// </summary>
        Guid AssociatedId { get; }

        /// <summary>
        /// Identity associated to the Shape.
        /// </summary>
        IIdentity Identity { get; }

        /// <summary>
        /// Position of the Shape.
        /// </summary>
        PointF Position { get; set; }
    }
}