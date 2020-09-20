using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
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
    //private List<IThreatEventScenario> _scenarios { get; set; }
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(ThreatModelChildAspect))]
    public class ThreatEventScenariosContainerAspect : InstanceLevelAspect
    {
        #region Extra elements to be added.
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, 
            LinesOfCodeAvoided = 1, Visibility = Visibility.Private)]
        [CopyCustomAttributes(typeof(JsonPropertyAttribute), 
            OverrideAction = CustomAttributeOverrideAction.MergeReplaceProperty)]
        [JsonProperty("scenarios")]
        public List<IThreatEventScenario> _scenarios { get; set; }
        #endregion

        #region Implementation of interface IThreatEventScenariosContainer.
        private Action<IThreatEventScenariosContainer, IThreatEventScenario> _threatEventScenarioAdded;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public event Action<IThreatEventScenariosContainer, IThreatEventScenario> ScenarioAdded
        {
            add
            {
                if (_threatEventScenarioAdded == null || !_threatEventScenarioAdded.GetInvocationList().Contains(value))
                {
                    _threatEventScenarioAdded += value;
                }
            }
            remove
            {
                _threatEventScenarioAdded -= value;
            }
        }

        private Action<IThreatEventScenariosContainer, IThreatEventScenario> _threatEventScenarioRemoved;
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public event Action<IThreatEventScenariosContainer, IThreatEventScenario> ScenarioRemoved
        {
            add
            {
                if (_threatEventScenarioRemoved == null || !_threatEventScenarioRemoved.GetInvocationList().Contains(value))
                {
                    _threatEventScenarioRemoved += value;
                }
            }
            remove
            {
                _threatEventScenarioRemoved -= value;
            }
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IEnumerable<IThreatEventScenario> Scenarios => _scenarios?.AsReadOnly();

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 1)]
        public IThreatEventScenario GetScenario(Guid id)
        {
            return _scenarios?.FirstOrDefault(x => x.Id == id);
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

                if (_scenarios == null)
                    _scenarios = new List<IThreatEventScenario>();

                _scenarios.Add(result);
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

            if (_scenarios == null)
                _scenarios = new List<IThreatEventScenario>();

            _scenarios.Add(scenario);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 10)]
        public bool RemoveScenario(Guid id)
        {
            bool result = false;

            var scenario = GetScenario(id);
            if (scenario != null)
            {
                result = _scenarios.Remove(scenario);
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