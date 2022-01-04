using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.QuantitativeRisk.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    public class QuantitativeRiskAnalyzer
    {
        #region Private member variables.
        private readonly List<double[]> _frequencyArrays = new List<double[]>();
        private readonly List<double[]> _magnitudeArrays = new List<double[]>();
        #endregion

        public const int DefaultSamples = 100000;

        public QuantitativeRiskAnalyzer(int samplesCount = DefaultSamples)
        {
            SamplesCount = samplesCount;
        }

        #region Public properties.
        public int SamplesCount { get; private set; }

        public Series AnnualizedValueSeries
        {
            get
            {
                Series result = null;

                if (_frequencyArrays.Count > 0)
                {
                    var temp = new double[SamplesCount];

                    IncrementAnnualizedValues(temp, _frequencyArrays[0], _magnitudeArrays[0]);
                    if (_frequencyArrays.Count > 1)
                        IncrementAnnualizedValues(temp, _frequencyArrays[1], _magnitudeArrays[1]);

                    result = new Series();
                    result.Samples.AddRange(temp);
                }

                return result;
            }
        }

        public Series PrimaryFrequencySeries
        {
            get
            {
                Series result = null;

                if (_frequencyArrays.Count > 0)
                {
                    result = new Series();
                    result.Samples.AddRange(_frequencyArrays[0]);
                }

                return result;
            }
        }

        public Series SecondaryFrequencySeries
        {
            get
            {
                Series result = null;

                if (_frequencyArrays.Count > 1)
                {
                    result = new Series();
                    result.Samples.AddRange(_frequencyArrays[1]);
                }

                return result;
            }
        }

        public Series PrimaryMagnitudeSeries
        {
            get
            {
                Series result = null;

                if (_magnitudeArrays.Count > 0)
                {
                    result = new Series();
                    result.Samples.AddRange(_magnitudeArrays[0]);
                }

                return result;
            }
        }

        public Series SecondaryMagnitudeSeries
        {
            get
            {
                Series result = null;

                if (_magnitudeArrays.Count > 1)
                {
                    result = new Series();
                    result.Samples.AddRange(_magnitudeArrays[1]);
                }

                return result;
            }
        }
        #endregion

        #region Public member functions.
        public bool Simulate([NotNull] IThreatEventScenario scenario, out IEnumerable<ValidationResult> validationResults)
        {
            var result = false;
            _frequencyArrays.Clear();
            _magnitudeArrays.Clear();

            var results = Validate(scenario);
            validationResults = results;
            if (!(results?.Any(x => x.IsBlockingError) ?? false))
            {
                var risk = scenario.GetRisk();
                if (risk.InDepth && risk.LossEventFrequency.IsValid && risk.LossMagnitude.SecondaryLossEventFrequency.IsValid)
                {
                    var primaryFrequency = GetSamples(risk.LossEventFrequency.FrequencyMin,
                        risk.LossEventFrequency.FrequencyMostLikely,
                        risk.LossEventFrequency.FrequencyMax,
                        risk.LossEventFrequency.FrequencyConfidence);
                    if (primaryFrequency != null)
                    {
                        var secondaryPercentage = GetSamples(risk.LossMagnitude.SecondaryLossEventFrequency.Minimum,
                            risk.LossMagnitude.SecondaryLossEventFrequency.MostLikely,
                            risk.LossMagnitude.SecondaryLossEventFrequency.Maximum,
                            risk.LossMagnitude.SecondaryLossEventFrequency.Confidence);
                        if (secondaryPercentage != null)
                        {
                            var primaryMagnitude = GetPrimaryLossMagnitudeSamples(risk);
                            if (primaryMagnitude != null)
                            {
                                var secondaryMagnitude = GetSecondaryLossMagnitudeSamples(risk);
                                if (secondaryMagnitude != null)
                                {
                                    _frequencyArrays.Add(primaryFrequency);
                                    var secondaryFrequency = new double[SamplesCount];
                                    for (var i = 0; i < SamplesCount; i++)
                                        secondaryFrequency[i] =
                                            primaryFrequency[i] * secondaryPercentage[i] / 100.0;
                                    _frequencyArrays.Add(secondaryFrequency);
                                    _magnitudeArrays.Add(primaryMagnitude);
                                    _magnitudeArrays.Add(secondaryMagnitude);
                                    result = true;
                                }
                                else
                                {
                                    results?.Add(new ValidationResult("Secondary Magnitude does not define a valid distribution", true));
                                }
                            }
                            else
                            {
                                results?.Add(new ValidationResult("Primary Magnitude does not define a valid distribution", true));
                            }
                        }
                        else
                        {
                            results?.Add(new ValidationResult(Resources.InvalidSLEFDistribution, true));
                        }
                    }
                    else
                        results?.Add(new ValidationResult(Resources.InvalidLEFDistribution, true));
                }
                else
                {
                    var frequency = GetSamples(risk.FrequencyMin, risk.FrequencyMostLikely,
                        risk.FrequencyMax, risk.FrequencyConfidence);
                    var magnitude = GetSamples(risk.MagnitudeMin, risk.MagnitudeMostLikely,
                        risk.MagnitudeMax, risk.MagnitudeConfidence);
                    if (frequency != null && magnitude != null)
                    {
                        _frequencyArrays.Add(frequency);
                        _magnitudeArrays.Add(magnitude);
                        result = true;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Private member functions.
        private List<ValidationResult> Validate([NotNull] IThreatEventScenario scenario)
        {
            var result = new List<ValidationResult>();

            var risk = scenario.GetRisk();
            if (risk != null)
            {
                if (risk.InDepth)
                {
                    if (risk.LossEventFrequency == null)
                        result.Add(new ValidationResult(Resources.UndefinedLEF, true));
                    else
                    {
                        if (!risk.LossEventFrequency.IsValid)
                            result.Add(new ValidationResult(Resources.InvalidLEF, true));
                    }

                    if (risk.LossMagnitude == null)
                        result.Add(new ValidationResult(Resources.UndefinedLM, true));
                    else
                    {
                        if (risk.LossMagnitude.SecondaryLossEventFrequency == null)
                            result.Add(new ValidationResult(Resources.NullSLEF, true));
                        else if (!risk.LossMagnitude.SecondaryLossEventFrequency.IsValid)
                            result.Add(new ValidationResult(Resources.InvalidSLEF, true));

                        if (risk.LossMagnitude.PrimaryLosses == null)
                            result.Add(new ValidationResult(Resources.InvalidPL, true));
                        else
                        {
                            var found = false;
                            if (risk.LossMagnitude.PrimaryLosses.Any(
                                x => x.Form == LossForm.Productivity && x.IsValid))
                                found = true;
                            else
                                result.Add(new ValidationResult(
                                    string.Format(Resources.InvalidPrimaryLoss,
                                        LossForm.Productivity.GetEnumLabel()),
                                    false));
                            if (risk.LossMagnitude.PrimaryLosses.Any(
                                x => x.Form == LossForm.Replacement && x.IsValid))
                                found = true;
                            else
                                result.Add(new ValidationResult(
                                    string.Format(Resources.InvalidPrimaryLoss,
                                        LossForm.Replacement.GetEnumLabel()),
                                    false));
                            if (risk.LossMagnitude.PrimaryLosses.Any(
                                x => x.Form == LossForm.Response && x.IsValid))
                                found = true;
                            else
                                result.Add(new ValidationResult(
                                    string.Format(Resources.InvalidPrimaryLoss, LossForm.Response.GetEnumLabel()),
                                    false));

                            if (!found)
                                result.Add(new ValidationResult(Resources.NoPrimaryLoss, true));
                        }

                        if (risk.LossMagnitude.SecondaryLosses == null)
                            result.Add(new ValidationResult(Resources.InvalidSL, true));
                        else
                        {
                            var found = false;
                            if (risk.LossMagnitude.SecondaryLosses.Any(
                                x => x.Form == LossForm.CompetitiveAdvantage && x.IsValid))
                                found = true;
                            else
                                result.Add(new ValidationResult(
                                    string.Format(Resources.InvalidSecondaryLoss,
                                        LossForm.CompetitiveAdvantage.GetEnumLabel()), false));
                            if (risk.LossMagnitude.SecondaryLosses.Any(
                                x => x.Form == LossForm.FinesJudgements && x.IsValid))
                                found = true;
                            else
                                result.Add(new ValidationResult(
                                    string.Format(Resources.InvalidSecondaryLoss,
                                        LossForm.FinesJudgements.GetEnumLabel()),
                                    false));
                            if (risk.LossMagnitude.SecondaryLosses.Any(
                                x => x.Form == LossForm.Reputation && x.IsValid))
                                found = true;
                            else
                                result.Add(new ValidationResult(
                                    string.Format(Resources.InvalidSecondaryLoss,
                                        LossForm.Reputation.GetEnumLabel()),
                                    false));

                            if (!found)
                                result.Add(new ValidationResult(Resources.NoSecondaryLoss, true));
                        }
                    }
                }
                else
                {
                    if (!risk.IsValid)
                        result.Add(new ValidationResult(Resources.InvalidRisk, true));
                }
            }
            else
            {
                result.Add(new ValidationResult(Resources.NullRisk, true));
            }

            return result;
        }

        private void IncrementAnnualizedValues([NotNull] double[] temp, 
            [NotNull] double[] frequencyArray, [NotNull] double[] magnitudeArray)
        {
            for (var i = 0; i < SamplesCount; i++)
            {
                temp[i] += frequencyArray[i] * magnitudeArray[i];
            }
        }

        private double[] GetPrimaryLossMagnitudeSamples([NotNull] Risk risk)
        {
            double[] result = null;

            if (risk.LossMagnitude?.PrimaryLosses?.Any() ?? false)
            {
                result = new double[SamplesCount];
                foreach (var loss in risk.LossMagnitude.PrimaryLosses)
                {
                    if (loss.IsValid && 
                        (loss.Form == LossForm.Productivity || loss.Form == LossForm.Replacement || loss.Form == LossForm.Response))
                    {
                        var samples = GetSamples(loss.MagnitudeMin, loss.MagnitudeMostLikely,
                            loss.MagnitudeMax, loss.MagnitudeConfidence);
                        if (samples != null)
                        {
                            for (var i = 0; i < SamplesCount; i++)
                            {
                                result[i] += samples[i];
                            }
                        }
                    }
                }
            }

            return result;
        }

        private double[] GetSecondaryLossMagnitudeSamples([NotNull] Risk risk)
        {
            double[] result = null;

            if (risk.LossMagnitude?.SecondaryLosses?.Any() ?? false)
            {
                result = new double[SamplesCount];
                foreach (var loss in risk.LossMagnitude.SecondaryLosses)
                {
                    if (loss.IsValid && 
                        (loss.Form == LossForm.CompetitiveAdvantage || loss.Form == LossForm.FinesJudgements || loss.Form == LossForm.Reputation))
                    {
                        var samples = GetSamples(loss.MagnitudeMin, loss.MagnitudeMostLikely,
                            loss.MagnitudeMax, loss.MagnitudeConfidence);
                        if (samples != null)
                        {
                            for (var i = 0; i < SamplesCount; i++)
                            {
                                result[i] += samples[i];
                            }
                        }
                    }
                }
            }

            return result;
        }

        private double[] GetSamples(double min, double mostLikely, double max, Confidence confidence)
        {
            double[] result = null;

            var pert = GetPertDistribution(min, mostLikely, max, confidence);
            if (pert != null)
            {
                result = new double[SamplesCount];
                pert.Samples(result);
            }

            return result;
        }

        private BetaScaled GetPertDistribution(double min, double mostLikely, 
            double max, Confidence confidence, System.Random randomSource = null)
        {
            var modepad = 0.000000001;
            var lambda = CalculateLambda(confidence);
            var mean = (min + lambda * mostLikely + max) / (lambda + 2.0);
            var effectiveMostLikely = mostLikely;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (mostLikely - mean == 0.0)
            {
                effectiveMostLikely += modepad;
                mean = (min + lambda * effectiveMostLikely + max) / (lambda + 2.0);
            }

            var alpha = ((mean - min) * ((2.0 * effectiveMostLikely) - min - max)) /
                        ((effectiveMostLikely - mean) * (max - min));
            var beta = (alpha * (max - mean)) / (mean - min);

            if (BetaScaled.IsValidParameterSet(alpha, beta, min, max - min))
                return new BetaScaled(alpha, beta, min, max - min, randomSource);
            else
                return null;
        }

        private int CalculateLambda(Confidence confidence)
        {
            var result = 1;

            switch (confidence)
            {
                case Confidence.Low:
                    result = 4;
                    break;
                case Confidence.Moderate:
                    result = 20;
                    break;
                case Confidence.High:
                    result = 160;
                    break;
            }

            return result;
        }
        #endregion
    }
}

