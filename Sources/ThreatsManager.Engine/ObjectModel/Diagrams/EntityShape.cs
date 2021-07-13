using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Diagrams
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    public class EntityShape : IEntityShape, IThreatModelChild, IInitializableObject
    {
        [JsonIgnore]
        private IEntity _entity;

        public EntityShape()
        {

        }

        public EntityShape([NotNull] IThreatModel model, [NotNull] IEntity entity) : this()
        {
            _modelId = model.Id;
            _model = model;
            _entity = entity;
            _associatedId = entity.Id;
        }

        public bool IsInitialized => Model != null && _associatedId != Guid.Empty;

        #region Specific implementation.
        public Scope PropertiesScope => Scope.EntityShape;

        [JsonProperty("id")]
        private Guid _associatedId;

        public Guid AssociatedId => _associatedId;

        [InitializationRequired]
        public IIdentity Identity => _entity ?? (_entity = Model.GetEntity(_associatedId));

        [JsonProperty("pos")]
        public PointF Position { get; set; }

        public IEntityShape Clone([NotNull] IEntityShapesContainer container)
        {
            EntityShape result = null;
            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                result = new EntityShape()
                {
                    _associatedId = _associatedId,
                    _model = model,
                    _modelId = model.Id,
                    Position = new PointF(Position.X, Position.Y),
                };
                this.CloneProperties(result);

                container.Add(result);
            }

            return result;
        }
        #endregion

        #region Additional placeholders required.
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
        private IPropertiesContainer PropertiesContainer => this;
        private List<IProperty> _properties { get; set; }
        #endregion

        #region Default implementation.
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

        public void Apply(IPropertySchema schema)
        {
        }

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
