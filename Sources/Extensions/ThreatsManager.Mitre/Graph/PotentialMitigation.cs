using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PotentialMitigation
    {
        public PotentialMitigation()
        {

        }

        public PotentialMitigation(IEnumerable<string> phases, string mitigationStrategy, string description, string effectiveness)
        {
            Phases = phases;
            MitigationStrategy = mitigationStrategy;
            Description = description;
            Effectiveness = effectiveness;
        }

        [JsonProperty("phases")]
        public IEnumerable<string> Phases { get; private set; }

        [JsonProperty("strategy")]
        public string MitigationStrategy { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("effectiveness")]
        public string Effectiveness { get; private set; }
    }
}