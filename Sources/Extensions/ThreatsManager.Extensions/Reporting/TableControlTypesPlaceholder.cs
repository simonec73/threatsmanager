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
    [Extension("90BAB459-CCDE-4320-B39B-B4BB3009F0B1", "Control Types Table", 51, ExecutionMode.Business)]
    public class TableControlTypesPlaceholderPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableControlTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableControlTypesPlaceholder();
        }
    }
    public class TableControlTypesPlaceholder : ITablePlaceholder
    {
        public string Name => "ControlTypes";
        public string Label => "Control Types";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => null;

        public TableItem GetTable([NotNull] IThreatModel model)
        {
            return new TableItem(Name, new[]
                {
                    new TableColumn("Control Type", 100),
                    new TableColumn("Description", 600)
                }, GetCells());
        }

        private IEnumerable<Cell> GetCells()
        {
            var result = new List<Cell>();

            var controlTypes = EnumExtensions.GetEnumLabels<SecurityControlType>()
                .Select(x => x.GetEnumValue<SecurityControlType>())
                .ToArray();
            foreach (var controlType in controlTypes)
            {
                result.Add(new Cell(controlType.GetEnumLabel()));
                result.Add(new Cell(controlType.GetEnumDescription()));
            }

            return result;
        }
    }
}
