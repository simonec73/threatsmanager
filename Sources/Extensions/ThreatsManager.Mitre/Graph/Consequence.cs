using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Consequence
    {
        public Consequence()
        {

        }

        public Consequence(IEnumerable<string> scopes, IEnumerable<string> impacts, Evaluation likelihood, string notes)
        {
            Scopes = scopes;
            Impacts = impacts;
            Likelihood = likelihood;
            Notes = notes;
        }

        [JsonProperty("scopes")]
        public IEnumerable<string> Scopes { get; private set; }

        [JsonProperty("impacts")]
        public IEnumerable<string> Impacts { get; private set; }

        [JsonProperty("likelihood")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Evaluation Likelihood { get; private set; }

        [JsonProperty("notes")]
        public string Notes { get; private set; }
    }
}