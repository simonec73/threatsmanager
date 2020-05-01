namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Level of mitigations applied to the Threat.
    /// </summary>
    public enum MitigationLevel
    {
        /// <summary>
        /// The Threat is not mitigated.
        /// </summary>
        [EnumLabel("Not Mitigated")]
        [EnumDescription("The Threat has not been mitigated.")]
        NotMitigated,

        /// <summary>
        /// The Threat has been partially mitigated.
        /// </summary>
        [EnumDescription("The Threat has been partially mitigated.")]
        Partial,

        /// <summary>
        /// The Threat has been completely mitigated.
        /// </summary>
        [EnumDescription("The Threat has been completely mitigated.")]
        Complete
    }
}