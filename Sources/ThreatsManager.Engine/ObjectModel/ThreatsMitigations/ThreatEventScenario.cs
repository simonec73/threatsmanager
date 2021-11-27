using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
    [IdentityAspect]
    [PropertiesContainerAspect]
    [ThreatModelChildAspect]
    [ThreatEventChildAspect]
    [TypeLabel("Scenario")]
    public class ThreatEventScenario : IThreatEventScenario, IInitializableObject
    {
        public ThreatEventScenario()
        {

        }

        public ThreatEventScenario([NotNull] IThreatEvent threatEvent, [NotNull] IThreatActor actor, string name = null) : this()
        {
            _id = Guid.NewGuid();
            _model = threatEvent.Model;
            _modelId = threatEvent.Model.Id;
            _threatEvent = threatEvent;
            _threatEventId = threatEvent.Id;
            _actorId = actor.Id;
            _actor = actor;
            Name = string.IsNullOrWhiteSpace(name) ? actor.Name : name;
            Description = actor.Description;
    
            _model.AutoApplySchemas(this);
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Specific implementation.
        public Scope PropertiesScope => Scope.ThreatEventScenario;

        [JsonProperty("severity")]
        private int _severityId;

        public int SeverityId => _severityId;

        private ISeverity _severity;

        [InitializationRequired]
        public ISeverity Severity
        {
            get => _severity ?? (_severity = Model.GetSeverity(_severityId));

            set
            {
                if (value != null && value.Equals(Model.GetSeverity(value.Id)))
                {
                    _severity = value;
                    _severityId = value.Id;
                }
                else
                {
                    _severity = null;
                    _severityId = (int)DefaultSeverity.Unknown;
                }
            }
        }

        [JsonProperty("actorId")]
        private Guid _actorId;

        private IThreatActor _actor;

        public Guid ActorId => _actorId;

        [InitializationRequired]
        public IThreatActor Actor
        {
            get => _actor ?? (_actor = Model.GetThreatActor(_actorId));
            set
            {
                if (value != null && value.Equals(Model.GetThreatActor(value.Id)))
                {
                    _actor = value;
                    _actorId = value.Id;
                }
                else
                {
                    _actor = null;
                    _actorId = Guid.Empty;
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
                result = new ThreatEventScenario()
                {
                    _id = _id,
                    Name = Name,
                    Description = Description,
                    _modelId = model.Id,
                    _model = model,
                    _actorId = _actorId,
                    _severityId = _severityId,
                    _threatEventId = _threatEventId,
                    Motivation = Motivation,
                };
                this.CloneProperties(result);

                container.Add(result);
            }

            return result;

        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        #endregion

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

        public IThreatModel Model { get; }

        public IThreatEvent ThreatEvent { get; }

        public event Action<IDirty, bool> DirtyChanged;
        public bool IsDirty { get; }
        public void SetDirty()
        {
        }

        public void ResetDirty()
        {
        }

        public bool IsDirtySuspended { get; }
        public void SuspendDirty()
        {
        }

        public void ResumeDirty()
        {
        }
        #endregion

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        private List<IProperty> _properties { get; set; }
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
        private Guid _threatEventId { get; set; }
        private IThreatEvent _threatEvent { get; set; }
        #endregion
    }
}