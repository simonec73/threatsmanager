namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Information about a Task.
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="id">Identified of the Task.</param>
        /// <param name="status">Status of the Task.</param>
        public TaskInfo(int id, TaskStatus status)
        {
            Id = id;
            Status = status;
        }

        /// <summary>
        /// Identifier of the Task.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Status of the Task.
        /// </summary>
        public TaskStatus Status { get; private set; }
    }
}