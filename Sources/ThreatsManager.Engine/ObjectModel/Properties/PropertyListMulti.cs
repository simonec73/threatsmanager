using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
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
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyListMulti, ThreatsManager.Engine")]
    public class PropertyListMulti : IPropertyListMulti, IInitializableObject
    {
        public PropertyListMulti()
        {
            
        }

        public PropertyListMulti([NotNull] IThreatModel model, [NotNull] IListMultiPropertyType propertyType) : this()
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
        [JsonProperty("items")]
        private List<string> _items;

        private List<IListItem> _values;

        [InitializationRequired]
        public string StringValue
        {
            get => _items.TagConcat();
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (PropertyType is IListPropertyType propertyType)
                {
                    Values = value?.TagSplit()
                        .Select(x => propertyType.Values.FirstOrDefault(y => string.CompareOrdinal(x, y.Id) == 0))
                        .Where(x => x != null);
                }
            }
        }

        [InitializationRequired]
        public virtual IEnumerable<IListItem> Values
        {
            get => _values?.AsReadOnly();

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                _items?.Clear();
                _values?.Clear();

                if ((value?.Any() ?? false) && PropertyType is IListMultiPropertyType propertyType)
                {
                    foreach (var item in value)
                    {
                        if (propertyType.Values.Any(x => x.Equals(item)))
                        {
                            if (_items == null)
                                _items = new List<string>();
                            if (_values == null)
                                _values = new List<IListItem>();

                            _items.Add(item.Id);
                            _values.Add(item);
                        }
                    }
                }

                InvokeChanged();
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