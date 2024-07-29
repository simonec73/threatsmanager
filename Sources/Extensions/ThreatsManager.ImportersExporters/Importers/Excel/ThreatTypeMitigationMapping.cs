using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ThreatTypeMitigationMapping
    {
        /// <summary>
        /// Identifier of the Threat Type.
        /// </summary>
        /// <remarks>If the containing <see cref="ThreatTypeMitigationMappingsDefinition"/> object
        /// specifies a Property Type for the Threat Type, then the Identifier of the Threat Type corresponds
        /// to the content of the corresponding Property, otherwise it refers to the Name.</remarks>
        [JsonProperty("threatTypeId")]
        public string ThreatTypeIdentifier { get; set; }

        /// <summary>
        /// Identifier of the Mitigation.
        /// </summary>
        /// <remarks>If the containing <see cref="ThreatTypeMitigationMappingsDefinition"/> object
        /// specifies a Property Type for the Mitigation, then the Identifier of the Mitigation corresponds
        /// to the content of the corresponding Property, otherwise it refers to the Name.</remarks>
        [JsonProperty("mitigationId")]
        public string MitigationIdentifier { get; set; }

        /// <summary>
        /// Identifies multiple threats to be associated with the mitigation, 
        /// with the relative strength of the mitigation for the threat.
        /// </summary>
        /// <remarks>It is a multi-line string where each line has the name of the threat to be associated,
        /// with the strength between parenthesis. For example "My threat (Average)".<para/>
        /// If defined, <see cref="ThreatTypeIdentifier"/> is ignored.<para/>
        /// It has the precedence over <see cref="BulkMitigation"/>.</remarks>
        [JsonProperty("bulkThreats")]
        public string BulkThreats { get; set; }

        /// <summary>
        /// Identifies multiple mitigations to be associated with the threat, 
        /// with the relative strength of the mitigation for the threat.
        /// </summary>
        /// <remarks>It is a multi-line string where each line has the name of the mitigation to be associated,
        /// with the strength between parenthesis. For example "My mitigation (Average)".<para/>
        /// If defined, <see cref="MitigationIdentifier"/> is ignored.<para/>
        /// <see cref="BulkThreats"/> has precedence over this.</remarks>
        [JsonProperty("bulkMitigation")]
        public string BulkMitigation { get; set; }

        /// <summary>
        /// Strength of the Mitigation relative to the threat.
        /// </summary>
        /// <remarks>It is ignored for every bulk load.</remarks>
        [JsonProperty("strength")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DefaultStrength Strength { get; set; }
    }
}
