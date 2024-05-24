﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Engine.Aspects;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.Entities
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertiesContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Flow Template")]
    public class FlowTemplate : IFlowTemplate, IInitializableObject, IForceSetId
    {
        public FlowTemplate()
        {
        }

        public FlowTemplate([Required] string name)
        {
            _id = Guid.NewGuid();
            Name = name;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
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
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("name")]
        protected string _name { get; set; }
        [JsonProperty("description")]
        protected string _description { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Parent]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
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
        public Scope PropertiesScope => Scope.FlowTemplate;

        public void SetId(Guid id)
        {
            _id = id;
        }

        [JsonProperty("flowType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FlowType FlowType { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? ThreatModelManager.Undefined : Name;
        }

        [InitializationRequired]
        public IDataFlow CreateFlow([Required] string name, Guid sourceId, Guid targetId)
        {
            IDataFlow result;

            using (var scope = UndoRedoManager.OpenScope("Create Flow from Template"))
            {
                result = Model?.AddDataFlow(name, sourceId, targetId, this);

                if (result != null)
                {
                    result.Description = Description;
                    result.FlowType = FlowType;
                    this.CloneProperties(result);
                    scope?.Complete();
                }
            }

            return result;
        }

        [InitializationRequired]
        public void ApplyTo([NotNull] IDataFlow flow)
        {
            using (var scope = UndoRedoManager.OpenScope("Apply Template to an existing Flow"))
            {
                flow.FlowType = FlowType;
                flow.ClearProperties();
                this.CloneProperties(flow);
                if (flow is DataFlow internalFlow)
                {
                    internalFlow._templateId = Id;
                }
                scope?.Complete();
            }
        }

        public IFlowTemplate Clone([NotNull] IFlowTemplatesContainer container)
        {
            FlowTemplate result = null;

            if (container is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Flow Template"))
                {
                    result = new FlowTemplate()
                    {
                        _id = Id,
                        Name = Name,
                        Description = Description,
                        FlowType = FlowType,
                        _model = model,
                        _modelId = model.Id,
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
        #endregion
    }
}
