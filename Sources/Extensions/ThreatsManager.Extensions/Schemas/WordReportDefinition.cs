using Newtonsoft.Json;
using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Extensions.Schemas
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    public class WordReportDefinition
    {
        public WordReportDefinition(string name, string pathname) 
        { 
            Name = name;
            Path = pathname;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }
}
