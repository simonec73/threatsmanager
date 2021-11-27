using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [AutoDirty]
    [DirtyAspect]
    [IdentityAspect]
    [PropertiesContainerAspect]
    [ThreatModelChildAspect]
    [TypeLabel("Threat Actor")]
    public class ThreatActor : IThreatActor, IInitializableObject
    {
        public ThreatActor()
        {

        }

        public ThreatActor([NotNull] IThreatModel model, DefaultActor actor)
        {
            _id = Guid.NewGuid();
            _model = model;
            _modelId = model.Id;
            _actor = actor;
            Name = actor.GetEnumLabel();
            Description = actor.GetEnumDescription();
        }

        public ThreatActor([NotNull] IThreatModel model, [Required] string name) : this()
        {
            _id = Guid.NewGuid();
            _model = model;
            _modelId = model.Id;
            _actor = DefaultActor.Unknown;
            Name = name;
        }

        public bool IsInitialized => _id != Guid.Empty && Model != null;

        #region Specific implementation.
        public Scope PropertiesScope => Scope.ThreatActor;

        [JsonProperty("actor")]
        private DefaultActor _actor = DefaultActor.Unknown;

        public DefaultActor ActorType => _actor;

        public IThreatActor Clone(IThreatActorsContainer container)
        {
            ThreatActor result = null;

            if (container is IThreatModel model)
            {
                result = new ThreatActor
                {
                    _id = Id, 
                    Name = Name, 
                    Description = Description,
                    _model = model, 
                    _modelId = model.Id,
                    _actor = ActorType
                };
                this.CloneProperties(result);
                container.Add(result);
            }

            return result;
        }

        public void Apply([NotNull] IThreatActor actor)
        {
            _id = actor.Id;
            Description = actor.Description;
            _actor = actor.ActorType;
            actor.CloneProperties(this);
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
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

        public void Apply(IPropertySchema schema)
        {
        }

        public IThreatModel Model { get; }

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
        protected Guid _id { get; set; }
        private List<IProperty> _properties { get; set; }
        protected Guid _modelId { get; set; }
        protected IThreatModel _model { get; set; }
        #endregion
    }
}