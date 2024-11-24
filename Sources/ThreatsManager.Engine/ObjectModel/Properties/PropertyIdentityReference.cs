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
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyIdentityReference, ThreatsManager.Engine")]
    public class PropertyIdentityReference : IPropertyIdentityReference
    {
        public PropertyIdentityReference()
        {
            
        }

        public PropertyIdentityReference([NotNull] IIdentityReferencePropertyType propertyType) : this()
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
        [property:NotRecorded]
        public string StringValue
        {
            get => _value.ToString("N");
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                var newValue = Guid.TryParse(value, out var result) ? result : Guid.Empty;
                if (newValue != _value)
                {
                    using (var scope = UndoRedoManager.OpenScope("Set Property Identity Reference"))
                    {
                        _value = newValue;
                        _identity = null;

                        scope?.Complete();
                    }
                }

                InvokeChanged();
            }
        }

        [JsonProperty("value")]
        [field:NotRecorded]
        private Guid _value { get; set; }

        public virtual Guid ValueId => _value;

        [Reference]
        [field: NotRecorded]
        private IIdentity _identity { get; set; }

        public virtual IIdentity Value
        {
            get => _identity ?? (_identity = Model?.GetIdentity(_value));
            set
            {
                if (value != null && (value.Id != _value))
                {
                    if (ReadOnly)
                        throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                    using (var scope = UndoRedoManager.OpenScope("Set Property Identity Reference"))
                    {
                        _value = value.Id;
                        _identity = value;
                        scope?.Complete();
                    }
                    InvokeChanged();
                }
                else if (value == null && _value != Guid.Empty)
                {
                    if (ReadOnly)
                        throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                    using (var scope = UndoRedoManager.OpenScope("Set Property Identity Reference"))
                    {
                        _value = Guid.Empty;
                        _identity = null;
                        scope?.Complete();
                    }
                    InvokeChanged();
                }
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
                            var containerPropertyType = containerSchema.GetPropertyType(propertyType.Name) as IIdentityReferencePropertyType;
                            if (containerPropertyType != null)
                                propertyTypeId = containerPropertyType.Id;
                        }
                    }
                }
            }

            if (propertyTypeId != Guid.Empty)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Property Identity Reference"))
                {
                    result = new PropertyIdentityReference()
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
