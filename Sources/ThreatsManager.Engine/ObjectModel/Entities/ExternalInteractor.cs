using System;
using System.Collections.Generic;
using System.Drawing;
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
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ImageConverter = ThreatsManager.Utilities.ImageConverter;

namespace ThreatsManager.Engine.ObjectModel.Entities
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
    [Serializable]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [GroupElementAspect]
    [PropertiesContainerAspect]
    [ThreatEventsContainerAspect]
    [VulnerabilitiesContainerAspect]
    [Recordable]
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

        public IThreatEvent AddThreatEvent(IThreatType threatType)
        {
            return null;
        }

        public bool RemoveThreatEvent(Guid id)
        {
            return false;
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
        [field: UpdateId("Id", "_modelId")]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [Child]
        [JsonProperty("properties")]
        private IList<IProperty> _properties { get; set; }
        [Child]
        [JsonProperty("threatEvents")]
        private IList<IThreatEvent> _threatEvents { get; set; }
        [Child]
        [JsonProperty("vulnerabilities")]
        private IList<IVulnerability> _vulnerabilities { get; set; }
        [JsonProperty("parentId")]
        private Guid _parentId { get; set; }
        [Reference]
        [field: NotRecorded]
        private IGroup _parent { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.ExternalInteractor;

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        public void Add([NotNull] IThreatEvent threatEvent)
        {
            if (_threatEvents == null)
                _threatEvents = new List<IThreatEvent>();

            _threatEvents.Add(threatEvent);
        }

        public event Action<IEntity, ImageSize> ImageChanged;

        [Reference]
        [JsonProperty("bigImage")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _bigImage;

        [field: NotRecorded]
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
        [JsonProperty("image")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _image;

        [field: NotRecorded]
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
        [JsonProperty("smallImage")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _smallImage;

        [field: NotRecorded]
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
        internal Guid _templateId;

        [Reference]
        [field: NotRecorded]
        internal IEntityTemplate _template { get; set; }

        [field: NotRecorded]
        public IEntityTemplate Template
        {
            get
            {
                if (_template == null && _templateId != Guid.Empty)
                {
                    _template = _model?.GetEntityTemplate(_templateId);
                }

                return _template;
            }
        }

        public void ResetTemplate()
        {
            this.BigImage = EntityType.ExternalInteractor.GetEntityImage(ImageSize.Big);
            this.Image = EntityType.ExternalInteractor.GetEntityImage(ImageSize.Medium);
            this.SmallImage = EntityType.ExternalInteractor.GetEntityImage(ImageSize.Small);
            this.ClearProperties();
            _model.AutoApplySchemas(this);

            _templateId = Guid.Empty;
            _template = null;
        }

        public IEntity Clone([NotNull] IEntitiesContainer container)
        {
            ExternalInteractor result = null;

            if (container is IThreatModel model)
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
                this.CloneProperties(result);
                this.CloneThreatEvents(result);

                container.Add(result);
            }

            return result;
        }
        #endregion
    }
}
