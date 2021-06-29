using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface that defines a Weakness, that is the definition of a issue from which Vulnerabilities can be derived.
    /// </summary>
    public interface IWeakness : IIdentity, IThreatModelChild, IPropertiesContainer, 
        IWeaknessMitigationsContainer, IDirty
    {
        /// <summary>
        /// Identifier of the Severity.
        /// </summary>
        int SeverityId { get; }

        /// <summary>
        /// Severity of the Threat Type.
        /// </summary>
        ISeverity Severity { get; set; }

        /// <summary>
        /// Generates a Vulnerability from the Weakness.
        /// </summary>
        /// <param name="identity">Reference identity to which the Vulnerability belongs.</param>
        /// <returns>Vulnerability generated from the current Weakness.</returns>
        IVulnerability GenerateVulnerability(IIdentity identity);

        /// <summary>
        /// Get the Mitigation Level for the Weakness.
        /// </summary>
        /// <returns>Evaluation of the Mitigation Level.</returns>
        MitigationLevel GetMitigationLevel();

        /// <summary>
        /// Creates a duplicate of the current Weakness and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Weakness.</returns>
        IWeakness Clone(IWeaknessesContainer container);
    }
}