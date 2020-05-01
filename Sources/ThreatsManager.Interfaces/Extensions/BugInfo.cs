namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Information about a Bug.
    /// </summary>
    public class BugInfo
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="id">Identified of the Bug.</param>
        /// <param name="status">Status of the Bug.</param>
        public BugInfo(int id, BugStatus status)
        {
            Id = id;
            Status = status;
        }

        /// <summary>
        /// Identifier of the Bug.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Status of the Bug.
        /// </summary>
        public BugStatus Status { get; private set; }
    }
}