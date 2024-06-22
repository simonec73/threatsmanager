namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface that represents a Url.
    /// </summary>
    public interface IUrl
    {
        /// <summary>
        /// Url.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Label.
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// Check if the Url is valid.
        /// </summary>
        bool IsValid { get; }
    }
}