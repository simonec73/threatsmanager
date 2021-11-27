namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Base interface representing a Configuration Panel Factory, that is a creator of Panels to extend the Configuration interface.
    /// </summary>
    [ExtensionDescription("Configuration Panel")]
    public interface IConfigurationPanelFactory : IExtension
    {
    }

    /// <summary>
    /// Interface representing a Configuration Panel Factory, that is a creator of Panels to extend the Configuration interface.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    public interface IConfigurationPanelFactory<T> : IConfigurationPanelFactory
    {
        /// <summary>
        /// Method to generate a new instance of the IConfigurationPanel.
        /// </summary>
        /// <returns>New instance of IPanel.</returns>
        IConfigurationPanel<T> Create();
    }
}
