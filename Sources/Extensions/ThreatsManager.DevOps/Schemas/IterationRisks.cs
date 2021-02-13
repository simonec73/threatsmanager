using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    class IterationRisks
    {
        public IterationRisks()
        {

        }

        public IterationRisks([NotNull] IEnumerable<IterationRisk> iterationRisks)
        {
            Items = new List<IterationRisk>(iterationRisks);
        }

        [JsonProperty("iterationRisks")]
        public List<IterationRisk> Items { get; set; }
    }
}