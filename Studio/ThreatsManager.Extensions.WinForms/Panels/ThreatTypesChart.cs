using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Charts;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels
{
    public partial class ThreatTypesChart : UserControl
    {
        private string _threatCopy;

        public ThreatTypesChart()
        {
            InitializeComponent();
            _tooltip.SetSuperTooltip(_chart, new DevComponents.DotNetBar.SuperTooltipInfo()
            {
                HeaderVisible = true,
                HeaderText = "Threats",
                FooterVisible = true,
                FooterText = "<a>Click here</a> to copy the threats.",
                Color = DevComponents.DotNetBar.eTooltipColor.System
            });
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
                                    InnerSliceLabel = "",
                                    Tag = GetThreatsTooltipText(severity)
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
                        var threatTypes = projectedSeverities.Where(x => x.Value == severity.Id).Select(x => x.Key).ToArray();
                        var count = threatTypes.Count();

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
                                    ValueY = new object[] { (object)count },
                                    OuterSliceLabel =
                                        $"{count} ({((float)count * 100f / (float)total).ToString("F0")}%)",
                                    InnerSliceLabel = "",
                                    Tag = GetThreatsTooltipText(model, threatTypes)
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

        private string GetThreatsTooltipText([NotNull] ISeverity severity)
        {
            string result = null;

            var types = severity.Model?.ThreatTypes.OrderBy(x => x.Name).ToArray();
            if (types?.Any() ?? false)
            {
                var builder = new StringBuilder();
                foreach (var type in types)
                {
                    var s = type.GetTopSeverity();
                    if (s != null && s.Id == severity.Id)
                        builder.AppendLine($"  - {type.Name}");
                }
                if (builder.Length > 0)
                    result = builder.ToString();
            }

            return result;
        }

        private string GetThreatsTooltipText([NotNull] IThreatModel model, IEnumerable<Guid> threatTypes)
        {
            string result = null;

            if (threatTypes?.Any() ?? false)
            {
                var types = threatTypes
                    .Select(x => model.GetThreatType(x))
                    .Where(x => x != null)
                    .OrderBy(x => x.Name)
                    .ToArray();

                var builder = new StringBuilder();
                foreach (var type in types)
                {
                    builder.AppendLine($"  - {type.Name}");
                }
                if (builder.Length > 0)
                    result = builder.ToString();
            }

            return result;
        }

        private void _snapshot_Click(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, this.ClientRectangle);

            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Bitmap, true, bitmap);
            Clipboard.SetDataObject(dataObject);
        }

        private void _tooltip_BeforeTooltipDisplay(object sender, DevComponents.DotNetBar.SuperTooltipEventArgs e)
        {
            if (_chart.ChartPanel.ChartContainers[0] is PieChart pieChart)
            {
                if (pieChart.GetHitItem(PointToClient(MousePosition), out var psp) == ItemHitArea.InPieSeriesPoint &&
                    psp.Tag is string text)
                {
                    var header = $"{psp.Name} severity Threat Types";
                    e.TooltipInfo.HeaderText = header;
                    e.TooltipInfo.BodyText = text;
                    e.Location = MousePosition;
                    _threatCopy = $"{header}:\n---\n{text.Replace("  - ", "")}";
                }
                else
                    e.Cancel = true;
            }
            else
                e.Cancel = true;
        }

        private void _tooltip_MarkupLinkClick(object sender, DevComponents.DotNetBar.MarkupLinkClickEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_threatCopy))
            {
                Clipboard.SetText(_threatCopy);
            }
        }
    }
}
