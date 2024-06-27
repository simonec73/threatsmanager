using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Utilities to manage URLs.
    /// </summary>
    public static class UrlDefinitionUtils
    {
        private readonly static Regex _regex = 
            new Regex(@"(?'label'[^\(]+) \((?'url'https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*))\)");

        /// <summary>
        /// Check whether the provided text is a valid URL Definition.
        /// </summary>
        /// <param name="text">Text to be checked.</param>
        /// <returns>Result of the check.</returns>
        /// <remarks>Url Definitions have the following structure "[label] ([url])".</remarks>
        public static bool IsUrlDefinition(this string text)
        {
            return IsUrlDefinition(text, out var match);
        }

        /// <summary>
        /// Check whether the provided text is a valid URL Definition.
        /// </summary>
        /// <param name="text">Text to be checked.</param>
        /// <param name="match">[out] Match object. If cant be used with GetLabelFromUrlDefinition and GetUrlFromUrlDefinition.</param>
        /// <returns>Result of the check.</returns>
        /// <remarks>Url Definitions have the following structure "[label] ([url])".</remarks>
        public static bool IsUrlDefinition(this string text, out Match match)
        {
            var result = false;
            match = null;

            if (!string.IsNullOrWhiteSpace(text))
            {
                match = _regex.Match(text);
                result = match.Success;
            }

            return result;
        }

        /// <summary>
        /// Get the label from a URL Definition.
        /// </summary>
        /// <param name="text">Url definition.</param>
        /// <returns>Label, if found, otherwise null.</returns>
        /// <remarks>Url Definitions have the following structure "[label] ([url])".</remarks>
        public static string GetLabelFromUrlDefinition(this string text)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(text))
            {
                result = GetLabelFromUrlDefinition(_regex.Match(text));
            }

            return result;
        }

        /// <summary>
        /// Get the label from a URL Definition that has already been validated with IsUrlDefinition.
        /// </summary>
        /// <param name="match">Match returned by IsUrlDefinition.</param>
        /// <returns>Label, if found, otherwise null.</returns>
        /// <remarks>Url Definitions have the following structure "[label] ([url])".</remarks>
        public static string GetLabelFromUrlDefinition(this Match match)
        {
            string result = null;

            if (match?.Success ?? false)
            {
                result = match.Groups["label"].Value;
            }

            return result;
        }

        /// <summary>
        /// Get the url from a URL Definition.
        /// </summary>
        /// <param name="text">Url definition.</param>
        /// <returns>Url, if found, otherwise null.</returns>
        /// <remarks>Url Definitions have the following structure "[label] ([url])".</remarks>
        public static string GetUrlFromUrlDefinition(this string text)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(text))
            {
                result = GetUrlFromUrlDefinition(_regex.Match(text));
            }

            return result;
        }

        /// <summary>
        /// Get the url from a URL Definition.
        /// </summary>
        /// <param name="match">Match returned by IsUrlDefinition.</param>
        /// <returns>Url, if found, otherwise null.</returns>
        /// <remarks>Url Definitions have the following structure "[label] ([url])".</remarks>
        public static string GetUrlFromUrlDefinition(this Match match)
        {
            string result = null;

            if (match?.Success ?? false)
            {
                result = match.Groups["url"].Value;
            }

            return result;
        }

        /// <summary>
        /// Split a string containing multiple URL Definitions into a list of IUrls.
        /// </summary>
        /// <param name="urlArray">String containing multiple URL Definitions.</param>
        /// <returns>Enumeration of URL Definitions.</returns>
        public static IEnumerable<IUrl> SplitUrlDefinitions(this string urlArray)
        {
            IEnumerable<IUrl> result = null;

            var list = urlArray?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (list?.Any() ?? false)
            {
                result = list.Select(x => new UrlDefinition(x)).Where(x => x.IsValid);
            }

            return result;
        }

        /// <summary>
        /// Merge a list of IUrls into a string containing multiple URL Definitions.
        /// </summary>
        /// <param name="urls">List of IUrls to be merged.</param>
        /// <returns>String containing the merged list.</returns>
        public static string MergeUrlDefinitions(this IEnumerable<IUrl> urls)
        {
            return urls?.Select(x => x.ToString()).Aggregate((x, y) => $"{x}\r\n{y}");
        }

        /// <summary>
        /// Set the URL for a specific label in a list of URL Definitions.
        /// </summary>
        /// <param name="urlArray">String containing multiple URL Definitions.</param>
        /// <param name="label">Label of the Url to be updated</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SetUrlDefinition(this string urlArray, string label, string url)
        {
            var values = urlArray?.SplitUrlDefinitions()?.ToList() ?? new List<IUrl>();
            var urlItem = values.FirstOrDefault(x => string.CompareOrdinal(x.Label, label) == 0);
            if (urlItem != null)
            {
                urlItem.Url = url;
            }
            else
            {
                values.Add(new UrlDefinition(label, url));
            }

            return values.MergeUrlDefinitions();
        }

        /// <summary>
        /// Set the URL for a specific label in a list of URL Definitions.
        /// </summary>
        /// <param name="urlArray">String containing multiple URL Definitions.</param>
        /// <param name="oldLabel">Current label of the Url to be updated.</param>
        /// <param name="newLabel">New label of the Url to be updated.</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SetUrlDefinition(this string urlArray, string oldLabel, string newLabel, string url)
        {
            var values = urlArray?.SplitUrlDefinitions()?.ToList() ?? new List<IUrl>();
            var urlItem = values.FirstOrDefault(x => string.CompareOrdinal(x.Label, oldLabel) == 0);
            if (urlItem != null)
            {
                urlItem.Label = newLabel;
                urlItem.Url = url;
            }
            else
            {
                values.Add(new UrlDefinition(newLabel, url));
            }

            return values.MergeUrlDefinitions();
        }

        /// <summary>
        /// Delete a URL Definition from a list of URL Definitions.
        /// </summary>
        /// <param name="urlArray">Source list of URL Definitions.</param>
        /// <param name="label"></param>
        /// <param name="newUrlArray"></param>
        /// <returns></returns>
        public static bool DeleteUrlDefinition(this string urlArray, string label, out string newUrlArray)
        {
            var result = false;
            newUrlArray = null;

            var values = urlArray?.SplitUrlDefinitions()?.ToList();
            var found = values?.Any(x => string.CompareOrdinal(x.Label, label) == 0) ?? false;
            if (found)
            {
                newUrlArray = values
                    .Where(x => string.CompareOrdinal(x.Label, label) != 0)
                    .MergeUrlDefinitions();
                result = true;
            }

            return result;
        }
    }
}
