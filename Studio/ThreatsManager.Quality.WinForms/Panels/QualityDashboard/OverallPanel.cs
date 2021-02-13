using System.Drawing;
using System.Windows.Forms;

namespace ThreatsManager.Quality.Panels.QualityDashboard
{
    public partial class OverallPanel : UserControl
    {
        public OverallPanel()
        {
            InitializeComponent();
        }

        public void SetHealthIndex(double healthIndex)
        {
            _overall.Ranges[0].EndValue = (float)healthIndex;
            _overall.GaugeLabel = QualityAnalyzersManager.GetHealthIndexDescription(healthIndex);

            if (healthIndex <= 10.0)
                _overall.Ranges[0].Color = Color.Red;
            else if (healthIndex <= 20.0)
                _overall.Ranges[0].Color = Color.Yellow;
            else
                _overall.Ranges[0].Color = Color.Green;
        }
    }
}
