using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    [Recordable]
    public class ThreatTypeMitigation : IThreatTypeMitigation, IInitializableObject
    {
        public ThreatTypeMitigation()
        {

        }

        public ThreatTypeMitigation([NotNull] IThreatModel model, [NotNull] IThreatType threatType, 
            [NotNull] IMitigation mitigation, IStrength strength) : this()
        {
            _model = model;
            _threatType = threatType;
            _mitigation = mitigation;
            Strength = strength;
        }

        public bool IsInitialized => Model != null && _threatTypeId != Guid.Empty && _mitigationId != Guid.Empty;

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
        #endregion

        #region Additional placeholders required.
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateId("Id", "_modelId")]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [Child]
        [JsonProperty("properties")]
        private IList<IProperty> _properties { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.ThreatTypeMitigation;

        [JsonProperty("threatTypeId")]
        [NotRecorded]
        private Guid _threatTypeId;

        public Guid ThreatTypeId => _threatTypeId;

        [Reference]
        [NotRecorded]
        private IThreatType _threatType;

        [InitializationRequired]
        public IThreatType ThreatType => _threatType ?? (_threatType = Model.GetThreatType(_threatTypeId));

        [JsonProperty("mitigationId")]
        [NotRecorded]
        private Guid _mitigationId;

        public Guid MitigationId => _mitigationId;

        [Reference]
        [NotRecorded]
        private IMitigation _mitigation;

        public IMitigation Mitigation => _mitigation ?? (_mitigation = Model.GetMitigation(_mitigationId));

        [JsonProperty("strength")]
        [NotRecorded]
        private int _strengthId;

        public int StrengthId => _strengthId;

        [Reference]
        [NotRecorded]
        [UpdateId("Id", "_strengthId")]
        private IStrength _strength;

        [InitializationRequired]
        public IStrength Strength
        {
            get => _strength ?? (_strength = Model?.GetStrength(_strengthId));

            set
            {
                if (value != null && value.Equals(Model?.GetStrength(value.Id)))
                {
                    _strength = value;
                }
            }
        }

        public IThreatTypeMitigation Clone(IThreatTypeMitigationsContainer container)
        {
            ThreatTypeMitigation result = null;

            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                result = new ThreatTypeMitigation()
                {
                    _model = model,
                    _threatTypeId = _threatTypeId,
                    _mitigationId = _mitigationId,
                    _strengthId = _strengthId,
                };
                container.Add(result);
            }

            return result;
        }

        public override string ToString()
        {
            return Mitigation.Name;
        }
        #endregion
    }
}