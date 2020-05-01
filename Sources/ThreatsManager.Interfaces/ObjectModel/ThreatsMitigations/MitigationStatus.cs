namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Status of the Mitigation.
    /// </summary>
    public enum MitigationStatus
    {
        /// <summary>
        /// The status has not been defined.
        /// </summary>
        [EnumDescription("The status has not been defined yet.")]
        Undefined,

        /// <summary>
        /// The mitigation already exists.
        /// </summary>
        [EnumDescription("The mitigation already exists.")]
        Existing,

        /// <summary>
        /// The mitigation has been proposed.
        /// </summary>
        [EnumDescription("The mitigation has been proposed.")]
        Proposed,

        /// <summary>
        /// The mitigation has been approved.
        /// </summary>
        [EnumDescription("The mitigation has been approved.")]
        Approved,

        /// <summary>
        /// The mitigation has been recently implemented.
        /// </summary>
        [EnumDescription("The mitigation has been recently implemented.")]
        Implemented,

        /// <summary>
        /// The mitigation has been already planned.
        /// </summary>
        [EnumDescription("The mitigation has been already planned.")]
        Planned
    }
}