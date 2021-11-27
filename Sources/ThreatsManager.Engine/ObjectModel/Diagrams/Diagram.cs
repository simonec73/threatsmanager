using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
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
    [IdentityAspect]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    [EntityShapesContainerAspect]
    [GroupShapesContainerAspect]
    [LinksContainerAspect]
    public class Diagram : IDiagram, IInitializableObject
    {
        public Diagram()
        {
            
        }

        public Diagram([NotNull] IThreatModel model, [Required] string name) : this()
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
        public event Action<IEntityShapesContainer, IEntityShape> EntityShapeAdded;
        public event Action<IEntityShapesContainer, IEntity> EntityShapeRemoved;
        public IEnumerable<IEntityShape> Entities { get; }
        public IEntityShape GetShape(IEntity entity)
        {
            return null;
        }

        public IEntityShape GetEntityShape(Guid entityId)
        {
            return null;
        }

        public IEntityShape AddShape(IEntity entity, PointF position)
        {
            return null;
        }

        public IEntityShape AddEntityShape(Guid entityId, PointF position)
        {
            return null;
        }

        public bool RemoveEntityShape(Guid entityId)
        {
            return false;
        }

        public bool RemoveShape(IEntity entity)
        {
            return false;
        }

        public bool RemoveShape(IEntityShape entityShape)
        {
            return false;
        }

        public event Action<IGroupShapesContainer, IGroupShape> GroupShapeAdded;
        public event Action<IGroupShapesContainer, IGroup> GroupShapeRemoved;
        public IEnumerable<IGroupShape> Groups { get; }
        public IGroupShape GetShape(IGroup @group)
        {
            return null;
        }

        public IGroupShape GetGroupShape(Guid groupId)
        {
            return null;
        }

        public IGroupShape AddShape(IGroup @group, PointF position, SizeF size)
        {
            return null;
        }

        public IGroupShape AddGroupShape(Guid groupId, PointF position, SizeF size)
        {
            return null;
        }

        public bool RemoveGroupShape(Guid groupId)
        {
            return false;
        }

        public bool RemoveShape(IGroup @group)
        {
            return false;
        }

        public bool RemoveShape(IGroupShape groupShape)
        {
            return false;
        }

        public event Action<ILinksContainer, ILink> LinkAdded;
        public event Action<ILinksContainer, IDataFlow> LinkRemoved;
        public IEnumerable<ILink> Links { get; }
        public ILink GetLink(Guid dataFlowId)
        {
            return null;
        }

        public ILink AddLink(IDataFlow dataFlow)
        {
            return null;
        }

        public bool RemoveLink(Guid dataFlow)
        {
            return false;
        }

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

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
        private List<IEntityShape> _entities { get; set; }
        private List<IGroupShape> _groups { get; set; }
        private List<ILink> _links { get; set; }
        private List<IProperty> _properties { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.Diagram;

        [JsonProperty("order")]
        public int Order { get; set; }

        public override string ToString()
        {
            return Name;
        }

        [InitializationRequired]
        public IDiagram Clone([NotNull] IDiagramsContainer container)
        {
            Diagram result = null;

            if (container is IThreatModel model)
            {
                result = new Diagram
                {
                    _id = Id, 
                    Name = Name, 
                    Description = Description,
                    _model = model, 
                    _modelId = model.Id
                };
                this.CloneProperties(result);
                container.Add(result);

                var entities = Entities?.ToArray();
                if (entities?.Any() ?? false)
                {
                    foreach (var entityShape in entities)
                    {
                        entityShape.Clone(result);
                    }
                }

                var groups = Groups?.ToArray();
                if (groups?.Any() ?? false)
                {
                    foreach (var groupShape in groups)
                    {
                        groupShape.Clone(result);
                    }
                }

                var links = Links?.ToArray();
                if (links?.Any() ?? false)
                {
                    foreach (var link in links)
                    {
                        link.Clone(result);
                    }
                }
            }

            return result;
        }

        [InitializationRequired]
        public void Add([NotNull] IEntityShape entityShape)
        {
            if (entityShape.Identity is IThreatModelChild child &&
                child.Model != _model)
                throw new ArgumentException();

            _entities.Add(entityShape);
        }
 
        [InitializationRequired]
        public void Add([NotNull] IGroupShape groupShape)
        {
            if (groupShape.Identity is IThreatModelChild child &&
                child.Model != _model)
                throw new ArgumentException();

            _groups.Add(groupShape);
        }
 
        [InitializationRequired]
        public void Add([NotNull] ILink link)
        {
            if (link.DataFlow is IThreatModelChild child &&
                child.Model != _model)
                throw new ArgumentException();

            _links.Add(link);
        }
        #endregion
    }
}
