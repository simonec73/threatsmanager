using System;
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
    public class ShadowPropertySingleLineString : PropertySingleLineString, IShadowProperty
    {
        public ShadowPropertySingleLineString()
        {

        }

        public ShadowPropertySingleLineString([NotNull] IPropertySingleLineString original) : base(original.Model,
            original.PropertyType as ISingleLineStringPropertyType)
        {
            _originalId = original.Id;
            _original = original;
        }

        #region Specific implementation.
        public override string StringValue
        {
            get => _overridden ? base.StringValue : Original?.StringValue;

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (string.CompareOrdinal(value, Original?.StringValue) != 0)
                {
                    if (!_overridden)
                    {
                        _overridden = true;
                        InvokeChanged();
                    }

                    if (string.CompareOrdinal(value, base.StringValue) != 0)
                    {
                        base.StringValue = value;
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