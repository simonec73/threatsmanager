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
    [NotifyPropertyChanged]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class WeaknessMitigation : IWeaknessMitigation, IInitializableObject
    {
        public WeaknessMitigation()
        {

        }

        public WeaknessMitigation([NotNull] IThreatModel model, [NotNull] IWeakness weakness, 
            [NotNull] IMitigation mitigation, IStrength strength) : this()
        {
            _model = model;
            _weakness = weakness;
            _mitigation = mitigation;
            Strength = strength;
        }

        public bool IsInitialized => Model != null && _weaknessId != Guid.Empty && _mitigationId != Guid.Empty;

        #region Default implementation.
        [Reference]
        public IThreatModel Model { get; }

        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        [Reference]
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
        public Scope PropertiesScope => Scope.WeaknessMitigation;

        [JsonProperty("weaknessId")]
        [NotRecorded]
        private Guid _weaknessId;

        public Guid WeaknessId => _weaknessId;

        [Reference]
        [UpdateId("Id", "_weaknessId")]
        [NotRecorded]
        private IWeakness _weakness;

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IWeakness Weakness => _weakness ?? (_weakness = Model.GetWeakness(_weaknessId));

        [JsonProperty("mitigationId")]
        [NotRecorded]
        private Guid _mitigationId;

        public Guid MitigationId => _mitigationId;

        [Reference]
        [NotRecorded]
        [UpdateId("Id", "_mitigationId")]
        private IMitigation _mitigation;

        [IgnoreAutoChangeNotification]
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
        [SafeForDependencyAnalysis]
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

        public IWeaknessMitigation Clone(IWeaknessMitigationsContainer container)
        {
            WeaknessMitigation result = null;

            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                result = new WeaknessMitigation()
                {
                    _model = model,
                    _weaknessId = _weaknessId,
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