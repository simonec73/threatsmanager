using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ShadowPropertyAspect]
    public class ShadowPropertyUrl : PropertyUrl, IShadowProperty
    {
        public ShadowPropertyUrl()
        {

        }

        public ShadowPropertyUrl([NotNull] IPropertyUrl original) : base(original.PropertyType as IUrlPropertyType)
        {
            _originalId = original.Id;
            _original = original;
        }

        #region Default implementation.
        [Reference]
        [field: NonSerialized]
        public IProperty Original { get; }
        public bool IsOverridden { get; }
        public void RevertToOriginal()
        {
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("originalId")]
        private Guid _originalId { get; set; }
        [JsonProperty("overridden")]
        private bool _overridden { get; set; }
        [Reference]
        [field: NonSerialized]
        [field: NotRecorded]
        private IProperty _original { get; set; }
        #endregion    

        #region Specific implementation.
        public override string StringValue
        {
            get => _overridden ? base.StringValue : Original?.StringValue;

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

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
    }
}