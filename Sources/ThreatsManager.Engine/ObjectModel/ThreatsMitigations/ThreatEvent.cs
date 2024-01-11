using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using IProperty = ThreatsManager.Interfaces.ObjectModel.Properties.IProperty;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [ParentIdChanger]
    [SeverityIdChanger]
    [ThreatTypeIdChanger]
    [PropertiesContainerAspect]
    [ThreatEventScenariosContainerAspect]
    [ThreatEventMitigationsContainerAspect]
    [VulnerabilitiesContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Threat Event")]
    public class ThreatEvent : IThreatEvent, IInitializableObject
    {
        public ThreatEvent()
        {

        }

        public ThreatEvent([NotNull] IThreatType threatType) : this()
        {
            _id = Guid.NewGuid();
            _threatType = threatType;
            _model = threatType.Model;
            Name = threatType.Name;
            Description = threatType.Description;
            Severity = threatType.Severity;
            var threatTypeProperties = threatType.Properties?.ToArray();
            if (threatTypeProperties?.Any() ?? false)
            {
                foreach (var property in threatTypeProperties)
                {
                    if (HasProperty(property.PropertyType))
                    {
                        var existingProperty = GetProperty(property.PropertyType);
                        existingProperty.StringValue = property.StringValue;
                    }
                    else
                    {
                        AddProperty(property);
                    }
                }
            }
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty && _threatTypeId != Guid.Empty;

        public static bool UseThreatTypeInfo { get; set; }

        #region Default implementation.
        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }
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

        public event Action<IThreatEventScenariosContainer, IThreatEventScenario> ScenarioAdded;
        public event Action<IThreatEventScenariosContainer, IThreatEventScenario> ScenarioRemoved;

        [Reference]
        [field: NotRecorded]
        public IEnumerable<IThreatEventScenario> Scenarios { get; }
        public IThreatEventScenario GetScenario(Guid id)
        {
            return null;
        }

        public IThreatEventScenario AddScenario(IThreatActor threatActor, ISeverity severity, string name = null)
        {
            return null;
        }

        public void Add(IThreatEventScenario scenario)
        {
        }

        public bool RemoveScenario(Guid id)
        {
            return false;
        }

        public event Action<IThreatEventMitigationsContainer, IThreatEventMitigation> ThreatEventMitigationAdded;
        public event Action<IThreatEventMitigationsContainer, IThreatEventMitigation> ThreatEventMitigationRemoved;
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityAdded;
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityRemoved;

        [Reference]
        [field: NotRecorded]
        [IgnoreAutoChangeNotification]
        public IEnumerable<IThreatEventMitigation> Mitigations { get; }
        public IThreatEventMitigation GetMitigation(Guid mitigationId)
        {
            return null;
        }

        public IThreatEventMitigation AddMitigation(IMitigation mitigation, IStrength strength, 
            MitigationStatus status = MitigationStatus.Proposed, string directives = null)
        {
            return null;
        }

        public void Add(IThreatEventMitigation mitigation)
        {
        }

        public bool RemoveMitigation(Guid mitigationId)
        {
            return false;
        }

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

        public Guid SourceTMId { get; }

        public string SourceTMName { get; }

        public string VersionId { get; }

        public string VersionAuthor { get; }

        public void SetSourceInfo(IThreatModel source)
        {
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [Child]
        [JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects)]
        private AdvisableCollection<IProperty> _properties { get; set; }
        [Child]
        [JsonProperty("scenarios")]
        private AdvisableCollection<ThreatEventScenario> _scenarios { get; set; }
        [Child]
        [JsonProperty("mitigations")]
        private AdvisableCollection<ThreatEventMitigation> _mitigations { get; set; }
        [Child]
        [JsonProperty("vulnerabilities")]
        private AdvisableCollection<Vulnerability> _vulnerabilities { get; set; }
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
        public Scope PropertiesScope => Scope.ThreatEvent;

        [JsonProperty("id")]
        private Guid _id { get; set; }

        public Guid Id => _id;

        [JsonProperty("name")] 
        private string _name { get; set; }

        [property: NotRecorded]
        [SafeForDependencyAnalysis]
        public string Name
        {
            get => UseThreatTypeInfo ? ThreatType?.Name : _name;

            set => _name = value;
        }

        [JsonProperty("description")]
        private string _description { get; set; }

        [property: NotRecorded]
        [SafeForDependencyAnalysis]
        public string Description         
        {
            get => UseThreatTypeInfo ? ThreatType?.Description : _description;

            set => _description = value;
        }

        [JsonProperty("parent")]
        [field: NotRecorded]
        private Guid _parentId { get; set; }

        [Parent]
        [field: NotRecorded]
        [field: UpdateParentId]
        [field: AutoApplySchemas]
        private IIdentity _parent { get; set;}

        public Guid ParentId => _parentId;

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IIdentity Parent
        {
            get
            {
                if (_parent == null)
                    _parent = Model.GetEntity(_parentId);

                if (_parent == null)
                    _parent = Model.GetDataFlow(_parentId);

                if (_parent == null && _parentId == _modelId)
                    _parent = _model;

                return _parent;
            }
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

        [NotRecorded]
        [JsonProperty("threatTypeId")]
        private Guid _threatTypeId { get; set; }

        public Guid ThreatTypeId => _threatTypeId;

        [Reference]
        [NotRecorded]
        [UpdateThreatTypeId]
        private IThreatType _threatType;

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IThreatType ThreatType => _threatType ?? (_threatType = Model?.GetThreatType(_threatTypeId));

        public IEnumerable<IVulnerability> Vulnerabilities => throw new NotImplementedException();

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

        public IThreatEvent Clone([NotNull] IThreatEventsContainer container)
        {
            ThreatEvent result = null;

            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Threat Event"))
                {
                    result = new ThreatEvent()
                    {
                        _id = _id,
                        _model = model,
                        _modelId = model.Id,
                        Name = Name,
                        Description = Description,
                        _parentId = _parentId,
                        _severityId = _severityId,
                        _threatTypeId = _threatTypeId
                    };
                    container.Add(result);
                    this.CloneProperties(result);
                    this.CloneVulnerabilities(result);

                    var scenarios = Scenarios?.ToArray();
                    if (scenarios?.Any() ?? false)
                    {
                        foreach (var scenario in scenarios)
                        {
                            scenario.Clone(result);
                        }
                    }

                    var mitigations = Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var mitigation in mitigations)
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

        private void AddProperty([NotNull] IProperty property)
        {
            var shadowClass = property.GetType().GetCustomAttributes<AssociatedPropertyClassAttribute>().FirstOrDefault();
            if (shadowClass != null)
            {
                var shadowClassType = Type.GetType(shadowClass.AssociatedType, false);
                if (shadowClassType != null)
                {
                    using (var scope = UndoRedoManager.OpenScope("Add Property to Threat Event"))
                    {
                        var shadowProperty = Activator.CreateInstance(shadowClassType, property) as IProperty;

                        if (shadowProperty != null)
                        {
                            if (_properties == null)
                                _properties = new AdvisableCollection<IProperty>();

                            UndoRedoManager.Attach(shadowProperty, Model);
                            _properties.Add(shadowProperty);
                            PropertyAdded?.Invoke(this, shadowProperty);
                            shadowProperty.Changed += delegate (IProperty prop)
                            {
                                PropertyValueChanged?.Invoke(this, prop);
                            };

                            scope?.Complete();
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion
    }
}