﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [MitigationIdChanger]
    [StrengthIdChanger]
    [ThreatEventIdChanger]
    [ThreatEventChildAspect]
    [PropertiesContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class ThreatEventMitigation : IThreatEventMitigation, IInitializableObject
    { 
        public ThreatEventMitigation()
        {

        }

        public ThreatEventMitigation([NotNull] IThreatEvent threatEvent, [NotNull] IMitigation mitigation, IStrength strength) : this()
        {
            _model = threatEvent.Model;
            _threatEvent = threatEvent;
            _mitigation = mitigation;
            _strength = strength;
        }

        public bool IsInitialized => Model != null && _threatEventId != Guid.Empty && _mitigationId != Guid.Empty;

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
        public Scope PropertiesScope => Scope.ThreatEventMitigation;

        [JsonProperty("mitigationId")]
        [NotRecorded]
        private Guid _mitigationId { get; set; }

        public Guid MitigationId => _mitigationId;

        [Reference]
        [NotRecorded]
        [UpdateMitigationId]
        private IMitigation _mitigation;

        [IgnoreAutoChangeNotification]
        public IMitigation Mitigation => _mitigation ?? (_mitigation = Model?.GetMitigation(_mitigationId));

        [JsonProperty("refId")]
        public Guid ReferenceId { get; set; }

        public string Name
        {
            get
            {
                string result;

                if (ReferenceId != Guid.Empty)
                {
                    result = Mitigation?.GetName(ReferenceId);
                }
                else
                {
                    result = Mitigation?.GetName(ThreatEvent?.Parent);
                }

                if (result == null)
                    result = ThreatModelManager.Undefined;

                return result;
            }
        }

        public string Description
        {
            get
            {
                string result;

                if (ReferenceId != Guid.Empty)
                {
                    result = Mitigation?.GetDescription(ReferenceId);
                }
                else
                {
                    result = Mitigation?.GetDescription(ThreatEvent?.Parent);
                }

                if (result == null)
                    result = ThreatModelManager.Undefined;

                return result;
            }
        }

        [JsonProperty("directives")]
        public string Directives { get; set; }

        [JsonProperty("strength")]
        [NotRecorded]
        private int _strengthId { get; set; }

        public int StrengthId => _strengthId;

        [Reference]
        [NotRecorded]
        [UpdateStrengthId]
        private IStrength _strength;

        [InitializationRequired]
        [SafeForDependencyAnalysis]
        public IStrength Strength
        {
            get => _strength ?? (_strength = Model?.GetStrength(_strengthId));

            set
            {
                if (value != null && value.Equals(Model.GetStrength(value.Id)))
                {
                    _strength = value;
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
                using (var scope = UndoRedoManager.OpenScope("Clone Threat Event Mitigation"))
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