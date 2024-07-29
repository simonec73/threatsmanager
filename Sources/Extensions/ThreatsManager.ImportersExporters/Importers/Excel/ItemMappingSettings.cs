using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ItemMappingSettings
    {
        [JsonProperty("items")]
        public List<ItemDetails> Items { get; set; }
    }
}
