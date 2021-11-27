using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
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
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyJsonSerializableObject, ThreatsManager.Engine")]
    public class PropertyJsonSerializableObject : IPropertyJsonSerializableObject
    {
        public PropertyJsonSerializableObject()
        {
            
        }

        public PropertyJsonSerializableObject([NotNull] IThreatModel model, [NotNull] IJsonSerializableObjectPropertyType propertyType) : this()
        {
            _id = Guid.NewGuid();
            _modelId = model.Id;
            _model = model;
            PropertyTypeId = propertyType.Id;
        }

        #region Additional placeholders required.
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
        protected Guid _id { get; set; }
        #endregion
        
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

        #region Specific implementation.
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

        [JsonProperty("value")]
        private object _value;

        public virtual object Value
        {
            get => _value;
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (value != _value)
                {
                    _value = value;
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