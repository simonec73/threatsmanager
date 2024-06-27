using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// An implementation of an IUrl.
    /// </summary>
    public class UrlDefinition : IUrl
    {
        /// <summary>
        /// Public default constructor.
        /// </summary>
        public UrlDefinition()
        {
        }

        /// <summary>
        /// Constructor taking a Url Definition as a string.
        /// </summary>
        /// <param name="text">Url definition as a string, in the format "[label] ([url])".</param>
        public UrlDefinition(string text)
        {
            if (text?.IsUrlDefinition(out var match) ?? false)
            {
                Label = match?.GetLabelFromUrlDefinition() ?? null;
                Url = match?.GetUrlFromUrlDefinition() ?? null;
            }
        }

        /// <summary>
        /// Constructor taking a Url Definition components.
        /// </summary>
        /// <param name="label">Label associated to the Url.</param>
        /// <param name="url">Url.</param>
        public UrlDefinition(string label, string url)
        {
            Label = label;
            Url = url;
        }

        /// <summary>
        /// Url associated to the current definition.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Label associated to the current definition.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Url definition validation.
        /// </summary>
        public bool IsValid => !string.IsNullOrWhiteSpace(Url) && !string.IsNullOrWhiteSpace(Label);

        /// <summary>
        /// Converts the Url Definition to its textual representation.
        /// </summary>
        /// <returns>Textual representation of the Url definition.</returns>
        public override string ToString()
        {
            return $"{Label} ({Url})";
        }
    }
}
