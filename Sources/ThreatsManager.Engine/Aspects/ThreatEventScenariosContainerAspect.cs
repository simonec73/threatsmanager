using System;
using System.Collections.Generic;
using System.Linq;
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
    //[JsonProperty("scenarios")]
    //private IList<IThreatEventScenario> _scenarios { get; set; }
    //#endregion    

    [PSerializable]
    public class ThreatEventScenariosContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_scenarios))]
        public Property<IList<IThreatEventScenario>> _scenarios;
        #endregion

        #region Implementation of interface IThreatEventScenariosContainer.
        private Action<IThreatEventScenariosContainer, IThreatEventScenario> _threatEventScenarioAdded;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "ScenarioAdded", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnScenarioAddedAdd(EventInterceptionArgs args)
        {
            if (_threatEventScenarioAdded == null || !_threatEventScenarioAdded.GetInvocationList().Contains(args.Handler))
            {
                _threatEventScenarioAdded += (Action<IThreatEventScenariosContainer, IThreatEventScenario>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnScenarioAddedAdd))]
        public void OnScenarioAddedRemove(EventInterceptionArgs args)
        {
            _threatEventScenarioAdded -= (Action<IThreatEventScenariosContainer, IThreatEventScenario>)args.Handler;
            args.ProceedRemoveHandler();
        }

        private Action<IThreatEventScenariosContainer, IThreatEventScenario> _threatEventScenarioRemoved;

        [OnEventAddHandlerAdvice]
        [MulticastPointcut(MemberName = "ScenarioRemoved", Targets = PostSharp.Extensibility.MulticastTargets.Event, Attributes = PostSharp.Extensibility.MulticastAttributes.AnyVisibility)]
        public void OnScenarioRemovedAdd(EventInterceptionArgs args)
        {
            if (_threatEventScenarioRemoved == null || !_threatEventScenarioRemoved.GetInvocationList().Contains(args.Handler))
            {
                _threatEventScenarioRemoved += (Action<IThreatEventScenariosContainer, IThreatEventScenario>)args.Handler;
                args.ProceedAddHandler();
            }
        }

        [OnEventRemoveHandlerAdvice(Master = nameof(OnScenarioRemovedAdd))]
        public void OnScenarioRemovedRemove(EventInterceptionArgs args)
        {
            _threatEventScenarioRemoved -= (Action<IThreatEventScenariosContainer, IThreatEventScenario>)args.Handler;
            args.ProceedRemoveHandler();
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IThreatEventScenario> Scenarios => _scenarios?.Get()?.AsEnumerable();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatEventScenario GetScenario(Guid id)
        {
            return _scenarios?.Get()?.FirstOrDefault(x => x.Id == id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 16)]
        public IThreatEventScenario AddScenario(IThreatActor threatActor, ISeverity severity, string name = null)
        {
            if (threatActor == null)
                throw new ArgumentNullException(nameof(threatActor));
            if (severity == null)
                throw new ArgumentNullException(nameof(severity));

            IThreatEventScenario result = null;

            if (Instance is IThreatEvent threatEvent)
            {
                result = new ThreatEventScenario(threatEvent, threatActor, name)
                {
                    Severity = severity
                };

                var scenarios = _scenarios?.Get();
                if (scenarios == null)
                { 
                    scenarios = new AdvisableCollection<IThreatEventScenario>();
                    _scenarios?.Set(scenarios);
                }

                _scenarios?.Get()?.Add(result);
                if (Instance is IDirty dirtyObject)
                    dirtyObject.SetDirty();
                if (Instance is IThreatEventScenariosContainer container)
                    _threatEventScenarioAdded?.Invoke(container, result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public void Add(IThreatEventScenario scenario)
        {
            if (scenario == null)
                throw new ArgumentNullException(nameof(scenario));
            if (scenario.ThreatEvent is IThreatModelChild child && child.Model != (Instance as IThreatEvent)?.Model)
                throw new ArgumentException();

            var scenarios = _scenarios?.Get();
            if (scenarios == null)
            {
                scenarios = new AdvisableCollection<IThreatEventScenario>();
                _scenarios?.Set(scenarios);
            }

            _scenarios?.Get()?.Add(scenario);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveScenario(Guid id)
        {
            bool result = false;

            var scenario = GetScenario(id);
            if (scenario != null)
            {
                result = _scenarios?.Get()?.Remove(scenario) ?? false;
                if (result)
                {
                    if (Instance is IDirty dirtyObject)
                        dirtyObject.SetDirty();
                    if (Instance is IThreatEvent threatEvent)
                        _threatEventScenarioRemoved?.Invoke(threatEvent, scenario);
                }
            }

            return result;
        }
        #endregion
    }
}