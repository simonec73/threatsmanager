using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by the extensions that require to show actions in the Main Ribbon.
    /// </summary>
    [ExtensionDescription("Main Ribbon Button")]
    public interface IMainRibbonExtension : IExtension
    {
        /// <summary>
        /// Event raised when there is the need to iterate between the open Panels.
        /// </summary>
        event Action<IMainRibbonExtension> IteratePanels;

        /// <summary>
        /// Event raised when there is the need to refresh the list of Panels.
        /// </summary>
        event Action<IMainRibbonExtension> RefreshPanels;

        /// <summary>
        /// Event generated when the status of the Ribbon Action changes.
        /// </summary>
        event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;

        /// <summary>
        /// Event raised to request to close all Panels created by a specific Factory.
        /// </summary>
        event Action<IPanelFactory> ClosePanels;

        /// <summary>
        /// Identifier of the object, to be used to identify it as owner of the Action.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name of the Ribbon for the Main Form that will contain the buttons defined by the Extension.
        /// </summary>
        Ribbon Ribbon { get; }

        /// <summary>
        /// Name of the Bar inside the Ribbon that should contain the buttons.
        /// </summary>
        string Bar { get; }

        /// <summary>
        /// Enumeration of the buttons to be shown in the Ribbon.
        /// </summary>
        IEnumerable<IActionDefinition> RibbonActions { get; }

        /// <summary>
        /// Ribbon Action to use to list the available forms.
        /// </summary>
        string PanelsListRibbonAction { get; }

        /// <summary>
        /// Get the list of Panels that need to be listed since the very start.
        /// </summary>
        /// <param name="model">Threat Model to use to identify the Panels.</param>
        /// <returns>Enumeration of Panels.</returns>
        /// <remarks>This list should be shown under the button identified by <see cref="PanelsListRibbonAction"/>.</remarks>
        IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model);

        /// <summary>
        /// Execute the action whose name is passed as argument.
        /// </summary>
        /// <param name="threatModel">Current Threat Model.</param>
        /// <param name="action">Action to be executed.</param>
        void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action);
    }
}