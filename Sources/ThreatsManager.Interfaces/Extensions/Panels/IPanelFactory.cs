using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel Factory, that is a creator of a specific type of Panels.
    /// </summary>
    public interface IPanelFactory
    {
        /// <summary>
        /// Behavior of the Panel Factory.
        /// </summary>
        InstanceMode Behavior { get; }

        /// <summary>
        /// Method to generate a new instance of IPanel.
        /// </summary>
        /// <param name="identity">Reference identity for the new instance. This parameter can be null.</param>
        /// <param name="action">IActionDefinition associated to the new instance of IPanel.</param>
        /// <returns>New instance of IPanel.</returns>
        IPanel Create(IIdentity identity, out IActionDefinition action);

        /// <summary>
        /// Method to generate a new instance of IPanel, based on the action passed as argument.
        /// </summary>
        /// <param name="action">Action to be used as a reference to create the panel.</param>
        /// <returns>New instance of IPanel.</returns>
        /// <remarks>This method is typically executed to open an instance that has already been defined.</remarks>
        IPanel Create(IActionDefinition action);
    }
}