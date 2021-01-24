using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Configuration;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class ResidualRiskEstimatorConfigurationDialog : Form
    {
        private IThreatModel _model;
        private bool _loading = false;

        public ResidualRiskEstimatorConfigurationDialog()
        {
            InitializeComponent();

            var estimators = ExtensionUtils.GetExtensions<IResidualRiskEstimator>()?.ToArray();
            if (estimators?.Any() ?? false)
            {
                _estimators.Items.AddRange(estimators);
            }
        }

        public void Initialize([NotNull] IThreatModel model)
        {
            _loading = true;
            _model = model;

            var normalizationReference =
                (new ExtensionConfigurationManager(_model, (new ConfigurationPanelFactory()).GetExtensionId())).NormalizationReference;
            if (normalizationReference == 0)
                _labelNormalizationContainer.Visible = false;
            else
            {
                _labelNormalization.Text =
                    $"The Acceptable Risk is going to be normalized to a reference Threat Model with {normalizationReference} objects.";
                _labelNormalizationContainer.Visible = true;
            }

            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
            var estimator = schemaManager.SelectedEstimator;
            _estimators.SelectedItem = estimator;
            if (estimator != null)
            {
                RefreshParameters(estimator, schemaManager);
            }

            _loading = false;
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(_model);
            var estimator = _estimators.SelectedItem as IResidualRiskEstimator;
            schemaManager.SelectedEstimator = estimator;

            var parameters = new List<ResidualRiskEstimatorParameter>();
            var rows = _parameters.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                parameters.Add(new ResidualRiskEstimatorParameter()
                {
                    Name = row.Cells[0].Value as string,
                    Value = (float) row.Cells[1].Value
                });
            }

            schemaManager.Parameters = parameters;
            schemaManager.Infinite = (float) _cap.Value;

            DialogResult = DialogResult.OK;
        }

        private void _estimator_SelectedIndexChanged(object sender, EventArgs e)
        {
            var estimator = _estimators.SelectedItem as IResidualRiskEstimator;
            if (estimator != null)
            {
                RefreshParameters(estimator, null);
            }
        }

        private void RefreshParameters([NotNull] IResidualRiskEstimator estimator, 
            ResidualRiskEstimatorPropertySchemaManager schemaManager)
        {
            if (schemaManager == null)
                schemaManager = new ResidualRiskEstimatorPropertySchemaManager(_model);

            var configured = schemaManager.Parameters?.ToArray();

            if (!_loading)
                schemaManager.Infinite = estimator.DefaultInfinite;

            var infinite = schemaManager.Infinite;
            if (infinite < 0)
                infinite = estimator.DefaultInfinite;

            _cap.Value = infinite;

            _parameters.PrimaryGrid.Rows.Clear();
            var parameters = estimator.GetAcceptableRiskParameters(_model)?.ToArray();
            foreach (var parameter in parameters)
            {
                var value = configured?
                    .FirstOrDefault(x => string.CompareOrdinal(parameter, x.Name) == 0)?
                    .Value ?? 0.0f;

                var row = new GridRow(parameter, value);
                _parameters.PrimaryGrid.Rows.Add(row);
            }
        }
    }
}
