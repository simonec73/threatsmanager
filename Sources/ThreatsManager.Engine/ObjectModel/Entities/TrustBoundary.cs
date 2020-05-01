using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
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
    [EntitiesReadOnlyContainerAspect]
    [GroupsReadOnlyContainerAspect]
    [GroupElementAspect]
    [TypeLabel("Trust Boundary")]
    [TypeInitial("D")]
    public class TrustBoundary : ITrustBoundary, IInitializableObject
    {
        public TrustBoundary()
        {
        }

        public TrustBoundary([NotNull] IThreatModel model, [Required] string name) : this()
        {
            _modelId = model.Id;
            _model = model;
            _id = Guid.NewGuid();
            Name = name;
  
            model.AutoApplySchemas(this);
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

        public IEnumerable<IEntity> Entities { get; }
        public IEntity GetEntity(Guid id)
        {
            return null;
        }

        public IEnumerable<IEntity> GetEntities(string name)
        {
            return null;
        }

        public IEnumerable<IEntity> SearchEntities(string filter)
        {
            return null;
        }

        public event Action<IGroupElement, IGroup, IGroup> ParentChanged;
        public Guid ParentId { get; }
        public IGroup Parent { get; }
        public void SetParent(IGroup parent)
        {
        }
        
        public IEnumerable<IGroup> Groups => null;

        public IGroup GetGroup(Guid id)
        {
            return null;
        }
        #endregion

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        private Guid _modelId { get; set; }
        private IThreatModel _model { get; set; }
        private IPropertiesContainer PropertiesContainer => this;
        private List<IProperty> _properties { get; set; }
        private Guid _parentId { get; set; }
        private IGroupElement GroupElement => this;
        #endregion

        #region Specific implementation.
        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        public IGroup Clone([NotNull] IGroupsContainer container)
        {
            IGroup result = null;

            if (container is IThreatModel model)
            {
                result = new TrustBoundary()
                {
                    _modelId = model.Id,
                    _model = model,
                    _id = Id,
                    Name = Name,
                    Description = Description,
                    _parentId = _parentId,
                };
                this.CloneProperties(result);

                container.Add(result);
            }

            return result;
        }
        #endregion
    }
}
