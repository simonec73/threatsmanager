namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property representing a Url.
    /// </summary>
    public interface IPropertyUrl : IProperty
    {
        /// <summary>
        /// Url of the Property.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Label of the Property.
        /// </summary>
        string Label { get; set; }
    }
}