using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        private static int _lastMitigation;

        [Child]
        [JsonProperty("mitigations", Order = 42)]
        private AdvisableCollection<Mitigation> _mitigations { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<IMitigation> Mitigations => _mitigations?.AsEnumerable();

        [InitializationRequired]
        public IMitigation GetMitigation(Guid id)
        {
            return _mitigations?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public IEnumerable<IMitigation> GetMitigations([Required] string name)
        {
            return _mitigations?.Where(x => string.CompareOrdinal(x.Name, name) == 0);
        }

        [InitializationRequired]
        public void Add([NotNull] IMitigation mitigation)
        {
            if (mitigation is Mitigation m)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Mitigation"))
                {
                    if (_mitigations == null)
                        _mitigations = new AdvisableCollection<Mitigation>();

                    UndoRedoManager.Attach(m, this);
                    _mitigations.Add(m);
                    scope?.Complete();
                }

                ChildCreated?.Invoke(m);
            }
            else
                throw new ArgumentException(nameof(mitigation));
        }

        [InitializationRequired]
        public IMitigation AddMitigation(string name = null)
        {
            if (name == null)
            {
                name = GetFirstAvailableMitigationName();
            }

            IMitigation result = new Mitigation(name);
            Add(result);
            RegisterEvents(result);

            return result;
        }

        [InitializationRequired]
        public bool RemoveMitigation(Guid id, bool force = false)
        {
            bool result = false;

            var mitigation = _mitigations?.FirstOrDefault(x => x.Id == id);
            if (mitigation != null && (force || !IsUsed(mitigation)))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Mitigation"))
                {
                    RemoveRelated(mitigation);

                    result = _mitigations.Remove(mitigation);
                    if (result)
                    {
                        UndoRedoManager.Detach(mitigation);
                        UnregisterEvents(mitigation);
                    }

                    scope?.Complete();
                }

                if (result)
                    ChildRemoved?.Invoke(mitigation);
            }

            return result;
        }

        private string GetFirstAvailableMitigationName()
        {
            _lastMitigation += 1;

            string result = $"{GetIdentityTypeName(typeof(Mitigation))} {_lastMitigation}";

            var mitigations = GetMitigations(result);
            if (mitigations?.Any() ?? false)
            {
                result = GetFirstAvailableMitigationName();
            }

            return result;
        }

        private bool IsUsed([NotNull] IMitigation mitigation)
        {
            return (_entities?.Any(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.MitigationId == mitigation.Id) ?? false) ?? false) ?? false) ||
                   (_flows?.Any(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.MitigationId == mitigation.Id) ?? false) ?? false) ?? false);
        }

        private void RemoveRelated([NotNull] IMitigation mitigation)
        {
            RemoveRelatedForEntities(mitigation);
            RemoveRelatedForDataFlows(mitigation);
            RemoveRelatedForThreatTypes(mitigation);
        }

        private void RemoveRelatedForEntities([NotNull] IMitigation mitigation)
        {
            var entities = _entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    RemoveMitigation(entity, mitigation);
                }
            }
        }

        private void RemoveRelatedForDataFlows([NotNull] IMitigation mitigation)
        {
            var dataFlows = _flows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    RemoveMitigation(dataFlow, mitigation);
                }
            }
        }
        
        private void RemoveRelatedForThreatTypes([NotNull] IMitigation mitigation)
        {
            var tTypes = this.ThreatTypes?.ToArray();
            if (tTypes?.Any() ?? false)
            {
                foreach (var tType in tTypes)
                {
                    tType.RemoveMitigation(mitigation.Id);

                    var weaknesses = tType.Weaknesses?.ToArray();
                    if (weaknesses?.Any() ?? false)
                    {
                        foreach (var weakness in weaknesses)
                        {
                            weakness.Weakness.RemoveMitigation(mitigation.Id);
                        }
                    }
                }
            }
        }

        private void RemoveMitigation([NotNull] IThreatEventsContainer container, [NotNull] IMitigation mitigation)
        {
            var events = container.ThreatEvents?.ToArray();
            if (events?.Any() ?? false)
            {
                foreach (var threatEvent in events)
                {
                    threatEvent.RemoveMitigation(mitigation.Id);

                    var vulns = threatEvent.Vulnerabilities?.ToArray();
                    if (vulns?.Any() ?? false)
                    {
                        foreach (var vuln in vulns)
                        {
                            vuln.RemoveMitigation(mitigation.Id);
                        }
                    }
                }
            }
        }
    }
}
