namespace ThreatsManager.DevOps
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
        /// <param name="assignedTo">Name of the assignee of the Work Item.</param>
        /// <param name="status">Status of the Work Item.</param>
        public WorkItemInfo(int id, string url, string assignedTo, WorkItemStatus status)
        {
            Id = id;
            Url = url;
            AssignedTo = assignedTo;
            Status = status;
        }

        /// <summary>
        /// Identifier of the Work Item.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Url of the Work Item.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Status of the Work Item.
        /// </summary>
        public WorkItemStatus Status { get; private set; }

        /// <summary>
        /// Name of the assignee of the activity.
        /// </summary>
        public string AssignedTo { get; private set; }
    }
}