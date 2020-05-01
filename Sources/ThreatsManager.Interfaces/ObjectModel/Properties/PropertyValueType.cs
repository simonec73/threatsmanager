namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Enumeration of the types of Properties.
    /// </summary>
    public enum PropertyValueType
    {
        /// <summary>
        /// Property contains a text.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.ISingleLineStringPropertyType, ThreatsManager.Interfaces")]
        [EnumLabel("Single Line String")]
        SingleLineString,

        /// <summary>
        /// Property contains a text.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IStringPropertyType, ThreatsManager.Interfaces")]
        String,

        /// <summary>
        /// Property contains a boolean value.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IBoolPropertyType, ThreatsManager.Interfaces")]
        Boolean,

        /// <summary>
        /// Property contains an integer type.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IIntegerPropertyType, ThreatsManager.Interfaces")]
        Integer,

        /// <summary>
        /// Property contains a decimal type.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IDecimalPropertyType, ThreatsManager.Interfaces")]
        Decimal,

        /// <summary>
        /// Property contains a list of tokens.
        /// </summary>
        /// <remarks>It is typically used for keywords.</remarks>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.ITokensPropertyType, ThreatsManager.Interfaces")]
        [EnumLabel("List of keywords")]
        Tokens,

        /// <summary>
        /// Property contains a list of elements, and allows to select a single item out of it.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IListPropertyType, ThreatsManager.Interfaces")]
        [EnumLabel("Single item selected from a list")]
        List,

        /// <summary>
        /// Property contains a list of elements, and allows to select multiple items out of it.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IListMultiPropertyType, ThreatsManager.Interfaces")]
        [EnumLabel("Multiple items selected from a list")]
        [UiHidden]
        ListMulti,

        /// <summary>
        /// Property contains an array of values, serialized as strings.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IArrayPropertyType, ThreatsManager.Interfaces")]
        [EnumLabel("Array of strings")]
        Array,

        /// <summary>
        /// Property contains an object that can be serialized using Json.NET.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IJsonSerializableObjectPropertyType, ThreatsManager.Interfaces")]
        [EnumLabel("Complex object serialized as Json")]
        [UiHidden]
        JsonSerializableObject,

        /// <summary>
        /// Property contains a reference to a Identity defined in the Threat Model.
        /// </summary>
        [EnumType("ThreatsManager.Interfaces.ObjectModel.Properties.IIdentityReferencePropertyType, ThreatsManager.Interfaces")]
        [EnumLabel("Reference to another object")]
        [UiHidden]
        IdentityReference
    }
}