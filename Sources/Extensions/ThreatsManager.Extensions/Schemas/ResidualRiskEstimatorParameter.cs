using Newtonsoft.Json;

namespace ThreatsManager.Extensions.Schemas
{
    /// <summary>
    /// Residual Risk Estimator Configuration parameter.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ResidualRiskEstimatorParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public float Value { get; set; }
    }
}