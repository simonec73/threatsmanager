using System;
using System.Collections.Generic;
using ThreatsManager.AutoThreatGeneration.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
#pragma warning disable 0067
    [Extension("E8AA1321-0EB9-45D2-BC96-F15BDEA867BE", "Apply Automatic Threat Generation Rules", 41, ExecutionMode.Simplified)]
    public class ApplyAutoGenRules : IMainRibbonExtension, IDesktopAlertAwareExtension, IAsker
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;
        public event Action<IAsker, object, string, string, bool, RequestOptions> Ask;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Analyze;
        public string Bar => "Auto Generation";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ApplyAutoGenRules", "Generate Threat Events && Mitigations", Properties.Resources.industrial_robot_big,
                Properties.Resources.industrial_robot)
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
                    case "ApplyAutoGenRules":
                        if (threatModel.HasTop())
                        {
                            Ask?.Invoke(this, threatModel, Resources.ApplyAutoGenRules_Caption, 
                                $"Do you want to generate all Threat Events and Mitigations?\nPress Yes to confirm.\nPress No to generate only the Top Threats and Mitigations.\nPress Cancel to avoid generating anything.",
                                false, RequestOptions.YesNoCancel);
                        }
                        else
                        {
                            Ask?.Invoke(this, threatModel, Resources.ApplyAutoGenRules_Caption, Resources.ApplyAutoGenRules_Confirm, 
                                false, RequestOptions.OkCancel);
                        }
                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Automatic Threat Event generation failed.\nPlease close the document without saving it.");
                throw;
            }
        }

        public void Answer(object context, AnswerType answer)
        {
            if ((answer == AnswerType.Yes || answer == AnswerType.Ok || answer == AnswerType.No) && context is IThreatModel threatModel)
            {
                if (threatModel.GenerateThreatEvents(answer == AnswerType.No))
                    ShowMessage?.Invoke("Threat Events generated successfully.");
                else
                {
                    ShowWarning?.Invoke("No Threat Event or Mitigation has been generated.");
                }
            }
        }

    }
}