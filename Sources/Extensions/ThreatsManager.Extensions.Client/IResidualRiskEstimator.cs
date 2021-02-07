using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions
{
    /// <summary>
    /// Calculator of the estimated Residual Risk.
    /// </summary>
    [ExtensionDescription("Residual Risk Estimator")]
    public interface IResidualRiskEstimator : IExtension
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
        /// Categorizes Mitigations belonging to a Threat Model based on their effectiveness.
        /// </summary>
        /// <param name="model">Reference Threat Model.</param>
        /// <returns>Dictionary having as key the identifier of the Mitigation and as value its Effectiveness.</returns>
        IDictionary<Guid, Effectiveness> CategorizeMitigations(IThreatModel model);

        /// <summary>
        /// Calculates the residual risk for Threat Types after implementing the listed mitigations. 
        /// </summary>
        /// <param name="model">Threat Model to be evaluated.</param>
        /// <param name="mitigations">Identifiers of the implemented Mitigations.</param>
        /// <returns>Dictionary with the identifiers of the Threat Types as key and the identifier of the projected severity as value.</returns>
        IDictionary<Guid, int> GetProjectedThreatTypesResidualRisk(IThreatModel model, IEnumerable<Guid> mitigations);

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
        /// <param name="normalizationReference">Normalization Reference.<para/>
        /// The Normalization Reference indicates the number of objects for which the Acceptable Risk is defined.<para/>
        /// For example, if the Normalization Reference is 40 and you have only 8 Entities and 12 Flows, then the Acceptable Risk level is halved.<para/>
        /// Analogously, if the Normalization Reference is 40 and you have 30 Entities and 50 Flows, then the Acceptable Risk level is doubled.<para/>
        /// If it is 0, then no normalization is applied.</param>
        /// <returns>Evaluation of the Acceptable Risk.</returns>
        /// <remarks>The value of the parameters can be negative, to express an unlimited value.</remarks>
        float GetAcceptableRisk(IThreatModel model, IDictionary<string, float> parameters, float infinite, int normalizationReference);

        /// <summary>
        /// Get a normalized evaluation of the current risk represented by the solution. 
        /// </summary>
        /// <param name="model">Threat Model to be analyzed.</param>
        /// <param name="normalizationReference">Normalization Reference.<para/>
        /// The Normalization Reference indicates the number of objects for which the Risk Evaluation is defined.<para/>
        /// For example, if the Normalization Reference is 40 and you have only 8 Entities and 12 Flows, then the Risk Evaluation is doubled.<para/>
        /// Analogously, if the Normalization Reference is 40 and you have 30 Entities and 50 Flows, then the Risk Evaluation is halved.<para/>
        /// If it is 0, then no normalization is applied.</param>
        /// <returns>Normalized evaluation of the current risk.
        /// <para>The returned value is normalized, that is independent from the size of the Threat Model, and allows to compare different iterations of the same Threat Model.</para>
        /// <para>The normalized evaluation is not thought to be used to compare different Threat Models.</para>
        /// </returns>
        float GetRiskEvaluation(IThreatModel model, int normalizationReference);
    }
}