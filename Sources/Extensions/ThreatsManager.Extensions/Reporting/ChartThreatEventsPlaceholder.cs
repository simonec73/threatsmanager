using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("ADB25F10-3C14-49CB-829C-E10F6DD22974", "Chart Threat Events Placeholder", 35, ExecutionMode.Business)]
    public class ChartThreatEventsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ChartThreatEvents";

        public IPlaceholder Create(string parameters = null)
        {
            return new ChartThreatEventsPlaceholder();
        }
    }

    public class ChartThreatEventsPlaceholder : IChartPlaceholder
    {
        public string Name => "ThreatEvents";
        public string Label => "Threat Events";
        public PlaceholderSection Section => PlaceholderSection.Chart;
        public Bitmap Image => null;

        public ChartType StandardChartType => ChartType.Pie;

        public IEnumerable<ChartItem> GetChart([NotNull] IThreatModel model)
        {
            IEnumerable<ChartItem> result = null;

            var severities = model.Severities?
                .Where(x => x.Visible && x.Id > 0)
                .OrderByDescending(x => x.Id).ToArray()
                .ToArray();

            if (severities?.Any() ?? false)
            {
                var list = new List<ChartItem>();

                foreach (var severity in severities)
                {
                    list.Add(new ChartItem(severity.Name, model.CountThreatEvents(severity), severity.BackColor));
                }

                result = list;
            }

            return result;
        }
    }
}
