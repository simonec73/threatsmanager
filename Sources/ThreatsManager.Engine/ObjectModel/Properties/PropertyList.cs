using System;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertyAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyList, ThreatsManager.Engine")]
    public class PropertyList : IPropertyList, IInitializableObject
    {
        public PropertyList()
        {
            
        }

        public PropertyList([NotNull] IListPropertyType propertyType) : this()
        {
            _id = Guid.NewGuid();
            PropertyTypeId = propertyType.Id;
            _model = propertyType.Model;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty && PropertyTypeId != Guid.Empty;

        #region Default implementation.
        public Guid Id { get; }
        public event Action<IProperty> Changed;
        public Guid PropertyTypeId { get; set; }
        [Reference]
        [field: NotRecorded]
        public IPropertyType PropertyType { get; }
        public bool ReadOnly { get; set; }
        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
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
        [JsonProperty("item")]
        [NotRecorded]
        private string _item { get; set; }

        [Reference]
        private IListItem _value;

        [InitializationRequired]
        [property: NotRecorded]
        public string StringValue
        {
            get => _item;
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (PropertyType is IListPropertyType propertyType)
                {
                    Value = propertyType.Values?.FirstOrDefault(x => string.CompareOrdinal(x.Id, value) == 0);
                }
            }
        }

        [InitializationRequired]
        public virtual IListItem Value 
        {
            get
            {
                if (_value == null && !string.IsNullOrWhiteSpace(_item))
                {
                    if (PropertyType is IListPropertyType listPropertyType)
                    {
                        _value = listPropertyType.Values?.FirstOrDefault(x => string.CompareOrdinal(x.Id, _item) == 0);
                    }
                }

                return _value;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (PropertyType is IListPropertyType listPropertyType)
                {
                    if (value != null)
                    {
                        _value = listPropertyType.Values?.FirstOrDefault(x => string.CompareOrdinal(x.Id, value.Id) == 0);
                        _item = _value != null ? value.Id : null;
                    }
                    else
                    {
                        _value = null;
                        _item = null;
                    }

                    InvokeChanged();
                }
            }
        }

        public override string ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }

        protected void InvokeChanged()
        {
            Changed?.Invoke(this);
        }
        #endregion
    }
}