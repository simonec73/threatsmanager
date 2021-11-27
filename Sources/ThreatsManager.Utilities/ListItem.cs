using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Utilities
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class ListItem : IListItem
    {
        public ListItem()
        {

        }

        public ListItem([Required] string id, string label = null)
        {
            Id = id;
            Label = label;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public override string ToString()
        {
            return Label ?? Id;
        }
    }
}