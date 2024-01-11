using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Aspects.Engine;
using System.Reflection;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertyTypeAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.PropertyArray, ThreatsManager.Engine")]
    public class ArrayPropertyType : IArrayPropertyType
    {
        public ArrayPropertyType()
        {

        }

        public ArrayPropertyType([Required] string name, [NotNull] IPropertySchema schema) : this()
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

        public Guid SourceTMId { get; }

        public string SourceTMName { get; }

        public string VersionId { get; }

        public string VersionAuthor { get; }

        public void SetSourceInfo(IThreatModel source)
        {
        }
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
        [field: UpdateThreatModelId]
        protected IThreatModel _model { get; set; }
        [JsonProperty("schema")]
        protected Guid _schemaId { get; set; }
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
        public IPropertyType Clone([NotNull] IPropertyTypesContainer container)
        {
            IPropertyType result = null;

            if (container is IPropertySchema schema)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Array Property Type"))
                {
                    result = new ArrayPropertyType()
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

                    if ((schema.Model?.Id ?? Guid.Empty) != _modelId)
                        result.SetSourceInfo(Model);

                    scope?.Complete();
                }
            }

            return result;
        }
        #endregion
    }
}