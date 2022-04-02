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
    [ThreatModelChildAspect]
    [PropertyAspect]
    [Recordable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyInteger, ThreatsManager.Engine")]
    public class PropertyInteger : IPropertyInteger
    {
        public PropertyInteger()
        {
            
        }

        public PropertyInteger([NotNull] IThreatModel model, [NotNull] IIntegerPropertyType propertyType) : this()
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
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateId("Id", "_modelId")]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("propertyTypeId")]
        protected Guid _propertyTypeId { get; set; }
        [JsonProperty("readOnly")]
        protected bool _readOnly { get; set; }
        #endregion

        #region Specific implementation.
        [property: NotRecorded]
        public string StringValue
        {
            get => Value.ToString();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (int.TryParse(value, out var result))
                {
                    Value = result;
                }
                else
                {
                    Value = default(int);
                }
            }
        }

        [JsonProperty("value")]
        [NotRecorded]
        private int _value;

        public virtual int Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    if (ReadOnly)
                        throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                    _value = value;
                    InvokeChanged();
                }
            }
        }

        public override string ToString()
        {
            return StringValue ?? string.Empty;
        }

        protected void InvokeChanged()
        {
            Changed?.Invoke(this);
        }
        #endregion
    }
}
