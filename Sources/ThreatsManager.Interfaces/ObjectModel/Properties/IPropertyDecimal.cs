namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing an decimal.
    /// </summary>
    public interface IPropertyDecimal : IProperty
    {
        /// <summary>
        /// Value of the Property.
        /// </summary>
        decimal Value { get; set; }
    }
}