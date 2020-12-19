using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("863E8714-D186-4D13-87F5-F8E52B1DD3CB", "Unbalanced Severities Quality Analyzer", 48, ExecutionMode.Simplified)]
    public class UnbalancedSeverities : IQualityAnalyzer
    {
        private const int CriticalMin = 0;
        private const int CriticalMax = 15;
        private const int HighMin = 10;
        private const int HighMax = 30;
        private const int MediumMin = 20;
        private const int MediumMax = 70;
        private const int LowMin = 10;
        private const int LowMax = 50;
        private const int InfoMin = 0;
        private const int InfoMax = 30;
        
        public override string ToString()
        {
            return Label;
        }

        public string Label => "Unbalanced Severities";
        public string Description => "Severities assigned to Threat Events should be Balanced.\n" +
                                     "The typical percentage for balanced Threat Models, are:\n" +
                                     $"- Critical: {CriticalMin}-{CriticalMax}%\n" +
                                     $"- High: {HighMin}-{HighMax}%\n" +
                                     $"- Medium: {MediumMin}-{MediumMax}%\n" +
                                     $"- Low: {LowMin}-{LowMax}%\n" +
                                     $"- Info: {InfoMin}-{InfoMax}%";

        public double MultiplicationFactor => 0.5;

        public bool GetThresholds([NotNull] IThreatModel model, 
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            out double minRed, out double maxRed, out double minYellow, out double maxYellow,
            out double minGreen, out double maxGreen)
        {
            bool result = false;
            minRed = 0;
            maxRed = 0;
            minYellow = double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = ((model.ThreatEvents?.Any() ?? false) ? 1 : 0) +
                        (model.Entities?
                            .Where(y => y.ThreatEvents?.Any() ?? false).Count() ?? 0) +
                        (model.DataFlows?
                            .Where(y => y.ThreatEvents?.Any() ?? false).Count() ?? 0);

            if (count > 0)
            {
                minGreen = 0;
                maxGreen = 5.4;
                minYellow = 5.6;
                maxYellow = 15.4;
                minRed = 15.6;
                maxRed = 20;

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

        public double Analyze([NotNull] IThreatModel model, 
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            out IEnumerable<object> instances)
        {
            double result = 0.0;
            instances = null;

            var critical = 0;
            var high = 0;
            var medium = 0;
            var low = 0;
            var info = 0;
            AnalyzeContainer(model, ref critical, ref high, ref medium, ref low, ref info);

            var entities = model.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    AnalyzeContainer(entity, ref critical, ref high, ref medium, ref low, ref info);
                }
            }

            var flows = model.DataFlows?.ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    AnalyzeContainer(flow, ref critical, ref high, ref medium, ref low, ref info);
                }
            }

            var total = critical + high + medium + low + info;
            if (total > 0)
            {
                var cP = (critical * 100) / total;
                var hP = (high * 100) / total;
                var mP = (medium * 100) / total;
                var lP = (low * 100) / total;
                var iP = (info * 100) / total;

                List<object> list = new List<object>();
                if (cP < CriticalMin || cP > CriticalMax)
                {
                    list.Add(
                        $"Threat Events with Critical Severity are out of Boundary: {cP}% ({CriticalMin}-{CriticalMax})");
                    if (cP < CriticalMin)
                        result += CriticalMin - cP;
                    else
                        result += cP - CriticalMax;
                }

                if (hP < HighMin || hP > HighMax)
                {
                    list.Add($"Threat Events with High Severity are out of Boundary: {hP}% ({HighMin}-{HighMax})");
                    if (hP < HighMin)
                        result += HighMin - hP;
                    else
                        result += hP - HighMax;
                }

                if (mP < MediumMin || mP > MediumMax)
                {
                    list.Add(
                        $"Threat Events with Medium Severity are out of Boundary: {mP}% ({MediumMin}-{MediumMax})");
                    if (mP < MediumMin)
                        result += MediumMin - mP;
                    else
                        result += mP - MediumMax;
                }

                if (lP < LowMin || lP > LowMax)
                {
                    list.Add($"Threat Events with Low Severity are out of Boundary: {lP}% ({LowMin}-{LowMax})");
                    if (lP < LowMin)
                        result += LowMin - lP;
                    else
                        result += lP - LowMax;
                }

                if (iP < InfoMin || iP > InfoMax)
                {
                    list.Add($"Informational Threat Events are out of Boundary: {iP}% ({InfoMin}-{InfoMax})");
                    if (iP < InfoMin)
                        result += InfoMin - iP;
                    else
                        result += iP - InfoMax;
                }

                if (list.Any())
                {
                    instances = list;
                }
            }

            return result;
        }

        private void AnalyzeContainer([NotNull] IThreatEventsContainer container, ref int critical,
            ref int high, ref int medium, ref int low, ref int info)
        {
            var threatEvents = container.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var te in threatEvents)
                {
                    if (te.SeverityId >= (int) DefaultSeverity.Critical)
                        critical++;
                    else if (te.SeverityId >= (int) DefaultSeverity.High)
                        high++;
                    else if (te.SeverityId >= (int) DefaultSeverity.Medium)
                        medium++;
                    else if (te.SeverityId >= (int) DefaultSeverity.Low)
                        low++;
                    else
                        info++;
                }
            }
        }
    }
}
