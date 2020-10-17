using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Field used by the DevOps Extensions to map internal fields to DevOps fields.
    /// </summary>
    public class IdentityField
    {
        /// <summary>
        /// Constructor used to create a reference to one of the standard fields.
        /// </summary>
        /// <param name="fieldType">Field Type.</param>
        /// <exception cref="ArgumentException">The Field Type cannot be Property: this type would require using the other constructor.</exception>
        public IdentityField(IdentityFieldType fieldType)
        {
            if (fieldType == IdentityFieldType.Property)
                throw new ArgumentException();

            FieldType = fieldType;
        }

        /// <summary>
        /// Constructor used to create a reference to a custom field.
        /// </summary>
        /// <param name="propertyType">Property Type for the custom field.</param>
        public IdentityField([NotNull] IPropertyType propertyType)
        {
            FieldType = IdentityFieldType.Property;
            PropertyType = propertyType;
        }

        /// <summary>
        /// Field Type.
        /// </summary>
        public IdentityFieldType FieldType { get; private set; }

        /// <summary>
        /// Property Type.
        /// </summary>
        /// <remarks>It is defined only if the FieldType is Property.</remarks>
        public IPropertyType PropertyType { get; private set; }

        /// <summary>
        /// Representation of the field.
        /// </summary>
        public override string ToString()
        {
            return FieldType == IdentityFieldType.Property ? PropertyType.ToString() : FieldType.ToString();
        }
    }
}