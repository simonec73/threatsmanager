using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Charts;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Panels
{
    public partial class ThreatTypesChart : UserControl
    {
        public ThreatTypesChart()
        {
            InitializeComponent();
        }

        public void RefreshChart([NotNull] IThreatModel model)
        {
            if (_chart.ChartPanel.ChartContainers[0] is PieChart pieChart)
            {
                pieChart.ChartSeries[0].SeriesPoints.Clear();
                var severities = model.Severities?.ToArray();
                var threatTypes = model.ThreatTypes?.ToArray();

                if ((severities?.Any() ?? false) && (threatTypes?.Any() ?? false))
                {
                    var total = model.AssignedThreatTypes;
                    foreach (var severity in severities)
                    {
                        var count = model.CountThreatEventsByType(severity);

                        if (count > 0)
                        {
                            var slice = pieChart.ChartSeries[0].SeriesPoints
                                .OfType<PieSeriesPoint>()
                                .FirstOrDefault(x => string.CompareOrdinal(x.Name, severity.Name) == 0);
                            if (slice == null)
                            {
                                slice = new PieSeriesPoint()
                                {
                                    Name = severity.Name,
                                    ValueX = severity.Name,
                                    ValueY = new object[] {(object) count},
                                    OuterSliceLabel =
                                        $"{count} ({((float) count * 100f / (float) total).ToString("F0")}%)",
                                    InnerSliceLabel = ""
                                };
                                slice.SliceVisualStyles.Default.Background.Color1 =
                                    Color.FromKnownColor(severity.BackColor);
                                slice.SliceVisualStyles.Default.SliceOuterLabelStyle.TextColor = Color.Black;
                                pieChart.ChartSeries[0].SeriesPoints.Add(slice);
                            }
                            else
                            {
                                slice.ValueY = new object[] {(object) count};
                                slice.OuterSliceLabel =
                                    $"{count} ({((float) count * 100f / (float) total).ToString("F0")}%)";
                            }
                        }
                    }
                }
            }
        }

        public void RefreshChart([NotNull] IThreatModel model, [NotNull] IDictionary<Guid, int> projectedSeverities)
        {
            if (_chart.ChartPanel.ChartContainers[0] is PieChart pieChart)
            {
                pieChart.ChartSeries[0].SeriesPoints.Clear();
                var severities = model.Severities?.ToArray();

                if ((severities?.Any() ?? false) && projectedSeverities.Any())
                {
                    var total = model.AssignedThreatTypes;
                    foreach (var severity in severities)
                    {
                        var count = projectedSeverities.Count(x => x.Value == severity.Id);

                        if (count > 0)
                        {
                            var slice = pieChart.ChartSeries[0].SeriesPoints
                                .OfType<PieSeriesPoint>()
                                .FirstOrDefault(x => string.CompareOrdinal(x.Name, severity.Name) == 0);
                            if (slice == null)
                            {
                                slice = new PieSeriesPoint()
                                {
                                    Name = severity.Name,
                                    ValueX = severity.Name,
                                    ValueY = new object[] {(object) count},
                                    OuterSliceLabel =
                                        $"{count} ({((float) count * 100f / (float) total).ToString("F0")}%)",
                                    InnerSliceLabel = ""
                                };
                                slice.SliceVisualStyles.Default.Background.Color1 =
                                    Color.FromKnownColor(severity.BackColor);
                                slice.SliceVisualStyles.Default.SliceOuterLabelStyle.TextColor = Color.Black;
                                pieChart.ChartSeries[0].SeriesPoints.Add(slice);
                            }
                            else
                            {
                                slice.ValueY = new object[] {(object) count};
                                slice.OuterSliceLabel =
                                    $"{count} ({((float) count * 100f / (float) total).ToString("F0")}%)";
                            }
                        }
                    }
                }
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
