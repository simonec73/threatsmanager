using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
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
    [IntroduceNotifyPropertyChanged]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [AssociatedIdChanger]
    [PropertiesContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class GroupShape : IGroupShape, IThreatModelChild, IInitializableObject
    {
        public GroupShape()
        {

        }

        public GroupShape([NotNull] IGroup group) : this()
        {
            _model = group.Model;
            _associatedId = group.Id;
        }

        public bool IsInitialized => Model != null && _associatedId != Guid.Empty;

        #region Default implementation.
        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }

        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        public bool HasProperty(IPropertyType propertyType)
        {
            return false;
        }
        [Reference]
        [field: NotRecorded]
        public IEnumerable<IProperty> Properties { get; }
        public IProperty GetProperty(IPropertyType propertyType)
        {
            return null;
        }

        public void Add(IProperty property)
        {
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
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
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
        public Scope PropertiesScope => Scope.GroupShape;

        [JsonProperty("id")]
        private Guid _associatedId { get; set; }

        public Guid AssociatedId => _associatedId;

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IIdentity Identity => Model?.GetGroup(_associatedId);

        [Child]
        private RecordablePointF _recordablePosition;

        [property: NotRecorded]
        [JsonProperty("pos")]
        [SafeForDependencyAnalysis]
        public PointF Position
        {
            get
            {
                return _recordablePosition?.Position ?? PointF.Empty;
            }

            set
            {
                if (_recordablePosition == null)
                    _recordablePosition = new RecordablePointF(value);
                else
                {
                    _recordablePosition.Position = value;
                }
            }
        }

        [Child]
        private RecordableSizeF _recordableSize;

        [property: NotRecorded]
        [JsonProperty("size")]
        [SafeForDependencyAnalysis]
        public SizeF Size
        {
            get
            {
                return _recordableSize?.Size ?? SizeF.Empty;
            }

            set
            {
                if (_recordableSize == null)
                    _recordableSize = new RecordableSizeF(value);
                else
                {
                    _recordableSize.Size = value;
                }
            }
        }

        public IGroupShape Clone([NotNull] IGroupShapesContainer container)
        {
            GroupShape result = null;
            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Group Shape"))
                {
                    result = new GroupShape()
                    {
                        _associatedId = _associatedId,
                        _model = model,
                        _modelId = model.Id,
                        Position = new PointF(Position.X, Position.Y),
                        Size = new SizeF(Size)
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

        public override string ToString()
        {
            var text = Identity?.ToString();
            return  string.IsNullOrWhiteSpace(text) ? ThreatModelManager.Undefined : text;
        }
        #endregion
    }
}
