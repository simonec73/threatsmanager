using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("4964236B-01CC-4FC6-9E7A-D4BEA3AF7F1A", "Chart Threat Types Placeholder", 34, ExecutionMode.Business)]
    public class ChartThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ChartThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new ChartThreatTypesPlaceholder();
        }
    }

    public class ChartThreatTypesPlaceholder : IChartPlaceholder
    {
        public string Name => "ThreatTypes";
        public string Label => "Threat Types";
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
                    list.Add(new ChartItem(severity.Name, model.CountThreatEventsByType(severity), severity.BackColor));
                }

                result = list;
            }

            return result;
        }
    }
}
