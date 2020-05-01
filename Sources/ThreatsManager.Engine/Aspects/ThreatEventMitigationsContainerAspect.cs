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
    //private IThreatEvent MySelf => this;
    //private List<IThreatEventMitigation> _mitigations { get; set; }
    //private IThreatEventMitigationsContainer MitigationsContainer => this;
    //#endregion    

    [PSerializable]
    public class ThreatEventMitigationsContainerAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("Model", IsRequired=true)]
        public Property<IThreatModel> Model;

        [ImportMember("IsInitialized", IsRequired=true)]
        public Property<bool> IsInitialized;

        [ImportMember("MySelf", IsRequired=true)]
        public Property<IThreatEvent> MySelf;

        [ImportMember("MitigationsContainer", IsRequired=true)]
        public Property<IThreatEventMitigationsContainer> MitigationsContainer;
        #endregion

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
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
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
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
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
            if (!(IsInitialized?.Get() ?? false))
                return null;

            return _mitigations?.FirstOrDefault(x => x.MitigationId == mitigationId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public void Add(IThreatEventMitigation mitigation)
        {
            if (!(IsInitialized?.Get() ?? false))
                return;
            if (mitigation == null)
                throw new ArgumentNullException(nameof(mitigation));
            if (mitigation.Mitigation is IThreatModelChild child &&
                child.Model != Model?.Get())
                throw new ArgumentException();

            if (_mitigations == null)
                _mitigations = new List<IThreatEventMitigation>();

            _mitigations.Add(mitigation);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 12)]
        public IThreatEventMitigation AddMitigation(IMitigation mitigation, IStrength strength, 
            MitigationStatus status = MitigationStatus.Proposed, string directives = null)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;
            if (mitigation == null)
                throw new ArgumentNullException(nameof(mitigation));

            IThreatEventMitigation result = null;

            if (GetMitigation(mitigation.Id) == null)
            {
                result = new ThreatEventMitigation(MySelf?.Get(), mitigation, strength);
                result.Status = status;
                result.Directives = directives;
                if (_mitigations == null)
                    _mitigations = new List<IThreatEventMitigation>();
                _mitigations.Add(result);
                Dirty.IsDirty = true;
                _threatEventMitigationAdded?.Invoke(MitigationsContainer?.Get(), result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public bool RemoveMitigation(Guid mitigationId)
        {
            if (!(IsInitialized?.Get() ?? false))
                return false;

            bool result = false;

            var mitigation = GetMitigation(mitigationId);
            if (mitigation != null)
            {
                result = _mitigations.Remove(mitigation);
                if (result)
                {
                    Dirty.IsDirty = true;
                    _threatEventMitigationRemoved?.Invoke(MitigationsContainer?.Get(), mitigation);
                }
            }

            return result;
        }
        #endregion
    }
}