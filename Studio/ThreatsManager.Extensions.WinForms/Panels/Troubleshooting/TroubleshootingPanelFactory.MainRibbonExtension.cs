using System;
using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Extensions.Panels.Troubleshooting
{
#pragma warning disable CS0067
    public partial class TroubleshootingPanelFactory
    {
        
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        
        public event Action<IPanelFactory, IIdentity> PanelCreationRequired;
        
        public event Action<IPanelFactory, IPanel> PanelDeletionRequired;
        
        public event Action<IPanelFactory, IPanel> PanelShowRequired;
        
        public event Action<IMainRibbonExtension> IteratePanels;
        
        public event Action<IMainRibbonExtension> RefreshPanels;

        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Help;
        public string Bar => "Guidance";

        public IEnumerable<IActionDefinition> RibbonActions
        {
            get
            {
                IEnumerable<IActionDefinition> result = null;
                
                var config = ThreatsManager.Engine.ExtensionsConfigurationManager.GetConfigurationSection();

                if (!(config?.DisableHelp ?? false))
                {
                    result = new List<IActionDefinition>
                    {
                        new ActionDefinition(Id, "CreatePanel", "Troubleshooting", Properties.Resources.lifebelt_big,
                            Properties.Resources.lifebelt)
                    };
                }

                return result;
            }
        }

        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList([NotNull] IThreatModel model)
        {
            return null;
        }

        [InitializationRequired]
        public void ExecuteRibbonAction(IThreatModel threatModel, [NotNull] IActionDefinition action)
        {
            switch (action.Name)
            {
                case "CreatePanel":
                    PanelCreationRequired?.Invoke(this, null);
                    break;
            }
        }
    }
}