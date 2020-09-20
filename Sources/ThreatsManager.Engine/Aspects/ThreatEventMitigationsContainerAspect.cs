using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //private List<IThreatEventMitigation> _mitigations { get; set; }
    //#endregion    

    [PSerializable]
    public class ThreatEventMitigationsContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("mitigations")]
        public List<IThreatEventMitigation> _mitigations { get; set; }
        #endregion

        #region Implementation of interface IThreatEventMitigationsContainer.
        private Action<IThreatEventMitigationsContainer, IThreatEventMitigation> _threatEventMitigationAdded;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public event Action<IThreatEventMitigationsContainer, IThreatEventMitigation> ThreatEventMitigationAdded
        {
            add
            {
                if (_threatEventMitigationAdded == null || !_threatEventMitigationAdded.GetInvocationList().Contains(value))
                {
                    _threatEventMitigationAdded += value;
                }
            }
            remove
            {
                _threatEventMitigationAdded -= value;
            }
        }

        private Action<IThreatEventMitigationsContainer, IThreatEventMitigation> _threatEventMitigationRemoved;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public event Action<IThreatEventMitigationsContainer, IThreatEventMitigation> ThreatEventMitigationRemoved
        {
            add
            {
                if (_threatEventMitigationRemoved == null || !_threatEventMitigationRemoved.GetInvocationList().Contains(value))
                {
                    _threatEventMitigationRemoved += value;
                }
            }
            remove
            {
                _threatEventMitigationRemoved -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IThreatEventMitigation> Mitigations => _mitigations?.AsReadOnly();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatEventMitigation GetMitigation(Guid mitigationId)
        {
            return _mitigations?.FirstOrDefault(x => x.MitigationId == mitigationId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public void Add(IThreatEventMitigation mitigation)
        {
            if (mitigation == null)
                throw new ArgumentNullException(nameof(mitigation));
            if (mitigation.ThreatEvent != Instance || 
                (mitigation.Mitigation is IThreatModelChild child && child.Model != (Instance as IThreatModelChild)?.Model))
                throw new ArgumentException();

            if (_mitigations == null)
                _mitigations = new List<IThreatEventMitigation>();

            _mitigations.Add(mitigation);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 13)]
        public IThreatEventMitigation AddMitigation(IMitigation mitigation, IStrength strength, 
            MitigationStatus status = MitigationStatus.Proposed, string directives = null)
        {
            if (mitigation == null)
                throw new ArgumentNullException(nameof(mitigation));

            IThreatEventMitigation result = null;

            if (GetMitigation(mitigation.Id) == null && Instance is IThreatEvent threatEvent)
            {
                result = new ThreatEventMitigation(threatEvent, mitigation, strength)
                {
                    Status = status, Directives = directives
                };
                if (_mitigations == null)
                    _mitigations = new List<IThreatEventMitigation>();
                _mitigations.Add(result);
                if (Instance is IDirty dirtyObject)
                    dirtyObject.SetDirty();
                _threatEventMitigationAdded?.Invoke(threatEvent, result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveMitigation(Guid mitigationId)
        {
            bool result = false;

            var mitigation = GetMitigation(mitigationId);
            if (mitigation != null)
            {
                result = _mitigations.Remove(mitigation);
                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                    if (Instance is IThreatEventMitigationsContainer container)
                        _threatEventMitigationRemoved?.Invoke(container, mitigation);
                }
            }

            return result;
        }
        #endregion
    }
}