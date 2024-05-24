using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.Entities
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
    [EntitiesReadOnlyContainerAspect]
    [GroupsReadOnlyContainerAspect]
    [GroupElementAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Trust Boundary")]
    [TypeInitial("T")]
    public class TrustBoundary : ITrustBoundary, IInitializableObject, IForceSetId
    {
        public TrustBoundary()
        {
        }

        public TrustBoundary([Required] string name) : this()
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

        [Reference]
        [field: NotRecorded]
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
        [Reference]
        [field: NotRecorded]
        public IGroup Parent { get; }
        public void SetParent(IGroup parent)
        {
        }

        [property: NotRecorded]
        public IEnumerable<IGroup> Groups => null;

        public IGroup GetGroup(Guid id)
        {
            return null;
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
        [JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects)]
        private AdvisableCollection<IProperty> _properties { get; set; }
        [JsonProperty("parentId")]
        private Guid _parentId { get; set; }
        [Reference]
        [field: NotRecorded]
        private IGroup _parent { get; set; }
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
        public Scope PropertiesScope => Scope.TrustBoundary;

        public void SetId(Guid id)
        {
            _id = id;
        }

        [JsonProperty("template")]
        internal Guid _templateId { get; set; }

        [property: NotRecorded]
        [IgnoreAutoChangeNotification]
        public ITrustBoundaryTemplate Template => Model?.GetTrustBoundaryTemplate(_templateId);

        public void ResetTemplate()
        {
            using (var scope = UndoRedoManager.OpenScope("Detach from Template"))
            {
                this.ClearProperties();
                Model?.AutoApplySchemas(this);

                _templateId = Guid.Empty;

                scope?.Complete();
            }
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? ThreatModelManager.Undefined : Name;
        }

        public IGroup Clone([NotNull] IGroupsContainer container)
        {
            IGroup result = null;

            if (container is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Trust Boundary"))
                {
                    result = new TrustBoundary()
                    {
                        _modelId = model.Id,
                        _model = model,
                        _id = Id,
                        Name = Name,
                        Description = Description,
                        _parentId = _parentId,
                        _templateId = _templateId
                    };
                    container.Add(result);
                    this.CloneProperties(result);

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
