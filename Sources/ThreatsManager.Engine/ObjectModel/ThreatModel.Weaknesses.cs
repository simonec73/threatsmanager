using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using ThreatsManager.Engine.ObjectModel.ThreatsMitigations;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Engine.ObjectModel
{
    public partial class ThreatModel
    {
        [Child]
        [JsonProperty("weaknesses", Order = 41)]
        private AdvisableCollection<Weakness> _weaknesses { get; set; }

        [IgnoreAutoChangeNotification]
        public IEnumerable<IWeakness> Weaknesses => _weaknesses?.AsEnumerable();

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
            if (weakness is Weakness w)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Weakness"))
                {
                    if (_weaknesses == null)
                        _weaknesses = new AdvisableCollection<Weakness>();

                    UndoRedoManager.Attach(w, this);
                    _weaknesses.Add(w);
                    scope?.Complete();

                    ChildCreated?.Invoke(w);
                }
            }
            else
                throw new ArgumentException(nameof(weakness));
        }

        [InitializationRequired]
        public IWeakness AddWeakness([Required] string name, [NotNull] ISeverity severity)
        {
            IWeakness result = null;

            if (GetWeakness(name) == null)
            {
                result = new Weakness(name, severity);
                Add(result);
                RegisterEvents(result);
            }

            return result;
        }

        [InitializationRequired]
        public bool RemoveWeakness(Guid id, bool force = false)
        {
            bool result = false;

            var weakness = GetWeakness(id) as Weakness;

            if (weakness != null && (force || !IsUsed(weakness)))
            {
                using (var scope = UndoRedoManager.OpenScope("Remove Weakness"))
                {
                    RemoveRelated(weakness);

                    result = _weaknesses.Remove(weakness);
                    if (result)
                    {
                        UndoRedoManager.Detach(weakness);
                        UnregisterEvents(weakness);
                        ChildRemoved?.Invoke(weakness);
                    }

                    scope?.Complete();
                }
            }

            return result;
        }
 
        private bool IsUsed([NotNull] IWeakness weakness)
        {
            return (_entities?.Any(x => x.Vulnerabilities?.Any(y => y.WeaknessId == weakness.Id) ?? false) ?? false) ||
                   (_entities?.Any(x => x.ThreatEvents?.Any(y => y.Vulnerabilities?.Any(z => z.WeaknessId == weakness.Id) ?? false) ?? false) ?? false) ||
                   (_flows?.Any(x => x.Vulnerabilities?.Any(y => y.WeaknessId == weakness.Id) ?? false) ?? false) ||
                   (_flows?.Any(x => x.ThreatEvents?.Any(y => y.Vulnerabilities?.Any(z => z.WeaknessId == weakness.Id) ?? false) ?? false) ?? false) ||
                   (Vulnerabilities?.Any(x => x.WeaknessId == weakness.Id) ?? false) ||
                   (ThreatEvents?.Any(x => x.Vulnerabilities?.Any(y => y.WeaknessId == weakness.Id) ?? false) ?? false);
        }

        private void RemoveRelated([NotNull] IWeakness weakness)
        {
            RemoveRelated(weakness, _entities);
            RemoveRelated(weakness, _flows);
            RemoveRelated(weakness, this);
        }

        private void RemoveRelated([NotNull] IWeakness weakness, IEnumerable<IVulnerabilitiesContainer> containers)
        {
            if (containers?.Any() ?? false)
            {
                foreach (var container in containers)
                {
                    RemoveRelated(weakness, container);
                }
            }
        }

        private void RemoveRelated([NotNull] IWeakness weakness, IVulnerabilitiesContainer container)
        {
            var vulnerabilities = container?.Vulnerabilities?.Where(x => x.WeaknessId == weakness.Id).ToArray();
            if (vulnerabilities?.Any() ?? false)
            {
                foreach (var vulnerability in vulnerabilities)
                {
                    container.RemoveVulnerability(vulnerability.Id);
                }
            }
        }
    }
}
