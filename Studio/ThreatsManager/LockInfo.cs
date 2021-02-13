using System;
using Newtonsoft.Json;
using ThreatsManager.Utilities;

namespace ThreatsManager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LockInfo
    {
        public void AutoLoad()
        {
            User = UserName.GetDisplayName();
            Machine = CurrentMachine;
            Timestamp = DateTime.Now;
        }

        public static string CurrentMachine => Environment.MachineName;

        [JsonProperty("user")]
        public string User { get; protected set; }

        [JsonProperty("machine")]
        public string Machine { get; protected set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; protected set; }
    }
}