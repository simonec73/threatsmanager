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
using ThreatsManager.Engine.Aspects;
using PostSharp.Patterns.Collections;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [ThreatTypeIdChanger]
    [WeaknessIdChanger]
    [PropertiesContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class ThreatTypeWeakness : IThreatTypeWeakness, IInitializableObject
    {
        public ThreatTypeWeakness()
        {

        }

        public ThreatTypeWeakness([NotNull] IThreatType threatType, 
            [NotNull] IWeakness weakness) : this()
        {
            _model = threatType.Model;
            _threatType = threatType;
            _weakness = weakness;
        }

        public bool IsInitialized => Model != null && _threatTypeId != Guid.Empty && _weaknessId != Guid.Empty;

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
        public Scope PropertiesScope => Scope.ThreatTypeMitigation;

        [JsonProperty("threatTypeId")]
        [NotRecorded]
        private Guid _threatTypeId { get; set; }

        public Guid ThreatTypeId => _threatTypeId;

        [Reference]
        [NotRecorded]
        [UpdateThreatTypeId]
        private IThreatType _threatType;

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IThreatType ThreatType => _threatType ?? (_threatType = Model.GetThreatType(_threatTypeId));

        [JsonProperty("weaknessId")]
        [NotRecorded]
        private Guid _weaknessId { get; set; }

        public Guid WeaknessId => _weaknessId;

        [Reference]
        [NotRecorded]
        [UpdateWeaknessId]
        private IWeakness _weakness;

        [IgnoreAutoChangeNotification]
        public IWeakness Weakness => _weakness ?? (_weakness = Model.GetWeakness(_weaknessId));

        public IThreatTypeWeakness Clone(IThreatTypeWeaknessesContainer container)
        {
            ThreatTypeWeakness result = null;

            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Threat Type Weakness"))
                {
                    result = new ThreatTypeWeakness()
                    {
                        _model = model,
                        _threatTypeId = _threatTypeId,
                        _weaknessId = _weaknessId
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
            return Weakness == null ? ThreatModelManager.Undefined : Weakness.ToString();
        }
        #endregion
    }
}