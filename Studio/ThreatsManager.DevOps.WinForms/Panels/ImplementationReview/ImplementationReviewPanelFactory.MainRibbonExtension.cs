using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Panels.ImplementationReview
{
    public partial class ImplementationReviewPanelFactory
    {
        public event Action<IMainRibbonExtension> IteratePanels;
        public event Action<IMainRibbonExtension> RefreshPanels;
        public event Action<IMainRibbonExtension, string, bool> ChangeRibbonActionStatus;
        public event Action<IPanelFactory> ClosePanels;

        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        public Ribbon Ribbon => Ribbon.Analyze;
        public string Bar => "DevOps";

        public IEnumerable<IActionDefinition> RibbonActions  => new List<IActionDefinition>
        {
            new ActionDefinition(Id, "CreatePanel", "Implementations\nReview", Properties.Resources.clipboard_checks_big,
                Properties.Resources.clipboard_checks, false)
        };
        public string PanelsListRibbonAction => null;

        public IEnumerable<IActionDefinition> GetStartPanelsList(IThreatModel model)
        {
            return null;
        }

        public void ExecuteRibbonAction(IThreatModel threatModel, IActionDefinition action)
        {
        }

        internal static void ChangeConfigureButtonStatus(bool status)
        {
            var extension = ExtensionUtils.GetExtension<ImplementationReviewPanelFactory>("07FB9D18-5D0A-461A-AF3B-EE697BB25E2A");
            extension?.ChangeRibbonActionStatus?.Invoke(extension, "CreatePanel", status);
        }
    }
}
