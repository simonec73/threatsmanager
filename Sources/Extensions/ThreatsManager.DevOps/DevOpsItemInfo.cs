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
        public DevOpsItemInfo([RegularExpression(@"(?<Id>[\d]+)#(?<Name>.*)#(?<Url>.*)#(?<WorkItemType>.*)#(?<AssignedTo>.*)")] string serialized)
        {
            var regex = new Regex(@"(?<Id>[\d]+)#(?<Name>.*)#(?<Url>.*)#(?<WorkItemType>.*)#(?<AssignedTo>.*)");
            var match = regex.Match(serialized);
            if (match.Success && int.TryParse(match.Groups["Id"].Value, out var id))
            {
                _id = id;
                _name = match.Groups["Name"].Value;
                _url = match.Groups["Url"].Value;
                _workItemType = match.Groups["WorkItemType"].Value;
                _assignedTo = match.Groups["AssignedTo"].Value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Identifier of the Item.</param>
        /// <param name="name">Name of the Item.</param>
        /// <param name="url">Url of the Item.</param>
        /// <param name="workItemType">Type of the Item.</param>
        public DevOpsItemInfo([Positive] int id, [Required] string name, [Required] string url, [Required] string workItemType, string assignedTo)
        {
            _id = id;
            _name = name;
            _url = url;
            _workItemType = workItemType;
            _assignedTo = assignedTo;
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

        [JsonProperty("workItemType")]
        private string _workItemType;

        public string WorkItemType => _workItemType;

        [JsonProperty("assignedTo")]
        private string _assignedTo;

        public string AssignedTo => _assignedTo;

        public string Serialize()
        {
            return $"{Id}#{Name}#{Url}#{WorkItemType}#{AssignedTo}";
        }

       public override string ToString()
        {
            return Name;
        }
    }
}