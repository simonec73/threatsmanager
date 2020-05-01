namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing an object that can be serialized using Json.NET.
    /// </summary>
    public interface IPropertyJsonSerializableObject : IProperty
    {
        /// <summary>
        /// Object contained in the Property.
        /// </summary>
        object Value { get; set; }
    }
}