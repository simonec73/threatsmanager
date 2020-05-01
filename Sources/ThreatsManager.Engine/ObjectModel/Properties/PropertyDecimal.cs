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
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [Serializable]
    [ThreatModelChildAspect]
    [PropertyAspect]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyDecimal, ThreatsManager.Engine")]
    public class PropertyDecimal : IPropertyDecimal
    {
        public PropertyDecimal()
        {
            
        }

        public PropertyDecimal([NotNull] IThreatModel model, [NotNull] IDecimalPropertyType propertyType) : this()
        {
            _id = Guid.NewGuid();
            _modelId = model.Id;
            _model = model;
            PropertyTypeId = propertyType.Id;
        }

        #region Additional placeholders required.
        private Guid _modelId { get; set; }
        private IThreatModel _model { get; set; }
        protected Guid _id { get; set; }
        #endregion
        
        #region Default implementation.
        public Guid Id { get; }
        public event Action<IProperty> Changed;
        public Guid PropertyTypeId { get; set; }
        public IPropertyType PropertyType { get; }
        public bool ReadOnly { get; set; }
        public IThreatModel Model { get; }
        #endregion

        #region Specific implementation.
        public string StringValue
        {
            get => Value.ToString();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (decimal.TryParse(value, out var result))
                {
                    Value = result;
                }
                else
                {
                    Value = default(decimal);
                }
            }
        }

        [JsonProperty("value")]
        private decimal _value;

        public virtual decimal Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    if (ReadOnly)
                        throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                    _value = value;
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
