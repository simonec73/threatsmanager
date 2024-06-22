using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    public class UrlItem : IUrl
    {
        public UrlItem()
        {
        }

        public UrlItem(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var regex = new Regex(@"(?'label'[^\(]+) \((?'url'https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*))\)");
                var match = regex.Match(text);
                if (match.Success)
                {
                    Label = match.Groups["label"].Value;
                    Url = match.Groups["url"].Value;
                }
            }
        }

        public UrlItem(string label, string url)
        {
            Label = label;
            Url = url;
        }

        public string Url { get; set; }

        public string Label { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(Url) && !string.IsNullOrWhiteSpace(Label);

        public override string ToString()
        {
            return $"{Label} ({Url})";
        }
    }
}
