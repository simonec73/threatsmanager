using System.Drawing;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Enumeration of the Default Severities.
    /// </summary>
    public enum DefaultSeverity
    {
        /// <summary>
        /// Undefined Severity.
        /// </summary>
        [EnumDescription("Unknown or unassigned severity.")]
        Unknown = 0,
        /// <summary>
        /// Informative Severity.
        /// </summary>
        [EnumDescription("Informational only. It is related to mitigated issues that have a negligible probability of causing even a minor damage to the solution and the involved counterparts.")]
        [BackColor(KnownColor.Green)]
        [TextColor(KnownColor.White)]
        Info = 1,
        /// <summary>
        /// Very Low Severity.
        /// </summary>
        [UiHidden]
        [EnumDescription("Very Low Severity. It is normally hidden, and is used to support importing Threats from CAPEC and other sources.")]
        [BackColor(KnownColor.GreenYellow)]
        [TextColor(KnownColor.Black)]
        VeryLow = 10,
        /// <summary>
        /// Low Severity.
        /// </summary>
        [EnumDescription("Low Severity. Low-level threats are related to minor issues, both for their probability and potential impact.")]
        [BackColor(KnownColor.Yellow)]
        [TextColor(KnownColor.Black)]
        Low = 25,
        /// <summary>
        /// Medium Severity.
        /// </summary>
        [EnumDescription("Medium Severity. Medium-level threats are related to average issues, both for their probability and potential impact.")]
        [BackColor(KnownColor.Orange)]
        [TextColor(KnownColor.Black)]
        Medium = 50,
        /// <summary>
        /// High Severity.
        /// </summary>
        [EnumDescription("High Severity. High-level threats are related to important issues both for their probability and potential impact. They have a significant probability of causing major damage to the solution and the involved counterparts.")]
        [BackColor(KnownColor.Red)]
        [TextColor(KnownColor.White)]
        High = 75,
        /// <summary>
        /// Very High Severity.
        /// </summary>
        [UiHidden]
        [EnumDescription("Very High Severity. It is normally hidden, and is used to support importing Threats from CAPEC and other sources.")]
        [BackColor(KnownColor.DarkRed)]
        [TextColor(KnownColor.White)]
        VeryHigh = 90,
        /// <summary>
        /// Critical Severity.
        /// </summary>
        [EnumDescription("Critical Severity. Critical-level threats are related to the most important issues, both for the probability and potential impact. Critical threats have a high probability of causing a catastrophic failure of the solution and critical damage to the involved counterparts.")]
        [BackColor(KnownColor.Black)]
        [TextColor(KnownColor.White)]
        Critical = 100
    }
}