using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    [ThreatModelChildAspect]
    [ThreatEventChildAspect]
    [PropertiesContainerAspect]
    public class ThreatEventMitigation : IThreatEventMitigation, IInitializableObject
    {
        public ThreatEventMitigation()
        {

        }

        public ThreatEventMitigation([NotNull] IThreatEvent threatEvent, [NotNull] IMitigation mitigation, IStrength strength) : this()
        {
            _model = threatEvent.Model;
            _modelId = threatEvent.Model.Id;
            _threatEventId = threatEvent.Id;
            _threatEvent = threatEvent;
            _mitigationId = mitigation.Id;
            _mitigation = mitigation;
            Strength = strength;

            _model.AutoApplySchemas(this);
        }

        public bool IsInitialized => Model != null && _threatEventId != Guid.Empty && _mitigationId != Guid.Empty;

        #region Specific implementation.
        public Scope PropertiesScope => Scope.ThreatEventMitigation;

        [JsonProperty("mitigationId")]
        private Guid _mitigationId;

        public Guid MitigationId => _mitigationId;

        private IMitigation _mitigation;

        public IMitigation Mitigation => _mitigation ?? (_mitigation = Model.GetMitigation(_mitigationId));

        [JsonProperty("directives")]
        public string Directives { get; set; }

        [JsonProperty("strength")]
        private int _strengthId;

        public int StrengthId => _strengthId;

        private IStrength _strength;

        [InitializationRequired]
        public IStrength Strength
        {
            get => _strength ?? (_strength = Model?.GetStrength(_strengthId));

            set
            {
                if (value != null && value.Equals(Model.GetStrength(value.Id)))
                {
                    _strength = value;
                    _strengthId = value.Id;
                    SetDirty();
                }
            }
        }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MitigationStatus Status { get; set; }

        public IThreatEventMitigation Clone(IThreatEventMitigationsContainer container)
        {
            ThreatEventMitigation result = null;

            if (container is IThreatEvent threatEvent && threatEvent.Model is IThreatModel model)
            {
                result = new ThreatEventMitigation
                {
                    _threatEventId = threatEvent.Id,
                    _mitigationId = MitigationId,
                    _model = model,
                    _modelId = model.Id,
                    Directives = Directives,
                    _strengthId = _strengthId,
                    Status = Status,
                };
                this.CloneProperties(result);
                container.Add(result);
            }

            return result;
        }

        public override string ToString()
        {
            return Mitigation.Name ?? "<undefined>";
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
            throw new NotImplementedException();
        }

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
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
        private List<IProperty> _properties { get; set; }
        private Guid _threatEventId { get; set; }
        private IThreatEvent _threatEvent { get; set; }
        #endregion
    }
}