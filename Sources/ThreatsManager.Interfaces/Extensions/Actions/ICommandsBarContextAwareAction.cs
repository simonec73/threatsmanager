namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface that is implemented by Context Aware Actions that should be shown in the Custom Ribbon.
    /// </summary>
    /// <remarks>If a ContextAwareAction implements this interface, then it must define Icon and SmallIcon.</remarks>
    public interface ICommandsBarContextAwareAction
    {
        /// <summary>
        /// Specifies the visibility context.
        /// </summary>
        /// <remarks>If not configured, the action is visible everywhere. 
        /// If configured, then it is visible only for the specific context.
        /// For example, the Diagram panel is identified by "Diagram", while the Roadmap panel by "Roadmap".</remarks>
        string VisibilityContext { get; }

        /// <summary>
        /// Definition of how the Action should be shown in the Custom Ribbon.
        /// </summary>
        ICommandsBarDefinition CommandsBar { get; }
    }
}