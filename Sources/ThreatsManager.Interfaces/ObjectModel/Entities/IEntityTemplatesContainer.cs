using System;
using System.Collections.Generic;
using System.Drawing;

namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Interface implemented by the containers of Entity Templates.
    /// </summary>
    public interface IEntityTemplatesContainer
    {
        /// <summary>
        /// Enumeration of Entity Templates.
        /// </summary>
        IEnumerable<IEntityTemplate> EntityTemplates { get; }
        
        /// <summary>
        /// Get an Entity Template given its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Entity Template.</param>
        /// <returns>Entity Template found in the container, otherwise null.</returns>
        IEntityTemplate GetEntityTemplate(Guid id);

        /// <summary>
        /// Get the Entity Templates of a specific Entity Type.
        /// </summary>
        /// <param name="type">Type of the Entity Template.</param>
        /// <returns>Enumeration of the Entity Templates.</returns>
        IEnumerable<IEntityTemplate> GetEntityTemplates(EntityType type);

        /// <summary>
        /// Adds the Entity Template passed as argument to the container.
        /// </summary>
        /// <param name="entityTemplate">Entity Template to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IEntityTemplate entityTemplate);

        /// <summary>
        /// Add an Entity Template to the Container.
        /// </summary>
        /// <param name="name">Name of the Entity Template.</param>
        /// <param name="description">Description of the Entity Template.</param>
        /// <param name="source">Source Entity for the Template.</param>
        /// <returns>New Entity Template.</returns>
        IEntityTemplate AddEntityTemplate(string name, string description, IEntity source);

        /// <summary>
        /// Add an Entity Template to the Container.
        /// </summary>
        /// <param name="name">Name of the Entity Template.</param>
        /// <param name="description">Description of the Entity Template.</param>
        /// <param name="bigImage">Big-sized image to be used for the Template. This should be 64x64 bits.</param>
        /// <param name="image">Mid-sized image to be used for the Template. This should be 32x32 bits.</param>
        /// <param name="smallImage">Small image to be used for the Template. This should be 16x16 bits.</param>
        /// <param name="source">Source Entity for the Template.</param>
        /// <returns>New Entity Template.</returns>
        IEntityTemplate AddEntityTemplate(string name, string description, 
            Bitmap bigImage, Bitmap image, Bitmap smallImage, IEntity source);

        /// <summary>
        /// Add an Entity Template to the Container.
        /// </summary>
        /// <param name="name">Name of the Entity Template.</param>
        /// <param name="description">Description of the Entity Template.</param>
        /// <param name="entityType">Type of the Entity to be created from the Template.</param>
        /// <returns>New Entity Template.</returns>
        IEntityTemplate AddEntityTemplate(string name, string description, EntityType entityType);

        /// <summary>
        /// Add an Entity Template to the Container.
        /// </summary>
        /// <param name="name">Name of the Entity Template.</param>
        /// <param name="description">Description of the Entity Template.</param>
        /// <param name="bigImage">Big-sized image to be used for the Template. This should be 64x64 bits.</param>
        /// <param name="image">Mid-sized image to be used for the Template. This should be 32x32 bits.</param>
        /// <param name="smallImage">Small image to be used for the Template. This should be 16x16 bits.</param>
        /// <param name="entityType">Type of the Entity to be created from the Template.</param>
        /// <returns>New Entity Template.</returns>
        IEntityTemplate AddEntityTemplate(string name, string description, 
            Bitmap bigImage, Bitmap image, Bitmap smallImage, EntityType entityType);

        /// <summary>
        /// Remove the Entity Template whose identifier is passed as argument.
        /// </summary>
        /// <param name="id">Identifier of the Entity Template.</param>
        /// <returns>True if the Entity Template has been found and removed, false otherwise.</returns>
        bool RemoveEntityTemplate(Guid id);
    }
}