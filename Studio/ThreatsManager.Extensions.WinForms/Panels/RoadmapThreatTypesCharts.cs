using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Roadmap;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels
{
    public partial class RoadmapThreatTypesCharts : UserControl
    {
        private RoadmapStatus _currentStatus = RoadmapStatus.NotAssessed;
        private IThreatModel _model;

        public RoadmapThreatTypesCharts()
        {
            InitializeComponent();
        }

        public void Initialize([NotNull] IThreatModel model)
        {
            _model = model;
            ShowRoadmapStatus(RoadmapStatus.ShortTerm);
        }

        private void _previous_Click(object sender, EventArgs e)
        {
            switch (_currentStatus)
            {
                case RoadmapStatus.ShortTerm:
                    ShowRoadmapStatus(RoadmapStatus.LongTerm);
                    break;
                case RoadmapStatus.MidTerm:
                    ShowRoadmapStatus(RoadmapStatus.ShortTerm);
                    break;
                case RoadmapStatus.LongTerm:
                    ShowRoadmapStatus(RoadmapStatus.MidTerm);
                    break;
            }
        }

        private void _next_Click(object sender, EventArgs e)
        {
            switch (_currentStatus)
            {
                case RoadmapStatus.ShortTerm:
                    ShowRoadmapStatus(RoadmapStatus.MidTerm);
                    break;
                case RoadmapStatus.MidTerm:
                    ShowRoadmapStatus(RoadmapStatus.LongTerm);
                    break;
                case RoadmapStatus.LongTerm:
                    ShowRoadmapStatus(RoadmapStatus.ShortTerm);
                    break;
            }
        }

        private void ShowRoadmapStatus(RoadmapStatus status)
        {
            bool ok = false;

            var mitigations = _model.Mitigations?.ToArray();

            if (mitigations?.Any() ?? false)
            {
                var selectedMitigations = new List<Guid>();

                switch (status)
                {
                    case RoadmapStatus.ShortTerm:
                        UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.ShortTerm);
                        break;
                    case RoadmapStatus.MidTerm:
                        UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.ShortTerm);
                        UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.MidTerm);
                        break;
                    case RoadmapStatus.LongTerm:
                        UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.ShortTerm);
                        UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.MidTerm);
                        UpdateSelectedMitigations(selectedMitigations, mitigations, RoadmapStatus.LongTerm);
                        break;
                }

                var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(_model);
                var projected = 
                    schemaManager.SelectedEstimator?.GetProjectedThreatTypesResidualRisk(_model, selectedMitigations);

                if (projected?.Any() ?? false)
                {
                    _chart.RefreshChart(_model, projected);
                    _currentStatus = status;
                    _chartName.Text = status.GetEnumLabel();
                    ok = true;
                }
            }

            _previous.Visible = ok;
            _next.Visible = ok;
            _chartName.Visible = ok;
        }

        private static void UpdateSelectedMitigations(List<Guid> selectedMitigations, 
            [NotNull] IEnumerable<IMitigation> mitigations, RoadmapStatus status)
        {
            var toBeAdded = mitigations
                .Where(x => x.GetStatus() == status)
                .Select(x => x.Id)
                .ToArray();
            if (toBeAdded.Any())
                selectedMitigations.AddRange(toBeAdded);
        }
    }
}
