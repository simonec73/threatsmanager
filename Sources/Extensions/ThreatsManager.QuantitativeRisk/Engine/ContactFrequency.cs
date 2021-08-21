using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    /// <summary>
    /// Contact Frequency. The probable frequency, within a given time-frame, that threat agents will come into contact with assets. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ContactFrequency : Frequency
    {
        /// <summary>
        /// Types of contact.
        /// </summary>
        [JsonProperty("contactType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ContactType ContactType { get; set; }
    }
}