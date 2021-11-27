using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FactBased
    {
        [JsonProperty("facts")]
        public List<Guid> AssociatedFacts { get; set; }
    }
}