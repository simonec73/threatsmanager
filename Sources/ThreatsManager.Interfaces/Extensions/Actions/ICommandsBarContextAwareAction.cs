using System.Collections.Generic;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface that is implemented by Context Aware Actions that should be shown in the Custom Ribbon.
    /// </summary>
    /// <remarks>If a ContextAwareAction implements this interface, then it must define Icon and SmallIcon.</remarks>
    public interface ICommandsBarContextAwareAction
    {
        /// <summary>
        /// Specifies the supported contexts.
        /// </summary>
        /// <remarks>If not configured, the action is visible for all contexts but those specified with <see cref="UnsupportedContexts"/>.<para/>
        /// If configured, then it is visible only for the specific contexts, with the exceptions of those specified in <see cref="UnsupportedContexts"/>.<para/>
        /// For example, the Diagram panel is identified by "Diagram", while the Roadmap panel by "Roadmap".<para/>
        /// If a context is included in <see cref="SupportedContexts"/> and here, it is considered as unsupported.</remarks>
        IEnumerable<string> SupportedContexts { get; }

        /// <summary>
        /// Specifies the unsupported contexts.
        /// </summary>
        /// <remarks>If not configured, the action is visible for all contexts specified with <see cref="SupportedContexts"/>.<para/>
        /// If configured, then it is visible for those contexts specified with <see cref="SupportedContexts"/>, with the exception of those specified here.<para/>
        /// For example, the Diagram panel is identified by "Diagram", while the Roadmap panel by "Roadmap".<para/>
        /// If a context is included in <see cref="SupportedContexts"/> and here, it is considered as unsupported.</remarks>
        IEnumerable<string> UnsupportedContexts { get; }

        /// <summary>
        /// Definition of how the Action should be shown in the Custom Ribbon.
        /// </summary>
        ICommandsBarDefinition CommandsBar { get; }
    }
}