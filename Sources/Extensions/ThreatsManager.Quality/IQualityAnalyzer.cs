using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality
{
    /// <summary>
    /// Interface that defines a module to analyze the Threat Model and determine potential quality issues.
    /// </summary>
    [ExtensionDescription("Quality Analyzer")]
    public interface IQualityAnalyzer : IExtension
    {
        /// <summary>
        /// Short description of the Analyzer.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Description of the Analyzer.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Factor that can be used to assign an weight to the Quality Analyzer.
        /// </summary>
        double MultiplicationFactor { get; }

        /// <summary>
        /// Calculate the thresholds for the Analyzer, based on the Threat Model.
        /// </summary>
        /// <param name="model">Model to be analyzed.</param>
        /// <param name="isFalsePositive">Function that evaluates an object to decide if it has already been marked as a False Positive.</param>
        /// <param name="minRed">Minimum value to be used to identify a problematic situation.</param>
        /// <param name="maxRed">Maximum value to be used to identify a problematic situation.</param>
        /// <param name="minYellow">Minimum value to be used to identify an intermediate situation.</param>
        /// <param name="maxYellow">Maximum value to be used to identify an intermediate situation.</param>
        /// <param name="minGreen">Minimum value to be used to identify a good situation.</param>
        /// <param name="maxGreen">Maximum value to be used to identify a good situation.</param>
        /// <returns>False if it is not possible to evaluate the Threat Model.</returns>
        bool GetThresholds(IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive,
            out double minRed, out double maxRed,
            out double minYellow, out double maxYellow,
            out double minGreen, out double maxGreen);

        /// <summary>
        /// Analyze the Threat Model and identifies where there are problems.
        /// </summary>
        /// <param name="model">Model to be analyzed.</param>
        /// <param name="isFalsePositive">Function that evaluates an object to decide if it has already been marked as a False Positive.</param>
        /// <param name="instances">Objects where problems have been identified.</param>
        /// <returns>Calculated quality value.</returns>
        double Analyze(IThreatModel model, Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            out IEnumerable<object> instances);
    }
}
