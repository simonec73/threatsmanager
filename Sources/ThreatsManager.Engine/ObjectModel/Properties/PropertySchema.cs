﻿using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;
using PostSharp.Patterns.Collections;
using ThreatsManager.Engine.Aspects;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public partial class PropertySchema : IPropertySchema
    {
        public PropertySchema()
        {
            
        }

        public PropertySchema([Required] string name, [Required] string nspace, int priority = 50) : this()
        {
            _id = Guid.NewGuid();
            Name = name;
            Namespace = nspace;
            Priority = priority;
            RequiredExecutionMode = ExecutionMode.Business;
            Visible = true;
        }

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }

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
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("scope")]
        public Scope AppliesTo { get; set; }

        [JsonProperty("autoApply")]
        public bool AutoApply { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }

        [JsonProperty("system")]
        public bool System { get; set; }

        [JsonProperty("notExportable")]
        public bool NotExportable { get; set; }

        [JsonProperty("executionMode", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(ExecutionMode.Business)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionMode RequiredExecutionMode { get; set; }

        public IPropertySchema Clone([NotNull] IPropertySchemasContainer container)
        {
            PropertySchema result = null;

            if (container is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Property Schema"))
                {
                    result = new PropertySchema()
                    {
                        _id = Id,
                        Name = Name,
                        Description = Description,
                        _model = model,
                        _modelId = model.Id,
                        Namespace = Namespace,
                        AppliesTo = AppliesTo,
                        AutoApply = AutoApply,
                        Priority = Priority,
                        RequiredExecutionMode = RequiredExecutionMode,
                        Visible = Visible,
                        System = System,
                    };
                    this.ClonePropertyTypes(result);

                    container.Add(result);

                    if (model.Id != _modelId)
                        result.SetSourceInfo(Model);

                    scope?.Complete();
                }
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Name} ({Namespace})";
        }

        public void Add([NotNull] IPropertyType propertyType)
        {
            if (propertyType.Model?.Id != _modelId)
                throw new ArgumentException("Property Type is not associated to the Threat Model.", nameof(propertyType));

            if (_propertyTypes == null)
                _propertyTypes = new AdvisableCollection<IPropertyType>();

            _propertyTypes.Add(propertyType);
        }
        #endregion
    }
}
