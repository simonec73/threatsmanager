using System;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Utilities.Exceptions;
using ThreatsManager.Utilities;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Newtonsoft.Json.Linq;
using System.Reflection.Emit;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [PropertyAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [AssociatedPropertyClass("ThreatsManager.Engine.ObjectModel.Properties.ShadowPropertyUrlList, ThreatsManager.Engine")]
    public class PropertyUrlList : IPropertyUrlList
    {
        public PropertyUrlList()
        {
        }

        public PropertyUrlList([NotNull] IUrlListPropertyType propertyType) : this()
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

        public Guid SourceTMId { get; }

        public string SourceTMName { get; }

        public string VersionId { get; }

        public string VersionAuthor { get; }

        public void SetSourceInfo(IThreatModel source)
        {
        }
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
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [JsonProperty("sourceTMId")]
        protected Guid _sourceTMId { get; set; }
        [JsonProperty("sourceTMName")]
        protected string _sourceTMName { get; set; }
        [JsonProperty("versionId")]
        protected string _versionId { get; set; }
        [JsonProperty("versionAuthor")]
        protected string _versionAuthor { get; set; }
        #endregion

        #region Specific implementation.
        [JsonProperty("value")]
        private string _value { get; set; }

        [property: NotRecorded]
        public virtual string StringValue
        {
            get => _value;
            set
            {
                if (ReadOnly)
                    throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

                if (string.CompareOrdinal(value, _value) != 0)
                {
                    _value = value;
                    InvokeChanged();
                }
            }
        }

        public IEnumerable<IUrl> Values
        {
            get
            {
                return _value?.SplitUrlDefinitions();
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

        public string GetUrl([Required] string label)
        {
            return Values?.FirstOrDefault(x => string.CompareOrdinal(x.Label, label) == 0)?.Url;
        }

        public void SetUrl([Required] string label, [Required] string url)
        {
            if (ReadOnly)
                throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

            var stringValue = _value?.SetUrlDefinition(label, url);
            if (string.CompareOrdinal(stringValue, _value) != 0)
            {
                _value = stringValue;
                InvokeChanged();
            }
        }

        public void SetUrl([Required] string oldLabel, [Required] string newLabel, [Required] string url)
        {
            if (ReadOnly)
                throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

            var stringValue = _value?.SetUrlDefinition(oldLabel, newLabel, url);
            if (string.CompareOrdinal(stringValue, _value) != 0)
            {
                _value = stringValue;
                InvokeChanged();
            }
        }

        public bool DeleteUrl([Required] string label)
        {
            if (ReadOnly)
                throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

            string newValue = null;
            var result = _value?.DeleteUrlDefinition(label, out newValue) ?? false;
            if (result)
            {
                _value = newValue;
                InvokeChanged();
            }

            return result;
        }

        public void ClearUrls()
        {
            if (ReadOnly)
                throw new ReadOnlyPropertyException(PropertyType?.Name ?? ThreatModelManager.Unknown);

            if (_value != null)
            {
                _value = null;
                InvokeChanged();
            }
        }
        #endregion

    }
}