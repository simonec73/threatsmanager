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
using System.Runtime.Serialization;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertyAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyArray, ThreatsManager.Engine")]
    public class PropertyArray : IPropertyArray, IInitializableObject
    {
        public PropertyArray()
        {
            
        }

        public PropertyArray([NotNull] IArrayPropertyType propertyType) : this()
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
        protected IThreatModel _model { get; set; }
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("propertyTypeId")]
        protected Guid _propertyTypeId { get; set; }
        [JsonProperty("readOnly")]
        protected bool _readOnly { get; set; }
        #endregion

        #region Specific implementation.
        [Reference]
        [JsonProperty("items")]
        [field:NotRecorded]
        private List<string> _legacyItems { get; set; }

        [Child]
        [JsonProperty("rows")]
        [field:NotRecorded]
        private AdvisableCollection<RecordableString> _items { get; set; }

        [InitializationRequired]
        public string StringValue
        {
            get => Value?.TagConcat();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                Value = value?.TagSplit();
            }
        }

        [InitializationRequired]
        [property:NotRecorded]
        public virtual IEnumerable<string> Value
        {
            get => _items?.Select(x => x.Value).AsEnumerable();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (value?.Any() ?? false)
                {
                    if (_items == null)
                    {
                        _items = new AdvisableCollection<RecordableString>();
                    }
                    else
                    {
                        if (_items.Any())
                        {
                            foreach (var item in _items)
                            {
                                UndoRedoManager.Detach(item);
                            }
                        }
                        _items.Clear();
                    }

                    foreach (var item in value)
                    {
                        var r = new RecordableString(item);
                        UndoRedoManager.Attach(r, Model);
                        _items.Add(r);
                    }
                }
                else
                {
                    _items = null;
                }
                Changed?.Invoke(this);
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

        #region On Deserialization.
        [OnDeserialized]
        public void PostDeserialization(StreamingContext context)
        {
            if (_legacyItems?.Any() ?? false)
            {
                if (_items == null)
                    _items = new AdvisableCollection<RecordableString>();

                foreach (var item in _legacyItems)
                {
                    var r = new RecordableString(item);
                    UndoRedoManager.Attach(r, Model);
                    _items.Add(r);
                }

                _legacyItems.Clear();
            }
        }
        #endregion
    }
}