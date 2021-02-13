using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Charts;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels
{
    public partial class MitigationStatusChart : UserControl
    {
        public MitigationStatusChart()
        {
            InitializeComponent();
        }

        public void RefreshChart([NotNull] IThreatModel model)
        {
            if (_chart.ChartPanel.ChartContainers[0] is PieChart pieChart)
            {
                pieChart.ChartSeries[0].SeriesPoints.Clear();
                var total = model.GetThreatEventMitigations()?.Count() ?? 0;
                if (total > 0)
                {
                    AddSlice(pieChart, model, MitigationStatus.Undefined, total, Color.Black);
                    AddSlice(pieChart, model, MitigationStatus.Existing, total, Color.Green);
                    AddSlice(pieChart, model, MitigationStatus.Implemented, total, Color.Olive);
                    AddSlice(pieChart, model, MitigationStatus.Proposed, total, Color.Orange);
                    AddSlice(pieChart, model, MitigationStatus.Approved, total, Color.Yellow);
                    AddSlice(pieChart, model, MitigationStatus.Planned, total, Color.YellowGreen);
                }
            }
        }

        private void AddSlice(PieChart pieChart, IThreatModel model, 
            MitigationStatus status, int total, Color background)
        {
            var count = model.CountMitigationsByStatus(status);

            if (count > 0)
            {
                var slice = pieChart.ChartSeries[0].SeriesPoints
                    .OfType<PieSeriesPoint>()
                    .FirstOrDefault(x => string.CompareOrdinal(x.Name, status.ToString()) == 0);
                if (slice == null)
                {
                    slice = new PieSeriesPoint()
                    {
                        Name = status.ToString(),
                        ValueX = status.GetEnumLabel(),
                        ValueY = new object[] {(object) count},
                        OuterSliceLabel = $"{count} ({((float) count * 100f / (float) total).ToString("F0")}%)",
                        InnerSliceLabel = ""
                    };
                    slice.SliceVisualStyles.Default.Background.Color1 = background;
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

        private void _snapshot_Click(object sender, System.EventArgs e)
        {
            var bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, this.ClientRectangle);

            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Bitmap, true, bitmap);
            Clipboard.SetDataObject(dataObject);
        }
    }
}
