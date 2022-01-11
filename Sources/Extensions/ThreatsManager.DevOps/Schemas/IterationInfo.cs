using System;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class IterationInfo
    {
        public IterationInfo(Iteration currentIteration)
        {
            if (currentIteration != null)
            {
                _iterationId = currentIteration.Id;
                _assignedBy = Utilities.UserName.GetDisplayName();
                _assignedOn = DateTime.Now;
            }
        }

        public bool IsValid => !string.IsNullOrWhiteSpace(_iterationId);

        [JsonProperty("iterationId")]
        private string _iterationId { get; set; }

        public string IterationId => _iterationId;

        [JsonProperty("assignedBy")]
        private string _assignedBy { get; set; }

        public string AssignedBy => _assignedBy;

        [JsonProperty("assignedOn")]
        private DateTime _assignedOn { get; set; }

        public DateTime AssignedOn => _assignedOn;

        public Iteration GetIteration([NotNull] IThreatModel model)
        {
            var schemaManager = new DevOpsConfigPropertySchemaManager(model);
            return schemaManager.GetIterations()?.FirstOrDefault(x => string.CompareOrdinal(x.Id, _iterationId) == 0);
        }
    }
}