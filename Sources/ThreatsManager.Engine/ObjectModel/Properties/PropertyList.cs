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
using ThreatsManager.Utilities;
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
        [JsonProperty("item")]
        [field: NotRecorded]
        private string _item { get; set; }

        [Reference]
        [field: NotRecorded]
        private IListItem _value { get; set; }

        [InitializationRequired]
        [property: NotRecorded]
        public string StringValue
        {
            get => _item;
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

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
                        using (var scope = UndoRedoManager.OpenScope("Get Property List Value"))
                        {
                            _value = listPropertyType.Values?.FirstOrDefault(x => string.CompareOrdinal(x.Id, _item) == 0);
                            scope?.Complete();
                        }
                    }
                }

                return _value;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                if (PropertyType is IListPropertyType listPropertyType)
                {
                    using (var scope = UndoRedoManager.OpenScope("Set Property List Value"))
                    {
                        if (value != null)
                        {
                            var comparer = new ListItemComparer();
                            if (!comparer.Equals(value, _value))
                            {
                                _value = listPropertyType.Values?.FirstOrDefault(x => string.CompareOrdinal(x.Id, value.Id) == 0);
                                _item = _value != null ? value.Id : null;
                            }
                        }
                        else if (_value != null)
                        {
                            _value = null;
                            _item = null;
                        }

                        scope?.Complete();
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
                            var containerPropertyType = containerSchema.GetPropertyType(propertyType.Name) as IListPropertyType;
                            if (containerPropertyType != null)
                                propertyTypeId = containerPropertyType.Id;
                        }
                    }
                }
            }

            if (propertyTypeId != Guid.Empty)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Property List"))
                {
                    result = new PropertyList()
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
    }
}