namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Enumeration of the Data Flow Types.
    /// </summary>
    public enum FlowType
    {
        /// <summary>
        /// The Source sends a command to the Target, and as a result data is read by the Source from the Target,
        /// and optionally written to the Target itself.
        /// </summary>
        /// <remarks>This is the most common type.</remarks>
        [EnumLabel("Read-Write/Command")]
        ReadWriteCommand,

        /// <summary>
        /// The Source reads data from the Target.
        /// </summary>
        /// <remarks>This is the typical scenario that occurs when the Target broadcasts or multi-cast data.</remarks>
        [EnumLabel("Read only")]
        Read,

        /// <summary>
        /// The Source sends a command to the Target and/or writes data to it.
        /// </summary>
        /// <remarks>This is the typical scenario that occurs when the Source
        /// sends a "fire and forget" request to the Target.
        /// It is also used when Pushing data.</remarks>
        [EnumLabel("Write/Command only")]
        WriteCommand
    }
}