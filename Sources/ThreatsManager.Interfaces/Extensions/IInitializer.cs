using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by the Extensions that are used to initialize the Threat Model.
    /// </summary>
    /// <remarks>This can be used to create Property Schemas, pre-populate the Threat Model or other tasks.
    /// <para>Initializers are called as a result of the creation of a new Threat Model.</para></remarks>
    [ExtensionDescription("Initializer")]
    public interface IInitializer : IExtension
    {
        /// <summary>
        /// Initialize the Threat Model.
        /// </summary>
        /// <param name="model">Threat Model to be initialized.</param>
        void Initialize(IThreatModel model);
    }
}