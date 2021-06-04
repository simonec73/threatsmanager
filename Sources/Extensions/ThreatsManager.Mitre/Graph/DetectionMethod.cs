using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Graph
{
    [JsonObject(MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DetectionMethod
    {
        public DetectionMethod()
        {

        }

        public DetectionMethod(string method, string description, string effectiveness)
        {
            Method = method;
            Description = description;
            Effectiveness = effectiveness;
        }

        [JsonProperty("method")]
        public string Method { get; private set; }

        [JsonProperty("desc")]
        public string Description { get; private set; }

        [JsonProperty("effectiveness")]
        public string Effectiveness { get; private set; }
    }
}