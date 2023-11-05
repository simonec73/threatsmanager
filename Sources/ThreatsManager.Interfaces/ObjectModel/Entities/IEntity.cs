using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface representing an entity, that is an External Interactor, a Process or a Data Store.
    /// </summary>
    public interface IEntity : IIdentity, IThreatModelChild, IGroupElement, 
        IPropertiesContainer, IImagesContainer, IVulnerabilitiesContainer, IThreatEventsContainer//, ILockable
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
        /// Disassociate the Entity from the underlying Template.
        /// </summary>
        void ResetTemplate();

        /// <summary>
        /// Creates a duplicate of the current Entity and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Entity.</returns>
        IEntity Clone(IEntitiesContainer container);

        /// <summary>
        /// Creates a new object as a copy of the current entity, eventually changing its Template.
        /// </summary>
        /// <param name="template">Entity Template to apply to the new instance.</param>
        /// <returns>New copy of the entity.</returns>
        /// <remarks>The new copy is not included in any diagram, but it has copies of the properties,
        /// flows, threats, vulnerabilities and everything else that is associated on the source instance.
        /// The name of the copy is obtained from the name of the source instance with suffix " (copy)".
        /// If the template is not compatible with the target entityType, it is ignored.</remarks>
        IEntity CopyAndConvert(IEntityTemplate template = null);

        /// <summary>
        /// Creates a new object as a copy of the current entity, eventually changing its type and/or Template.
        /// </summary>
        /// <param name="entityType">Target type of the new instance.</param>
        /// <param name="template">Entity Template to apply to the new instance.</param>
        /// <returns>New copy of the entity.</returns>
        /// <remarks>The new copy is not included in any diagram, but it has copies of the properties,
        /// flows, threats, vulnerabilities and everything else that is associated on the source instance.
        /// The name of the copy is obtained from the name of the source instance with suffix " (copy)".
        /// If the template is not compatible with the target entityType, it is ignored.</remarks>
        IEntity CopyAndConvert(EntityType entityType, IEntityTemplate template = null);
    }
}