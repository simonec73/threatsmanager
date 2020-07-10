using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.Entities
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [Serializable]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [PropertiesContainerAspect]
    [TypeLabel("Trust Boundary Template")]
    public class TrustBoundaryTemplate : ITrustBoundaryTemplate, IInitializableObject
    {
        public TrustBoundaryTemplate()
        {
        }

        public TrustBoundaryTemplate([NotNull] IThreatModel model, [Required] string name)
        {
            _modelId = model.Id;
            _model = model;
            _id = Guid.NewGuid();
            Name = name;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
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

        public void ClearProperties()
        {
        }

        #endregion

        #region Additional placeholders required.
        protected Guid _id { get; set; }
        private Guid _modelId { get; set; }
        private IThreatModel _model { get; set; }
        private IPropertiesContainer PropertiesContainer => this;
        private List<IProperty> _properties { get; set; }
        #endregion        
 
        #region Specific implementation.
        public override string ToString()
        {
            return Name ?? "<undefined>";
        }

        [InitializationRequired]
        public ITrustBoundary CreateTrustBoundary([Required] string name)
        {
            ITrustBoundary result = _model.AddGroup<ITrustBoundary>(name);

            if (result != null)
            {
                result.Description = Description;
                this.CloneProperties(result);
            }

            return result;
        }

        public void ApplyTo([NotNull] ITrustBoundary trustBoundary)
        {
            trustBoundary.ClearProperties();
            this.CloneProperties(trustBoundary);
            if (trustBoundary is TrustBoundary internalTb)
            {
                internalTb._templateId = Id;
                internalTb._template = this;
            }
        }

        public ITrustBoundaryTemplate Clone([NotNull] ITrustBoundaryTemplatesContainer container)
        {
            TrustBoundaryTemplate result = null;

            if (container is IThreatModel model)
            {
                result = new TrustBoundaryTemplate()
                {
                    _id = Id,
                    Name = Name,
                    Description = Description,
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
