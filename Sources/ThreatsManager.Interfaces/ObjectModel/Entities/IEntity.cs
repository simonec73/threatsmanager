using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface representing an entity, that is an External Interactor, a Process or a Data Store.
    /// </summary>
    public interface IEntity : IIdentity, IThreatModelChild, IGroupElement, 
        IPropertiesContainer, IImagesContainer, IThreatEventsContainer, IDirty//, ILockable
    {
        /// <summary>
        /// Event raised when an Image for the Entity changes.
        /// </summary>
        event Action<IEntity, ImageSize> ImageChanged;

        /// <summary>
        /// Template used to generate the Entity.
        /// </summary>
        /// <remarks>It returns null if there is no known Template which generated the Entity.</remarks>
        IEntityTemplate Template { get; }

        /// <summary>
        /// Creates a duplicate of the current Entity and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Entity.</returns>
        IEntity Clone(IEntitiesContainer container);
    }
}