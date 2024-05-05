using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
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
    [ThreatActorIdChanger]
    [SeverityIdChanger]
    [ThreatEventIdChanger]
    [ThreatEventChildAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Scenario")]
    public class ThreatEventScenario : IThreatEventScenario, IInitializableObject, IForceSetId
    {
        public ThreatEventScenario()
        {

        }

        public ThreatEventScenario([NotNull] IThreatEvent threatEvent, [NotNull] IThreatActor actor, string name = null) : this()
        {
            _id = Guid.NewGuid();
            _model = threatEvent.Model;
            _threatEvent = threatEvent;
            _actor = actor;
            Name = string.IsNullOrWhiteSpace(name) ? actor.Name : name;
            Description = actor.Description;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        public bool HasProperty(IPropertyType propertyType)
        {
            return false;
        }
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IProperty> Properties { get; }
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

        [Reference]
        [field: NotRecorded]
        public IThreatEvent ThreatEvent { get; }

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
        [Reference]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [JsonProperty("threatEventId")]
        private Guid _threatEventId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateThreatEventId]
        private IThreatEvent _threatEvent { get; set; }
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
        public Scope PropertiesScope => Scope.ThreatEventScenario;

        public void SetId(Guid id)
        {
            _id = id;
        }

        [NotRecorded]
        [JsonProperty("severity")]
        private int _severityId { get; set; }

        public int SeverityId => _severityId;

        [NotRecorded]
        [Reference]
        [UpdateSeverityId]
        private ISeverity _severity;

        [InitializationRequired]
        [SafeForDependencyAnalysis]
        public ISeverity Severity
        {
            get => _severity ?? (_severity = Model.GetSeverity(_severityId));

            set
            {
                if (value != null && value.Equals(Model.GetSeverity(value.Id)))
                {
                    _severity = value;
                }
                else
                {
                    _severity = null;
                }
            }
        }

        [JsonProperty("actorId")]
        [NotRecorded]
        private Guid _actorId { get; set; }

        [Reference]
        [NotRecorded]
        [UpdateActorId]
        private IThreatActor _actor;

        public Guid ActorId => _actorId;

        [InitializationRequired]
        [SafeForDependencyAnalysis]
        public IThreatActor Actor
        {
            get => _actor ?? (_actor = Model?.GetThreatActor(_actorId));
            set
            {
                if (value != null && value.Equals(Model?.GetThreatActor(value.Id)))
                {
                    _actor = value;
                }
                else
                {
                    _actor = null;
                }
            }
        }

        [JsonProperty("motivation")]
        public string Motivation { get; set; }

        public IThreatEventScenario Clone([NotNull] IThreatEventScenariosContainer container)
        {
            IThreatEventScenario result = null;

            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Scenario"))
                {
                    result = new ThreatEventScenario()
                    {
                        _id = _id,
                        Name = Name,
                        Description = Description,
                        _model = model,
                        _actorId = _actorId,
                        _severityId = _severityId,
                        _threatEventId = _threatEventId,
                        Motivation = Motivation,
                    };
                    container.Add(result);
                    this.CloneProperties(result);

                    if (model.Id != _modelId)
                        result.SetSourceInfo(Model);

                    scope?.Complete();
                }
            }

            return result;

        }

        public override string ToString()
        {
            return Name ?? ThreatModelManager.Undefined;
        }
        #endregion
    }
}