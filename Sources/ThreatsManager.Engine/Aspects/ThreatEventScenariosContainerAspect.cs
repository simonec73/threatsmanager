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
    //private IThreatEventScenariosContainer ScenariosContainer => this;
    //private IThreatEvent MySelf => this;
    //#endregion    

    [PSerializable]
    [AspectTypeDependency(AspectDependencyAction.Require, AspectDependencyPosition.Any, typeof(ThreatModelChildAspect))]
    public class ThreatEventScenariosContainerAspect : InstanceLevelAspect
    {
        #region Imports from the extended class.
        [ImportMember("Model", IsRequired=true)]
        public Property<IThreatModel> Model;

        [ImportMember("IsInitialized", IsRequired=true)]
        public Property<bool> IsInitialized;

        [ImportMember("ScenariosContainer", IsRequired=true)]
        public Property<IThreatEventScenariosContainer> ScenariosContainer;

        [ImportMember("MySelf", IsRequired=true)]
        public Property<IThreatEvent> MySelf;
        #endregion

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
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
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
        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 6)]
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

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 3)]
        public IThreatEventScenario GetScenario(Guid id)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;

            return _scenarios?.FirstOrDefault(x => x.Id == id);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 14)]
        public IThreatEventScenario AddScenario(IThreatActor threatActor, ISeverity severity,
            string name = null)
        {
            if (!(IsInitialized?.Get() ?? false))
                return null;
            if (threatActor == null)
                throw new ArgumentNullException(nameof(threatActor));
            if (severity == null)
                throw new ArgumentNullException(nameof(severity));

            IThreatEventScenario result = new ThreatEventScenario(MySelf?.Get(), threatActor, name)
            {
                Severity = severity
            };

            if (_scenarios == null)
                _scenarios = new List<IThreatEventScenario>();

            _scenarios.Add(result);
            Dirty.IsDirty = true;
            _threatEventScenarioAdded?.Invoke(ScenariosContainer?.Get(), result);

            return result;
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public void Add(IThreatEventScenario scenario)
        {
            if (!(IsInitialized?.Get() ?? false))
                return;
            if (scenario == null)
                throw new ArgumentNullException(nameof(scenario));
            if (scenario.ThreatEvent is IThreatModelChild child &&
                child.Model != Model?.Get())
                throw new ArgumentException();

            if (_scenarios == null)
                _scenarios = new List<IThreatEventScenario>();

            _scenarios.Add(scenario);
        }

        [IntroduceMember(OverrideAction = MemberOverrideAction.OverrideOrFail, LinesOfCodeAvoided = 9)]
        public bool RemoveScenario(Guid id)
        {
            if (!(IsInitialized?.Get() ?? false))
                return false;

            bool result = false;

            var scenario = GetScenario(id);
            if (scenario != null)
            {
                result = _scenarios.Remove(scenario);
                if (result)
                {
                    Dirty.IsDirty = true;
                    _threatEventScenarioRemoved?.Invoke(ScenariosContainer?.Get(), scenario);
                }
            }

            return result;
        }
        #endregion
    }
}