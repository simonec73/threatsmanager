using System;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Template for Entities.
    /// </summary>
    public interface IEntityTemplate : IIdentity, IThreatModelChild, IImagesContainer,
        IPropertiesContainer //, ILockable
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
        /// Creates a duplicate of the current Entity Template and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Entity Template.</returns>
        IEntityTemplate Clone(IEntityTemplatesContainer container);
    }
}