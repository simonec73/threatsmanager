namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Extensions Initializers are extensions that must be called to load and initialize the Extension.
    /// </summary>
    /// <remarks>They differentiate from the Initializers, which implement interface <see cref="IInitializer"/> because they are
    /// executed when Threats Manager Studio is loaded, not when a new Threat Model is created.
    /// </remarks>
    [ExtensionDescription("Extension Initializer")]
    public interface IExtensionInitializer : IExtension
    {
        /// <summary>
        /// Initialize the Extension.
        /// </summary>
        void Initialize();
    }
}