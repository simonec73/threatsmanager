using Newtonsoft.Json;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    /// <summary>
    /// Threat Event Frequency. The probable frequency, within a given time-frame, that threat agents will act in a manner that may result in loss. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ThreatEventFrequency : Frequency
    {
        /// <summary>
        /// Contact Frequency. The probable frequency, within a given time-frame, that threat agents will come into contact with assets. 
        /// </summary>
        [JsonProperty("cf")]
        public ContactFrequency ContactFrequency { get; set; }

        /// <summary>
        /// Probability of Action. The probability that a threat agent will act upon an asset once contact has occurred.
        /// </summary>
        [JsonProperty("poa")]
        public Frequency ProbabilityOfAction { get; set; }
    }
}