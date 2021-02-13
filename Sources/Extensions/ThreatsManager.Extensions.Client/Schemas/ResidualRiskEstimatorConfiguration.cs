using System.Collections.Generic;
using Newtonsoft.Json;

namespace ThreatsManager.Extensions.Schemas
{
    /// <summary>
    /// Configuration of the Residual Risk Estimator.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ResidualRiskEstimatorConfiguration
    {
        [JsonProperty("parameters")]
        public List<ResidualRiskEstimatorParameter> Parameters { get; set; }
    }
}
