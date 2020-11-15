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
    //private List<IThreatEvent> _threatEvents { get; set; }
    //#endregion    

    [PSerializable]
    public class ThreatEventsContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("threatEvents")]
        public List<IThreatEvent> _threatEvents { get; set; }
        #endregion

        #region Implementation of interface IThreatEventsContainer.
        private Action<IThreatEventsContainer, IThreatEvent> _threatEventAdded;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventAdded
        {
            add
            {
                if (_threatEventAdded == null || !_threatEventAdded.GetInvocationList().Contains(value))
                {
                    _threatEventAdded += value;
                }
            }
            remove
            {
                _threatEventAdded -= value;
            }
        }

        private Action<IThreatEventsContainer, IThreatEvent> _threatEventRemoved;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public event Action<IThreatEventsContainer, IThreatEvent> ThreatEventRemoved
        {
            add
            {
                if (_threatEventRemoved == null || !_threatEventRemoved.GetInvocationList().Contains(value))
                {
                    _threatEventRemoved += value;
                }
            }
            remove
            {
                _threatEventRemoved -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonIgnoreAttribute), OverrideAction = CustomAttributeOverrideAction.Ignore)]
        [JsonIgnore]
        public IEnumerable<IThreatEvent> ThreatEvents => _threatEvents?.AsReadOnly();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatEvent GetThreatEvent(Guid id)
        {
            return _threatEvents?.FirstOrDefault(x => x.Id == id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatEvent GetThreatEventByThreatType(Guid threatTypeId)
        {
            return _threatEvents?.FirstOrDefault(x => x.ThreatTypeId == threatTypeId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public void Add(IThreatEvent threatEvent)
        {
            if (threatEvent == null)
                throw new ArgumentNullException(nameof(threatEvent));
            if (threatEvent is IThreatModelChild child && child.Model != (Instance as IThreatModelChild)?.Model)
                throw new ArgumentException();

            if (_threatEvents == null)
                _threatEvents = new List<IThreatEvent>();

            _threatEvents.Add(threatEvent);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 14)]
        public IThreatEvent AddThreatEvent(IThreatType threatType)
        {
            IThreatEvent result = null;

            if (threatType != null && Instance is IIdentity identity)
            {
                IThreatModel model = (Instance as IThreatModel) ?? (Instance as IThreatModelChild)?.Model;

                if (model != null)
                {
                    if (_threatEvents?.All(x => x.ThreatTypeId != threatType.Id) ?? true)
                    {
                        result = new ThreatEvent(model, threatType, identity);
                        if (_threatEvents == null)
                            _threatEvents = new List<IThreatEvent>();
                        _threatEvents.Add(result);
                        if (Instance is IDirty dirtyObject)
                            dirtyObject.SetDirty();
                        if (Instance is IThreatEventsContainer container)
                            _threatEventAdded?.Invoke(container, result);
                    }
                }
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveThreatEvent(Guid id)
        {
            bool result = false;

            var threatEvent = GetThreatEvent(id);
            if (threatEvent != null)
            {
                result = _threatEvents.Remove(threatEvent);
                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                    if (Instance is IThreatEventsContainer container)
                        _threatEventRemoved?.Invoke(container, threatEvent);
                }
            }

            return result;
        }
        #endregion
    }
}
