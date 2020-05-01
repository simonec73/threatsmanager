namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Enumeration of the Default Strengths.
    /// </summary>
    public enum DefaultStrength
    {
        /// <summary>
        /// Strength is negligible.
        /// </summary>
        [EnumDescription("Negligible strength. It has no measurable impact on mitigating the Threat.")]
        Negligible = 0,

        /// <summary>
        /// Strength is low.
        /// </summary>
        [EnumDescription("Low strength. It has some importance for mitigating the Threat.")]
        Weak = 25,

        /// <summary>
        /// Strength is average.
        /// </summary>
        [EnumDescription("Strength is average. It significantly mitigates the Threat, but probably not enough.")]
        Average = 50,

        /// <summary>
        /// Strength is important.
        /// </summary>
        [EnumDescription("Strength is important, but it may not be enough to address the Threat alone.")]
        Strong = 75,

        /// <summary>
        /// Strength is maximum: it solves the threat.
        /// </summary>
        [EnumDescription("Strength is maximum: it may be enough to mitigate the Threat.")]
        Maximum = 100
    }
}