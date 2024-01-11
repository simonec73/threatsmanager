using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface representing the relationship between Threat Types and Weaknesses.
    /// </summary>
    public interface IThreatTypeWeakness : IThreatModelChild, IPropertiesContainer, ISourceInfo
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
        /// Identifier of the Weakness.
        /// </summary>
        Guid WeaknessId { get; }

        /// <summary>
        /// Weakness.
        /// </summary>
        IWeakness Weakness { get; }

        /// <summary>
        /// Creates a duplicate of the current Weakness and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Weakness.</returns>
        IThreatTypeWeakness Clone(IThreatTypeWeaknessesContainer container);
    }
}