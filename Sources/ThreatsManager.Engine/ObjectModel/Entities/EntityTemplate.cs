using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ImageConverter = ThreatsManager.Utilities.ImageConverter;
using ThreatsManager.Engine.Aspects;
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
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Entity Template")]
    public class EntityTemplate : IEntityTemplate, IInitializableObject, IForceSetId
    {
        public EntityTemplate()
        {
        }

        public EntityTemplate([Required] string name)
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
        protected IThreatModel _model { get; set; }
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
        public Scope PropertiesScope => Scope.EntityTemplate;

        public void SetId(Guid id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        [JsonProperty("entityType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityType EntityType { get; set; }

        public event Action<IEntityTemplate, ImageSize> ImageChanged;

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

        [InitializationRequired]
        public IEntity CreateEntity([Required] string name)
        {
            IEntity result = null;

            using (var scope = UndoRedoManager.OpenScope("Create Entity"))
            {
                switch (EntityType)
                {
                    case EntityType.ExternalInteractor:
                        result = Model?.AddEntity<IExternalInteractor>(name, this);
                        break;
                    case EntityType.Process:
                        result = Model?.AddEntity<IProcess>(name, this);
                        break;
                    case EntityType.DataStore:
                        result = Model?.AddEntity<IDataStore>(name, this);
                        break;
                }

                if (result != null)
                {
                    result.Description = Description;
                    result.BigImage = this.GetImage(ImageSize.Big);
                    result.Image = this.GetImage(ImageSize.Medium);
                    result.SmallImage = this.GetImage(ImageSize.Small);
                    this.CloneProperties(result);
                    scope?.Complete();
                }
            }

            return result;
        }

        public void ApplyTo(IEntity entity)
        {
            using (var scope = UndoRedoManager.OpenScope("Apply Template to an existing Entity"))
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

                scope?.Complete();
            }
        }

        public IEntityTemplate Clone([NotNull] IEntityTemplatesContainer container)
        {
            EntityTemplate result = null;

            if (container is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Entity Template"))
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
