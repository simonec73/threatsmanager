using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Calculator of the estimated Residual Risk.
    /// </summary>
    public interface IResidualRiskEstimator
    {
        /// <summary>
        /// Default value to represent unlimited/infinite.
        /// </summary>
        float DefaultInfinite { get; }

        /// <summary>
        /// Calculate the estimated risk for the Threat Model, starting from the assumption that the listed mitigations are implemented.
        /// </summary>
        /// <param name="model">Threat Model for which the Residual Risk must be estimated.</param>
        /// <param name="mitigations">Identifiers of the implemented Mitigations.</param>
        /// <param name="min">[Out] Minimum estimated residual risk.</param>
        /// <param name="max">[Out] Minimum estimated residual risk.</param>
        /// <returns>Most probable estimated residual risk.</returns>
        float Estimate(IThreatModel model, IEnumerable<Guid> mitigations, out float min, out float max);

        /// <summary>
        /// Get a list of parameters that can be used for calculating the Acceptable Risk.
        /// </summary>
        /// <param name="model">Model for which the parameters must be identified.</param>
        /// <returns>List of required parameters.</returns>
        /// <remarks>Each parameter will require to provide a value as float.</remarks>
        IEnumerable<string> GetAcceptableRiskParameters(IThreatModel model);

        /// <summary>
        /// Calculate the Acceptable Risk for the Threat Model.
        /// </summary>
        /// <param name="model">Model to be analyzed.</param>
        /// <param name="parameters">Parameters of the calculation.</param>
        /// <param name="infinite">Value to be used to represent infinite/unlimited.</param>
        /// <returns>Evaluation of the Acceptable Risk.</returns>
        /// <remarks>The value of the parameters can be negative, to express an unlimited value.</remarks>
        float GetAcceptableRisk(IThreatModel model, IDictionary<string, float> parameters, float infinite);
    }
}