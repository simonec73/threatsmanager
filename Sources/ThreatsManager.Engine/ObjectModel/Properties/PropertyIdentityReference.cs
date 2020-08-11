using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
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
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyIdentityReference, ThreatsManager.Engine")]
    public class PropertyIdentityReference : IPropertyIdentityReference
    {
        public PropertyIdentityReference()
        {
            
        }

        public PropertyIdentityReference([NotNull] IThreatModel model, [NotNull] IIdentityReferencePropertyType propertyType) : this()
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
        private Guid _value;

        public virtual Guid ValueId => _value;

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
