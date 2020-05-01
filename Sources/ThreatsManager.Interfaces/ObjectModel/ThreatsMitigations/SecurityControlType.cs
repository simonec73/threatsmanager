namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Enumeration of the types of Security Controls.
    /// </summary>
    public enum SecurityControlType
    {
        /// <summary>
        /// Unknown or undefined control type.
        /// </summary>
        [EnumDescription("Unknown or undefined control type.")]
        Unknown,
        /// <summary>
        /// Preventive control, which reduces the probability or impact of the threat event,
        /// effectively gaining time to the defenders and forcing the attacker to make more noise
        /// as a result of the various attempts to overcome the security control.
        /// </summary>
        [EnumDescription("Preventive control, which reduces the probability or impact of the threat event, effectively gaining time to the defenders and forcing the attacker to make more noise as a result of the various attempts to overcome the security control.")]
        Preventive,
        /// <summary>
        /// Detective control, which allows detecting an attack while it is in progress.
        /// </summary>
        [EnumDescription("Detective control, which allows detecting an attack while it is in progress.")]
        Detective,
        /// <summary>
        /// Corrective control, which allows responding to the attack.
        /// </summary>
        [EnumDescription("Corrective control, which allows responding to the attack.")]
        Corrective,
        /// <summary>
        /// Recovery control, which allows to recover from the damages occurred from an attack.
        /// </summary>
        [EnumDescription("Recovery control, which allows to recover from the damages occurred from an attack.")]
        Recovery,
        /// <summary>
        /// Deterrent controls, which are mostly there to convince the potential attacker that the cost for her may be higher than acceptable, compared with the potential gain.
        /// </summary>
        [EnumDescription("Deterrent controls, which are mostly there to convince the potential attacker that the cost for her may be higher than acceptable, compared with the potential gain.")]
        Deterrent,
        /// <summary>
        /// Other types of Security Controls.
        /// </summary>
        [EnumDescription("Other types of Security Controls.")]
        Other
    }
}