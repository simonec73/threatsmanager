namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Effectiveness of the mitigation.
    /// </summary>
    public enum Effectiveness
    {
        /// <summary>
        /// Effectiveness has not been evaluated.
        /// </summary>
        Unknown,
        /// <summary>
        /// Identifies one of the less effective mitigations.
        /// </summary>
        Minor,
        /// <summary>
        /// Identifies a mitigation with average effectiveness.
        /// </summary>
        Average,
        /// <summary>
        /// Identifies a mitigation with top effectiveness.
        /// </summary>
        Major
    }
}