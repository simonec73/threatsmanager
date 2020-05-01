namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing a boolean.
    /// </summary>
    public interface IPropertyBool : IProperty
    {
        /// <summary>
        /// Value of the Property.
        /// </summary>
        bool Value { get; set; }
    }
}