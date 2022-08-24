using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using ThreatsManager.Engine.Aspects;
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
    [Serializable]
    [NotifyPropertyChanged]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [AssociatedIdChanger]
    [PropertiesContainerAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class Link : ILink, IThreatModelChild, IInitializableObject
    {
        public Link()
        {

        }

        public Link([NotNull] IDataFlow dataFlow) : this()
        {
            _model = dataFlow.Model;
            _associated = dataFlow;
        }

        public bool IsInitialized => Model != null && _associatedId != Guid.Empty;

        #region Default implementation.
        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }

        public event Action<IPropertiesContainer, IProperty> PropertyAdded;
        public event Action<IPropertiesContainer, IProperty> PropertyRemoved;
        public event Action<IPropertiesContainer, IProperty> PropertyValueChanged;
        [Reference]
        [field: NotRecorded]
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

        public void ClearProperties()
        {
        }

        public void Apply(IPropertySchema schema)
        {
        }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Reference]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        [Child]
        [JsonProperty("properties")]
        private IList<IProperty> _properties { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.Link;

        [Reference]
        [field: NotRecorded]
        [field: UpdateAssociatedId]
        private IDataFlow _associated;

        [JsonProperty("id")]
        private Guid _associatedId { get; set; }

        public Guid AssociatedId => _associatedId;

        [InitializationRequired]
        [IgnoreAutoChangeNotification]
        public IDataFlow DataFlow => _associated ?? (_associated = _model?.GetDataFlow(_associatedId));

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
    }
}
