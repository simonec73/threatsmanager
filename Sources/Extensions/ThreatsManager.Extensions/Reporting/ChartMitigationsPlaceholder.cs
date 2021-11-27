using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("66B49B5B-9CFB-4984-B0A8-755DCB6AA1E9", "Chart Mitigations Placeholder", 36, ExecutionMode.Business)]
    public class ChartMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ChartMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new ChartMitigationsPlaceholder();
        }
    }

    public class ChartMitigationsPlaceholder : IChartPlaceholder
    {
        public string Name => "Mitigations";
        public string Label => "Mitigations";
        public PlaceholderSection Section => PlaceholderSection.Chart;
        public Bitmap Image => null;

        public ChartType StandardChartType => ChartType.Pie;

        public IEnumerable<ChartItem> GetChart([NotNull] IThreatModel model)
        {
            IEnumerable<ChartItem> result = null;

            var states = EnumExtensions.GetEnumLabels<MitigationStatus>().ToArray();

            if (states?.Any() ?? false)
            {
                var list = new List<ChartItem>();

                foreach (var status in states)
                {
                    var mitigationStatus = status.GetEnumValue<MitigationStatus>();
                    KnownColor color;
                    switch (mitigationStatus)
                    {
                        case MitigationStatus.Existing:
                            color = KnownColor.Green;
                            break;
                        case MitigationStatus.Proposed:
                            color = KnownColor.Orange;
                            break;
                        case MitigationStatus.Approved:
                            color = KnownColor.Yellow;
                            break;
                        case MitigationStatus.Implemented:
                            color = KnownColor.Olive;
                            break;
                        case MitigationStatus.Planned:
                            color = KnownColor.YellowGreen;
                            break;
                        default:
                            color = KnownColor.Black;
                            break;
                    }

                    list.Add(new ChartItem(status, model.CountMitigationsByStatus(mitigationStatus), color));
                }

                result = list;
            }

            return result;
        }
    }
}
