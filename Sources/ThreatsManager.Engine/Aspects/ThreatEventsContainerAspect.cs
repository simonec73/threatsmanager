using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Patterns.Collections;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[Child]
    //[JsonProperty("threatEvents")]
    //private IList<IThreatEvent> _threatEvents { get; set; }
    //#endregion    

    [PSerializable]
    public class ThreatEventsContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_threatEvents))]
        public Property<IList<IThreatEvent>> _threatEvents;
        #endregion

        #region Implementation of interface IThreatEventsContainer.
        private Action<IThreatEventsContainer, IThreatEvent> _threatEventAdded;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "ThreatEventAdded", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnThreatEventAddedAdd(EventInterceptionArgs args)
        {
            if (_threatEventAdded == null || !_threatEventAdded.GetInvocationList().Contains(args.Handler))
            {
                _threatEventAdded += (Action<IThreatEventsContainer, IThreatEvent>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnThreatEventAddedAdd))]
        public void OnThreatEventAddedRemove(EventInterceptionArgs args)
        {
            _threatEventAdded -= (Action<IThreatEventsContainer, IThreatEvent>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IThreatEventsContainer, IThreatEvent> _threatEventRemoved;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "ThreatEventRemoved", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnThreatEventRemovedAdd(EventInterceptionArgs args)
        {
            if (_threatEventRemoved == null || !_threatEventRemoved.GetInvocationList().Contains(args.Handler))
            {
                _threatEventRemoved += (Action<IThreatEventsContainer, IThreatEvent>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnThreatEventRemovedAdd))]
        public void OnThreatEventRemovedRemove(EventInterceptionArgs args)
        {
            _threatEventRemoved -= (Action<IThreatEventsContainer, IThreatEvent>)args.Handler;
            args.ProceedRemoveHandler();
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        [CopyCustomAttributes(typeof(JsonIgnoreAttribute), OverrideAction = CustomAttributeOverrideAction.Ignore)]
        [JsonIgnore]
        public IEnumerable<IThreatEvent> ThreatEvents => _threatEvents?.Get()?.AsEnumerable();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatEvent GetThreatEvent(Guid id)
        {
            return _threatEvents?.Get()?.FirstOrDefault(x => x.Id == id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatEvent GetThreatEventByThreatType(Guid threatTypeId)
        {
            return _threatEvents?.Get()?.FirstOrDefault(x => x.ThreatTypeId == threatTypeId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public void Add(IThreatEvent threatEvent)
        {
            if (threatEvent == null)
                throw new ArgumentNullException(nameof(threatEvent));
            if (threatEvent is IThreatModelChild child && child.Model != (Instance as IThreatModelChild)?.Model)
                throw new ArgumentException();

            var threatEvents = _threatEvents?.Get();
            if (threatEvents == null)
            {
                threatEvents = new AdvisableCollection<IThreatEvent>();
                _threatEvents?.Set(threatEvents);
            }

            threatEvents.Add(threatEvent);
            if (Instance is IThreatEventsContainer container)
                _threatEventAdded?.Invoke(container, threatEvent);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public IThreatEvent AddThreatEvent(IThreatType threatType)
        {
            IThreatEvent result = null;

            if (Instance is IIdentity identity)
            {
                IThreatModel model = (Instance as IThreatModel) ?? (Instance as IThreatModelChild)?.Model;

                if (model != null)
                {
                    if (_threatEvents?.Get()?.All(x => x.ThreatTypeId != threatType.Id) ?? true)
                    {
                        result = new ThreatEvent(threatType);
                        Add(result);
                        if (Instance is IDirty dirtyObject)
                            dirtyObject.SetDirty();
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
                result = _threatEvents?.Get()?.Remove(threatEvent) ?? false;
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