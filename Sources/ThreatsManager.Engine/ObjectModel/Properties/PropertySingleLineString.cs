using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
    [PropertyAspect]
    [ThreatModelChildAspect]
    [Recordable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertySingleLineString, ThreatsManager.Engine")]
    public class PropertySingleLineString : IPropertySingleLineString
    {
        public PropertySingleLineString()
        {
        }

        public PropertySingleLineString([NotNull] IThreatModel model, [NotNull] ISingleLineStringPropertyType propertyType) : this()
        {
            _id = Guid.NewGuid();
            _modelId = model.Id;
            _model = model;
            PropertyTypeId = propertyType.Id;
        }

        #region Default implementation.
        public Guid Id { get; }
        public event Action<IProperty> Changed;
        public Guid PropertyTypeId { get; set; }
        public IPropertyType PropertyType { get; }
        public bool ReadOnly { get; set; }
        public IThreatModel Model { get; }

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
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("propertyTypeId")]
        protected Guid _propertyTypeId { get; set; }
        [JsonProperty("readOnly")]
        protected bool _readOnly { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateId("Id", "_modelId")]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        #endregion

        #region Specific implementation.
        [JsonProperty("value")]
        [NotRecorded]
        private string _value;

        public virtual string StringValue
        {
            get => _value;
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (string.CompareOrdinal(value, _value) != 0)
                {
                    _value = value;
                }
            }
        }

        public override string ToString()
        {
            return StringValue;
        }

        protected void InvokeChanged()
        {
            Changed?.Invoke(this);
        }
        #endregion

    }
}