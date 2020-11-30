namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Definition of a Field for the DevOps system.
    /// </summary>
    public interface IDevOpsField
    {
        /// <summary>
        /// Identifier of the field.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Label of the field.
        /// </summary>
        string Label { get; set; }
    }
}