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
        public string StringValue
        {
            get => _value.ToString("N");
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

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
        [NotRecorded]
        private Guid _value { get; set; }

        public virtual Guid ValueId => _value;

        [Reference]
        [NotRecorded]
        private IIdentity _identity;

        public virtual IIdentity Value
        {
            get => _identity ?? (_identity = Model?.GetIdentity(_value));
            set
            {
                if (value != null && (value.Id != _value))
                {
                    if (ReadOnly)
                        throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

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
                        throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

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
        #endregion
    }
}
