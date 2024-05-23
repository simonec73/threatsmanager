using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Engine.Aspects;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [IdentityAspect]
    [PropertiesContainerAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [SeverityIdChanger]
    [Recordable(AutoRecord = false)]
    [SourceInfoAspect]
    [Undoable]
    [TypeLabel("Weakness")]
    public partial class Weakness : IWeakness, IInitializableObject, IForceSetId
    {
        public Weakness()
        {
        }
        
        public Weakness([Required] string name, [NotNull] ISeverity severity) : this()
        {
            _id = Guid.NewGuid();
            Name = name;
            _severity = severity;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementations.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IProperty> Properties { get; }
        public bool HasProperty(IPropertyType propertyType)
        {
            return false;
        }
        public IProperty GetProperty(IPropertyType propertyType)
        {
            return null;
        }

        public IProperty AddProperty(IPropertyType propertyType, string value)
        {
            return null;
        }

        public bool RemoveProperty(IPropertyType propertyType)
        {
            return false;
        }

        public bool RemoveProperty(Guid propertyTypeId)
        {
            return false;
        }

        public void ClearProperties()
        {
        }

        public void Apply(IPropertySchema schema)
        {
        }

        public void Unapply(IPropertySchema schema)
        {
        }

        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }

        public Guid SourceTMId { get; }

        public string SourceTMName { get; }

        public string VersionId { get; }

        public string VersionAuthor { get; }

        public void SetSourceInfo(IThreatModel source)
        {
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("name")]
        protected string _name { get; set; }
        [JsonProperty("description")]
        protected string _description { get; set; }
        [Child]
        [JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects)]
        private AdvisableCollection<IProperty> _properties { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Parent]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
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
        public Scope PropertiesScope => Scope.Weakness;

        public void SetId(Guid id)
        {
            _id = id;
        }

        [JsonProperty("severity")]
        private int _severityId { get; set; }

        public int SeverityId => _severityId;

        [Reference]
        [NotRecorded]
        [UpdateSeverityId]
        private ISeverity _severity;

        [InitializationRequired]
        [SafeForDependencyAnalysis]
        [property: NotRecorded]
        public ISeverity Severity
        {
            get
            {
                if ((_severity?.Id ?? -1) != _severityId)
                    _severity = Model?.GetSeverity(_severityId);

                return _severity;
            }

            set
            {
                if (value != null && value.Equals(Model.GetSeverity(value.Id)))
                {
                    _severity = value;
                }
            }
        }

        public MitigationLevel GetMitigationLevel()
        {
            MitigationLevel result = MitigationLevel.NotMitigated;

            var mitigations = Mitigations?.ToArray();
            var level = 0;

            if (mitigations?.Any() ?? false)
            {
                foreach (var m in mitigations)
                {
                    level += m.StrengthId;
                }
            }

            if (level >= 100)
                result = MitigationLevel.Complete;
            else if (level > 0)
                result = MitigationLevel.Partial;

            return result;
        }

        public IWeakness Clone([NotNull] IWeaknessesContainer container)
        {
            Weakness result = null;

            if (container is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Weakness"))
                {
                    result = new Weakness()
                    {
                        _id = Id,
                        Name = Name,
                        Description = Description,
                        _model = model,
                        _severityId = _severityId
                    };
                    container.Add(result);
                    this.CloneProperties(result);

                    if (_mitigations?.Any() ?? false)
                    {
                        foreach (var mitigation in _mitigations)
                        {
                            mitigation.Clone(result);
                        }
                    }

                    if (model.Id != _modelId)
                        result.SetSourceInfo(Model);

                    scope?.Complete();
                }
            }

            return result;
        }

        [InitializationRequired]
        public IVulnerability GenerateVulnerability()
        {
            return new Vulnerability(this);
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? ThreatModelManager.Undefined : Name;
        }
        #endregion
    }
}