using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [JsonProperty("threatTypes")]
        private List<IThreatType> _threatTypes;

        public IEnumerable<IThreatType> ThreatTypes => _threatTypes?.AsReadOnly();

        [InitializationRequired]
        public IEnumerable<IThreatType> SearchThreatTypes(string filter)
        {
            IEnumerable<IThreatType> result = null;

            Dictionary<Guid, int> points = new Dictionary<Guid, int>();
            var threats = _threatTypes?.ToArray();
            if (threats?.Any() ?? false)
            {
                foreach (var threat in threats)
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        points[threat.Id] = Matches(threat, filter);
                }

                result = points.Where(x => x.Value > 0).OrderByDescending(x => x.Value)
                    .Select(x => threats.FirstOrDefault(y => y.Id == x.Key));
            }

            return result;
        }

        private int Matches([NotNull] IThreatType threatType, [Required] string filter)
        {
            int result = 0;

            if ((threatType.Name?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                result++;
            if ((threatType.Description?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                result++;

            var properties = threatType.Properties?.ToArray();
            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    if ((property.StringValue?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                        result++;

                    if (property is IPropertyTokens propertyTokens)
                    {
                        var values = propertyTokens.Value?.ToArray();
                        if (values?.Any() ?? false)
                        {
                            foreach (var value in values)
                            {
                                if (string.Compare(filter, value, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    result += 10;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        [InitializationRequired]
        public IThreatType GetThreatType(Guid id)
        {
            return _threatTypes?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public IThreatType GetThreatType([Required] string name)
        {
            return _threatTypes?.FirstOrDefault(x => name.IsEqual(x.Name));
        }

        [InitializationRequired]
        public void Add([NotNull] IThreatType threatType)
        {
            if (_threatTypes == null)
                _threatTypes = new List<IThreatType>();

            _threatTypes.Add(threatType);

            SetDirty();
            ChildCreated?.Invoke(threatType);
        }

        [InitializationRequired]
        public IThreatType AddThreatType([Required] string name, [NotNull] ISeverity severity)
        {
            IThreatType result = null;

            if (GetThreatType(name) == null)
            {
                result = new ThreatType(this, name, severity);
                Add(result);
                RegisterEvents(result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveThreatType(Guid id, bool force = false)
        {
            bool result = false;

            var threatType = GetThreatType(id);

            if (threatType != null && (force || !IsUsed(threatType)))
            {
                RemoveRelated(threatType);

                result = _threatTypes.Remove(threatType);
                if (result)
                {
                    UnregisterEvents(threatType);
                    SetDirty();
                    ChildRemoved?.Invoke(threatType);
                }
            }

            return result;
        }
 
        private bool IsUsed([NotNull] IThreatType threatType)
        {
            return (_entities?.Any(x => x.ThreatEvents?.Any(y => y.ThreatTypeId == threatType.Id) ?? false) ?? false) ||
                   (_dataFlows?.Any(x => x.ThreatEvents?.Any(y => y.ThreatTypeId == threatType.Id) ?? false) ?? false) || 
                   (ThreatEvents?.Any(x => x.ThreatTypeId == threatType.Id) ?? false);
        }

        private void RemoveRelated([NotNull] IThreatType threatType)
        {
            RemoveRelatedForEntities(threatType);
            RemoveRelatedForDataFlows(threatType);
            var events = ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id).ToArray();
            if (events?.Any() ?? false)
            {
                foreach (var threatEvent in events)
                {
                    RemoveThreatEvent(threatEvent.Id);
                }
            }
        }

        private void RemoveRelatedForEntities([NotNull] IThreatType threatType)
        {
            var entities = _entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    var events = entity.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id).ToArray();
                    if (events?.Any() ?? false)
                    {
                        foreach (var threatEvent in events)
                        {
                            entity.RemoveThreatEvent(threatEvent.Id);
                        }
                    }
                }
            }
        }

        private void RemoveRelatedForDataFlows([NotNull] IThreatType threatType)
        {
            var dataFlows = _dataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    var events = dataFlow.ThreatEvents?.Where(x => x.ThreatTypeId == threatType.Id).ToArray();
                    if (events?.Any() ?? false)
                    {
                        foreach (var threatEvent in events)
                        {
                            dataFlow.RemoveThreatEvent(threatEvent.Id);
                        }
                    }
                }
            }
        }
    }
}
