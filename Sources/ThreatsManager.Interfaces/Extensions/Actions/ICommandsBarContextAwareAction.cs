namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface that is implemented by Context Aware Actions that should be shown in the Custom Ribbon.
    /// </summary>
    /// <remarks>If a ContextAwareAction implements this interface, then it must define Icon and SmallIcon.</remarks>
    public interface ICommandsBarContextAwareAction
    {
        /// <summary>
        /// Definition of how the Action should be shown in the Custom Ribbon.
        /// </summary>
        ICommandsBarDefinition CommandsBar { get; }
    }
}