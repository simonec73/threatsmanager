using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Relationship
    {
        public Relationship()
        {

        }

        internal Relationship(RelationshipType type, [Required] string source, [Required] string id, string viewId = null)
        {
            Type = type;
            CounterpartySource = source;
            CounterpartyId = id;
            ViewId = viewId;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public RelationshipType Type { get; private set; }

        [JsonProperty("source")]
        public string CounterpartySource { get; private set;  }

        [JsonProperty("id")]
        public string CounterpartyId { get; private set;  }

        [JsonProperty("view")]
        public string ViewId { get; private set; }

        public Node GetNode([NotNull] MitreGraph graph)
        {
            return graph.GetNode(CounterpartySource, CounterpartyId);
        }
    }
}