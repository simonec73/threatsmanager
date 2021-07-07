using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("13F783E0-53C7-4566-B938-DCDF2759C28E", "Threat Actors Table", 49, ExecutionMode.Business)]
    public class TableThreatActorsPlaceholderPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "TableThreatActors";

        public IPlaceholder Create(string parameters = null)
        {
            return new TableThreatActorsPlaceholder();
        }
    }
    public class TableThreatActorsPlaceholder : ITablePlaceholder
    {
        public string Name => "ThreatActors";
        public string Label => "Threat Actors";
        public PlaceholderSection Section => PlaceholderSection.Table;
        public Bitmap Image => Icons.Resources.actor_small;

        public TableItem GetTable([PostSharp.Patterns.Contracts.NotNull] IThreatModel model)
        {
            TableItem result = null;

            var threatActors = model.ThreatActors?.OrderBy(x => x.Name).ToArray();
            if (threatActors?.Any() ?? false)
            {
                result = new TableItem(Name, new[]
                {
                    new TableColumn("Threat Actor", 100),
                    new TableColumn("Description", 600)
                }, GetCells(threatActors));
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private IEnumerable<Cell> GetCells([PostSharp.Patterns.Contracts.NotNull] IEnumerable<IThreatActor> threatActors)
        {
            var result = new List<Cell>();

            if (threatActors.Any())
            {
                foreach (var threatActor in threatActors)
                {
                    result.Add(new Cell(threatActor.Name));
                    result.Add(new Cell(threatActor.Description));
                }
            }

            return result;
        }
    }
}
