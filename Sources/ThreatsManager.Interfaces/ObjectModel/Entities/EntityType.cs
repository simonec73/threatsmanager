namespace ThreatsManager.Interfaces.ObjectModel.Entities
{
    /// <summary>
    /// Type of the Entity.
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// External Interactor.
        /// </summary>
        [EnumLabel("External Interactor")]
        ExternalInteractor,

        /// <summary>
        /// Process.
        /// </summary>
        Process,

        /// <summary>
        /// Data Store.
        /// </summary>
        [EnumLabel("Data Store")]
        DataStore
    }
}