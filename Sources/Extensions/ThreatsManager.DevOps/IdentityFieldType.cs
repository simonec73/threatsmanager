namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Type of the Identity Field.
    /// </summary>
    public enum IdentityFieldType
    {
        /// <summary>
        /// Identifier of the Identity.
        /// </summary>
        Id,

        /// <summary>
        /// Name of the Identity.
        /// </summary>
        Name,

        /// <summary>
        /// Description of the Identity.
        /// </summary>
        /// <remarks>For Mitigations, it includes the Directives for the associated Directives.</remarks>
        Description,

        /// <summary>
        /// Priority, retrieved from the Roadmap.
        /// </summary>
        Priority,

        /// <summary>
        /// A Property Type.
        /// </summary>
        Property
    }
}