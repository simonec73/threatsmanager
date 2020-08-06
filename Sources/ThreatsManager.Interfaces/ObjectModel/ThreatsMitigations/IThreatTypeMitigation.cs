using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface representing the relationship between Threat Types and Mitigations.
    /// </summary>
    public interface IThreatTypeMitigation : IThreatModelChild, IPropertiesContainer, IDirty
    {
        /// <summary>
        /// Identifier of the Threat Type.
        /// </summary>
        Guid ThreatTypeId { get; }

        /// <summary>
        /// Threat Type.
        /// </summary>
        IThreatType ThreatType { get; }

        /// <summary>
        /// Identifier of the Mitigation.
        /// </summary>
        Guid MitigationId { get; }

        /// <summary>
        /// Mitigation.
        /// </summary>
        IMitigation Mitigation { get; }

        /// <summary>
        /// Identifier of the Strength.
        /// </summary>
        int StrengthId { get; }

        /// <summary>
        /// Strength of the Mitigation.
        /// </summary>
        /// <remarks>It represents how much strength has the Mitigation over the Threat Type.</remarks>
        IStrength Strength { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Mitigation and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Mitigation.</returns>
        IThreatTypeMitigation Clone(IThreatTypeMitigationsContainer container);
    }
}