using System;
using System.Collections.Generic;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace SampleWinFormExtensions.Panels.Definitions
{
    public partial class DefinitionsPanelFactory
    {
        public event Action<IPanelFactory, IIdentity> PanelCreationRequired;
        public event Action<IPanelFactory, IPanel> PanelDeletionRequired;
        public event Action<IPanelFactory, IPanel> PanelShowRequired;

        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        public event Action<IMainRibbonExtension> IteratePanels;
        
        public event Action<IMainRibbonExtension> RefreshPanels;
        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.View;
        public string Bar => "Other";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "CreatePanel", "Definitions", Properties.Resources.book_open_big,
                Properties.Resources.book_open)
        };

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
            switch (action.Name)
            {
                case "CreatePanel":
                    PanelCreationRequired?.Invoke(this, action.Tag as IIdentity);
                    break;
            }
        }
    }
}