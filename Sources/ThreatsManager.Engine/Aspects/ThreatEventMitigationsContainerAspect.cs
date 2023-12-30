using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Serialization;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects.Engine;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[Child]
    //[JsonProperty("mitigations")]
    //private AdvisableCollection<ThreatEventMitigation> _mitigations { get; set; }
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(SimpleNotifyPropertyChangedAttribute))]
    public class ThreatEventMitigationsContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_mitigations))]
        public Property<AdvisableCollection<ThreatEventMitigation>> _mitigations;
        #endregion

        #region Implementation of interface IThreatEventMitigationsContainer.
        private Action<IThreatEventMitigationsContainer, IThreatEventMitigation> _threatEventMitigationAdded;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "ThreatEventMitigationAdded", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnThreatEventMitigationAddedAdd(EventInterceptionArgs args)
        {
            if (_threatEventMitigationAdded == null || !_threatEventMitigationAdded.GetInvocationList().Contains(args.Handler))
            {
                _threatEventMitigationAdded += (Action<IThreatEventMitigationsContainer, IThreatEventMitigation>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnThreatEventMitigationAddedAdd))]
        public void OnThreatEventMitigationAddedRemove(EventInterceptionArgs args)
        {
            _threatEventMitigationAdded -= (Action<IThreatEventMitigationsContainer, IThreatEventMitigation>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IThreatEventMitigationsContainer, IThreatEventMitigation> _threatEventMitigationRemoved;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "ThreatEventMitigationRemoved", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnThreatEventMitigationRemovedAdd(EventInterceptionArgs args)
        {
            if (_threatEventMitigationRemoved == null || !_threatEventMitigationRemoved.GetInvocationList().Contains(args.Handler))
            {
                _threatEventMitigationRemoved += (Action<IThreatEventMitigationsContainer, IThreatEventMitigation>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnThreatEventMitigationRemovedAdd))]
        public void OnThreatEventMitigationRemovedRemove(EventInterceptionArgs args)
        {
            _threatEventMitigationRemoved -= (Action<IThreatEventMitigationsContainer, IThreatEventMitigation>)args.Handler;
            args.ProceedRemoveHandler();
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IThreatEventMitigation> Mitigations => _mitigations?.Get()?.AsEnumerable();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatEventMitigation GetMitigation(Guid mitigationId)
        {
            return _mitigations?.Get()?.FirstOrDefault(x => x.MitigationId == mitigationId);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 11)]
        public void Add(IThreatEventMitigation mitigation)
        {
            if (mitigation is ThreatEventMitigation tem)
            {
                using (var scope = UndoRedoManager.OpenScope("Add a Mitigation to a Threat Event"))
                {
                    var mitigations = _mitigations?.Get();
                    if (mitigations == null)
                    {
                        mitigations = new AdvisableCollection<ThreatEventMitigation>();
                        _mitigations?.Set(mitigations);
                    }

                    UndoRedoManager.Attach(tem, tem.Model);
                    mitigations.Add(tem);
                    scope?.Complete();
                }

                if (Instance is IThreatEventMitigationsContainer container)
                {
                    _threatEventMitigationAdded?.Invoke(container, tem);
                }
            }
            else
                throw new ArgumentNullException(nameof(mitigation));
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
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
                    Status = status,
                    Directives = directives
                };
                Add(result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveMitigation(Guid mitigationId)
        {
            bool result = false;

            var mitigation = GetMitigation(mitigationId) as ThreatEventMitigation;
            if (mitigation != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove a Mitigation from a Threat Event"))
                {
                    result = _mitigations?.Get()?.Remove(mitigation) ?? false;
                    if (result)
                    {
                        UndoRedoManager.Detach(mitigation);
                        scope?.Complete();
                    }
                }

                if (result && Instance is IThreatEventMitigationsContainer container)
                    _threatEventMitigationRemoved?.Invoke(container, mitigation);
            }

            return result;
        }
        #endregion
    }
}