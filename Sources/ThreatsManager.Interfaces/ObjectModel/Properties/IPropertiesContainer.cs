using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Container of Properties.
    /// </summary>
    public interface IPropertiesContainer
    {
        /// <summary>
        /// Event raised when a property is added.
        /// </summary>
        /// <returns>The instance that has had the property added and the new property.</returns>
        /// <seealso cref="IProperty"/>
        event Action<IPropertiesContainer, IProperty> PropertyAdded;

        /// <summary>
        /// Event raised when a property is removed.
        /// </summary>
        /// <returns>The instance that has had the property removed and the property.</returns>
        /// <seealso cref="IProperty"/>
        event Action<IPropertiesContainer, IProperty> PropertyRemoved;

        /// <summary>
        /// Event raised when the value of a property is changed.
        /// </summary>
        /// <returns>The instance that has had the value of the property changed and the property.</returns>
        /// <seealso cref="IProperty"/>
        event Action<IPropertiesContainer, IProperty> PropertyValueChanged;

        /// <summary>
        /// List of Properties.
        /// </summary>
        /// <seealso cref="IProperty"/>
        IEnumerable<IProperty> Properties { get; }

        /// <summary>
        /// Scope associated to the container.
        /// </summary>
        Scope PropertiesScope { get; }

        /// <summary>
        /// Verifies if the Container includes a Property of the specified Property Type.
        /// </summary>
        /// <param name="propertyType">Property Type for which the property is sought.</param>
        /// <returns>True if the Property is defined, otherwise false.</returns>
        bool HasProperty(IPropertyType propertyType);

        /// <summary>
        /// Get the property of a specific type.
        /// </summary>
        /// <param name="propertyType">Type of property to be searched.</param>
        /// <returns>Property of the specified type, if found, <c>null</c> otherwise.</returns>
        /// <seealso cref="IPropertyType"/>
        IProperty GetProperty(IPropertyType propertyType);

        /// <summary>
        /// Add a property to the list.
        /// </summary>
        /// <param name="propertyType">Type of property to be added.</param>
        /// <param name="value">Value of the property.</param>
        /// <returns>The newly generated property.</returns>
        /// <seealso cref="IPropertyType"/>
        IProperty AddProperty(IPropertyType propertyType, string value);

        /// <summary>
        /// Remove a property from the list.
        /// </summary>
        /// <param name="propertyType">Type of property to be removed.</param>
        /// <returns>True if the property has been successfully removed.</returns>
        /// <remarks>If the property is missing from the list, then it returns false.</remarks>
        /// <seealso cref="IPropertyType"/>
        bool RemoveProperty(IPropertyType propertyType);

        /// <summary>
        /// Remove a property from the list.
        /// </summary>
        /// <param name="propertyTypeId">Identifier of property to be removed.</param>
        /// <returns>True if the property has been successfully removed.</returns>
        /// <remarks>If the property is missing from the list, then it returns false.</remarks>
        /// <seealso cref="IPropertyType"/>
        bool RemoveProperty(Guid propertyTypeId);

        /// <summary>
        /// Remove all properties from the list.
        /// </summary>
        void ClearProperties();

        /// <summary>
        /// Apply a Property Schema to the container.
        /// </summary>
        /// <param name="schema">Property Schema to be applied.</param>
        /// <remarks>It will be applied only if the Scope of the Schema is compatible with the container,
        /// and if it has not been applied yet.</remarks>
        void Apply(IPropertySchema schema);
    }
}