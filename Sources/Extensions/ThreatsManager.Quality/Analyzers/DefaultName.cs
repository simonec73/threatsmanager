using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Analyzers
{
    [Extension("208AB31D-2B01-4BE2-B726-90896EE67C69", "Objects with Default Name Quality Analyzer", 4, ExecutionMode.Simplified)]
    public class DefaultName : IQualityAnalyzer
    {
        public string Label => "Default Name Objects";
        public string Description  =>
            "Objects with a default name should be considered a mistake because they do not allow to understand their meaning.";

        public double MultiplicationFactor => 1.0;

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
            minYellow = Double.NaN;
            maxYellow = double.NaN;
            minGreen = 0;
            maxGreen = 0;

            var count = (model.Entities?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) +
                        (model.DataFlows?.Where(x => !isFalsePositive(this, x)).Count() ?? 0) +
                        (model.Groups?.Where(x => !isFalsePositive(this, x)).Count() ?? 0);

            if (count > 0)
            {
                minGreen = 0;
                maxGreen = 0.49;
                minRed = 0.51;
                maxRed = 1;

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

            var found = new List<IIdentity>();

            var entities = model.Entities?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (entities?.Any() ?? false)
            {
                Regex eiRegex = null;
                Regex pRegex = null;
                Regex dsRegex = null;
                Regex regex = null;

                foreach (var entity in entities)
                {
                    if (entity is IExternalInteractor)
                    {
                        if (eiRegex == null)
                            eiRegex = new Regex($"^{model.GetIdentityTypeName(entity)} [0-9]+$");
                        regex = eiRegex;
                    } else if (entity is IProcess)
                    {
                        if (pRegex == null)
                            pRegex = new Regex($"^{model.GetIdentityTypeName(entity)} [0-9]+$");
                        regex = pRegex;
                    } if (entity is IDataStore)
                    {
                        if (dsRegex == null)
                            dsRegex = new Regex($"^{model.GetIdentityTypeName(entity)} [0-9]+$");
                        regex = dsRegex;
                    }                  
                    
                    if (!string.IsNullOrWhiteSpace(entity.Name) && (regex?.Match(entity.Name).Success ?? false))
                        found.Add(entity);
                }
            }

            var flows = model.DataFlows?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    if (string.CompareOrdinal(flow.Name, "Flow") == 0)
                        found.Add(flow);
                }
            }

            var groups = model.Groups?.Where(x => !isFalsePositive(this, x)).ToArray();
            if (groups?.Any() ?? false)
            {
                Regex tbRegex = null;
                
                foreach (var group in groups)
                {
                    if (tbRegex == null)
                        tbRegex = new Regex($"^{model.GetIdentityTypeName(group)} [0-9]+$");

                    if (!string.IsNullOrWhiteSpace(group.Name) && (tbRegex?.Match(group.Name).Success ?? false))
                        found.Add(group);
                }
            }

            result = found.Count;
            instances = found;

            return result;
        }
    }
}