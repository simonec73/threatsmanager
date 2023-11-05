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

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertyAspect]
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
        public string StringValue
        {
            get => _value.ToString("N");
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");
                _value = Guid.TryParse(value, out var result) ? result : Guid.Empty;
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

                    _value = value.Id;
                    _identity = value;
                    InvokeChanged();
                }
                else if (value == null && _value != Guid.Empty)
                {
                    if (ReadOnly)
                        throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                    _value = Guid.Empty;
                    _identity = null;
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
