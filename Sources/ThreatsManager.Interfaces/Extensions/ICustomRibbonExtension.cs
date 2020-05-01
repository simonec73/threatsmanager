using System;
using System.Collections.Generic;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by the extensions that require to show actions in the Windows-specific Ribbon.
    /// </summary>
    public interface ICustomRibbonExtension
    {
        /// <summary>
        /// Identifier of the object, to be used to identify it as owner of the Action.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Label for the custom Tab.
        /// </summary>
        string TabLabel { get; }

        /// <summary>
        /// Definition of the various Commands Bar to be added.
        /// </summary>
        IEnumerable<ICommandsBarDefinition> CommandBars { get; }

        /// <summary>
        /// Execute the action whose name is passed as argument.
        /// </summary>
        /// <param name="action">Action to be executed.</param>
        void ExecuteCustomAction(IActionDefinition action);

        /// <summary>
        /// Event generated when the status of the Ribbon Action changes.
        /// </summary>
        event Action<string, bool> ChangeCustomActionStatus;
    }
}