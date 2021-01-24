using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Charts;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Configuration;
using ThreatsManager.Extensions.Panels.Roadmap;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels
{
    public partial class RoadmapChart : UserControl
    {
        public RoadmapChart()
        {
            InitializeComponent();
        }

        public float AcceptableRisk
        {
            get
            {
                if (float.TryParse(Chart.AxisY.ReferenceLines.FirstOrDefault()?.AxisValue?.ToString(), out var result))
                    return result;
                else
                    return 0f;
            }
            set
            {
                var referenceLine = Chart.AxisY.ReferenceLines.FirstOrDefault();
                if (referenceLine != null)
                    referenceLine.AxisValue = value;
            }
        }

        public float Current
        {
            get
            {
                if (float.TryParse(Series.SeriesPoints[0].ValueY[0]?.ToString(), out var result))
                    return result;
                else
                    return 0f;
            }
            set => Series.SeriesPoints[0].ValueY = new object[] {(object)value};
        }

        public float ShortTerm
        {
            get
            {
                if (float.TryParse(Series.SeriesPoints[1].ValueY[0]?.ToString(), out var result))
                    return result;
                else
                    return 0f;
            }
            set => Series.SeriesPoints[1].ValueY = new object[] {(object)value};
        }

        public float MidTerm
        {
            get
            {
                if (float.TryParse(Series.SeriesPoints[2].ValueY[0]?.ToString(), out var result))
                    return result;
                else
                    return 0f;
            }
            set => Series.SeriesPoints[2].ValueY = new object[] {(object)value};
        }

        public float LongTerm
        {
            get
            {
                if (float.TryParse(Series.SeriesPoints[3].ValueY[0]?.ToString(), out var result))
                    return result;
                else
                    return 0f;
            }
            set => Series.SeriesPoints[3].ValueY = new object[] {(object)value};
        }

        public void RefreshChart([NotNull] IThreatModel model)
        {
            var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(model);
            var estimator = schemaManager.SelectedEstimator;
            if (estimator != null)
            {
                var reference = RefreshSeries(model, schemaManager, estimator);
                RefreshAcceptableRisk(model, schemaManager, estimator, reference);
            }
        }

        private float RefreshSeries([NotNull] IThreatModel model, 
            [NotNull] ResidualRiskEstimatorPropertySchemaManager schemaManager, 
            [NotNull] IResidualRiskEstimator estimator)
        {
            float result = 0f;

            var mitigations = model.Mitigations?.ToArray();
            if (estimator != null && (mitigations?.Any() ?? false))
            {
                var selectedMitigations = new List<Guid>();

                result = estimator.Estimate(model, null, out var currentMin, out var currentMax);
                Current = 100f;

                var shortTerm = mitigations
                    .Where(x => x.GetStatus() == RoadmapStatus.ShortTerm)
                    .Select(x => x.Id)
                    .ToArray();
                if (shortTerm.Any())
                    selectedMitigations.AddRange(shortTerm);
                ShortTerm = estimator.Estimate(model, selectedMitigations, out var shortTermMin, out var shortTermMax) * 100f / result;
                
                var midTerm = mitigations
                    .Where(x => x.GetStatus() == RoadmapStatus.MidTerm)
                    .Select(x => x.Id)
                    .ToArray();
                if (midTerm.Any())
                    selectedMitigations.AddRange(midTerm);
                MidTerm = estimator.Estimate(model, selectedMitigations, out var midTermMin, out var midTermMax) * 100f / result;
                
                var longTerm = mitigations
                    .Where(x => x.GetStatus() == RoadmapStatus.LongTerm)
                    .Select(x => x.Id)
                    .ToArray();
                if (longTerm.Any())
                    selectedMitigations.AddRange(longTerm);
                LongTerm = estimator.Estimate(model, selectedMitigations, out var longTermMin, out var longTermMax) * 100f / result;
            }

            return result;
        }

        private void RefreshAcceptableRisk([NotNull] IThreatModel model,
            [NotNull] ResidualRiskEstimatorPropertySchemaManager schemaManager, 
            [NotNull] IResidualRiskEstimator estimator, 
            float reference)
        {
            var parameters = schemaManager.Parameters?.ToArray();
            if (parameters?.Any() ?? false)
            {
                var infinite = schemaManager.Infinite;
                if (infinite < 0)
                    infinite = estimator.DefaultInfinite;

                var normalizationReference =
                    (new ExtensionConfigurationManager(model, (new ConfigurationPanelFactory()).GetExtensionId())).NormalizationReference;

                var p = parameters.ToDictionary(x => x.Name, x => x.Value);
                AcceptableRisk = estimator.GetAcceptableRisk(model, p, infinite, normalizationReference) * 100f / reference;
            }
            else
            {
                AcceptableRisk = 0f;
            }
        }

        private ChartXy Chart => _chart.ChartPanel.ChartContainers[0] as ChartXy;
        private ChartSeries Series => Chart?.ChartSeries[0];

        private void _chart_PreRenderSeriesBar(object sender, PreRenderSeriesBarEventArgs e)
        {
            if (float.TryParse(e.SeriesPoint.ValueY[0]?.ToString(), out var value) && value <= AcceptableRisk)
            {
                using (Brush br = new SolidBrush(Color.Green))
                    e.Graphics.FillRectangle(br, e.Bounds);

                e.Cancel = true;
            }
        }

        private void _snapshot_Click(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, this.ClientRectangle);

            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Bitmap, true, bitmap);
            Clipboard.SetDataObject(dataObject);
        }
    }
}
