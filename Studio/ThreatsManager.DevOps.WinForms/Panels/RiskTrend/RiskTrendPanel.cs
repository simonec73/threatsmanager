using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Charts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.DevOps.Panels.RiskTrend
{
    public partial class RiskTrendPanel : UserControl, IShowThreatModelPanel<Form>
    {
        public RiskTrendPanel()
        {
            InitializeComponent();
        }

        public void SetThreatModel(IThreatModel threatModel)
        {
            var schemaManager = new DevOpsConfigPropertySchemaManager(threatModel);
            var iterations = schemaManager.GetIterations()?
                .OrderBy(x => x.Start)
                .ToArray();

            if (iterations?.Any() ?? false)
            {
                if (Chart?.ChartSeries.FirstOrDefault() is ChartSeries series)
                { 
                    foreach (var iteration in iterations)
                    {
                        var point = new SeriesPoint(iteration.Name);
                        var risk = schemaManager.GetIterationRisk(iteration);
                        point.ValueY = new object[] {((object) risk)};
                        series.SeriesPoints.Add(point);
                    }
                }

                var residualRiskSchemaManager = new ResidualRiskEstimatorPropertySchemaManager(threatModel);
                var estimator = residualRiskSchemaManager.SelectedEstimator;
                if (estimator != null)
                {
                    var parameters = residualRiskSchemaManager.Parameters?.ToArray();
                    if (parameters?.Any() ?? false)
                    {
                        var infinite = residualRiskSchemaManager.Infinite;
                        if (infinite < 0)
                            infinite = estimator.DefaultInfinite;

                        var p = parameters.ToDictionary(x => x.Name, x => x.Value);
                        AcceptableRisk = estimator.GetAcceptableRisk(threatModel, p, infinite, 0);
                    }
                    else
                    {
                        AcceptableRisk = 0f;
                    }
                }
            }
        }

        private readonly Guid _id = new Guid();

        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        private ChartXy Chart => _chart.ChartPanel.ChartContainers.FirstOrDefault() as ChartXy;

        private float AcceptableRisk
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
    }
}
