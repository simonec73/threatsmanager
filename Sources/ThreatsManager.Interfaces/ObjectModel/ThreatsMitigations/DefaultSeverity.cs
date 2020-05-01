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
        [EnumDescription("Informational only. It is related to mitigated issues or to topics that are included for completeness.")]
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
        [EnumDescription("Low Severity. It is related to minor issues.")]
        [BackColor(KnownColor.Yellow)]
        [TextColor(KnownColor.Black)]
        Low = 25,
        /// <summary>
        /// Medium Severity.
        /// </summary>
        [EnumDescription("Medium Severity. It is related to issues that should represent some concern and thus should not be overlooked.")]
        [BackColor(KnownColor.Orange)]
        [TextColor(KnownColor.Black)]
        Medium = 50,
        /// <summary>
        /// High Severity.
        /// </summary>
        [EnumDescription("High Severity. It is related to important issues that may cause major disruption and thus should be seriously considered.")]
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
        [EnumDescription("Critical Severity. It is related to the most important issues, which could cause a catastrophic failure of the solution and critical damage to the involved counterparts.")]
        [BackColor(KnownColor.Black)]
        [TextColor(KnownColor.White)]
        Critical = 100
    }
}