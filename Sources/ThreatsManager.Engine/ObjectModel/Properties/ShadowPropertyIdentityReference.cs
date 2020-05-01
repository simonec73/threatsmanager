using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Exceptions;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [ShadowPropertyAspect]
    public class ShadowPropertyIdentityReference : PropertyIdentityReference, IShadowProperty
    {
        public ShadowPropertyIdentityReference()
        {

        }

        public ShadowPropertyIdentityReference([NotNull] IPropertyIdentityReference original) : base(original.Model, original.PropertyType as IIdentityReferencePropertyType)
        {
            _originalId = original.Id;
            _original = original;
        }

        #region Specific implementation.
        public override IIdentity Value
        {
            get
            {
                var result = base.Value;

                if (!_overridden)
                {
                    if (Original is IPropertyIdentityReference originalProperty)
                        result = originalProperty.Value;
                }

                return result;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? "<unknown>");

                if (Original is IPropertyIdentityReference originalProperty)
                {
                    if (value != originalProperty.Value)
                    {
                        if (!_overridden)
                        {
                            _overridden = true;
                            InvokeChanged();
                        }

                        if (base.Value != value)
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

        public override Guid ValueId
        {
            get
            {
                var result = base.ValueId;

                if (!_overridden)
                {
                    if (Original is IPropertyIdentityReference originalProperty)
                        result = originalProperty.ValueId;
                }

                return result;
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