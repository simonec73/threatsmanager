namespace ThreatsManager.DevOps
{
    public class WorkItemSummary
    {
        public WorkItemSummary(int id, WorkItemStatus status, string assignedTo)
        {
            Id = id;
            Status = status;
            AssignedTo = assignedTo;
        }

        public int Id { get; }

        public WorkItemStatus Status { get; }

        public string AssignedTo { get; }
    }
}
