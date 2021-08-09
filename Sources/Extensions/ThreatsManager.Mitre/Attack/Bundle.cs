using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ThreatsManager.Mitre.Attack
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Bundle
    {
        [JsonProperty("type")]
        public string Type;

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("spec_version")]
        public string SpecVersion;

        [JsonProperty("objects")]
        public IEnumerable<AttackObject> Objects;

        public DateTime GetLastChangeDateTime()
        {
            var result = DateTime.MinValue;

            var objs = Objects?.ToArray();
            if (objs?.Any() ?? false)
            {
                foreach (var obj in objs)
                {
                    if (obj.Modified > result)
                        result = obj.Modified;
                }
            }

            return result;
        }
    }
}
