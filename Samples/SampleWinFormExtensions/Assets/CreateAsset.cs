using System.Collections.Generic;
using System;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;
using ThreatsManager.SampleWinFormExtensions.Assets;

namespace ThreatsManager.Extensions.Actions
{
    [Extension("19F18199-9B6F-4A34-A4BE-EDB8EA00FA75", "Create Asset Context Aware Action", 500, ExecutionMode.Simplified)]
    public class CreateAsset : IMainRibbonExtension
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Home;
        public string Bar => "Assets";

        public IEnumerable<IActionDefinition> RibbonActions => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "CreateAsset", "Create Asset", Icons.Resources.undefined_big,
                Icons.Resources.undefined)
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
                case "CreateAsset":
                    var dialog = new CreateAssetDialog(threatModel);
                    dialog.Show(Form.ActiveForm);

                    break;
            }
        }
    }
}
