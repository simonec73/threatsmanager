using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    class ArtifactSettings
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ArtifactType ArtifactType { get; set; }
    }
}