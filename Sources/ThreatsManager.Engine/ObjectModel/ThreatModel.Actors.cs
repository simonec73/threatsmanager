using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [JsonProperty("actors")]
        private List<IThreatActor> _actors;

        public IEnumerable<IThreatActor> ThreatActors => _actors?.AsReadOnly();

        [InitializationRequired]
        public IThreatActor GetThreatActor(Guid id)
        {
            return _actors?.FirstOrDefault(x => id == x.Id);
        }

        [InitializationRequired]
        public IThreatActor GetThreatActor(DefaultActor actor)
        {
            return actor != DefaultActor.Unknown ? _actors?.FirstOrDefault(x => x.ActorType == actor) : null;
        }

        [InitializationRequired]
        public void Add([NotNull] IThreatActor actor)
        {
            if (_actors == null)
                _actors = new List<IThreatActor>();

            _actors.Add(actor);

            SetDirty();
            ChildCreated?.Invoke(actor);
        }

        [InitializationRequired]
        public IThreatActor AddThreatActor(DefaultActor actor)
        {
            IThreatActor result = null;

            if (actor != DefaultActor.Unknown)
            {
                var threatActor = GetThreatActor(actor);
                if (threatActor == null)
                {
                    result = new ThreatActor(this, actor);
                    Add(result);
                    RegisterEvents(result);
                }
            }

            return result;
        }

        [InitializationRequired]
        public IThreatActor AddThreatActor([Required] string name, string description)
        {
            IThreatActor result = new ThreatActor(this, name)
            {
                Description = description
            };
            Add(result);
            RegisterEvents(result);

            return result;
        }

        [InitializationRequired(false)]
        public bool RemoveThreatActor(Guid id, bool force = false)
        {
            bool result = false;

            var actor = GetThreatActor(id);
            if (actor != null && (force || !IsUsed(actor)))
            {
                RemoveRelated(actor);

                result = _actors.Remove(actor);
                if (result)
                {
                    UnregisterEvents(actor);
                    SetDirty();
                    ChildRemoved?.Invoke(actor);
                }
            }

            return result;
        }

        private bool IsUsed([NotNull] IThreatActor actor)
        {
            return (_entities?.Any(x => x.ThreatEvents?.Any(y => y.Scenarios?.Any(z => z.Actor == actor) ?? false) ?? false) ?? false) ||
                   (_dataFlows?.Any(x => x.ThreatEvents?.Any(y => y.Scenarios?.Any(z => z.Actor == actor) ?? false) ?? false) ?? false);
        }

        private void RemoveRelated([NotNull] IThreatActor actor)
        {
            RemoveRelatedForEntities(actor);
            RemoveRelatedForDataFlows(actor);
        }

        private void RemoveRelatedForEntities([NotNull] IThreatActor actor)
        {
            var entities = _entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    var events = entity.ThreatEvents?.ToArray();
                    if (events?.Any() ?? false)
                    {
                        foreach (var threatEvent in events)
                        {
                            var scenarios = threatEvent.Scenarios?.Where(x => x.ActorId == actor.Id).ToArray();
                            if (scenarios?.Any() ?? false)
                            {
                                foreach (var scenario in scenarios)
                                    threatEvent.RemoveScenario(scenario.Id);
                            }
                        }
                    }
                }
            }
        }

        private void RemoveRelatedForDataFlows([NotNull] IThreatActor actor)
        {
            var dataFlows = _dataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    var events = dataFlow.ThreatEvents?.ToArray();
                    if (events?.Any() ?? false)
                    {
                        foreach (var threatEvent in events)
                        {
                            var scenarios = threatEvent.Scenarios?.Where(x => x.ActorId == actor.Id).ToArray();
                            if (scenarios?.Any() ?? false)
                            {
                                foreach (var scenario in scenarios)
                                    threatEvent.RemoveScenario(scenario.Id);
                            }
                        }
                    }
                }
            }
        }
    }
}