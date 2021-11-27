using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
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
    [PropertiesContainerAspect]
    [ThreatModelChildAspect]
    public class StrengthDefinition : IStrength, IInitializableObject
    {
        public StrengthDefinition()
        {
            
        }

        public StrengthDefinition([NotNull] IThreatModel model, [Range(0, 100)] int id, [Required] string name) : this()
        {
            Id = id;
            Name = name;
            _modelId = model.Id;
            _model = model;
            Visible = true;
        }

        public bool IsInitialized => Model != null;

        #region Specific implementation.
        public Scope PropertiesScope => Scope.Strength;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("visible", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(true)]
        public bool Visible { get; set; }

        public IStrength Clone([NotNull] IStrengthsContainer container)
        {
            StrengthDefinition result = null;

            if (container is IThreatModel model)
            {
                result = new StrengthDefinition
                {
                    Id = Id, 
                    Name = Name, 
                    Description = Description,
                    _model = model, 
                    _modelId = model.Id,
                    Visible = Visible
                };
                this.CloneProperties(result);
                container.Add(result);
            }

            return result;
        }

        public int CompareTo(object obj)
        {
            var comparer = new StrengthComparer();
            return comparer.Compare(this, obj as IStrength);
        }

        public int CompareTo(IStrength other)
        {
            var comparer = new StrengthComparer();
            return comparer.Compare(this, other);
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion

        #region Default implementation.
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

        #region Additional placeholders required.
        private List<IProperty> _properties { get; set; }
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
        #endregion
    }
}