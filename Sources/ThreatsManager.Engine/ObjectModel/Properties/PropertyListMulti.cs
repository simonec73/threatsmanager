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
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertyAspect]
    [SourceInfoAspect]
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
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("propertyTypeId")]
        protected Guid _propertyTypeId { get; set; }
        [JsonProperty("readOnly")]
        protected bool _readOnly { get; set; }
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
        [Reference]
        [JsonProperty("items")]
        [field: NotRecorded]
        private List<string> _legacyItems { get; set; }

        [Child]
        [JsonProperty("rows")]
        [field:NotRecorded]
        private AdvisableCollection<RecordableString> _items { get; set; }

        [Reference]
        [field:NotRecorded]
        private List<IListItem> _values { get; set; }

        [InitializationRequired]
        [property:NotRecorded]
        public string StringValue
        {
            get => _items?.Select(x => x.Value).TagConcat();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

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
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                if (PropertyType is IListMultiPropertyType propertyType)
                {
                    var existing = _items?.Select(x => x.Value).OrderBy(x => x).TagConcat();
                    var incoming = value?.Select(x => x.Id).OrderBy(x => x).TagConcat();

                    if (string.CompareOrdinal(existing, incoming) != 0)
                    {
                        using (var scope = UndoRedoManager.OpenScope("Set Property List Multi Values"))
                        {
                            if (_items?.Any() ?? false)
                            {
                                foreach (var item in _items)
                                {
                                    UndoRedoManager.Detach(item);
                                }
                                _items.Clear();
                            }
                            _values?.Clear();

                            foreach (var item in value)
                            {
                                if (propertyType.Values.Any(x => string.CompareOrdinal(x.Id, item.Id) == 0))
                                {
                                    if (_items == null)
                                        _items = new AdvisableCollection<RecordableString>();

                                    if (_values == null)
                                        _values = new List<IListItem>();

                                    var newItem = new RecordableString(item.Id);
                                    UndoRedoManager.Attach(newItem, Model);
                                    _items.Add(newItem);
                                    _values.Add(item);
                                }
                            }

                            scope?.Complete();
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

        public IProperty Clone(IPropertiesContainer container)
        {
            IProperty result = null;

            IThreatModel model = container as IThreatModel;
            if (model == null && container is IThreatModelChild child)
                model = child.Model;

            var propertyTypeId = Guid.Empty;
            if (model != null)
            {
                var propertyType = model.GetPropertyType(_propertyTypeId);
                if (propertyType != null)
                {
                    var schema = model.GetSchema(propertyType.SchemaId);
                    if (schema != null)
                    {
                        var containerSchema = model.GetSchema(schema.Name, schema.Namespace);
                        if (containerSchema != null)
                        {
                            var containerPropertyType = containerSchema.GetPropertyType(propertyType.Name) as IListMultiPropertyType;
                            if (containerPropertyType != null)
                                propertyTypeId = containerPropertyType.Id;
                        }
                    }
                }
            }

            if (propertyTypeId != Guid.Empty)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Property List Multi"))
                {
                    result = new PropertyListMulti()
                    {
                        _id = Id,
                        _propertyTypeId = propertyTypeId,
                        _model = model,
                        _modelId = model.Id,
                        StringValue = StringValue,
                        _readOnly = _readOnly
                    };
                    container.Add(result);

                    if (model.Id != _modelId)
                        result.SetSourceInfo(Model);
                    UndoRedoManager.Attach(result, model);

                    scope?.Complete();
                }
            }

            return result;
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