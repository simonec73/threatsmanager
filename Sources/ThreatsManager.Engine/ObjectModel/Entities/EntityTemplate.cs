using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ImageConverter = ThreatsManager.Utilities.ImageConverter;

namespace ThreatsManager.Engine.ObjectModel.Entities
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
    [TypeLabel("Entity Template")]
    public class EntityTemplate : IEntityTemplate, IInitializableObject
    {
        public EntityTemplate()
        {
        }

        public EntityTemplate([NotNull] IThreatModel model, [Required] string name)
        {
            _modelId = model.Id;
            _model = model;
            _id = Guid.NewGuid();
            Name = name;
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
        private List<IProperty> _properties { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.EntityTemplate;

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        [JsonProperty("entityType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityType EntityType { get; set; }

        public event Action<IEntityTemplate, ImageSize> ImageChanged;

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

        [InitializationRequired]
        public IEntity CreateEntity([Required] string name)
        {
            IEntity result = null;

            switch (this.EntityType)
            {
                case EntityType.ExternalInteractor:
                    result = _model.AddEntity<IExternalInteractor>(name, this);
                    break;
                case EntityType.Process:
                    result = _model.AddEntity<IProcess>(name, this);
                    break;
                case EntityType.DataStore:
                    result = _model.AddEntity<IDataStore>(name, this);
                    break;
            }

            if (result != null)
            {
                result.Description = Description;
                result.BigImage = this.GetImage(ImageSize.Big);
                result.Image = this.GetImage(ImageSize.Medium);
                result.SmallImage = this.GetImage(ImageSize.Small);
                this.CloneProperties(result);
            }

            return result;
        }

        public void ApplyTo(IEntity entity)
        {
            entity.BigImage = this.GetImage(ImageSize.Big);
            entity.Image = this.GetImage(ImageSize.Medium);
            entity.SmallImage = this.GetImage(ImageSize.Small);
            entity.ClearProperties();
            this.CloneProperties(entity);
            switch (entity)
            {
                case ExternalInteractor externalInteractor:
                    externalInteractor._templateId = Id;
                    externalInteractor._template = this;
                    break;
                case Process process:
                    process._templateId = Id;
                    process._template = this;
                    break;
                case DataStore dataStore:
                    dataStore._templateId = Id;
                    dataStore._template = this;
                    break;
            }
        }

        public IEntityTemplate Clone([NotNull] IEntityTemplatesContainer container)
        {
            EntityTemplate result = null;

            if (container is IThreatModel model)
            {
                result = new EntityTemplate()
                {
                    _id = Id,
                    Name = Name,
                    Description = Description,
                    _model = model,
                    _modelId = model.Id,
                    EntityType = EntityType,
                    Image = Image,
                    SmallImage = SmallImage,
                    BigImage = BigImage
                };
                this.CloneProperties(result);

                container.Add(result);
            }

            return result;
        }
        #endregion
    }
}
