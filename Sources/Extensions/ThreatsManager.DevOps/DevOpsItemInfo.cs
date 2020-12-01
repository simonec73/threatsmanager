using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DevOpsItemInfo : IDevOpsItemInfo
    {
        private DevOpsItemInfo()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serialized">Serialized value of the Item, obtained with <see cref="Serialize"/>.</param>
        public DevOpsItemInfo([RegularExpression(@"(?<Id>[\d]+)#(?<Name>.*)#(?<Url>.*)")] string serialized)
        {
            var regex = new Regex(@"(?<Id>[\d]+)#(?<Name>.*)#(?<Url>.*)");
            var match = regex.Match(serialized);
            if (match.Success && int.TryParse(match.Groups["Id"].Value, out var id))
            {
                _id = id;
                _name = match.Groups["Name"].Value;
                _url = match.Groups["Url"].Value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Identifier of the Item.</param>
        /// <param name="name">Name of the Item.</param>
        public DevOpsItemInfo([Positive] int id, [Required] string name, [Required] string url)
        {
            _id = id;
            _name = name;
            _url = url;
        }

        [JsonProperty("id")]
        private int _id;

        public int Id => _id;

        [JsonProperty("name")]
        private string _name;

        public string Name => _name;

        [JsonProperty("url")]
        private string _url;

        public string Url => _url;

        public string Serialize()
        {
            return $"{Id}#{Name}#{Url}";
        }

       public override string ToString()
        {
            return Name;
        }
    }
}