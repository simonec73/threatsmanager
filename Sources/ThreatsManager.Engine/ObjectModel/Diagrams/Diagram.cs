using System;
using System.Collections.Generic;
using System.Drawing;
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
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.Diagrams
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
    [EntityShapesContainerAspect]
    [GroupShapesContainerAspect]
    [LinksContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class Diagram : IDiagram, IInitializableObject, IForceSetId
    {
        public Diagram()
        {
            
        }

        public Diagram([Required] string name) : this()
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
        public event Action<IEntityShapesContainer, IEntityShape> EntityShapeAdded;
        public event Action<IEntityShapesContainer, IEntity> EntityShapeRemoved;
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IEntityShape> Entities { get; }
        public IEntityShape GetShape(IEntity entity)
        {
            return null;
        }

        public void Add(IEntityShape entityShape)
        {
        }

        public void Add(IGroupShape groupShape)
        {
        }

        public void Add(ILink link)
        {
        }

        public void Add(IProperty property)
        {
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
        [Reference]
        [field: NotRecorded]
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
        [Reference]
        [field: NotRecorded]
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
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [Child]
        [JsonProperty("entities")]
        private AdvisableCollection<EntityShape> _entities { get; set; }
        [Child]
        [JsonProperty("groups")]
        private AdvisableCollection<GroupShape> _groups { get; set; }
        [Child]
        [JsonProperty("links")]
        private AdvisableCollection<Link> _links { get; set; }
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
        public Scope PropertiesScope => Scope.Diagram;

        public void SetId(Guid id)
        {
            _id = id;
        }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("dpi")]
        public int? Dpi { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? ThreatModelManager.Undefined : Name;
        }

        [InitializationRequired]
        public IDiagram Clone([NotNull] IDiagramsContainer container)
        {
            Diagram result = null;

            if (container is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Diagram"))
                {
                    result = new Diagram
                    {
                        _id = Id,
                        Name = Name,
                        Description = Description,
                        _model = model,
                        _modelId = model.Id,
                        Order = Order,
                        Dpi = Dpi
                    };
                    container.Add(result);
                    this.CloneProperties(result);

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
