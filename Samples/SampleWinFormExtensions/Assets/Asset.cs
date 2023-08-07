using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using System;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class Asset : ICloneable
    {
        public Asset([Required] string name)
        {
            Name = name;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }

        public object Clone()
        {
            return new Asset(Name)
            {
                Description = Description,
                Value = Value
            };
        }
    }
}
