using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Consequence
    {
        public Consequence()
        {

        }

        public Consequence(IEnumerable<string> scopes, IEnumerable<string> impacts, string notes)
        {
            Scopes = scopes;
            Impacts = impacts;
            Notes = notes;
        }

        [JsonProperty("scopes")]
        public IEnumerable<string> Scopes { get; private set; }

        [JsonProperty("impacts")]
        public IEnumerable<string> Impacts { get; private set; }

        [JsonProperty("notes")]
        public string Notes { get; private set; }
    }
}