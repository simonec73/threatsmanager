using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
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
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyJsonSerializableObject, ThreatsManager.Engine")]
    public class PropertyJsonSerializableObject : IPropertyJsonSerializableObject
    {
        public PropertyJsonSerializableObject()
        {
            
        }

        public PropertyJsonSerializableObject([NotNull] IJsonSerializableObjectPropertyType propertyType) : this()
        {
            _id = Guid.NewGuid();
            PropertyTypeId = propertyType.Id;
            _model = propertyType.Model;
        }

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
        [property: NotRecorded]
        public string StringValue
        {
#pragma warning disable SCS0028 // Type information used to serialize and deserialize objects
#pragma warning disable SEC0030 // Insecure Deserialization - Newtonsoft JSON
            get
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);

                using(JsonWriter writer = new JsonTextWriter(sw))
                {
                    var serializer = new JsonSerializer {TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented};
                    serializer.Serialize(writer, Value);
                }

                return sb.ToString();
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (!string.IsNullOrWhiteSpace(value))
                {
                    using (var textReader = new StringReader(value))
                    using (var reader = new JsonTextReader(textReader))
                    {
                        var serializer = new JsonSerializer
                        {
                            TypeNameHandling = TypeNameHandling.All,
                            MaxDepth = 128, 
                            SerializationBinder = new KnownTypesBinder(),
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        Value = serializer.Deserialize(reader);
                    }
                }
                else
                {
                    Value = null;
                }
            }
#pragma warning restore SEC0030 // Insecure Deserialization - Newtonsoft JSON
#pragma warning restore SCS0028 // Type information used to serialize and deserialize objects
        }

        [Reference]
        [JsonProperty("value", TypeNameHandling = TypeNameHandling.Objects)]
        [NotRecorded]
        private object _value { get; set; }

        public virtual object Value
        {
            get => _value;
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (value != _value)
                {
                    using (var scope = UndoRedoManager.OpenScope("Set Property Json Serializable Object"))
                    {
                        UndoRedoManager.Attach(value, _model);
                        _value = value;

                        if (value is IThreatModelAware aware)
                        {
                            aware.ModelId = _modelId;
                        }

                        scope?.Complete();
                    }

                    InvokeChanged();
                }
            }
        }

        public override string ToString()
        {
            return _value?.ToString() ?? string.Empty;
        }

        protected void InvokeChanged()
        {
            Changed?.Invoke(this);
        }
        #endregion
    }
}