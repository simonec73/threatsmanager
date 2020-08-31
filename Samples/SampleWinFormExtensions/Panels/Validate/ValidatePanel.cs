using System;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace SampleWinFormExtensions.Panels.Validate
{
    public partial class ValidatePanel : UserControl, IShowThreatModelPanel<Form>
    {
        private readonly Guid _id = Guid.NewGuid();

        public ValidatePanel()
        {
            InitializeComponent();
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetThreatModel(IThreatModel model)
        {
            if (model != null)
            {
                _chart.Series[0].Points[0].YValues[0] = model.Entities?.OfType<IExternalInteractor>()
                             .Count(x => x.ThreatEvents?.Any() ?? false) ?? 0;
                _chart.Series[0].Points[1].YValues[0] = model.Entities?.OfType<IExternalInteractor>()
                             .Count(x => !(x.ThreatEvents?.Any() ?? false)) ?? 0;
                _chart.Series[0].Points[2].YValues[0] = model.Entities?.OfType<IProcess>()
                             .Count(x => x.ThreatEvents?.Any() ?? false) ?? 0;
                _chart.Series[0].Points[3].YValues[0] = model.Entities?.OfType<IProcess>()
                             .Count(x => !(x.ThreatEvents?.Any() ?? false)) ?? 0;;
                _chart.Series[0].Points[4].YValues[0] = model.Entities?.OfType<IDataStore>()
                             .Count(x => x.ThreatEvents?.Any() ?? false) ?? 0;
                _chart.Series[0].Points[5].YValues[0] = model.Entities?.OfType<IDataStore>()
                             .Count(x => !(x.ThreatEvents?.Any() ?? false)) ?? 0;
                _chart.Series[0].Points[6].YValues[0] = model.DataFlows?.Count(x => x.ThreatEvents?.Any() ?? false) ?? 0;
                _chart.Series[0].Points[7].YValues[0] = model.DataFlows?.Count(x => !(x.ThreatEvents?.Any() ?? false)) ?? 0;

                _chart.Series[1].Points[0].YValues[0] = model.Entities?
                             .Count(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any() ?? false) ?? false) ?? 0 +
                         model.DataFlows?
                             .Count(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any() ?? false) ?? false) ?? 0;
                _chart.Series[1].Points[1].YValues[0] = model.Entities?
                             .Count(x => x.ThreatEvents?.Any(y => !(y.Mitigations?.Any() ?? false)) ?? false) ?? 0 +
                         model.DataFlows?
                             .Count(x => x.ThreatEvents?.Any(y => !(y.Mitigations?.Any() ?? false)) ?? false) ?? 0;
            }
        }
        #endregion

        public IActionDefinition ActionDefinition => new ActionDefinition(Guid.NewGuid(), "Validate", "Validate", 
            Properties.Resources.wax_seal_big, Properties.Resources.wax_seal);
    }
}
