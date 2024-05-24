using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Engine.ObjectModel.ThreatsMitigations
{
#pragma warning disable CS0067
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    [SimpleNotifyPropertyChanged]
    [IntroduceNotifyPropertyChanged]
    [IdentityAspect]
    [ThreatModelChildAspect]
    [ThreatModelIdChanger]
    [PropertiesContainerAspect]
    [SourceInfoAspect]
    [Recordable(AutoRecord = false)]
    [Undoable]
    public class Mitigation : IMitigation, IInitializableObject, IForceSetId
    {
        public Mitigation()
        {

        }

        public Mitigation([Required] string name)
        {
            _id = Guid.NewGuid();
            Name = name;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

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
        public Scope PropertiesScope => Scope.Mitigation;

        public void SetId(Guid id)
        {
            _id = id;
        }

        [JsonProperty("controlType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SecurityControlType ControlType { get; set; }

        public IMitigation Clone([NotNull] IMitigationsContainer container)
        {
            Mitigation result = null;

            if (container is IThreatModel model)
            {
                using (var scope = UndoRedoManager.OpenScope("Clone Mitigation"))
                {
                    result = new Mitigation
                    {
                        _id = Id,
                        Name = Name,
                        Description = Description,
                        _model = model,
                        _modelId = model.Id,
                        ControlType = ControlType
                    };
                    container.Add(result);
                    this.CloneProperties(result);

                    if (model.Id != _modelId)
                        result.SetSourceInfo(Model);

                    scope?.Complete();
                }
            }

            return result;
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? ThreatModelManager.Undefined : Name;
        }

        #region Specialized Mitigations.
        [JsonProperty("specialized")]
        [Child]
        private AdvisableCollection<SpecializedMitigation> _specialized { get; set; }

        public IEnumerable<ISpecializedMitigation> Specialized => _specialized.AsEnumerable();

        public ISpecializedMitigation AddSpecializedMitigation([NotNull] IItemTemplate template, string name, string description)
        {
            SpecializedMitigation result = null;

            if (!(_specialized?.Any(x => x.TargetId == template.Id) ?? false))
            {
                using (var scope = UndoRedoManager.OpenScope("Add Specialized Mitigation"))
                {
                    if (_specialized == null)
                        _specialized = new AdvisableCollection<SpecializedMitigation>();

                    result = new SpecializedMitigation(template, name, description);
                    _specialized.Add(result);
                    scope?.Complete();
                }
            }

            return result;
        }

        public bool RemoveSpecializedMitigation(IItemTemplate template)
        {
            return RemoveSpecializedMitigation(template.Id);
        }

        public bool RemoveSpecializedMitigation(Guid templateId)
        {
            bool result = false;

            var item = _specialized?.FirstOrDefault(x => x.TargetId == templateId);
            if (item != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Specialized Mitigation"))
                {
                    result = _specialized.Remove(item);
                    scope?.Complete();
                }
            }

            return result;
        }

        public ISpecializedMitigation GetSpecializedMitigation(IItemTemplate template)
        {
            return GetSpecializedMitigation(template?.Id ?? Guid.Empty);
        }

        public ISpecializedMitigation GetSpecializedMitigation(Guid templateId)
        {
            return _specialized?.FirstOrDefault(x => x.TargetId == templateId);
        }

        public string GetName(IIdentity identity)
        {
            string result = _name ?? ThreatModelManager.Unknown;

            IItemTemplate template = null;
            if (identity is IEntity entity)
            {
                template = entity.Template;
            }
            else if (identity is IDataFlow flow)
            {
                template = flow.Template;
            }

            var specialized = GetSpecializedMitigation(template);
            if (specialized != null && !string.IsNullOrWhiteSpace(specialized.Name))
            {
                result = specialized.Name;
            }

            return result;
        }

        public string GetName(Guid identityId)
        {
            string result = null;

            IIdentity identity = Model?.GetEntity(identityId);
            if (identity == null)
                identity = Model?.GetDataFlow(identityId);
            if (identity != null)
                result = GetName(identity);

            return result;
        }

        public string GetDescription(IIdentity identity)
        {
            string result = _description ?? ThreatModelManager.Undefined;

            IItemTemplate template = null;
            if (identity is IEntity entity)
            {
                template = entity.Template;
            }
            else if (identity is IDataFlow flow)
            {
                template = flow.Template;
            }

            var specialized = GetSpecializedMitigation(template);
            if (specialized != null && !string.IsNullOrWhiteSpace(specialized.Description))
            {
                result = specialized.Description;
            }

            return result;
        }

        public string GetDescription(Guid identityId)
        {
            string result = null;

            IIdentity identity = Model?.GetEntity(identityId);
            if (identity == null)
                identity = Model?.GetDataFlow(identityId);
            if (identity != null)
                result = GetDescription(identity);

            return result;
        }
        #endregion
        #endregion
    }
}
