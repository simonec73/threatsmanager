using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality
{
    /// <summary>
    /// Utility class to perform quality analysis of Threat Models.
    /// </summary>
    public class QualityAnalyzersManager
    {
        private static readonly IEnumerable<IQualityAnalyzer> _analyzers;

        static QualityAnalyzersManager()
        {
            _analyzers = ExtensionUtils.GetExtensions<IQualityAnalyzer>();
        }

        /// <summary>
        /// Get the list of configured Quality Analyzers.
        /// </summary>
        public static IEnumerable<IQualityAnalyzer> QualityAnalyzers => _analyzers;

        /// <summary>
        /// Calculate the Normalized Threat Model Health Index.
        /// </summary>
        /// <param name="model">Model to be analyzed.</param>
        /// <param name="isFalsePositive">Function that evaluates an object to decide if it has already been marked as a False Positive.</param>
        /// <param name="outcomes">Health of the analysis.</param>
        /// <returns>Normalized Threat Model Health Index.</returns>
        /// <remarks>For the normalized 30 represents perfect health.</remarks>
        public double Analyze([NotNull] IThreatModel model, 
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            out IEnumerable<QualityAnalyzerResult> outcomes)
        {
            double result = 0;
            outcomes = null;

            if (_analyzers?.Any() ?? false)
            {
                double calculated = 0.0;
                List<QualityAnalyzerResult> results = new List<QualityAnalyzerResult>();

                foreach (var analyzer in _analyzers)
                {
                    var analyzerResult = new QualityAnalyzerResult(analyzer, isFalsePositive, model);
                    results.Add(analyzerResult);
                    calculated += analyzerResult.Health;
                }

                outcomes = results.ToArray();
                result = calculated * 30.0 / GetMaxAssessment();
            }

            return result;
        }
        
        /// <summary>
        /// Calculates the de
        /// </summary>
        /// <param name="healthIndex"></param>
        /// <returns></returns>
        public static string GetHealthIndexDescription(double healthIndex)
        {
            string result;

            if (healthIndex <= 10)
                result = "Poor";
            else if (healthIndex <= 20)
                result = "Insufficient";
            else if (healthIndex <= 24)
                result = "Sufficient";
            else if (healthIndex <= 27)
                result = "Good";
            else if (healthIndex <= 29)
                result = "Great";
            else
                result = "Best";

            return result;
        }

        private static double GetMaxAssessment()
        {
            return _analyzers?.Sum(x => x.MultiplicationFactor * 2.0) ?? 0.0;
        }
    }
}
