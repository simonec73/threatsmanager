﻿using System;
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
    [AutoDirty]
    [DirtyAspect]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    [Recordable]
    public class WeaknessMitigation : IWeaknessMitigation, IInitializableObject
    {
        public WeaknessMitigation()
        {

        }

        public WeaknessMitigation([NotNull] IThreatModel model, [NotNull] IWeakness weakness, 
            [NotNull] IMitigation mitigation, IStrength strength) : this()
        {
            _model = model;
            _modelId = model.Id;
            _weaknessId = weakness.Id;
            _weakness = weakness;
            _mitigationId = mitigation.Id;
            _mitigation = mitigation;
            Strength = strength;
        }

        public bool IsInitialized => Model != null && _weaknessId != Guid.Empty && _mitigationId != Guid.Empty;

        #region Specific implementation.
        public Scope PropertiesScope => Scope.WeaknessMitigation;

        [JsonProperty("weaknessId")]
        private Guid _weaknessId;

        public Guid WeaknessId => _weaknessId;

        [Reference]
        private IWeakness _weakness;

        [InitializationRequired]
        public IWeakness Weakness => _weakness ?? (_weakness = Model.GetWeakness(_weaknessId));

        [JsonProperty("mitigationId")]
        private Guid _mitigationId;

        public Guid MitigationId => _mitigationId;

        [Reference]
        private IMitigation _mitigation;

        public IMitigation Mitigation => _mitigation ?? (_mitigation = Model.GetMitigation(_mitigationId));

        [JsonProperty("strength")]
        private int _strengthId;

        public int StrengthId => _strengthId;

        [Reference]
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

        public IWeaknessMitigation Clone(IWeaknessMitigationsContainer container)
        {
            WeaknessMitigation result = null;

            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                result = new WeaknessMitigation()
                {
                    _model = model,
                    _modelId = model.Id,
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
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Parent]
        [field: NotRecorded]
        [field: UpdateId("Id", "_modelId")]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [Child]
        [JsonProperty("properties")]
        private IList<IProperty> _properties { get; set; }
        #endregion
    }
}