namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Interface implemented by objects stored in IPropertyJsonSerializableObject, which allows to merge the objects during import.
    /// </summary>
    public interface IMergeable<T> : IMergeable where T : IMergeable
    {
        /// <summary>
        /// Merges toBeMerged with the current object.
        /// </summary>
        /// <param name="toBeMerged">Object to be merged.</param>
        void Merge(T toBeMerged);
    }

    /// <summary>
    /// Mergeable object.
    /// </summary>
    public interface IMergeable
    {
        /// <summary>
        /// Merges toBeMerged with the current object.
        /// </summary>
        /// <param name="toBeMerged">Object to be merged.</param>
        void Merge(IMergeable toBeMerged);
    }
}
