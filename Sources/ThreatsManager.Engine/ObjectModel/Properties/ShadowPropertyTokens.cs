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
    public class ShadowPropertyTokens : PropertyTokens, IShadowProperty
    {
        public ShadowPropertyTokens()
        {

        }

        public ShadowPropertyTokens([NotNull] IPropertyTokens original) : base(original.Model, original.PropertyType as ITokensPropertyType)
        {
            _originalId = original.Id;
            _original = original;
        }

        #region Specific implementation.
        public override IEnumerable<string> Value
        {
            get
            {
                IEnumerable<string> result = base.Value;

                if (!_overridden)
                {
                    if (Original is IPropertyTokens originalProperty)
                        result = originalProperty.Value;
                }

                return result;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (Original is IPropertyTokens originalProperty)
                {
                    if (!Equals(value, originalProperty.Value))
                    {
                        if (!_overridden)
                        {
                            _overridden = true;
                            InvokeChanged();
                        }

                        if (!Equals(base.Value, value))
                        {
                            base.Value = value;
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

        private bool Equals(IEnumerable<string> first, IEnumerable<string> second)
        {
            var f = first?.ToArray();
            var s = second?.ToArray();

            return (!(f?.Any() ?? false) && !(s?.Any() ?? false)) ||
                   ((f?.Any() ?? false) && (s?.Any() ?? false) && f.SequenceEqual(s));
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