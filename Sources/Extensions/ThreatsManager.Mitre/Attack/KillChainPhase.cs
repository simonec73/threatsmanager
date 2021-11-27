using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Attack
{
    [JsonObject(MemberSerialization.OptIn)]
    public class KillChainPhase
    {
        [JsonProperty("kill_chain_name")]
        public string Name;

        [JsonProperty("phase_name")]
        public string Phase;
    }
}