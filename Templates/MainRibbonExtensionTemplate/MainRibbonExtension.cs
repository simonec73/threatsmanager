using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace $rootnamespace$
{
    /// <summary>
    /// $safeitemname$ is used to show a button in the Ribbon.
    /// </summary>
    // TODO: Change Label, Priority and ExecutionMode. 
    [Extension("$guid1$", "$itemname$ Main Ribbon Extension", 100, ExecutionMode.Simplified)]
    public class $safeitemname$ : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;

        // TODO: select the right Ribbon.
        public Ribbon Ribbon => Ribbon.Home;
        // TODO: select the Bar to contain the button.
        public string Bar => "Bar";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            // TODO: change the name of the action here and in ExecuteRibbonAction.
            // TODO: fill the fourth parameter with the big icon (64x64 pixels) and the fifth with a smaller icon (32x32 pixels)
            new ActionDefinition(Id, "SomeAction", "$itemname$", null, null)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            try
            {
                switch (action.Name)
                {
                    case "SomeAction":
                        // TODO: specify the execution for the action.
                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke($"{action.Name} failed.");
                throw;
            }
        }
    }
}