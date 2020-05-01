namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing an integer.
    /// </summary>
    public interface IPropertyInteger : IProperty
    {
        /// <summary>
        /// Value of the Property.
        /// </summary>
        int Value { get; set; }
    }
}