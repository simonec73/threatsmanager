using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Extension called before the Threat Model is saved.
    /// </summary>
    [ExtensionDescription("Before save processor")]
    public interface IBeforeSaveProcessor : IExtension
    {
        /// <summary>
        /// Method called before saving the Threat Model.
        /// </summary>
        /// <param name="model">Threat Model to be saved.</param>
        void Execute(IThreatModel model);
    }
}
