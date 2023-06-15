using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Utilities.Exceptions;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertyAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyListMulti, ThreatsManager.Engine")]
    public class PropertyListMulti : IPropertyListMulti, IInitializableObject
    {
        public PropertyListMulti()
        {
            
        }

        public PropertyListMulti([NotNull] IListMultiPropertyType propertyType) : this()
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
        [Child]
        [JsonProperty("items")]
        [NotRecorded]
        private AdvisableCollection<RecordableString> _items;

        [Reference]
        [NotRecorded]
        private List<IListItem> _values;

        [InitializationRequired]
        public string StringValue
        {
            get => _items?.Select(x => x.Value).TagConcat();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (PropertyType is IListPropertyType propertyType)
                {
                    Values = value?.TagSplit()
                        .Select(x => propertyType.Values.FirstOrDefault(y => string.CompareOrdinal(x, y.Id) == 0))
                        .Where(x => x != null);
                }
            }
        }

        [InitializationRequired]
        public virtual IEnumerable<IListItem> Values
        {
            get => _values?.AsReadOnly();

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                _items?.Clear();
                _values?.Clear();

                if ((value?.Any() ?? false) && PropertyType is IListMultiPropertyType propertyType)
                {
                    foreach (var item in value)
                    {
                        if (propertyType.Values.Any(x => string.CompareOrdinal(x.Id, item.Id) == 0))
                        {
                            if (_items == null)
                                _items = new AdvisableCollection<RecordableString>();
                            if (_values == null)
                                _values = new List<IListItem>();

                            _items.Add(new RecordableString(item.Id));
                            _values.Add(item);
                        }
                    }
                }

                InvokeChanged();
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