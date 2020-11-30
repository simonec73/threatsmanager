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
    /// Result from a Quality Analyzer.
    /// </summary>
    public class QualityAnalyzerResult
    {
        internal QualityAnalyzerResult([NotNull] IQualityAnalyzer analyzer, 
            Func<IQualityAnalyzer, IPropertiesContainer, bool> isFalsePositive, 
            [NotNull] IThreatModel model)
        {
            Id = analyzer.GetExtensionId();
            Value = analyzer.Analyze(model, isFalsePositive, out var findings);
            Findings = findings?.ToArray();
            OkByDefinition = !analyzer.GetThresholds(model, isFalsePositive, 
                out var minRed, out var maxRed,
                out var minYellow, out var maxYellow,
                out var minGreen, out var maxGreen);
            MinRed = minRed;
            MaxRed = maxRed;
            MinYellow = minYellow;
            MaxYellow = maxYellow;
            MinGreen = minGreen;
            MaxGreen = maxGreen;

            double intermediateHealth;
            if (OkByDefinition)
                intermediateHealth = 2;
            else
            {
                ProcessThresholds(minRed, maxRed, minYellow, maxYellow, minGreen, maxGreen, 
                    out var redToGreen, out var min, out var intermediate1,
                    out var intermediate2, out var max);

                if (redToGreen)
                {
                    if (Value <= intermediate1)
                    {
                        intermediateHealth = 0;
                        Assessment = AssessmentOutcome.Poor;
                    }                    
                    else if (!Double.IsNaN(intermediate2) && Value <= intermediate2)
                    {
                        intermediateHealth = 1;
                        Assessment = AssessmentOutcome.Weak;
                    }                    
                    else
                    {
                        intermediateHealth = 2;
                        Assessment = AssessmentOutcome.Good;
                    }
                }
                else
                {
                    if (Value < intermediate1)
                    {
                        intermediateHealth = 2;
                        Assessment = AssessmentOutcome.Good;
                    }                    
                    else if (!Double.IsNaN(intermediate2) && Value < intermediate2)
                    {
                        intermediateHealth = 1;
                        Assessment = AssessmentOutcome.Weak;
                    }                    
                    else
                    {
                        intermediateHealth = 0;
                        Assessment = AssessmentOutcome.Poor;
                    }
                }
            }

            Health = intermediateHealth * analyzer.MultiplicationFactor;
        }

        /// <summary>
        /// ID of the Quality Analyzer.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Value calculated by the Quality Analyzer.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Flag that is shown if the execution must be considered ok by definition,
        /// because there are no elements that may be analyzed.
        /// </summary>
        public bool OkByDefinition { get; private set; }

        /// <summary>
        /// Health of the Analysis.
        /// </summary>
        public double Health { get; private set; }

        /// <summary>
        /// List of elements that need to be addressed.
        /// </summary>
        public IEnumerable<object> Findings { get; private set; }

        /// <summary>
        /// Minimum Red.
        /// </summary>
        public double MinRed { get; private set; }

        /// <summary>
        /// Maximum Red.
        /// </summary>
        public double MaxRed { get; private set; }

        /// <summary>
        /// Minimum Yellow.
        /// </summary>
        public double MinYellow { get; private set; }

        /// <summary>
        /// Maximum Yellow.
        /// </summary>
        public double MaxYellow { get; private set; }

        /// <summary>
        /// Minimum Green.
        /// </summary>
        public double MinGreen { get; private set; }

        /// <summary>
        /// Maximum Green.
        /// </summary>
        public double MaxGreen { get; private set; }

        /// <summary>
        /// Assessment of the Health in textual form.
        /// </summary>
        public AssessmentOutcome Assessment { get; private set; }

        private void ProcessThresholds(double minRed, double maxRed, 
            double minYellow, double maxYellow, double minGreen, double maxGreen,
            out bool redToGreen, out double min, out double intermediate1, out double intermediate2, out double max)
        {
            if (minRed < minGreen)
            {
                redToGreen = true;
                min = minRed;
                if (Double.IsNaN(minYellow) || Double.IsNaN(maxYellow))
                {
                    intermediate1 = (maxRed + minGreen) / 2;
                    intermediate2 = Double.NaN;
                }
                else
                {
                    intermediate1 = (maxRed + minYellow) / 2;
                    intermediate2 = (maxYellow + minGreen) / 2;
                }
                max = maxGreen;
            }
            else
            {
                redToGreen = false;
                min = minGreen;
                if (Double.IsNaN(minYellow) || Double.IsNaN(maxYellow))
                {
                    intermediate1 = (maxGreen + minRed) / 2;
                    intermediate2 = Double.NaN;
                }
                else
                {
                    intermediate1 = (maxGreen + minYellow) / 2;
                    intermediate2 = (maxYellow + minRed) / 2;
                }
                max = maxRed;
            }
        }
    }
}