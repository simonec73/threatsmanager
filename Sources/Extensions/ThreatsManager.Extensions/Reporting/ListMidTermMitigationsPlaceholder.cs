using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("4EA4A192-BF44-4D37-A1DE-FD8056AACBAD", "Mid Term Mitigation List Placeholder", 44, ExecutionMode.Business)]
    public class ListMidTermMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListMidTermMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListMidTermMitigationsPlaceholder();
        }
    }

    public class ListMidTermMitigationsPlaceholder : IListPlaceholder
    {
        public string Name => "MidTermMitigations";
        public string Label => "Mid Term Mitigations";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.mitigations_small;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var schema = new RoadmapPropertySchemaManager(model);
            var mitigations = model.GetUniqueMitigations()?
                .Where(x => schema.GetStatus(x) == RoadmapStatus.MidTerm)
                .OrderBy(x => x.Name)
                .ToArray();

            if (mitigations?.Any() ?? false)
            {
                var dict = new Dictionary<string, IPropertyType>();

                foreach (var mitigation in mitigations)
                {
                    var properties = mitigation.Properties?
                        .Where(x => !(x.PropertyType?.DoNotPrint ?? true))
                        .Select(x => x.PropertyType)
                        .ToArray();

                    if (properties?.Any() ?? false)
                    {
                        foreach (var property in properties)
                        {
                            if (!dict.ContainsKey(property.Name))
                                dict.Add(property.Name, property);
                        }
                    }
                }

                result = dict
                    .OrderBy(x => model.GetSchema(x.Value.SchemaId).Priority)
                    .ThenBy(x => model.GetSchema(x.Value.SchemaId).Namespace)
                    .ThenBy(x => model.GetSchema(x.Value.SchemaId).Name)
                    .ThenBy(x => x.Value.Priority)
                    .ToArray();
            }

            return result;
        }

        public IEnumerable<ListItem> GetList(IThreatModel model)
        {
            IEnumerable<ListItem> result = null;

            var schema = new RoadmapPropertySchemaManager(model);
            var mitigations = model.GetUniqueMitigations()?
                .Where(x => schema.GetStatus(x) == RoadmapStatus.MidTerm)
                .OrderBy(x => x.Name)
                .ToArray();

            if (mitigations?.Any() ?? false)
            {
                var list = new List<ListItem>();

                foreach (var mitigation in mitigations)
                {
                    var items = new List<ItemRow>();

                    var tems = model.GetThreatEventMitigations(mitigation)?.ToArray();

                    if (tems?.Any() ?? false)
                    {
                        items.Add(new TextRow("Control Type", mitigation.ControlType.GetEnumLabel()));
                        items.Add(new TextRow("Description", mitigation.Description));
                        items.Add(new TableRow("Affected Threats", new []
                        {
                            new TableColumn("Object", 150),
                            new TableColumn("Threat", 150),
                            new TableColumn("Strength", 100),
                            new TableColumn("Status", 100)
                        }, GetAffectedThreatsCells(tems)));
                        items.Add(new TableRow("Directives", new[]
                        {
                            new TableColumn("Object", 150),
                            new TableColumn("Directives", 350),
                        }, GetDirectivesCells(tems)));

                        var itemRows = mitigation.GetItemRows()?.ToArray();
                        if (itemRows?.Any() ?? false)
                            items.AddRange(itemRows);

                        list.Add(new ListItem(mitigation.Name, mitigation.Id, items));
                    }
                }

                result = list;
            }

            return result;
        }
        
        private IEnumerable<Cell> GetAffectedThreatsCells(IEnumerable<IThreatEventMitigation> mitigations)
        {
            IEnumerable<Cell> result = null;

            var list = mitigations?
                .OrderByDescending(x => x.Strength, new StrengthComparer())
                .ThenBy(x => x.ThreatEvent.Parent.Name)
                .ThenBy(x => x.ThreatEvent.Name).ToArray();
            if (list?.Any() ?? false)
            {
                var cells = new List<Cell>();

                foreach (var item in list)
                {
                    cells.Add(new Cell($"{item.ThreatEvent.Parent.Name}", 
                        $"[{item.Model.GetIdentityTypeInitial(item.ThreatEvent.Parent)}] ", null, new [] {item.ThreatEvent.ParentId}));
                    cells.Add(new Cell(item.ThreatEvent.Name, null, null, 
                        new [] {item.ThreatEvent.Id, item.ThreatEvent.ThreatTypeId}));
                    cells.Add(new Cell(item.Strength.Name ?? DefaultStrength.Average.GetEnumLabel()));
                    cells.Add(new Cell(item.Status.GetEnumLabel()));
                }

                result = cells;
            }

            return result;
        }

        private IEnumerable<Cell> GetDirectivesCells(IEnumerable<IThreatEventMitigation> mitigations)
        {
            IEnumerable<Cell> result = null;

            var list = mitigations?
                .Where(x => !string.IsNullOrWhiteSpace(x.Directives))
                .OrderByDescending(x => x.Strength, new StrengthComparer())
                .ThenBy(x => x.ThreatEvent.Parent.Name)
                .ThenBy(x => x.ThreatEvent.Name).ToArray();
            if (list?.Any() ?? false)
            {
                var cells = new List<Cell>();

                foreach (var item in list)
                {
                    cells.Add(new Cell($"{item.ThreatEvent.Parent.Name}", 
                        $"[{item.Model.GetIdentityTypeInitial(item.ThreatEvent.Parent)}] ",
                        null,
                        new[] {item.ThreatEvent.ParentId}));
                    cells.Add(new Cell(item.Directives));
                }

                result = cells;
            }

            return result;
        }
    }
}
