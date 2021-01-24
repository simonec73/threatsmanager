using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.AutoThreatGeneration.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.AutoThreatGeneration.Actions
{
#pragma warning disable CS0067
    [Extension("5B01F8FA-011F-4A71-84E0-690EC8D560E9", "Remove all Threat Events", 45, ExecutionMode.Simplified)]
    public class RemoveAllThreatEvents : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Analyze;
        public string Bar => "Auto Generation";

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "RemoveThreatEvents", "Remove All Threat Events", 
                Icons.Resources.threat_events_big_delete,
                Icons.Resources.threat_events_delete)
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
                    case "RemoveThreatEvents":
                        if (MessageBox.Show(Form.ActiveForm,
                                Resources.RemoveAllThreatEvents_Confirm,
                                Resources.RemoveAllThreatEvents_Caption, MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                        {
                            Do(threatModel);

                            ShowMessage?.Invoke("Threat Events removed successfully.");
                        }

                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Threat Events removal failed.\nPlease close the document without saving it.");
                throw;
            }
        }

        private void Do([NotNull] IThreatModel threatModel)
        {
            RemoveThreatEvents(threatModel);

            var entities = threatModel.Entities?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    RemoveThreatEvents(entity);
                }
            }

            var flows = threatModel.DataFlows?.ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    RemoveThreatEvents(flow);
                }
            }
        }

        private void RemoveThreatEvents([NotNull] IThreatEventsContainer container)
        {
            var tes = container.ThreatEvents?.Select(x => x.Id).ToArray();
            if (tes?.Any() ?? false)
            {
                foreach (var te in tes)
                {
                    container.RemoveThreatEvent(te);
                }
            }
        }
    }
}