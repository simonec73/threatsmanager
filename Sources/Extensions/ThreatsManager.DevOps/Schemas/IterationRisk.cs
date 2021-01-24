using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class IterationRisk
    {
        public IterationRisk([NotNull] Iteration iteration, float risk)
        {
            _iterationId = iteration.Id;
            Risk = risk;
        }

        [JsonProperty("iterationId")]
        private string _iterationId { get; set; }

        public string IterationId => _iterationId;

        [JsonProperty("risk")]
        public float Risk { get; set; }

        public Iteration GetIteration([NotNull] IThreatModel model)
        {
            var schemaManager = new DevOpsConfigPropertySchemaManager(model);
            return schemaManager.GetIterations()?.FirstOrDefault(x => string.CompareOrdinal(x.Id, _iterationId) == 0);
        }
    }
}