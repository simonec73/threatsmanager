using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property representing a list of Urls.
    /// </summary>
    public interface IPropertyUrlList : IProperty
    {
        /// <summary>
        /// Urls for the property.
        /// </summary>
        IEnumerable<IUrl> Values { get; }

        /// <summary>
        /// Get a Url from the list.
        /// </summary>
        /// <param name="label">Label of the URL.</param>
        /// <returns>The Url associated to the label.</returns>
        string GetUrl(string label);

        /// <summary>
        /// Set a Url into the list.
        /// </summary>
        /// <param name="label">Label of the Url.</param>
        /// <param name="url">Url.</param>
        void SetUrl(string label, string url);

        /// <summary>
        /// Set a Url into the list.
        /// </summary>
        /// <param name="oldLabel">Current label of the Url.</param>
        /// <param name="newLabel">New label of the Url.</param>
        /// <param name="url">Url.</param>
        void SetUrl(string oldLabel, string newLabel, string url);

        /// <summary>
        /// Remove a Url from the list.
        /// </summary>
        /// <param name="label">Label of the Url.</param>
        /// <returns>True if the Url has been found and removed.</returns>
        bool DeleteUrl(string label);

        /// <summary>
        /// Clear the list of Urls.
        /// </summary>
        void ClearUrls();
    }
}