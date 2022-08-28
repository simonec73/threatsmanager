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
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Utilities.Exceptions;
using PostSharp.Patterns.Collections;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [PropertyAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyTokens, ThreatsManager.Engine")]
    public class PropertyTokens : IPropertyTokens
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
        #endregion

        #region Specific implementation.
        [property: NotRecorded]
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

        [Child]
        [JsonProperty("values")]
        [NotRecorded]
        private IList<string> _values;

        public virtual IEnumerable<string> Value
        {
            get => _values?.AsEnumerable();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (value?.Any() ?? false)
                {
                    if (_values == null)
                        _values = new AdvisableCollection<string>();
                    else
                        _values.Clear();
                    foreach (var item in value)
                        _values.Add(item);
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
    }
}
