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
    public class ControlTypeAssociation
    {
        /// <summary>
        /// Identifier of the Mitigation.
        /// </summary>
        /// <remarks>If the containing <see cref="ControlTypeAssociationsDefinition"/> object
        /// specifies a Property Type for the Mitigation, then the Identifier of the Mitigation corresponds
        /// to the content of the corresponding Property, otherwise it refers to the Name.</remarks>
        [JsonProperty("id")]
        public string Identifier { get; set; }

        /// <summary>
        /// Control Type for the Mitigation.
        /// </summary>
        [JsonProperty("controlType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SecurityControlType ControlType { get; set; }
    }
}
