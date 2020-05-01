using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Diagrams
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [Serializable]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    public class Link : ILink, IThreatModelChild, IInitializableObject
    {
        private IDataFlow _dataFlow;

        public Link()
        {

        }

        public Link([NotNull] IDataFlow dataFlow) : this()
        {
            _dataFlow = dataFlow;
            _modelId = dataFlow.Model.Id;
            _model = dataFlow.Model;
            _associatedId = _dataFlow.Id;
        }

        public bool IsInitialized => Model != null && _associatedId != Guid.Empty;

        #region Specific implementation.
        [JsonProperty("id")]
        private Guid _associatedId;

        public Guid AssociatedId => _associatedId;

        [InitializationRequired]
        public IDataFlow DataFlow => _dataFlow ?? (_dataFlow = _model?.GetDataFlow(_associatedId));

        public ILink Clone(ILinksContainer container)
        {
            Link result = null;
            if (container is IThreatModelChild child && child.Model is IThreatModel model)
            {
                result = new Link()
                {
                    _associatedId = _associatedId,
                    _model = model,
                    _modelId = model.Id,
                };
                this.CloneProperties(result);

                container.Add(result);
            }

            return result;
        }

        #endregion

        #region Additional placeholders required.
        private Guid _modelId { get; set; }
        private IThreatModel _model { get; set; }
        private IPropertiesContainer PropertiesContainer => this;
        private List<IProperty> _properties { get; set; }
        #endregion

        #region Default implementation.
        public IThreatModel Model { get; }

        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        public IEnumerable<IProperty> Properties { get; }
        public bool HasProperty(IPropertyType propertyType)
        {
            return false;
        }
        public IProperty GetProperty(IPropertyType propertyType)
        {
            return null;
        }

        public IProperty AddProperty(IPropertyType propertyType, string value)
        {
            return null;
        }

        public bool RemoveProperty(IPropertyType propertyType)
        {
            return false;
        }
 
        public bool RemoveProperty(Guid propertyTypeId)
        {
            return false;
        }
        #endregion
    }
}
