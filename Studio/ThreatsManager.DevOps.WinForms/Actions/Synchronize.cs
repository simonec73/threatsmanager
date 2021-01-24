using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Actions
{
#pragma warning disable CS0067
    [Extension("406AC7AC-FBDA-48CC-96C7-F15738083D23", "Synchronize DevOps status", 65, ExecutionMode.Management)]
    public class Synchronize : IMainRibbonExtension, IStatusInfoProviderUpdateRequestor, IDesktopAlertAwareExtension, IAsker
    {
        private static Synchronize _instance;

        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;
        public event Action UpdateStatusInfoProviders;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Analyze;
        public string Bar => "DevOps";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public event Action<IAsker, object, string, string, bool, RequestOptions> Ask;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "Synchronize", "Synchronize", Properties.Resources.cloud_updown_big,
                Properties.Resources.cloud_updown, false),
        };

        public string PanelsListRibbonAction => null;

        public Synchronize()
        {
            _instance = this;
        }

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
                    case "Synchronize":
                        Ask?.Invoke(this, threatModel, "Confirm synchronization", "Do you confirm synchronization with the selected DevOps system?",
                            false, RequestOptions.YesNo);
                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Synchronization failed.");
                throw;
            }
        }

        public void Answer(object context, AnswerType answer)
        {
            if (answer == AnswerType.Yes && context is IThreatModel model)
            {
                DevOpsManager.UpdateAsync(model);
            }
        }

        internal static void ChangeSynchronizeButtonStatus(bool status)
        {
            _instance?.ChangeRibbonActionStatus?.Invoke(_instance, "Synchronize", status);
        }
    }
}