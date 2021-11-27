using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ExternalNode : Node
    {
        public ExternalNode()
        {

        }

        internal ExternalNode([NotNull] MitreGraph graph, string source, string id, string url, string description) : base(graph, source, id)
        {
            Url = url;
            Description = description;
        }

        [JsonProperty("url")]
        public string Url { get; private set; }
    }
}