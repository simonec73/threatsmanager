using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface representing the relationship between Threat Events and Mitigations.
    /// </summary>
    public interface IThreatEventMitigation : IThreatModelChild, IThreatEventChild, 
        IPropertiesContainer, ISourceInfo //, ILockable
    {
        /// <summary>
        /// Identifier of the Mitigation.
        /// </summary>
        Guid MitigationId { get; }

        /// <summary>
        /// Mitigation.
        /// </summary>
        IMitigation Mitigation { get; }

        /// <summary>
        /// Identifier of the Entity used as a reference for getting Name and Description of the Threat Event Mitigation.
        /// </summary>
        /// <remarks>If defined, it will force the Threat Event Mitigation to show a specialized Name and Description.</remarks>
        Guid ReferenceId { get; set; }

        /// <summary>
        /// Name to use for the Threat Event Mitigation.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the Threat Event Mitigation.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Specific directives.
        /// </summary>
        string Directives { get; set; }

        /// <summary>
        /// Identifier of the Strength.
        /// </summary>
        int StrengthId { get; }

        /// <summary>
        /// Strength of the Mitigation.
        /// </summary>
        /// <remarks>It represents how much strength has the Mitigation over the Threat Event.</remarks>
        IStrength Strength { get; set; }

        /// <summary>
        /// Status of the Mitigation.
        /// </summary>
        MitigationStatus Status { get; set; }

        /// <summary>
        /// Creates a duplicate of the current Mitigation and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Mitigation.</returns>
        IThreatEventMitigation Clone(IThreatEventMitigationsContainer container);
    }
}