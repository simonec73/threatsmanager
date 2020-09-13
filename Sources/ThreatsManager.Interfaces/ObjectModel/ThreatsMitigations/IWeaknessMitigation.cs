using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface representing the relationship between Weaknesses and Mitigations.
    /// </summary>
    public interface IWeaknessMitigation : IThreatModelChild, IPropertiesContainer, IDirty
    {
        /// <summary>
        /// Identifier of the Weakness.
        /// </summary>
        Guid WeaknessId { get; }

        /// <summary>
        /// Weakness.
        /// </summary>
        IWeakness Weakness { get; }

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
        /// <remarks>It represents how much strength has the Mitigation over the Weakness.</remarks>
        IStrength Strength { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Mitigation and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Mitigation.</returns>
        IWeaknessMitigation Clone(IWeaknessMitigationsContainer container);
    }
}