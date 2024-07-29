namespace ThreatsManager.ImportersExporters.Importers.Excel
{
    enum HitPolicy
    {
        /// <summary>
        /// Skip the object altogether.
        /// </summary>
        Skip,

        /// <summary>
        /// Replace the values with those specified in the imported file.
        /// </summary>
        Replace,

        /// <summary>
        /// Add a new object.
        /// </summary>
        Add
    }
}