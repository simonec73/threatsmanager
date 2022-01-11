using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Threshold
    {
        private Threshold()
        {
        }

        public Threshold([Positive] int id, decimal value)
        {
            _severityId = id;
            Value = value;
        }

        public Threshold([NotNull] ISeverity severity, [Positive] decimal value) : this(severity.Id, value)
        {
        }

        [JsonProperty("severity")]
        private int _severityId { get; set; }

        public int SeverityId => _severityId;

        [JsonProperty("threshold")]
        public decimal Value { get; set; }
    }
}
