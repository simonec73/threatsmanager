using System;
using System.Collections.Generic;
using System.Drawing;
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
            _associated = group;
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
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.GroupShape;

        [Reference]
        [field: NotRecorded]
        [field: UpdateAssociatedId]
        private IGroup _associated { get; set; }

        [JsonProperty("id")]
        private Guid _associatedId { get; set; }

        public Guid AssociatedId => _associatedId;

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IIdentity Identity => _associated ?? (_associated = Model?.GetGroup(_associatedId));

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
                result = new GroupShape()
                {
                    _associatedId = _associatedId,
                    _model = model,
                    _modelId = model.Id,
                    Position = new PointF(Position.X, Position.Y),
                    Size = new SizeF(Size)
                };
                this.CloneProperties(result);

                container.Add(result);
            }

            return result;
        }
        #endregion
    }
}
