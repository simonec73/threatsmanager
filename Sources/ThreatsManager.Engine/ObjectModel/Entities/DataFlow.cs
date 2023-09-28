using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Entities
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [Serializable]
    [IdentityAspect]
    [PropertiesContainerAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [ThreatEventsContainerAspect]
    [VulnerabilitiesContainerAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Flow")]
    [TypeInitial("F")]
    public class DataFlow : IDataFlow, IInitializableObject
    {
        public DataFlow()
        {
            
        }

        public DataFlow([Required] string name, Guid sourceId, Guid targetId) : this()
        {
            _id = Guid.NewGuid();
            Name = name;
            _sourceId = sourceId;
            _targetId = targetId;
            FlowType = FlowType.ReadWriteCommand;
        }

        public DataFlow([Required] string name, [NotNull] IEntity source, [NotNull] IEntity target) : this(name, source.Id, target.Id)
        {
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementation.
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

        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAdded;
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemoved;

        [Reference]
        [field: NotRecorded]
        public IEnumerable<IThreatEvent> ThreatEvents { get; }
        public IThreatEvent GetThreatEvent(Guid id)
        {
            return null;
        }

        public IThreatEvent GetThreatEventByThreatType(Guid threatTypeId)
        {
            return null;
        }

        public void Add(IThreatEvent threatEvent)
        {
        }

        public IThreatEvent AddThreatEvent(IThreatType threatType)
        {
            return null;
        }

        public bool RemoveThreatEvent(Guid id)
        {
            return false;
        }

        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }

        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityAdded;
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityRemoved;
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IVulnerability> Vulnerabilities { get; }
        public IVulnerability GetVulnerability(Guid id)
        {
            return null;
        }

        public IVulnerability GetVulnerabilityByWeakness(Guid weaknessId)
        {
            return null;
        }

        public void Add(IVulnerability vulnerability)
        {
        }

        public IVulnerability AddVulnerability(IWeakness weakness)
        {
            return null;
        }

        public bool RemoveVulnerability(Guid id)
        {
            return false;
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
        [Child]
        [JsonProperty("threatEvents")]
        private AdvisableCollection<ThreatEvent> _threatEvents { get; set; }
        [Child]
        [JsonProperty("vulnerabilities")]
        private AdvisableCollection<Vulnerability> _vulnerabilities { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Parent]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        #endregion    

        #region Specific implementation.
        public Scope PropertiesScope => Scope.DataFlow;

        [JsonProperty("source")]
        private Guid _sourceId { get; set; }

        public Guid SourceId => _sourceId;

        [JsonProperty("target")]
        private Guid _targetId { get; set; }

        public Guid TargetId => _targetId;

        [property: NotRecorded]
        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IEntity Source => Model.GetEntity(_sourceId);

        [property: NotRecorded]
        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IEntity Target => Model.GetEntity(_targetId);

        [JsonProperty("flowType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FlowType FlowType { get; set; }

        [JsonProperty("template")]
        internal Guid _templateId { get; set; }

        [Reference]
        [property: NotRecorded]
        internal IFlowTemplate _template { get; set; }

        [property: NotRecorded]
        [IgnoreAutoChangeNotification]
        public IFlowTemplate Template
        {
            get
            {
                if (_template == null && _templateId != Guid.Empty)
                {
                    _template = Model?.GetFlowTemplate(_templateId);
                }

                return _template;
            }
        }

        public void ResetTemplate()
        {
            using (var scope = UndoRedoManager.OpenScope("Detach from Template"))
            {
                this.ClearProperties();
                Model?.AutoApplySchemas(this);

                _templateId = Guid.Empty;
                _template = null;

                scope?.Complete();
            }
        }

        public IDataFlow Clone([NotNull] IDataFlowsContainer container)
        {
            DataFlow result = null;

            if (container is IThreatModel model)
            {
                result = new DataFlow()
                {
                    _id = Id,
                    Name = Name,
                    Description = Description,
                    _model = model,
                    _modelId = model.Id,
                    _sourceId = _sourceId,
                    _targetId = _targetId,
                    _templateId = _templateId,
                    FlowType = FlowType
                };
                this.CloneProperties(result);
                this.CloneThreatEvents(result);

                container.Add(result);
            }

            return result;
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion
    }
}
