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
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class SpecializedMitigation : ISpecializedMitigation
    {
        public SpecializedMitigation() { }

        public SpecializedMitigation([NotNull] IItemTemplate template, [Required] string name, string description) 
        {
            _templateId = template.Id;
            Name = name;
            Description = description;
        }

        #region Default implementation.
        public Guid SourceTMId { get; }

        public string SourceTMName { get; }

        public string VersionId { get; }

        public string VersionAuthor { get; }

        public void SetSourceInfo(IThreatModel source)
        {
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("sourceTMId")]
        protected Guid _sourceTMId { get; set; }
        [JsonProperty("sourceTMName")]
        protected string _sourceTMName { get; set; }
        [JsonProperty("versionId")]
        protected string _versionId { get; set; }
        [JsonProperty("versionAuthor")]
        protected string _versionAuthor { get; set; }
        #endregion

        #region Specific implementation.
        [JsonProperty("template")]
        internal Guid _templateId { get; set; }

        [property: NotRecorded]
        public Guid TargetId => _templateId;

        [JsonProperty("name")]
        public string Name { get; set;}

        [JsonProperty("description")]
        public string Description { get; set; }
        #endregion
    }
}
