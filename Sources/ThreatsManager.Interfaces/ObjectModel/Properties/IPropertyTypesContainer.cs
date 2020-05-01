using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface that defines a container of Property Types.
    /// </summary>
    public interface IPropertyTypesContainer
    {
        /// <summary>
        /// Enumeration of the defined Property Types.
        /// </summary>
        IEnumerable<IPropertyType> PropertyTypes { get; }

        /// <summary>
        /// Event raised when a Property Type is added.
        /// </summary>
        event Action<IPropertySchema, IPropertyType> PropertyTypeAdded;

        /// <summary>
        /// Event Raised when a Property Type is removed.
        /// </summary>
        event Action<IPropertySchema, IPropertyType> PropertyTypeRemoved;

        /// <summary>
        /// Get a Property Type from the container, given its Id.
        /// </summary>
        /// <param name="id">Identifier of the Property Type.</param>
        /// <returns>Searched Property Type, if found, otherwise null.</returns>
        IPropertyType GetPropertyType(Guid id);

        /// <summary>
        /// Get a Property Type by name.
        /// </summary>
        /// <param name="name">Name of the Property Type.</param>
        /// <returns>The searched Property Type.</returns>
        IPropertyType GetPropertyType(string name);

        /// <summary>
        /// Adds the Property Type passed as argument to the container.
        /// </summary>
        /// <param name="propertyType">Property Type to be added to the container.</param>
        /// <exception cref="ArgumentException">The argument is not associated to the same Threat Model of the Container.</exception>
        void Add(IPropertyType propertyType);

        /// <summary>
        /// Creates a new Property Type.
        /// </summary>
        /// <param name="name">Name of the Property Type.</param>
        /// <param name="type">Type of the new Property.</param>
        /// <returns>The new Property Type. It may return null, for example because a Property Type with the same name already exists.</returns>
        IPropertyType AddPropertyType(string name, PropertyValueType type);

        /// <summary>
        /// Removes an existing Property Type.
        /// </summary>
        /// <param name="id">Identifier of the Property Type.</param>
        /// <returns>True if the Property Type has been deleted successfully, otherwise false.</returns>
        bool RemovePropertyType(Guid id);
    }
}