using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.DevOps.Dialogs
{
    public partial class DevOpsIterationAssignmentDialog : Form
    {
        private readonly IMitigation _mitigation;
        private readonly IThreatModel _model;

        public DevOpsIterationAssignmentDialog()
        {
            InitializeComponent();
        }

        public DevOpsIterationAssignmentDialog([NotNull] IMitigation mitigation) : this()
        {
            _mitigation = mitigation;
            _model = mitigation.Model;
            LoadModel();
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (_iterations.SelectedItem is Iteration iteration)
            {
                var schemaManager = new DevOpsPropertySchemaManager(_model);
                schemaManager.SetFirstSeenOn(_mitigation, iteration);
            }
        }

        private void _add_Click(object sender, EventArgs e)
        {
            var dialog = new DevOpsManageIterationsDialog(_model);
            dialog.ShowDialog(Form.ActiveForm);
            _iterations.Items.Clear();
            LoadModel();
        }

        private void LoadModel()
        {
            var schemaManager = new DevOpsConfigPropertySchemaManager(_model);
            var iterations = schemaManager.GetIterations()?.ToArray();
            if (iterations?.Any() ?? false)
            {
                _iterations.Items.AddRange(iterations);

                var propertySchemaManager = new DevOpsPropertySchemaManager(_model);
                var firstSeenOn = propertySchemaManager.GetFirstSeenOn(_mitigation);
                if (firstSeenOn != null)
                {
                    var item = _iterations.Items.OfType<Iteration>()
                        .FirstOrDefault(x => string.CompareOrdinal(x.Id, firstSeenOn.IterationId) == 0);
                    if (item != null)
                    {
                        _iterations.SelectedItem = item;
                    }
                }
            }
        }

        private void _iterations_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = _iterations.SelectedItem is Iteration;
        }
    }
}
