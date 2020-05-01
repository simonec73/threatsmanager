using System;
using System.Collections.Generic;
using System.Linq;
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
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [Serializable]
    [PropertyAspect]
    [ThreatModelChildAspect]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyTokens, ThreatsManager.Engine")]
    public class PropertyTokens : IPropertyTokens
    {
        public PropertyTokens()
        {

        }

        public PropertyTokens([NotNull] IThreatModel model, [NotNull] ITokensPropertyType propertyType) : this()
        {
            _id = Guid.NewGuid();
            _modelId = model.Id;
            _model = model;
            PropertyTypeId = propertyType.Id;
        }

        #region Specific implementation.
        public string StringValue
        {
            get => Value?.TagConcat();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                Value = value?.TagSplit();
            }
        }

        [JsonProperty("values")]
        private List<string> _values;

        public virtual IEnumerable<string> Value
        {
            get => _values?.AsReadOnly();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (value?.Any() ?? false)
                {
                    if (_values == null)
                        _values = new List<string>();
                    else
                        _values.Clear();
                    _values.AddRange(value);
                }
                else
                {
                    _values = null;
                }
                Changed?.Invoke(this);
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
        #endregion

        #region Default implementation.
        public Guid Id { get; }
        public event Action<IProperty> Changed;
        public Guid PropertyTypeId { get; set; }
        public IPropertyType PropertyType { get; }
        public bool ReadOnly { get; set; }
        public IThreatModel Model { get; }
        #endregion

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        private Guid _modelId { get; set; }
        private IThreatModel _model { get; set; }
        #endregion
    }
}
