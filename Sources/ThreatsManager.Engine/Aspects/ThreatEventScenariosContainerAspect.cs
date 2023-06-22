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
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Engine.Aspects
{
    //#region Additional placeholders required.
    //[Child]
    //[JsonProperty("scenarios")]
    //private AdvisableCollection<ThreatEventScenario> _scenarios { get; set; }
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, typeof(NotifyPropertyChangedAttribute))]
    public class ThreatEventScenariosContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [ImportMember(nameof(_scenarios))]
        public Property<AdvisableCollection<ThreatEventScenario>> _scenarios;
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

                Add(result);
            }

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 7)]
        public void Add(IThreatEventScenario scenario)
        {
            if (scenario is ThreatEventScenario tes)
            {
                if (tes.ThreatEvent is IThreatModelChild child && child.Model != (Instance as IThreatEvent)?.Model)
                    throw new ArgumentException();

                using (var scope = UndoRedoManager.OpenScope("Add scenario to Threat Event"))
                {
                    var scenarios = _scenarios?.Get();
                    if (scenarios == null)
                    {
                        scenarios = new AdvisableCollection<ThreatEventScenario>();
                        _scenarios?.Set(scenarios);
                    }

                    _scenarios?.Get()?.Add(tes);
                    UndoRedoManager.Attach(tes);
                    scope?.Complete();
                }
            }
            else
                throw new ArgumentNullException(nameof(scenario));
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveScenario(Guid id)
        {
            bool result = false;

            var scenario = GetScenario(id) as ThreatEventScenario;
            if (scenario != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove scenario from Threat Event"))
                {
                    result = _scenarios?.Get()?.Remove(scenario) ?? false;
                    if (result)
                    {
                        UndoRedoManager.Detach(scenario);
                        scope?.Complete();

                        if (Instance is IThreatEvent threatEvent)
                            _threatEventScenarioRemoved?.Invoke(threatEvent, scenario);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}