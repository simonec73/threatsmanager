using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace SampleExtensions.RibbonExtensions
{
    [Extension("45636082-DB4B-4E9A-8D25-06384EFA104F",
        "Azure Scanning Context Aware Action", 100, ExecutionMode.Expert)]
    public class ScanAzureSubscription : IMainRibbonExtension, IDesktopAlertAwareExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Import;
        public string Bar => "Scanning";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "ImportAzure", "Import Azure Subscription", Properties.Resources.Microsoft_Azure,
                Properties.Resources.Microsoft_Azure)
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
                    case "ImportAzure":

                        threatModel.AddEntity<IProcess>("Azure Function");
                        threatModel.AddEntity<IProcess>("Azure ML");
                        threatModel.AddEntity<IDataStore>("Azure SQL");
                        threatModel.AddEntity<IDataStore>("Azure Storage Blob");

                        ShowMessage?.Invoke("Import Azure Subscription succeeded.");

                        break;
                }
            }
            catch
            {
                ShowWarning?.Invoke("Import Azure Subscription failed.");
            }
        }
    }
}