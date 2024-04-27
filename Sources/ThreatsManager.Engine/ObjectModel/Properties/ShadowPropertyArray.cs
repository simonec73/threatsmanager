﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ShadowPropertyArray : PropertyArray, IShadowProperty
    {
        public ShadowPropertyArray()
        {

        }

        public ShadowPropertyArray([NotNull] IPropertyArray original) : base(original.PropertyType as IArrayPropertyType)
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
        public override IEnumerable<string> Value
        {
            get
            {
                var result = base.Value;

                if (!_overridden)
                {
                    if (Original is IPropertyArray originalProperty)
                        result = originalProperty.Value;
                }

                return result;
            }

            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                if (Original is IPropertyArray originalProperty)
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
                            base.Value = value.ToArray();
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
    }
}