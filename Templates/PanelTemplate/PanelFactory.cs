using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace $rootnamespace$
{
    /// <summary>
    /// $safeitemname$ is used to create Panels.
    /// </summary>
    /// <remarks>Panel Factories are necessary, because they allow to create multiple instances.</remarks>
    // TODO: Change Label, Priority and ExecutionMode. 
    [Extension(typeof(IContextAwareAction), "$guid1$", 
        "$itemname$ Context Aware Action", 100, ExecutionMode.Simplified)]
	public class $safeitemrootname$ : IPanelFactory<Form>, IMainRibbonExtension, IPanelFactoryActionsRequestor
	{
        #region IPanelFactory implementation.
        // TODO: choose the correct InstanceMode.
        public InstanceMode Behavior => InstanceMode.Single;

        public IPanel<Form> Create(IIdentity identity, out IActionDefinition action)
        {
            var result = new $chosenname$();

            action = result.ActionDefinition;

            return result;
        }

        public IPanel<Form> Create(IActionDefinition action)
        {
            return new $chosenname$();
        }
        #endregion

        #region IPanelFactoryActionsRequestor implementation.
        public event Action<IPanelFactory, IIdentity> PanelCreationRequired;
                        
        public event Action<IPanelFactory, IPanel> PanelDeletionRequired;
                        
        public event Action<IPanelFactory, IPanel> PanelShowRequired;
        #endregion

        #region IMainRibbonExtension implementation.
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        // TODO: select the right Ribbon.
        public Ribbon Ribbon => Ribbon.Home;
        // TODO: select the Bar to contain the button.
        public string Bar => "Bar";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            // TODO: fill the fourth parameter with the big icon (64x64 pixels) and the fifth with a smaller icon (32x32 pixels)
            new ActionDefinition(Id, "CreatePanel", "$chosenname$", null, null)
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
        #endregion
	}
}
