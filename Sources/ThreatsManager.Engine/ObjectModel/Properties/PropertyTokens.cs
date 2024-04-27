using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Utilities.Exceptions;
using PostSharp.Patterns.Collections;
using ThreatsManager.Interfaces;
using System.Runtime.Serialization;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [PropertyAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyTokens, ThreatsManager.Engine")]
    public class PropertyTokens : IPropertyTokens, IInitializableObject
    {
        public PropertyTokens()
        {

        }

        public PropertyTokens([NotNull] ITokensPropertyType propertyType) : this()
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
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("propertyTypeId")]
        protected Guid _propertyTypeId { get; set; }
        [JsonProperty("readOnly")]
        protected bool _readOnly { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        protected IThreatModel _model { get; set; }
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
        [property: NotRecorded]
        [InitializationRequired]
        public string StringValue
        {
            get => Value?.TagConcat();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                Value = value?.TagSplit();
            }
        }

        [Reference]
        [JsonProperty("values")]
        [field: NotRecorded]
        private List<string> _legacyValues { get; set; }

        [Child]
        [JsonProperty("tokens")]
        [field: NotRecorded]
        private AdvisableCollection<RecordableString> _values { get; set; }

        [InitializationRequired]
        public virtual IEnumerable<string> Value
        {
            get => _values?.Select(x => x.Value).ToArray();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                var existing = _values?.Select(x => x.Value).OrderBy(x => x).TagConcat();
                var incoming = value?.OrderBy(x => x).TagConcat();

                if (string.CompareOrdinal(existing, incoming) != 0)
                {
                    using (var scope = UndoRedoManager.OpenScope("Set Property Tokens"))
                    {
                        if (_values?.Any() ?? false)
                        {
                            foreach (var item in _values)
                            {
                                UndoRedoManager.Detach(item);
                            }
                            _values.Clear();
                        }

                        foreach (var item in value)
                        {
                            if (_values == null)
                                _values = new AdvisableCollection<RecordableString>();

                            var r = new RecordableString(item);
                            UndoRedoManager.Attach(r, Model);
                            _values.Add(r);
                        }

                        scope?.Complete();
                    }
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
            if (_legacyValues?.Any() ?? false)
            {
                if (_values == null)
                    _values = new AdvisableCollection<RecordableString>();

                foreach (var value in _legacyValues)
                {
                    var r = new RecordableString(value);
                    UndoRedoManager.Attach(r, Model);
                    _values.Add(r);
                }

                _legacyValues.Clear();
            }
        }
        #endregion
    }
}
