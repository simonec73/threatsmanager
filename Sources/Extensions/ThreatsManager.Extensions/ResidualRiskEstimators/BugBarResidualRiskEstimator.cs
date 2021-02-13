using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.ResidualRiskEstimators
{
    [Extension("C1F10EC7-FF20-4FE5-99EC-2DBF3C1BC216", "Bug Bar Residual Risk Estimator", 10, ExecutionMode.Business)]
    public class BugBarResidualRiskEstimator : IResidualRiskEstimator
    {
        class MitigationWeight
        {
            public MitigationWeight(Guid mitigationId, int effectiveness)
            {
                MitigationId = mitigationId;
                Effectiveness = effectiveness;
            }

            public Guid MitigationId { get; private set; }
            public int Effectiveness { get; set; }
        }

        public float DefaultInfinite => 10f;

        public float Estimate([NotNull] IThreatModel model, IEnumerable<Guid> mitigations, out float min, out float max)
        {
            float result = 0f;

            var threatEvents = model.GetThreatEvents()?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    var risk = (float)threatEvent.SeverityId;
                    var mitigationsStrength = threatEvent.Mitigations?
                        .Where(x => (x.Status != MitigationStatus.Existing) &&
                                    (x.Status != MitigationStatus.Implemented) &&
                                    (mitigations?.Any(y => y == x.MitigationId) ?? false))
                        .Sum(x => x.StrengthId);
                    result += risk * (100f - Math.Min((mitigationsStrength ?? 0), 100f)) / 100f;
                }
            }

            min = result * 0.9f;
            max = result * 1.1f;

            return result;
        }

        public IDictionary<Guid, Effectiveness> CategorizeMitigations([NotNull] IThreatModel model)
        {
            IDictionary<Guid, Effectiveness> result = null;

            var mitigations = model.GetUniqueMitigations()?
                .Select(x => new MitigationWeight(x.Id, CalculateMitigationEffectiveness(model, x)))
                .OrderByDescending(x => x.Effectiveness)
                .ToArray();
            if (mitigations?.Any() ?? false)
            {
                var list = new Dictionary<Guid, Effectiveness>();

                int count = mitigations.Length / 3;
                if (count > 0)
                {
                    int i = 0;
                    foreach (var mitigation in mitigations)
                    {
                        var pos = Math.DivRem(i, count, out var remainder);
                        i++;
                        Effectiveness effectiveness;
                        if (mitigation.Effectiveness == 0)
                            effectiveness = Effectiveness.Minor;
                        else
                            switch (pos)
                            {
                                case 0:
                                    effectiveness = Effectiveness.Major;
                                    break;
                                case 1:
                                    effectiveness = Effectiveness.Average;
                                    break;
                                default:
                                    effectiveness = Effectiveness.Minor;
                                    break;
                            }

                        list.Add(mitigation.MitigationId, effectiveness);
                    }
                }
                else
                {
                    foreach (var mitigation in mitigations)
                    {
                        list.Add(mitigation.MitigationId, Effectiveness.Average);
                    }
                }

                result = list;
            }

            return result;
        }

        public IDictionary<Guid, int> GetProjectedThreatTypesResidualRisk(IThreatModel model, IEnumerable<Guid> mitigations)
        {
            IDictionary<Guid, int> result = null;

            var threatEvents = model.GetThreatEvents()?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                var dict = new Dictionary<Guid, int>();

                foreach (var threatEvent in threatEvents)
                {
                    var mitigationsStrength = threatEvent.Mitigations?
                        .Where(x => (x.Status != MitigationStatus.Existing) &&
                                    (x.Status != MitigationStatus.Implemented) &&
                                    (mitigations?.Any(y => y == x.MitigationId) ?? false))
                        .Sum(x => x.StrengthId);
                    var newSeverity = model.GetMappedSeverity(
                        Convert.ToInt32(((float)threatEvent.SeverityId) *
                                       (100f - Math.Min((mitigationsStrength ?? 0), 100f)) / 100f));

                    if (newSeverity != null)
                    {
                        var newSeverityId = newSeverity.Id == 0 ? (int)DefaultSeverity.Info : newSeverity.Id;
                        if (dict.ContainsKey(threatEvent.ThreatTypeId))
                        {
                            if (dict[threatEvent.ThreatTypeId] < newSeverityId)
                                dict[threatEvent.ThreatTypeId] = newSeverityId;
                        }
                        else
                        {
                            dict[threatEvent.ThreatTypeId] = newSeverityId;
                        }
                    }
                }

                result = dict;
            }

            return result;
        }

        private int CalculateMitigationEffectiveness([NotNull] IThreatModel model, [NotNull] IMitigation mitigation)
        {
            int result = 0;

            var threats = model.GetThreatEvents()?
                .Where(x => x.Mitigations?.Any(y =>
                    y.MitigationId == mitigation.Id && y.Status != MitigationStatus.Implemented &&
                    y.Status != MitigationStatus.Existing) ?? false)
                .ToArray();
            if (threats?.Any() ?? false)
            {
                foreach (var threat in threats)
                {
                    var threatMitigation = threat.Mitigations.First(x => x.MitigationId == mitigation.Id);
                    result += threatMitigation.StrengthId * threat.SeverityId;
                }
            }

            return result;
        }

        public IEnumerable<string> GetAcceptableRiskParameters([NotNull] IThreatModel model)
        {
            return model.Severities?.Where(x => x.Visible && x.Id > 0)
                .Select(x => $"Maximum {x.Name} Severity Threat Events count").ToArray();
        }

        public float GetAcceptableRisk([NotNull] IThreatModel model,
            IDictionary<string, float> parameters, float infinite, int normalizationReference)
        {
            float result = 0f;

            var severities = model.Severities?.ToArray();
            if ((parameters?.Any() ?? false) && (severities?.Any() ?? false))
            {
                var regex = new Regex("Maximum (?'severity'.*) Severity Threat Events count");
                foreach (var parameter in parameters)
                {
                    var match = regex.Match(parameter.Key);
                    if (match.Success)
                    {
                        var severityName = match.Groups["severity"].Value;
                        var severity = severities.FirstOrDefault(x => string.CompareOrdinal(x.Name, severityName) == 0);
                        if (severity != null)
                        {
                            result += severity.Id *
                                      (parameter.Value < 0 ? infinite : Math.Min(parameter.Value, infinite));
                        }
                    }
                }
            }

            if (normalizationReference > 0)
                result *= ((float)((model.Entities?.Count() ?? 0) + (model.DataFlows?.Count() ?? 0))) / (float)normalizationReference;

            return result;
        }

        public float GetRiskEvaluation(IThreatModel model, int normalizationReference)
        {
            var result = 0f;

            var threatEvents = model.GetThreatEvents()?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                var totalSeverity = 0;

                foreach (var threatEvent in threatEvents)
                {
                    totalSeverity += threatEvent.SeverityId;
                }

                if (normalizationReference > 0)
                    result = (float) totalSeverity * (float) normalizationReference /
                             ((float) ((model.Entities?.Count() ?? 0) + (model.DataFlows?.Count() ?? 0)));
                else
                    result = (float)totalSeverity;
            }

            return result;
        }

        public override string ToString()
        {
            return this.GetExtensionLabel();
        }
    }
}