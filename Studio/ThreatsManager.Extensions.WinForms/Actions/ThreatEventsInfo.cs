using System.Collections.Generic;
using Newtonsoft.Json;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Actions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ThreatEventsInfo
    {
        [JsonProperty("threats")]
        public List<IThreatEvent> ThreatEvents;
    }
}