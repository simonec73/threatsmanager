using System;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.Diagrams
{
    /// <summary>
    /// Interface implemented by the Links.
    /// </summary>
    public interface ILink : IPropertiesContainer, IDirty
    {
        /// <summary>
        /// Identifier of the associated Data Flow.
        /// </summary>
        Guid AssociatedId { get; }

        /// <summary>
        /// Associated Data Flow.
        /// </summary>
        IDataFlow DataFlow { get; }

        /// <summary>
        /// Creates a duplicate of the current Link and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Link.</returns>
        ILink Clone(ILinksContainer container);
    }
}