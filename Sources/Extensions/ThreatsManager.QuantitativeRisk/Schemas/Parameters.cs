using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.QuantitativeRisk.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Parameters
    {
        [JsonProperty("items")]
        public List<Parameter> Items;
    }
}
