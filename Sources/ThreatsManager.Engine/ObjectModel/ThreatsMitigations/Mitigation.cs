using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
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
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [Serializable]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    public class Mitigation : IMitigation, IInitializableObject
    {
        public Mitigation()
        {

        }

        public Mitigation([NotNull] IThreatModel model, [Required] string name)
        {
            _id = Guid.NewGuid();
            Name = name;
            _model = model;
            _modelId = model.Id;
    
            model.AutoApplySchemas(this);
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Specific implementation.
        [JsonProperty("controlType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SecurityControlType ControlType { get; set; }

        public IMitigation Clone([NotNull] IMitigationsContainer container)
        {
            Mitigation result = null;

            if (container is IThreatModel model)
            {
                result = new Mitigation
                {
                    _id = Id, 
                    Name = Name, 
                    Description = Description,
                    _model = model, 
                    _modelId = model.Id,
                    ControlType = ControlType
                };
                this.CloneProperties(result);
                container.Add(result);
            }

            return result;
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        private IPropertiesContainer PropertiesContainer => this;
        private List<IProperty> _properties { get; set; }
        private Guid _modelId { get; set; }
        private IThreatModel _model { get; set; }
        #endregion    

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
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

        public IThreatModel Model { get; }
        #endregion
    }
}
