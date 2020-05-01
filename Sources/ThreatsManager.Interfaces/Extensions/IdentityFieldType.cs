namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Type of the Identity Field.
    /// </summary>
    public enum IdentityFieldType
    {
        /// <summary>
        /// Identifier of the Identity.
        /// </summary>
        Id,

        /// <summary>
        /// Name of the Identity.
        /// </summary>
        Name,

        /// <summary>
        /// Description of the Identity.
        /// </summary>
        /// <remarks>For Threat Event Mitigations, it includes the Directives.</remarks>
        Description,

        /// <summary>
        /// Priority, automatically calculated Threats or Mitigations.
        /// </summary>
        Priority,

        /// <summary>
        /// Severity, as defined for Threats.
        /// </summary>
        Severity,

        /// <summary>
        /// Indicates where the Threat or Mitigation applies.
        /// </summary>
        AppliesTo,

        /// <summary>
        /// A Property Type.
        /// </summary>
        Property
    }
}