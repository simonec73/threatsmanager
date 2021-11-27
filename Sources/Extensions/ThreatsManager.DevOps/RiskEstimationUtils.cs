using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps
{
    public static class RiskEstimationUtils
    {
        public static float EvaluateRisk(this IThreatModel model, int normalizationReference)
        {
            float result = 0f;

            if (model != null)
            {
                var riskEstimatorSchemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
                var estimator = riskEstimatorSchemaManager.SelectedEstimator;
                if (estimator != null)
                {
                    result = estimator.GetRiskEvaluation(model, normalizationReference);
                }
            }

            return result;
        }
    }
}
