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
    [Extension("87B2DFC7-576C-46C0-B197-79A099E2A0AF", "Severities Table", 48, ExecutionMode.Business)]
    public class TableSeveritiesPlaceholderPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableSeverities";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableSeveritiesPlaceholder();
        }
    }
    public class TableSeveritiesPlaceholder : ITablePlaceholder
    {
        public string Name => "Severities";
        public string Label => "Severities";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => Icons.Resources.severity_small;

        public TableItem GetTable([NotNull] IThreatModel model)
        {
            TableItem result = null;

            var severities = model.Severities?.Where(x => x.Visible).OrderByDescending(x => x, new SeverityComparer()).ToArray();
            if (severities?.Any() ?? false)
            {
                result = new TableItem(Name, new[]
                {
                    new TableColumn("Severity", 100),
                    new TableColumn("Description", 600)
                }, GetCells(severities));
            }

            return result;
        }

        private IEnumerable<Cell> GetCells([NotNull] IEnumerable<ISeverity> severities)
        {
            var result = new List<Cell>();

            if (severities.Any())
            {
                foreach (var severity in severities)
                {
                    result.Add(new Cell(severity.Name, severity.TextColor, severity.BackColor, false, true));
                    result.Add(new Cell(severity.Description));
                }
            }

            return result;
        }
    }
}
