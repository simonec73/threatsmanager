using Newtonsoft.Json;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    /// <summary>
    /// FAIR Risk. The probable frequency and probable magnitude of future loss.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Risk : Estimation
    {
        [JsonProperty("inDepth")]
        public bool InDepth { get; set; }

        /// <summary>
        /// Loss Event Frequency. The probable frequency, within a given time-frame, that loss will materialize from a threat agent's action.
        /// </summary>
        [JsonProperty("lef")]
        public LossEventFrequency LossEventFrequency { get; set; }

        /// <summary>
        /// Loss Magnitude. The probable magnitude of primary and secondary loss resulting from an event.
        /// </summary>
        [JsonProperty("lm")]
        public LossMagnitude LossMagnitude { get; set; }
    }
}