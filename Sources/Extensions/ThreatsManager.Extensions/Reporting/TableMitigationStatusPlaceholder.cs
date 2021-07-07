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
    [Extension("BE5C5485-B8D2-4CE0-9814-227BC9CD9B89", "Mitigation Status Table", 52, ExecutionMode.Business)]
    public class TableMitigationStatusPlaceholderPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableMitigationStatus";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableMitigationStatusPlaceholder();
        }
    }
    public class TableMitigationStatusPlaceholder : ITablePlaceholder
    {
        public string Name => "MitigationStatus";
        public string Label => "Mitigation Status";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => null;

        public TableItem GetTable([NotNull] IThreatModel model)
        {
            return new TableItem(Name, new[]
                {
                    new TableColumn("Status Name", 100),
                    new TableColumn("Description", 600)
                }, GetCells());
        }

        private IEnumerable<Cell> GetCells()
        {
            var result = new List<Cell>();

            var states = EnumExtensions.GetEnumLabels<MitigationStatus>()
                .Select(x => x.GetEnumValue<MitigationStatus>())
                .ToArray();
            foreach (var status in states)
            {
                result.Add(new Cell(status.GetEnumLabel()));
                result.Add(new Cell(status.GetEnumDescription()));
            }

            return result;
        }
    }
}
