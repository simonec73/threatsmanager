using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
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
using ImageSize = ThreatsManager.Interfaces.ImageSize;

namespace ThreatsManager.Engine.ObjectModel.Entities
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [Serializable]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [GroupElementAspect]
    [PropertiesContainerAspect]
    [ThreatEventsContainerAspect]
    [TypeInitial("P")]
    public class Process : IProcess, IInitializableObject
    {
        public Process()
        {
        }

        public Process([NotNull] IThreatModel model, [Required] string name)
        {
            _modelId = model.Id;
            _model = model;
            _id = Guid.NewGuid();
            Name = name;
    
            model.AutoApplySchemas(this);
        }

        public Process([NotNull] IThreatModel model, [NotNull] IProcess process) : this(model, process.Name)
        {
            Description = process.Description;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IThreatModel Model { get; }
        public event Action<IGroupElement, IGroup, IGroup> ParentChanged;
        public Guid ParentId { get; }
        public IGroup Parent { get; }
        public void SetParent(IGroup parent)
        {
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

        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAdded;
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemoved;

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
        #endregion

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        private Guid _modelId { get; set; }
        private IThreatModel _model { get; set; }
        private IPropertiesContainer PropertiesContainer => this;
        private List<IProperty> _properties { get; set; }
        private List<IThreatEvent> _threatEvents { get; set; }
        private IThreatEventsContainer ThreatEventsContainer => this;
        private Guid _parentId { get; set; }
        private IGroupElement GroupElement => this;
        #endregion
 
        #region Specific implementation.
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

        [JsonProperty("bigImage")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _bigImage;

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

        [JsonProperty("image")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _image;

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

        [JsonProperty("smallImage")] 
        [JsonConverter(typeof(ImageConverter))]
        private Bitmap _smallImage;

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

        private IEntityTemplate _template { get; set; }

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

        public IEntity Clone([NotNull] IEntitiesContainer container)
        {
            Process result = null;

            if (container is IThreatModel model)
            {
                result = new Process()
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
