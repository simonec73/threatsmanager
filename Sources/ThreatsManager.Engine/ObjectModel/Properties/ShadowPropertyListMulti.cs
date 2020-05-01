using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ShadowPropertyAspect]
    public class ShadowPropertyListMulti : PropertyListMulti, IShadowProperty
    {
        public ShadowPropertyListMulti()
        {

        }

        public ShadowPropertyListMulti([NotNull] IPropertyListMulti original) : base(original.Model, original.PropertyType as IListMultiPropertyType)
        {
            _originalId = original.Id;
            _original = original;
        }

        #region Specific implementation.
        public override IEnumerable<IListItem> Values
        {
            get
            {
                var result = base.Values;

                if (!_overridden)
                {
                    if (Original is IPropertyListMulti originalProperty)
                        result = originalProperty.Values;
                }

                return result;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (Original is IPropertyListMulti originalProperty)
                {
                    if (!Equals(value, originalProperty.Values))
                    {
                        if (!_overridden)
                        {
                            _overridden = true;
                            InvokeChanged();
                        }

                        if (!Equals(base.Values, value))
                        {
                            base.Values = value;
                        }
                    }
                    else
                    {
                        if (_overridden)
                        {
                            _overridden = false;
                            InvokeChanged();
                        }
                    }
                }
            }
        }

        private bool Equals(IEnumerable<IListItem> first, IEnumerable<IListItem> second)
        {
            var f = first?.ToArray();
            var s = second?.ToArray();

            return (!(f?.Any() ?? false) && !(s?.Any() ?? false)) ||
                   ((f?.Any() ?? false) && (s?.Any() ?? false) && f.SequenceEqual(s, new ListItemComparer()));
        }
        #endregion

        #region Default implementation.
        public IProperty Original { get; }
        public bool IsOverridden { get; }
        public void RevertToOriginal()
        {
        }
        #endregion

        #region Additional placeholders required.
        private Guid _originalId { get; set; }
        private bool _overridden { get; set; }
        private IProperty _original { get; set; }
        #endregion    
    }
}