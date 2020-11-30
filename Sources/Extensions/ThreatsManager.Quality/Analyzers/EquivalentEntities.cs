using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("A5516681-A026-44DA-84C5-EED00DE6BDA6", "Equivalent Entities Quality Analyzer", 19, ExecutionMode.Simplified)]
    public class EquivalentEntities : IQualityAnalyzer
    {
        public string Label => "Equivalent Entities";

        public string Description =>
            "Entities are Equivalent if they have the same Threats and Mitigations,\nand they are associated to equivalent Flows.\nEquivalent Entities should be avoided because they complicate the Threat Model unnecessarily.\nNote: Scenarios are not considered.";

        public double MultiplicationFactor => 0.5;

        public override string ToString()
        {
            return Label;
        }

        public bool GetThresholds(IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, out double minRed, out double maxRed, out double minYellow,
            out double maxYellow, out double minGreen, out double maxGreen)
        {
            bool result = false;
            minRed = 0;
            maxRed = 0;
            minYellow = double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = (model.Entities?.Where(y => !isFalsePositive(this, y)).Count() ?? 0);

            if (count >= 10)
            {
                minGreen = 0.0;
                maxGreen = count * 0.25;
                minYellow = Math.Floor(maxGreen * 10.0) / 10.0 + Math.Floor(count / 10.0) / 10;
                maxYellow = count * 0.75;
                minRed = Math.Floor(maxYellow * 10.0) / 10.0 + Math.Floor(count / 10.0) / 10;
                maxRed = count;

                result = true;
            }
            else if (count > 0)
            {
                minGreen = 0.0;
                maxGreen = count * 0.75;
                minRed = maxGreen + count * 0.02;
                maxRed = count;

                result = true;
            }
            else
            {
                minGreen = 0;
                maxGreen = 1;
                minRed = double.NaN;
                maxRed = double.NaN;
            }

            return result;
        }

        public double Analyze(IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, out IEnumerable<object> instances)
        {
            double result = 0.0;
            instances = null;

            var entities = model.Entities?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (entities?.Any() ?? false)
            {
                var flows = model.DataFlows?.ToArray();

                if (flows?.Any() ?? false)
                {
                    var found = new List<IEntity>();

                    foreach (var entityA in entities)
                    {
                        foreach (var entityB in entities)
                        {
                            if (entityA != entityB && AreEquivalent(entityA, entityB, flows))
                            {
                                if (!found.Contains(entityA))
                                    found.Add(entityA);
                                if (!found.Contains(entityB))
                                    found.Add(entityB);
                                break;
                            }
                        }
                    }

                    result = found.Count;
                    instances = found;
                }
                else
                {
                    if (entities.Any())
                        instances = entities;
                    result = entities.Length;
                }
            }

            return result;
        }

        private bool AreEquivalent([NotNull] IEntity left, [NotNull] IEntity right, 
            [NotNull] IEnumerable<IDataFlow> flows)
        {
            bool result = false;

            if (left.GetEntityType() == right.GetEntityType())
            {
                result = AreEquivalent(left, right) &&
                         AreEquivalent(left, flows.Where(x => x.Source == left), 
                             right, flows.Where(x => x.Source == right), false) &&
                         AreEquivalent(left, flows.Where(x => x.Target == left), 
                             right, flows.Where(x => x.Target == right), true);
            }

            return result;
        }

        private bool AreEquivalent([NotNull] IThreatEventsContainer left, [NotNull] IThreatEventsContainer right)
        {
            bool result = false;

            var leftThreats = left.ThreatEvents?.ToArray();
            var rightThreats = right.ThreatEvents?.ToArray();

            if (leftThreats?.Any() ?? false)
            {
                if (rightThreats?.Any() ?? false)
                {
                    if (leftThreats.Length == rightThreats.Length)
                    {
                        result = true;

                        foreach (var threat in leftThreats)
                        {
                            var rightThreat = rightThreats.FirstOrDefault(x => x.ThreatTypeId == threat.ThreatTypeId);
                            if (rightThreat == null || !AreEquivalent(threat, rightThreat))
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!(rightThreats?.Any() ?? false))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool AreEquivalent([NotNull] IThreatEvent left, [NotNull] IThreatEvent right)
        {
            return (left.SeverityId == right.SeverityId) && 
                   AreEquivalent(left.Mitigations, right.Mitigations);
        }

        private bool AreEquivalent(IEnumerable<IThreatEventMitigation> left,
            IEnumerable<IThreatEventMitigation> right)
        {
            bool result = false;

            if (left?.Any() ?? false)
            {
                if (right?.Any() ?? false)
                {
                    if (left.Count() == right.Count())
                    {
                        result = true;

                        foreach (var mitigation in left)
                        {
                            var rightMitigation = right.FirstOrDefault(x => x.MitigationId == mitigation.MitigationId);
                            if (rightMitigation == null || 
                                (mitigation.Status != rightMitigation.Status) ||
                                (mitigation.StrengthId != rightMitigation.StrengthId) ||
                                string.CompareOrdinal(mitigation.Directives, rightMitigation.Directives) != 0)
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!(right?.Any() ?? false))
                {
                    result = true;
                }
            }                

            return result;
        }

        private bool AreEquivalent([NotNull] IEntity left, IEnumerable<IDataFlow> leftFlows, 
            [NotNull] IEntity right, IEnumerable<IDataFlow> rightFlows, bool incoming)
        {
            bool result = false;

            if (leftFlows?.Any() ?? false)
            {
                if (rightFlows?.Any() ?? false)
                {
                    if (leftFlows.Count() == rightFlows.Count())
                    {
                        result = true;

                        foreach (var flow in leftFlows)
                        {
                            IDataFlow rightFlow = null;
                            if (incoming)
                            {
                                if (flow.Source is IEntity source)
                                {
                                    rightFlow = rightFlows.FirstOrDefault(x => x.Source == source);
                                }
                            }
                            else
                            {
                                if (flow.Target is IEntity target)
                                {
                                    rightFlow = rightFlows.FirstOrDefault(x => x.Target == target);
                                }
                            }

                            if (rightFlow == null || 
                                (flow.FlowType != rightFlow.FlowType) ||
                                !AreEquivalent(flow, rightFlow))
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!(rightFlows?.Any() ?? false))
                {
                    result = true;
                }
            }                

            return result;
        }

        private bool AreEquivalentFlows([NotNull] IDataFlow left, [NotNull] IDataFlow right)
        {
            bool result = false;

            if (left.FlowType == right.FlowType)
            {
                result = AreEquivalent(left, right);
            }

            return result;
        }
    }
}