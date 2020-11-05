namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Information about a Work Item.
    /// </summary>
    public class WorkItemInfo
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="id">Identified of the Work Item.</param>
        /// <param name="status">Status of the Work Item.</param>
        public WorkItemInfo(int id, WorkItemStatus status)
        {
            Id = id;
            Status = status;
        }

        /// <summary>
        /// Identifier of the Work Item.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Status of the Work Item.
        /// </summary>
        public WorkItemStatus Status { get; private set; }
    }
}