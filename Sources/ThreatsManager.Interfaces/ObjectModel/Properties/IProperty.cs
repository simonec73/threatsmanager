using System;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Generic interface implemented by Properties.
    /// </summary>
    public interface IProperty : IThreatModelChild, IDirty
    {
        /// <summary>
        /// Identifier of the Property.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Event raised when a property is changed.
        /// </summary>
        /// <returns>The changed property.</returns>
        event Action<IProperty> Changed;

        /// <summary>
        /// Identifier of the Property Type associated to the Property.
        /// </summary>
        Guid PropertyTypeId { get; set; }

        /// <summary>
        /// Property Type associated to the Property.
        /// </summary>
        IPropertyType PropertyType { get; }

        /// <summary>
        /// Value of the Property, as string.
        /// </summary>
        /// <remarks>This is used to serialize the property, not to show it: for that goal, please override ToString().</remarks>
        string StringValue { get; set; }

        /// <summary>
        /// Flag that allows preventing undesired change of the Property Value.
        /// </summary>
        /// <remarks>Changing the value of a ReadOnly property raises a ReadOnlyPropertyException.</remarks>
        bool ReadOnly { get; set; }
    }
}