using ThreatsManager.Interfaces;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Policies
{
    [Extension("58583B10-BFF1-40D3-B927-3DFC4E013246", "Risk Normalization Policy", 100, ExecutionMode.Business)]
    public class RiskNormalizationPolicy : Policy
    {
        protected override string PolicyName => "RiskNormalization";

        public int? RiskNormalization => (int?)Value;
    }
}
