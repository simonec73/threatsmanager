using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Entities
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [Serializable]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    [TypeLabel("Flow Template")]
    public class FlowTemplate : IFlowTemplate, IInitializableObject
    {
        public FlowTemplate()
        {
        }

        public FlowTemplate([NotNull] IThreatModel model, [Required] string name)
        {
            _modelId = model.Id;
            _model = model;
            _id = Guid.NewGuid();
            Name = name;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
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

        #endregion

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        private Guid _modelId { get; set; }
        private IThreatModel _model { get; set; }
        private IPropertiesContainer PropertiesContainer => this;
        private List<IProperty> _properties { get; set; }
        #endregion        
 
        #region Specific implementation.
        [JsonProperty("flowType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FlowType FlowType { get; set; }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        [InitializationRequired]
        public IDataFlow CreateFlow([Required] string name, Guid sourceId, Guid targetId)
        {
            IDataFlow result = _model.AddDataFlow(name, sourceId, targetId, this);

            if (result != null)
            {
                result.Description = Description;
                result.FlowType = FlowType;
                this.CloneProperties(result);
            }

            return result;
        }

        public void ApplyTo([NotNull] IDataFlow flow)
        {
            flow.ClearProperties();
            this.CloneProperties(flow);
            if (flow is DataFlow internalFlow)
            {
                internalFlow._templateId = Id;
                internalFlow._template = this;
            }
        }

        public IFlowTemplate Clone([NotNull] IFlowTemplatesContainer container)
        {
            FlowTemplate result = null;

            if (container is IThreatModel model)
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
                this.CloneProperties(result);

                container.Add(result);
            }

            return result;
        }
        #endregion
    }
}
