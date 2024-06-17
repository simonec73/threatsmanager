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
        [NotRecorded]
        private string _value { get; set; }

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
                IEnumerable<IUrl> result = null;

                var lines = _value?.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (lines != null)
                {
                    var list = new List<IUrl>();
                    foreach (var line in lines)
                    {
                        var url = new UrlItem(line);
                        if (url.IsValid)
                            list.Add(url);
                    }

                    if (list.Any())
                        result = list;
                }

                return result;
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
            var values = Values?.ToList() ?? new List<IUrl>();
            var urlItem = values.FirstOrDefault(x => string.CompareOrdinal(x.Label, label) == 0);
            if (urlItem != null)
                urlItem.Url = url;
            else
            {
                values.Add(new UrlItem(label, url));
            }

            StringValue = values.Select(x => x.ToString()).Aggregate((x, y) => $"{x}\r\n{y}");
        }

        public void SetUrl([Required] string originalLabel, [Required] string newLabel, [Required] string url)
        {
            var values = Values?.ToList() ?? new List<IUrl>();
            var urlItem = values.FirstOrDefault(x => string.CompareOrdinal(x.Label, originalLabel) == 0);
            if (urlItem != null)
            {
                urlItem.Label = newLabel;
                urlItem.Url = url;
            }
            else
            {
                values.Add(new UrlItem(newLabel, url));
            }

            StringValue = values.Select(x => x.ToString()).Aggregate((x, y) => $"{x}\r\n{y}");
        }

        public bool DeleteUrl([Required] string label)
        {
            bool result = false;

            var values = Values?.ToList();
            var found = values?.Any(x => string.CompareOrdinal(x.Label, label) == 0) ?? false;
            if (found)
            {
                StringValue = values
                    .Select(x => string.CompareOrdinal(x.Label, label) != 0 ? null : x.ToString())
                    .Aggregate((x, y) => $"{x}\r\n{y}");
                result = true;
            }

            return result;
        }

        public void ClearUrls()
        {
            StringValue = null;
        }
        #endregion

    }
}