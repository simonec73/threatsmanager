namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Information related to an item stored in the DevOps system.
    /// </summary>
    public interface IDevOpsItemInfo
    {
        /// <summary>
        /// Identifier of the Item.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Name of the Item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Url of the Item.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Type of the Item.
        /// </summary>
        string WorkItemType { get; }

        /// <summary>
        /// Assignee of the Item.
        /// </summary>
        string AssignedTo { get; }

        /// <summary>
        /// Serialize the content of the object.
        /// </summary>
        /// <returns>Serialized object.</returns>
        string Serialize();
    }
}