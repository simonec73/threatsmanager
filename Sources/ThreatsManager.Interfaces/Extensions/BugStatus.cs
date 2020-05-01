namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Status of the Bug.
    /// </summary>
    public enum BugStatus
    {
        /// <summary>
        /// The status of the Bug is unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// The Bug has been created.
        /// </summary>
        Created,
        /// <summary>
        /// Bug fixing has started.
        /// </summary>
        [EnumLabel("In Progress")]
        InProgress,
        /// <summary>
        /// Bug has been fixed.
        /// </summary>
        Fixed,
        /// <summary>
        /// Bug has been removed.
        /// </summary>
        Removed
    }
}