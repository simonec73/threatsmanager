using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using PostSharp.Aspects.Advices;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using IProperty = ThreatsManager.Interfaces.ObjectModel.Properties.IProperty;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    [ThreatEventScenariosContainerAspect]
    [ThreatEventMitigationsContainerAspect]
    [TypeLabel("Threat Event")]
    public class ThreatEvent : IThreatEvent, IInitializableObject
    {
        public ThreatEvent()
        {

        }

        public ThreatEvent([NotNull] IThreatModel model, [NotNull] IThreatType threatType, [NotNull] IIdentity parent) : this()
        {
            _id = Guid.NewGuid();
            _model = model;
            _modelId = model.Id;
            _parentId = parent.Id;
            _parent = parent;
            _threatType = threatType;
            _threatTypeId = threatType.Id;
            Name = threatType.Name;
            Description = threatType.Description;
            Severity = threatType.Severity;
            var threatTypeProperties = threatType.Properties?.ToArray();
            if (threatTypeProperties?.Any() ?? false)
            {
                foreach (var property in threatTypeProperties)
                {
                    AddProperty(property);
                }
            }

            model.AutoApplySchemas(this);
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty && _threatTypeId != Guid.Empty;

        public static bool UseThreatTypeInfo { get; set; }

        #region Specific implementation.
        public Scope PropertiesScope => Scope.ThreatEvent;

        [JsonProperty("id")]
        private Guid _id { get; set; }

        public Guid Id => _id;

        [JsonProperty("name")] 
        private string _name { get; set; }

        public string Name
        {
            get => UseThreatTypeInfo ? ThreatType?.Name : _name;

            set => _name = value;
        }

        [JsonProperty("description")]
        private string _description { get; set; }

        public string Description         
        {
            get => UseThreatTypeInfo ? ThreatType?.Description : _description;

            set => _description = value;
        }

        [JsonProperty("parent")]
        private Guid _parentId;

        private IIdentity _parent;

        public Guid ParentId => _parentId;

        [InitializationRequired]
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
        private int _severityId;

        public int SeverityId => _severityId;

        private ISeverity _severity;

        [InitializationRequired]
        public ISeverity Severity
        {
            get => _severity ?? (_severity = Model?.GetSeverity(_severityId));

            set
            {
                if (value != null && value.Equals(Model.GetSeverity(value.Id)))
                {
                    _severity = value;
                    _severityId = value.Id;
                    SetDirty();
                }
            }
        }

        [JsonProperty("threatTypeId")]
        private Guid _threatTypeId;

        public Guid ThreatTypeId => _threatTypeId;

        private IThreatType _threatType;

        [InitializationRequired]
        public IThreatType ThreatType => _threatType ?? (_threatType = Model.GetThreatType(_threatTypeId));

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
                this.CloneProperties(result);

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

                container.Add(result);
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
                    var shadowProperty = Activator.CreateInstance(shadowClassType, property) as IProperty;

                    if (shadowProperty != null)
                    {
                        if (_properties == null)
                            _properties = new List<IProperty>();
                        _properties.Add(shadowProperty);
                        SetDirty();
                        PropertyAdded?.Invoke(this, shadowProperty);
                        shadowProperty.Changed += delegate(IProperty prop)
                        {
                            PropertyValueChanged?.Invoke(this, prop);
                        };
                    }
                }
            }
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion

        #region Default implementation.
        public IThreatModel Model { get; }
        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
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

        public event Action<IThreatEventScenariosContainer, IThreatEventScenario> ScenarioAdded;
        public event Action<IThreatEventScenariosContainer, IThreatEventScenario> ScenarioRemoved;

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
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
        private List<IProperty> _properties { get; set; }
        private List<IThreatEventScenario> _scenarios { get; set; }
        private List<IThreatEventMitigation> _mitigations { get; set; }
        #endregion

        #region Implementation of interface IThreatEventVulnerabilitiesContainer.
        private Action<IThreatEventVulnerabilitiesContainer, IThreatEventVulnerability> _threatEventVulnerabilityAdded;
        public event Action<IThreatEventVulnerabilitiesContainer, IThreatEventVulnerability> ThreatEventVulnerabilityAdded
        {
            add
            {
                if (_threatEventVulnerabilityAdded == null || !_threatEventVulnerabilityAdded.GetInvocationList().Contains(value))
                {
                    _threatEventVulnerabilityAdded += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_threatEventVulnerabilityAdded != null) _threatEventVulnerabilityAdded -= value;
            }
        }

        private Action<IThreatEventVulnerabilitiesContainer, IThreatEventVulnerability> _threatEventVulnerabilityRemoved;
        public event Action<IThreatEventVulnerabilitiesContainer, IThreatEventVulnerability> ThreatEventVulnerabilityRemoved
        {
            add
            {
                if (_threatEventVulnerabilityRemoved == null || !_threatEventVulnerabilityRemoved.GetInvocationList().Contains(value))
                {
                    _threatEventVulnerabilityRemoved += value;
                }
            }
            remove
            {
                // ReSharper disable once DelegateSubtraction
                if (_threatEventVulnerabilityRemoved != null) _threatEventVulnerabilityRemoved -= value;
            }
        }

        [JsonProperty("threatEventVulnerabilities")]
        private List<IThreatEventVulnerability> _vulnerabilities { get; set; }

        public IEnumerable<IThreatEventVulnerability> ThreatEventVulnerabilities => _vulnerabilities?.AsReadOnly();

        public IThreatEventVulnerability GetThreatEventVulnerability(Guid vulnerabilityId)
        {
            return _vulnerabilities?.FirstOrDefault(x => x.VulnerabilityId == vulnerabilityId);
        }

        public void Add([NotNull] IThreatEventVulnerability association)
        {
            if (_vulnerabilities == null)
                _vulnerabilities = new List<IThreatEventVulnerability>();
            if (association.ThreatEvent != this || Model.FindVulnerability(association.VulnerabilityId) == null)
                throw new ArgumentException();

            _vulnerabilities.Add(association);
        }

        public IThreatEventVulnerability AddThreatEventVulnerability([NotNull] IVulnerability vulnerability)
        {
            IThreatEventVulnerability result = null;

            result = new ThreatEventVulnerability(this, vulnerability);
            if (_vulnerabilities == null)
                _vulnerabilities = new List<IThreatEventVulnerability>();
            _vulnerabilities.Add(result);
            if (Model is IDirty dirtyObject)
                dirtyObject.SetDirty();
            _threatEventVulnerabilityAdded?.Invoke(this, result);

            return result;
        }

        public bool RemoveThreatEventVulnerability(Guid vulnerabilityId)
        {
            bool result = false;

            var vulnerability = GetThreatEventVulnerability(vulnerabilityId);
            if (vulnerability != null)
            {
                result = _vulnerabilities.Remove(vulnerability);
                if (result)
                {
                    if (Model is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                    _threatEventVulnerabilityRemoved?.Invoke(this, vulnerability);
                }
            }

            return result;
        }

        public IEnumerable<IThreatEventMitigation> GetEffectiveMitigations()
        {
            IEnumerable<IThreatEventMitigation> result = null;

            var list = new List<IThreatEventMitigation>();
            if (_mitigations?.Any() ?? false)
                list.AddRange(_mitigations);

            var vulnerabilities = _vulnerabilities?.ToArray();
            if (vulnerabilities?.Any() ?? false)
            {
                Dictionary<Guid, IVulnerabilityMitigation> selected = new Dictionary<Guid, IVulnerabilityMitigation>();

                foreach (var vulnerability in vulnerabilities)
                {
                    var mitigations = vulnerability.Vulnerability?.Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var mitigation in mitigations)
                        {
                            var current = selected.Where(x => x.Key == mitigation.MitigationId)
                                .Select(x => x.Value).FirstOrDefault();

                            if (current == null)
                            {
                                selected.Add(mitigation.MitigationId, mitigation);
                            }
                            else if ((mitigation.Strength?.Id ?? 0) > (current.Strength?.Id ?? 0))
                            {
                                selected[mitigation.MitigationId] = mitigation;
                            }
                        }
                    }
                }

                if (selected.Any())
                {
                    foreach (var item in selected.Values)
                    {
                        if (list.All(x => x.MitigationId != item.MitigationId))
                        {
                            list.Add(new ThreatEventMitigation(this, item.Mitigation, item.Strength)
                            {
                                Directives = "Mitigation inherited from Vulnerabilities"
                            });
                        }
                    }
                }
            }

            if (list.Any())
                result = list.AsReadOnly();

            return result;
        }
        #endregion
    }
}