using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Utilities.Exceptions;
using ThreatsManager.Utilities;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

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
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyUrl, ThreatsManager.Engine")]
    public class PropertyUrl : IPropertyUrl
    {
        public PropertyUrl()
        {
        }

        public PropertyUrl([NotNull] IUrlPropertyType propertyType) : this()
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
        [field: AutoApplySchemas]
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
        [JsonProperty("value")]
        [field: NotRecorded]
        private string _value { get; set; }

        public virtual string StringValue
        {
            get => _value;
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                if (string.CompareOrdinal(value, _value) != 0)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var regex = new Regex(@"(?'label'[^\(]+) \((?'url'https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*))\)");
                        var match = regex.Match(value);
                        if (match.Success)
                        {
                            _value = value;
                            InvokeChanged();
                        }
                        else
                            throw new ArgumentException($"'{value}' is invalid: it must be in the format 'label (url)'.");
                    }
                    else
                    {
                        _value = null;
                        InvokeChanged();
                    }
                }
            }
        }

        public string Url
        {
            get
            {
                string result = null;

                if (_value != null)
                {
                    var regex = new Regex(@"(?'label'[^\(]+) \((?'url'https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*))\)");
                    var match = regex.Match(_value);
                    if (match.Success)
                    {
                        result = match.Groups["url"].Value;
                    }
                }

                return result;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                var stringValue = $"{Label ?? ThreatModelManager.Undefined} ({value})";
                if (string.CompareOrdinal(stringValue, _value) != 0)
                {
                    _value = stringValue;
                    InvokeChanged();
                }
            }
        }

        public string Label
        {
            get
            {
                string result = null;

                if (_value != null)
                {
                    var regex = new Regex(@"(?'label'[^\(]+) \((?'url'https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*))\)");
                    var match = regex.Match(_value);
                    if (match.Success)
                    {
                        result = match.Groups["label"].Value;
                    }
                }

                return result;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                var stringValue = $"{value} ({Url ?? "https://www.threatsmanager.com"})";
                if (string.CompareOrdinal(stringValue, _value) != 0)
                {
                    _value = stringValue;
                    InvokeChanged();
                }
            }
        }

        public override string ToString()
        {
            return StringValue;
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
                            var containerPropertyType = containerSchema.GetPropertyType(propertyType.Name) as IUrlPropertyType;
                            if (containerPropertyType != null)
                                propertyTypeId = containerPropertyType.Id;
                        }
                    }
                }
            }

            if (propertyTypeId != Guid.Empty)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Property Url"))
                {
                    result = new PropertyUrl()
                    {
                        _id = Id,
                        _propertyTypeId = propertyTypeId,
                        _model = model,
                        _modelId = model.Id,
                        _value = _value,
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