using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("34EBFE2C-FB0C-481E-B034-53F36851D2C4", "Strengths Table", 50, ExecutionMode.Business)]
    public class TableStrengthsPlaceholderPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableStrengths";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableStrengthsPlaceholder();
        }
    }
    public class TableStrengthsPlaceholder : ITablePlaceholder
    {
        public string Name => "Strengths";
        public string Label => "Strengths";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => Icons.Resources.strength_small;

        public TableItem GetTable([PostSharp.Patterns.Contracts.NotNull] IThreatModel model)
        {
            TableItem result = null;

            var strengths = model.Strengths?.Where(x => x.Visible).OrderByDescending(x => x.Id).ToArray();
            if (strengths?.Any() ?? false)
            {
                result = new TableItem(Name, new[]
                {
                    new TableColumn("Strength", 100),
                    new TableColumn("Description", 600)
                }, GetCells(strengths));
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private IEnumerable<Cell> GetCells([PostSharp.Patterns.Contracts.NotNull] IEnumerable<IStrength> strengths)
        {
            var result = new List<Cell>();

            if (strengths.Any())
            {
                foreach (var strength in strengths)
                {
                    result.Add(new Cell(strength.Name));
                    result.Add(new Cell(strength.Description));
                }
            }

            return result;
        }
    }
}
