namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing a single value selected from the associated Property Type.
    /// </summary>
    public interface IPropertyList : IProperty
    {
        /// <summary>
        /// Selected Value for the property.
        /// </summary>
        IListItem Value { get; set; }
    }
}