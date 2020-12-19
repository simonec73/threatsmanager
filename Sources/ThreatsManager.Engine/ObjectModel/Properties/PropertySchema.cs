using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
    [IdentityAspect]
    [ThreatModelChildAspect]
    public partial class PropertySchema : IPropertySchema
    {
        public PropertySchema()
        {
            
        }

        public PropertySchema([Required] IThreatModel model, [Required] string name, [Required] string nspace, int priority = 50) : this()
        {
            _id = Guid.NewGuid();
            Name = name;
            Namespace = nspace;
            _model = model;
            _modelId = model.Id;
            Priority = priority;
            RequiredExecutionMode = ExecutionMode.Business;
            Visible = true;
        }

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
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
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Name} ({Namespace})";
        }

        public void Add([NotNull] IPropertyType propertyType)
        {
            if (_propertyTypes == null)
                _propertyTypes = new List<IPropertyType>();

            _propertyTypes.Add(propertyType);
        }
        #endregion

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IThreatModel Model { get; }

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
    }
}
