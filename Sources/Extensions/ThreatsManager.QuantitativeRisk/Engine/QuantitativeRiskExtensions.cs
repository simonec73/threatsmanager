using System;
using System.Collections.Generic;
using System.Text;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.QuantitativeRisk.Schemas;

namespace ThreatsManager.QuantitativeRisk.Engine
{
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
