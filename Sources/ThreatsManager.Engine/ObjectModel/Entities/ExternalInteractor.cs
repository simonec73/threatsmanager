using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;
using ImageConverter = ThreatsManager.Utilities.ImageConverter;
using PostSharp.Patterns.Collections;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using System.Windows.Markup;

namespace ThreatsManager.Engine.ObjectModel.Entities
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [Serializable]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [GroupElementAspect]
    [PropertiesContainerAspect]
    [ThreatEventsContainerAspect]
    [VulnerabilitiesContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("External Interactor")]
    [TypeInitial("E")]
    public class ExternalInteractor : IExternalInteractor, IInitializableObject
    {
        public ExternalInteractor()
        {
        }

        public ExternalInteractor([Required] string name)
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
        public event Action<IGroupElement, IGroup, IGroup> ParentChanged;
        public Guid ParentId { get; }
        [Reference]
        [field: NotRecorded]
        public IGroup Parent { get; }
        public void SetParent(IGroup parent)
        {
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

        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAdded;
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemoved;

        [Reference]
        [field: NotRecorded]
        public IEnumerable<IThreatEvent> ThreatEvents { get; }
        public IThreatEvent GetThreatEvent(Guid id)
        {
            return null;
        }

        public IThreatEvent GetThreatEventByThreatType(Guid threatTypeId)
        {
            return null;
        }

        public void Add(IThreatEvent threatEvent)
        {
        }

        public IThreatEvent AddThreatEvent(IThreatType threatType)
        {
            return null;
        }

        public bool RemoveThreatEvent(Guid id)
        {
            return false;
        }

        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityAdded;
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityRemoved;
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IVulnerability> Vulnerabilities { get; }
        public IVulnerability GetVulnerability(Guid id)
        {
            return null;
        }

        public IVulnerability GetVulnerabilityByWeakness(Guid weaknessId)
        {
            return null;
        }

        public void Add(IVulnerability vulnerability)
        {
        }

        public IVulnerability AddVulnerability(IWeakness weakness)
        {
            return null;
        }

        public bool RemoveVulnerability(Guid id)
        {
            return false;
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
        [Child]
        [JsonProperty("threatEvents")]
        private AdvisableCollection<ThreatEvent> _threatEvents { get; set; }
        [Child]
        [JsonProperty("vulnerabilities")]
        private AdvisableCollection<Vulnerability> _vulnerabilities { get; set; }
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
        public Scope PropertiesScope => Scope.ExternalInteractor;

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        public event Action<IEntity, ImageSize> ImageChanged;

        [Reference]
        [NotRecorded]
        [JsonProperty("bigImage")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _bigImage { get; set; }

        public Bitmap BigImage 
        {
            get => _bigImage;

            set
            {
                if (_bigImage != value)
                {
                    _bigImage = value;
                    ImageChanged?.Invoke(this, ImageSize.Big);
                }
            }
        }

        [Reference]
        [NotRecorded]
        [JsonProperty("image")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _image { get; set; }

        public Bitmap Image 
        {
            get => _image;

            set
            {
                if (_image != value)
                {
                    _image = value;
                    ImageChanged?.Invoke(this, ImageSize.Medium);
                }
            }
        }

        [Reference]
        [NotRecorded]
        [JsonProperty("smallImage")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _smallImage { get; set; }

        public Bitmap SmallImage 
        {
            get => _smallImage;

            set
            {
                if (_smallImage != value)
                {
                    _smallImage = value;
                    ImageChanged?.Invoke(this, ImageSize.Small);
                }
            }
        }

        [JsonProperty("template")]
        internal Guid _templateId { get; set; }

        [Reference]
        [property: NotRecorded]
        internal IEntityTemplate _template { get; set; }

        [property: NotRecorded]
        [IgnoreAutoChangeNotification]
        public IEntityTemplate Template
        {
            get
            {
                if (_template == null && _templateId != Guid.Empty)
                {
                    _template = Model?.GetEntityTemplate(_templateId);
                }

                return _template;
            }
        }

        public void ResetTemplate()
        {
            using (var scope = UndoRedoManager.OpenScope("Detach from Template"))
            {
                this.BigImage = EntityType.ExternalInteractor.GetEntityImage(ImageSize.Big);
                this.Image = EntityType.ExternalInteractor.GetEntityImage(ImageSize.Medium);
                this.SmallImage = EntityType.ExternalInteractor.GetEntityImage(ImageSize.Small);
                this.ClearProperties();
                Model?.AutoApplySchemas(this);

                _templateId = Guid.Empty;
                _template = null;

                scope?.Complete();
            }
        }

        public IEntity Clone([NotNull] IEntitiesContainer container)
        {
            ExternalInteractor result = null;

            if (container is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone External Interactor"))
                {
                    result = new ExternalInteractor()
                    {
                        _id = Id,
                        Name = Name,
                        Description = Description,
                        _model = model,
                        _modelId = model.Id,
                        _parentId = _parentId,
                        _templateId = _templateId
                    };
                    container.Add(result);
                    this.CloneProperties(result);
                    this.CloneThreatEvents(result);
                    this.CloneVulnerabilities(result);

                    if (model.Id != _modelId)
                        result.SetSourceInfo(Model);

                    scope?.Complete();
                }
            }

            return result;
        }

        public IEntity CopyAndConvert(IEntityTemplate template = null)
        {
            return CopyAndConvert(EntityType.ExternalInteractor, template);
        }

        public IEntity CopyAndConvert(EntityType entityType, IEntityTemplate template = null)
        {
            return this.CloneAndConvert(entityType, template);
        }
        #endregion
    }
}
