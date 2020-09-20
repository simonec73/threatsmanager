using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Contracts;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //private List<IVulnerability> _vulnerabilities { get; set; }
    //#endregion    

    [PSerializable]
    public class VulnerabilitiesContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("vulnerabilities")]
        public List<IVulnerability> _vulnerabilities { get; set; }
        #endregion

        #region Implementation of interface IVulnerabilitiesContainer.
        private Action<IVulnerabilitiesContainer, IVulnerability> _vulnerabilityAdded;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityAdded
        {
            add
            {
                if (_vulnerabilityAdded == null || !_vulnerabilityAdded.GetInvocationList().Contains(value))
                {
                    _vulnerabilityAdded += value;
                }
            }
            remove
            {
                _vulnerabilityAdded -= value;
            }
        }

        private Action<IVulnerabilitiesContainer, IVulnerability> _vulnerabilityRemoved;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public event Action<IVulnerabilitiesContainer, IVulnerability> VulnerabilityRemoved
        {
            add
            {
                if (_vulnerabilityRemoved == null || !_vulnerabilityRemoved.GetInvocationList().Contains(value))
                {
                    _vulnerabilityRemoved += value;
                }
            }
            remove
            {
                _vulnerabilityRemoved -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonIgnoreAttribute), OverrideAction = CustomAttributeOverrideAction.Ignore)]
        [JsonIgnore]
        public IEnumerable<IVulnerability> Vulnerabilities => _vulnerabilities?.AsReadOnly();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IVulnerability GetVulnerability(Guid id)
        {
            return _vulnerabilities?.FirstOrDefault(x => x.Id == id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IVulnerability GetVulnerabilityByWeakness(Guid weaknessId)
        {
            return _vulnerabilities?.FirstOrDefault(x => x.WeaknessId == weaknessId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public void Add(IVulnerability vulnerability)
        {
            if (vulnerability == null)
                throw new ArgumentNullException(nameof(vulnerability));
            if (vulnerability is IThreatModelChild child && child.Model != (Instance as IThreatModelChild)?.Model)
                throw new ArgumentException();

            if (_vulnerabilities == null)
                _vulnerabilities = new List<IVulnerability>();

            _vulnerabilities.Add(vulnerability);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 14)]
        public IVulnerability AddVulnerability(IWeakness weakness)
        {
            IVulnerability result = null;

            if (Instance is IIdentity identity)
            {
                IThreatModel model = (Instance as IThreatModel) ?? (Instance as IThreatModelChild)?.Model;

                if (model != null)
                {
                    if (_vulnerabilities?.All(x => x.WeaknessId != weakness.Id) ?? true)
                    {
                        result = new Vulnerability(model, weakness, identity);
                        if (_vulnerabilities == null)
                            _vulnerabilities = new List<IVulnerability>();
                        _vulnerabilities.Add(result);
                        if (Instance is IDirty dirtyObject)
                            dirtyObject.SetDirty();
                        if (Instance is IVulnerabilitiesContainer container)
                            _vulnerabilityAdded?.Invoke(container, result);
                    }
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveVulnerability(Guid id)
        {
            bool result = false;

            var vulnerability = GetVulnerability(id);
            if (vulnerability != null)
            {
                result = _vulnerabilities.Remove(vulnerability);
                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                    if (Instance is IVulnerabilitiesContainer container)
                        _vulnerabilityRemoved?.Invoke(container, vulnerability);
                }
            }

            return result;
        }
        #endregion
    }
}
