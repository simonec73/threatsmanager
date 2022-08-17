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

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [NotifyPropertyChanged]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [PropertyTypeAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.PropertyJsonSerializableObject, ThreatsManager.Engine")]
    public class JsonSerializableObjectPropertyType : IJsonSerializableObjectPropertyType
    {
        public JsonSerializableObjectPropertyType()
        {

        }

        public JsonSerializableObjectPropertyType([Required] string name, [NotNull] IPropertySchema schema) : this()
        {
            _id = Guid.NewGuid();
            _schemaId = schema.Id;
            _model = schema.Model;
            Name = name;
            Visible = true;
        }

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }
        public Guid SchemaId { get; }
        public int Priority { get; set; }
        public bool Visible { get; set; }
        public bool DoNotPrint { get; set; }
        public bool ReadOnly { get; set; }
        public string CustomPropertyViewer { get; set; }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("name")]
        protected string _name { get; set; }
        [JsonProperty("description")]
        protected string _description { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateId("Id", "_modelId")]
        protected IThreatModel _model { get; set; }
        [JsonProperty("schema")]
        protected Guid _schemaId { get; set; }
        #endregion

        #region Specific implementation.
        public override string ToString()
        {
            return Name;
        }

        public IPropertyType Clone([NotNull] IPropertyTypesContainer container)
        {
            IPropertyType result = null;

            if (container is IPropertySchema schema)
            {
                result = new JsonSerializableObjectPropertyType
                {
                    _id = _id,
                    _schemaId = schema.Id,
                    _model = schema.Model,
                    _modelId = schema.Model?.Id ?? Guid.Empty,
                    Name = Name,
                    Description = Description,
                    Visible = Visible,
                    DoNotPrint = DoNotPrint,
                    ReadOnly = ReadOnly,
                    CustomPropertyViewer = CustomPropertyViewer,
                    Priority = Priority
                };
                container.Add(result);
            }
            return result;
        }
        #endregion
    }
}