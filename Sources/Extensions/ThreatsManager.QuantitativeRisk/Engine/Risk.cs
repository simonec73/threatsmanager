using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.QuantitativeRisk.Schemas;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    /// <summary>
    /// FAIR Risk. The probable frequency and probable magnitude of future loss.
    /// </summary>
    public class Risk : Estimation
    {
        public bool InDepth { get; set; }

        /// <summary>
        /// Loss Event Frequency. The probable frequency, within a given time-frame, that loss will materialize from a threat agent's action.
        /// </summary>
        public LossEventFrequency LossEventFrequency { get; set; }

        /// <summary>
        /// Loss Magnitude. The probable magnitude of primary and secondary loss resulting from an event.
        /// </summary>
        public LossMagnitude LossMagnitude { get; set; }
    }

    public static class QuantitativeRiskExtensions
    {
        public static Risk GetRisk(this IThreatEventScenario scenario)
        {
            Risk result = null;

            var model = scenario.Model;
            if (model != null)
            {
                var schemaManager = new QuantitativeRiskSchemaManager(model);
                result = schemaManager.GetRisk(scenario);
            }

            return result;
        }

        public static void SetRisk(this IThreatEventScenario scenario, Risk risk)
        {
            var model = scenario.Model;
            if (model != null)
            {
                var schemaManager = new QuantitativeRiskSchemaManager(model);
                schemaManager.SetRisk(scenario, risk);
            }
        }
    }
}