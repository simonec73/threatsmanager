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
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
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
        public Scope PropertiesScope => Scope.Mitigation;

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
        private List<IProperty> _properties { get; set; }
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
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

        public void ClearProperties()
        {
        }

        public void Apply(IPropertySchema schema)
        {
        }

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
