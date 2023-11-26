using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;
using ThreatsManager.Engine.Aspects;
using PostSharp.Patterns.Collections;
using Newtonsoft.Json.Converters;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [IdentityAspect]
    [PropertiesContainerAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [Recordable(AutoRecord = false)]
    [Undoable]
    [TypeLabel("Threat Actor")]
    public class ThreatActor : IThreatActor, IInitializableObject
    {
        public ThreatActor()
        {

        }

        public ThreatActor(DefaultActor actor)
        {
            _id = Guid.NewGuid();
            _actor = actor;
            Name = actor.GetEnumLabel();
            Description = actor.GetEnumDescription();
        }

        public ThreatActor([Required] string name) : this()
        {
            _id = Guid.NewGuid();
            _actor = DefaultActor.Unknown;
            Name = name;
        }

        public bool IsInitialized => _id != Guid.Empty && Model != null;

        #region Default implementation.
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
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

        public void Unapply(IPropertySchema schema)
        {
        }

        [Reference]
        [field: NotRecorded]
        public IThreatModel Model { get; }
        #endregion

        #region Additional placeholders required.
        [JsonProperty("id")]
        protected Guid _id { get; set; }
        [JsonProperty("name")]
        protected string _name { get; set; }
        [JsonProperty("description")]
        protected string _description { get; set; }
        [Child]
        [JsonProperty("properties", ItemTypeNameHandling = TypeNameHandling.Objects)]
        private AdvisableCollection<IProperty> _properties { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Parent]
        [field: NotRecorded]
        [field: UpdateThreatModelId]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.ThreatActor;

        [JsonProperty("actor")]
        [JsonConverter(typeof(StringEnumConverter))]
        private DefaultActor _actor { get; set; }

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
                container.Add(result);
                this.CloneProperties(result);
            }

            return result;
        }

        public void Apply([NotNull] IThreatActor actor)
        {
            using (var scope = UndoRedoManager.OpenScope("Apply a different Actor Type"))
            {
                _id = actor.Id;
                Description = actor.Description;
                _actor = actor.ActorType;
                actor.CloneProperties(this);
                scope?.Complete();
            }
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion
    }
}