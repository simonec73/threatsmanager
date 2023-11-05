using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class SpecializedMitigation : ISpecializedMitigation
    {
        public SpecializedMitigation() { }

        public SpecializedMitigation([NotNull] IItemTemplate template, string name, string description) 
        {
            _templateId = template.Id;
            Name = name;
            Description = description;
        }

        [JsonProperty("template")]
        internal Guid _templateId { get; set; }

        [property: NotRecorded]
        public Guid TargetId => _templateId;

        [JsonProperty("name")]
        public string Name { get; set;}

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
