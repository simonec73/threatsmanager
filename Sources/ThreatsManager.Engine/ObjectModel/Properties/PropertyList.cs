using System;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
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
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyList, ThreatsManager.Engine")]
    public class PropertyList : IPropertyList, IInitializableObject
    {
        public PropertyList()
        {
            
        }

        public PropertyList([NotNull] IThreatModel model, [NotNull] IListPropertyType propertyType) : this()
        {
            _id = Guid.NewGuid();
            _modelId = model.Id;
            _model = model;
            PropertyTypeId = propertyType.Id;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty && PropertyTypeId != Guid.Empty;

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
        [JsonProperty("item")]
        private string _item;

        private IListItem _value;

        [InitializationRequired]
        public string StringValue
        {
            get => _item;
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (PropertyType is IListPropertyType propertyType)
                {
                    Value = propertyType.Values?.FirstOrDefault(x => string.CompareOrdinal(x.Id, value) == 0);
                }
            }
        }

        [InitializationRequired]
        public virtual IListItem Value 
        {
            get
            {
                if (_value == null && !string.IsNullOrWhiteSpace(_item))
                {
                    if (PropertyType is IListPropertyType listPropertyType)
                    {
                        _value = listPropertyType.Values?.FirstOrDefault(x => string.CompareOrdinal(x.Id, _item) == 0);
                    }
                }

                return _value;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (PropertyType is IListPropertyType listPropertyType)
                {
                    if (value != null)
                    {
                        _value = listPropertyType.Values?.FirstOrDefault(x => string.CompareOrdinal(x.Id, value.Id) == 0);
                        _item = _value != null ? value.Id : null;
                    }
                    else
                    {
                        _value = null;
                        _item = null;
                    }

                    InvokeChanged();
                }
            }
        }

        public override string ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }

        protected void InvokeChanged()
        {
            Changed?.Invoke(this);
        }
        #endregion
    }
}