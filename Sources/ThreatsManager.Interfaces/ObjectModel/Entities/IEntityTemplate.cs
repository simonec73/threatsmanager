using System;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Template for Entities.
    /// </summary>
    public interface IEntityTemplate : IItemTemplate, IImagesContainer
    {
        /// <summary>
        /// Event raised when an Image for the Entity Template changes.
        /// </summary>
        event Action<IEntityTemplate, ImageSize> ImageChanged;

        /// <summary>
        /// Type of the Entity associated to the Template.
        /// </summary>
        EntityType EntityType { get; set; }

        /// <summary>
        /// Create a new entity based on the Template.
        /// </summary>
        /// <param name="name">Name of the new Entity.</param>
        /// <returns>New entity created from the Template.</returns>
        IEntity CreateEntity(string name);

        /// <summary>
        /// Apply the Template to an existing Entity.
        /// </summary>
        /// <param name="entity">Entity which needs to receive the Template.</param>
        /// <remarks>Applies all the properties to the Entity and changes its Template to point to the new one.</remarks>
        void ApplyTo(IEntity entity);

        /// <summary>
        /// Creates a duplicate of the current Template and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Template.</returns>
        IEntityTemplate Clone(IEntityTemplatesContainer container);
    }
}