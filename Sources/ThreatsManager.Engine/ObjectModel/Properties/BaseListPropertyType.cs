﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.Aspects;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [PropertyTypeAspect]
    [Recordable]
    public class BaseListPropertyType : IPropertyType
    {
        public BaseListPropertyType()
        {

        }

        public BaseListPropertyType([Required] string name, [NotNull] IPropertySchema schema) : this()
        {
            _id = Guid.NewGuid();
            _schemaId = schema.Id;
            _model = schema.Model;
            Name = name;
            Visible = true;
        }

        #region Default implementation.
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }
        public Guid SchemaId { get; set; }
        public int Priority { get; set; }
        public bool Visible { get; set; }
        public bool DoNotPrint { get; set; }
        public bool ReadOnly { get; set; }
        public string CustomPropertyViewer { get; set; }

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

        #region Additional placeholders required.
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("name")]
        protected string _name { get; set; }
        [JsonProperty("description")]
        protected string _description { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateId("Id", "_modelId")]
        protected IThreatModel _model { get; set; }
        [JsonProperty("schema")]
        protected Guid _schemaId { get; set; }
        #endregion

        #region Specific implementation.
        /// <summary>
        /// Identifier of the List Provider that provides the list of available items.
        /// </summary>
        [JsonProperty("listProvider")]
        [NotRecorded]
        protected string _listProviderId;

        [Reference]
        [NotRecorded]
        private IListProviderExtension _listProvider;

        private IListProviderExtension ListProvider
        {
            get
            {
                if (_listProvider == null && !string.IsNullOrWhiteSpace(_listProviderId))
                {
                    _listProvider = Manager.Instance.GetExtension<IListProviderExtension>(_listProviderId);
                }

                return _listProvider;
            }
        }

        public void SetListProvider([NotNull] IListProviderExtension listProvider)
        {
            _listProviderId = listProvider.GetExtensionId();
            _listProvider = listProvider;
        }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("cachedList")]
        [NotRecorded]
        [Reference]
        protected IEnumerable<IListItem> _cachedList;

        public IEnumerable<IListItem> Values
        {
            get
            {
                IEnumerable<IListItem> result = null;

                var listProvider = ListProvider;

                if (listProvider != null)
                {
                    result = listProvider.GetAvailableItems(Context);
                    _cachedList = result?.ToArray();
                }
                else
                {
                    result = _cachedList;
                }

                return result;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual IPropertyType Clone(IPropertyTypesContainer container)
        {
            return null;
        }
        #endregion
    }
}