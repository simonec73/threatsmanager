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
        [JsonProperty("weaknesses")]
        private List<IWeakness> _weaknesses;

        public IEnumerable<IWeakness> Weaknesses => _weaknesses?.AsReadOnly();

        [InitializationRequired]
        public IEnumerable<IWeakness> SearchWeaknesses(string filter)
        {
            IEnumerable<IWeakness> result = null;

            Dictionary<Guid, int> points = new Dictionary<Guid, int>();
            var weaknesses = _weaknesses?.ToArray();
            if (weaknesses?.Any() ?? false)
            {
                foreach (var weakness in weaknesses)
                {
                    if (!string.IsNullOrWhiteSpace(filter))
                        points[weakness.Id] = Matches(weakness, filter);
                }

                result = points.Where(x => x.Value > 0).OrderByDescending(x => x.Value)
                    .Select(x => weaknesses.FirstOrDefault(y => y.Id == x.Key));
            }

            return result;
        }

        private int Matches([NotNull] IWeakness weakness, [Required] string filter)
        {
            int result = 0;

            if ((weakness.Name?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                result++;
            if ((weakness.Description?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0)
                result++;

            var properties = weakness.Properties?.ToArray();
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
        public IWeakness GetWeakness(Guid id)
        {
            return _weaknesses?.FirstOrDefault(x => x.Id == id);
        }

        [InitializationRequired]
        public IWeakness GetWeakness([Required] string name)
        {
            return _weaknesses?.FirstOrDefault(x => name.IsEqual(x.Name));
        }

        [InitializationRequired]
        public void Add([NotNull] IWeakness weakness)
        {
            if (_weaknesses == null)
                _weaknesses = new List<IWeakness>();

            _weaknesses.Add(weakness);

            SetDirty();
            ChildCreated?.Invoke(weakness);
        }

        [InitializationRequired]
        public IWeakness AddWeakness([Required] string name, [NotNull] ISeverity severity)
        {
            IWeakness result = null;

            if (GetWeakness(name) == null)
            {
                result = new Weakness(this, name, severity);
                Add(result);
                RegisterEvents(result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveWeakness(Guid id, bool force = false)
        {
            bool result = false;

            var weakness = GetWeakness(id);

            if (weakness != null && (force || !IsUsed(weakness)))
            {
                RemoveRelated(weakness);

                result = _weaknesses.Remove(weakness);
                if (result)
                {
                    UnregisterEvents(weakness);
                    SetDirty();
                    ChildRemoved?.Invoke(weakness);
                }
            }

            return result;
        }
 
        private bool IsUsed([NotNull] IWeakness weakness)
        {
            return (_entities?.Any(x => x.Vulnerabilities?.Any(y => y.WeaknessId == weakness.Id) ?? false) ?? false) ||
                   (_dataFlows?.Any(x => x.Vulnerabilities?.Any(y => y.WeaknessId == weakness.Id) ?? false) ?? false) || 
                   (Vulnerabilities?.Any(x => x.WeaknessId == weakness.Id) ?? false);
        }

        private void RemoveRelated([NotNull] IWeakness weakness)
        {
            RemoveRelatedForEntities(weakness);
            RemoveRelatedForDataFlows(weakness);
            var vulnerabilities = Vulnerabilities?.Where(x => x.WeaknessId == weakness.Id).ToArray();
            if (vulnerabilities?.Any() ?? false)
            {
                foreach (var vulnerability in vulnerabilities)
                {
                    RemoveVulnerability(vulnerability.Id);
                }
            }
        }

        private void RemoveRelatedForEntities([NotNull] IWeakness weakness)
        {
            var entities = _entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    var vulnerabilities = entity.Vulnerabilities?.Where(x => x.WeaknessId == weakness.Id).ToArray();
                    if (vulnerabilities?.Any() ?? false)
                    {
                        foreach (var vulnerability in vulnerabilities)
                        {
                            entity.RemoveVulnerability(vulnerability.Id);
                        }
                    }
                }
            }
        }

        private void RemoveRelatedForDataFlows([NotNull] IWeakness weakness)
        {
            var dataFlows = _dataFlows?.ToArray();
            if (dataFlows?.Any() ?? false)
            {
                foreach (var dataFlow in dataFlows)
                {
                    var vulnerabilities = dataFlow.Vulnerabilities?.Where(x => x.WeaknessId == weakness.Id).ToArray();
                    if (vulnerabilities?.Any() ?? false)
                    {
                        foreach (var vulnerability in vulnerabilities)
                        {
                            dataFlow.RemoveVulnerability(vulnerability.Id);
                        }
                    }
                }
            }
        }
    }
}
