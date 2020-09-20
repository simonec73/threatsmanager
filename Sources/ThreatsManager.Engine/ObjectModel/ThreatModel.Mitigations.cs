using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        private static int _lastMitigation;

        [JsonProperty("mitigations")]
        private List<IMitigation> _mitigations;

        public IEnumerable<IMitigation> Mitigations => _mitigations?.AsReadOnly();

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
            if (_mitigations == null)
                _mitigations = new List<IMitigation>();

            _mitigations.Add(mitigation);

            SetDirty();
            ChildCreated?.Invoke(mitigation);
        }

        [InitializationRequired]
        public IMitigation AddMitigation(string name = null)
        {
            if (name == null)
            {
                name = GetFirstAvailableMitigationName();
            }

            IMitigation result = new Mitigation(this, name);
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
                RemoveRelated(mitigation);

                result = _mitigations.Remove(mitigation);
                if (result)
                {
                    UnregisterEvents(mitigation);
                    SetDirty();
                    ChildRemoved?.Invoke(mitigation);
                }
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
                   (_dataFlows?.Any(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.MitigationId == mitigation.Id) ?? false) ?? false) ?? false);
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
                    var events = entity.ThreatEvents?.ToArray();
                    if (events?.Any() ?? false)
                    {
                        foreach (var threatEvent in events)
                        {
                            threatEvent.RemoveMitigation(mitigation.Id);
                        }
                    }
                }
            }
        }

        private void RemoveRelatedForDataFlows([NotNull] IMitigation mitigation)
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
                            threatEvent.RemoveMitigation(mitigation.Id);
                        }
                    }
                }
            }
        }
        
        private void RemoveRelatedForThreatTypes([NotNull] IMitigation mitigation)
        {
            var threatTypes = _threatTypes?.ToArray();
            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    threatType.RemoveMitigation(mitigation.Id);
                }
            }
        }  
    }
}
