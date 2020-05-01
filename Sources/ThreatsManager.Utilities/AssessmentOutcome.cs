using ThreatsManager.Interfaces;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Outcomes of the Assessment.
    /// </summary>
    public enum AssessmentOutcome
    {
        /// <summary>
        /// Good health.
        /// </summary>
        [EnumLabel("Good Health")]
        Good,
        /// <summary>
        /// Weak health.
        /// </summary>
        [EnumLabel("Weak Health")]
        Weak,
        /// <summary>
        /// Poor health.
        /// </summary>
        [EnumLabel("Poor Health")]
        Poor
    }
}