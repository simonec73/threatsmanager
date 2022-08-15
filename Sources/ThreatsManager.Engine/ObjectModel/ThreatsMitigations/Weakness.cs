﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
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
    [NotifyPropertyChanged]
    [IdentityAspect]
    [PropertiesContainerAspect]
    [ThreatModelChildAspect]
    [Recordable(AutoRecord = false)]
    [TypeLabel("Weakness")]
    public partial class Weakness : IWeakness, IInitializableObject
    {
        public Weakness()
        {
        }
        
        public Weakness([Required] string name, [NotNull] ISeverity severity) : this()
        {
            _id = Guid.NewGuid();
            Name = name;
            _severity = severity;
        }

        public bool IsInitialized => Model != null && _id != Guid.Empty;

        #region Default implementations.
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
        [JsonProperty("properties")]
        private IList<IProperty> _properties { get; set; }
        [JsonProperty("modelId")]
        protected Guid _modelId { get; set; }
        [Parent]
        [field: NotRecorded]
        [field: UpdateId("Id", "_modelId")]
        [field: AutoApplySchemas]
        protected IThreatModel _model { get; set; }
        #endregion

        #region Specific implementation.
        public Scope PropertiesScope => Scope.Weakness;

        [JsonProperty("severity")]
        [NotRecorded]
        private int _severityId;

        [Reference]
        [NotRecorded]
        [UpdateId("Id", "_severityId")]
        private ISeverity _severity;

        public int SeverityId => _severityId;

        [InitializationRequired]
        [SafeForDependencyAnalysis]
        public ISeverity Severity
        {
            get => _severity ?? (_severity = Model?.GetSeverity(_severityId));

            set
            {
                if (value != null && value.Equals(Model?.GetSeverity(value.Id)))
                {
                    _severity = value;
                }
            }
        }
        
        public MitigationLevel GetMitigationLevel()
        {
            MitigationLevel result = MitigationLevel.NotMitigated;

            var mitigations = Mitigations?.ToArray();
            var level = 0;

            if (mitigations?.Any() ?? false)
            {
                foreach (var m in mitigations)
                {
                    level += m.StrengthId;
                }
            }

            if (level >= 100)
                result = MitigationLevel.Complete;
            else if (level > 0)
                result = MitigationLevel.Partial;

            return result;
        }

        public IWeakness Clone([NotNull] IWeaknessesContainer container)
        {
            Weakness result = null;

            if (container is IThreatModel model)
            {
                result = new Weakness()
                {
                    _id = Id,
                    Name = Name,
                    Description = Description,
                    _model = model,
                    _severityId = _severityId
                };
                this.CloneProperties(result);

                if (_mitigations?.Any() ?? false)
                {
                    foreach (var mitigation in _mitigations)
                    {
                        var m = model.GetMitigation(mitigation.MitigationId);
                        var s = model.GetStrength(mitigation.StrengthId);
                        if (m != null && s != null)
                        {
                            var newMitigation = result.AddMitigation(m, s);
                            mitigation.CloneProperties(newMitigation);
                        }
                    }
                }

                container.Add(result);
            }

            return result;
        }

        [InitializationRequired]
        public IVulnerability GenerateVulnerability()
        {
            return new Vulnerability(this);
        }

        public override string ToString()
        {
            return Name ?? "<undefined>";
        }
        #endregion
    }
}