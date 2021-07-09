using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("61FB2189-8F8C-4FF9-AE91-658706D3D39F", "Threat Type List Placeholder", 44, ExecutionMode.Business)]
    public class ListThreatTypesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListThreatTypes";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListThreatTypesPlaceholder();
        }
    }

    public class ListThreatTypesPlaceholder : IListPlaceholder
    {
        public string Name => "ThreatTypes";
        public string Label => "Threat Types";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.threat_types_small;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var threatTypes = model.ThreatTypes?
                .OrderByDescending(x => x.Severity, new SeverityComparer())
                .ToArray();

            if (threatTypes?.Any() ?? false)
            {
                var dict = new Dictionary<string, IPropertyType>();

                foreach (var threatType in threatTypes)
                {
                    var properties = threatType.Properties?
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

                    var eventProperties = new ListThreatEventsPlaceholder().GetProperties(model)?.ToArray();
                    if (eventProperties?.Any() ?? false)
                    {
                        foreach (var ep in eventProperties)
                        {
                            var text = $"[From Events] {ep.Key}";
                            if (!dict.ContainsKey(text))
                                dict.Add(text, ep.Value);
                        }
                    }
                }

                result = dict.Where(x => !x.Key.StartsWith("[From Events] "))
                    .OrderBy(x => model.GetSchema(x.Value.SchemaId).Priority)
                    .ThenBy(x => model.GetSchema(x.Value.SchemaId).Namespace)
                    .ThenBy(x => model.GetSchema(x.Value.SchemaId).Name)
                    .ThenBy(x => x.Value.Priority)
                    .Union(dict.Where(x => x.Key.StartsWith("[From Events] "))
                        .OrderBy(x => model.GetSchema(x.Value.SchemaId).Priority)
                        .ThenBy(x => x.Value.Priority))
                    .ToArray();
            }

            return result;
        }

        public IEnumerable<ListItem> GetList(IThreatModel model)
        {
            IEnumerable<ListItem> result = null;

            var threatTypes = model.ThreatTypes?
                .OrderByDescending(x => x.Severity, new SeverityComparer())
                .ThenBy(x => x.Name)
                .ToArray();

            if (threatTypes?.Any() ?? false)
            {
                var list = new List<ListItem>();

                var eventProperties = new ListThreatEventsPlaceholder().GetProperties(model)?
                    .OrderBy(x => model.GetSchema(x.Value.SchemaId).Priority)
                    .ThenBy(x => model.GetSchema(x.Value.SchemaId).Namespace)
                    .ThenBy(x => model.GetSchema(x.Value.SchemaId).Name)
                    .ThenBy(x => x.Value.Priority)
                    .ToArray();

                foreach (var threatType in threatTypes)
                {
                    var threatEvents = model.GetThreatEvents(threatType)?.ToArray();
                    if (threatEvents?.Any() ?? false)
                    {
                        var items = new List<ItemRow>();

                        items.Add(new TextRow("Severity", threatType.Severity.Name,
                            threatType.Severity.TextColor, threatType.Severity.BackColor, true, true, 75));
                        items.Add(new TextRow("Description", threatType.Description));
                        items.Add(new ListRow("Affected Objects", 
                            threatEvents.Select(x => 
                                new Line($"{x.Parent.Name}", 
                                    $"[{model.GetIdentityTypeInitial(x.Parent)}] ",
                                    $" ({x.Severity.Name})",
                                    new [] {x.ParentId}))));
                        items.Add(new TableRow("Approved Mitigations", new[]
                        {
                            new TableColumn("Object", 150),
                            new TableColumn("Mitigation", 200),
                            new TableColumn("Severity", 75),
                            new TableColumn("Strength", 75)
                        }, GetCells(GetMitigations(threatEvents, MitigationStatus.Approved))));
                        items.Add(new TableRow("Existing Mitigations", new[]
                        {
                            new TableColumn("Object", 150),
                            new TableColumn("Mitigation", 200),
                            new TableColumn("Severity", 75),
                            new TableColumn("Strength", 75)
                        }, GetCells(GetMitigations(threatEvents, MitigationStatus.Existing))));
                        items.Add(new TableRow("Implemented Mitigations", new[]
                        {
                            new TableColumn("Object", 150),
                            new TableColumn("Mitigation", 200),
                            new TableColumn("Severity", 75),
                            new TableColumn("Strength", 75)
                        }, GetCells(GetMitigations(threatEvents, MitigationStatus.Implemented))));
                        items.Add(new TableRow("Planned Mitigations", new[]
                        {
                            new TableColumn("Object", 150),
                            new TableColumn("Mitigation", 200),
                            new TableColumn("Severity", 75),
                            new TableColumn("Strength", 75)
                        }, GetCells(GetMitigations(threatEvents, MitigationStatus.Planned))));
                        items.Add(new TableRow("Proposed Mitigations", new[]
                        {
                            new TableColumn("Object", 150),
                            new TableColumn("Mitigation", 200),
                            new TableColumn("Severity", 75),
                            new TableColumn("Strength", 75)
                        }, GetCells(GetMitigations(threatEvents, MitigationStatus.Proposed))));

                        var itemRows = threatType.GetItemRows()?.ToArray();
                        if (itemRows?.Any() ?? false)
                            items.AddRange(itemRows);

                        if (eventProperties?.Any() ?? false)
                        {
                            foreach (var ep in eventProperties)
                            {
                                if (threatEvents.Any(x => x.HasProperty(ep.Value)))
                                    items.Add(new TableRow($"[From Events] {ep.Key}", new []
                                    {
                                        new TableColumn("Object", 150),
                                        new TableColumn("Value", 350)
                                    }, GetCells(threatEvents.Where(x => x.HasProperty(ep.Value)), ep.Value)));
                            }
                        }

                        list.Add(new ListItem(threatType.Name, threatType.Id, items));
                    }
                }

                result = list;
            }

            return result;
        }

        private IEnumerable<IThreatEventMitigation> GetMitigations(IEnumerable<IThreatEvent> threatEvents, MitigationStatus status)
        {
            IEnumerable<IThreatEventMitigation> result = null;

            var list = threatEvents?.ToArray();
            if (list?.Any() ?? false)
            {
                var mitigations = new List<IThreatEventMitigation>();

                foreach (var item in list)
                {
                    var ms = item.Mitigations?
                        .Where(x => x.Status == status)
                        .ToArray();
                    if (ms?.Any() ?? false)
                        mitigations.AddRange(ms);
                }

                result = mitigations
                    .OrderBy(x => x.ThreatEvent.Parent.Name)
                    .ThenBy(x => x.Mitigation.Name);
            }

            return result;
        }
        
        private IEnumerable<Cell> GetCells(IEnumerable<IThreatEventMitigation> mitigations)
        {
            IEnumerable<Cell> result = null;

            var list = mitigations?.ToArray();
            if (list?.Any() ?? false)
            {
                var cells = new List<Cell>();

                foreach (var item in list)
                {
                    cells.Add(new Cell($"{item.ThreatEvent.Parent.Name}",
                        $"[{item.Model.GetIdentityTypeInitial(item.ThreatEvent.Parent)}] ",
                        null,
                        new[]
                        {item.ThreatEvent.ParentId}));
                    cells.Add(new Cell(item.Mitigation.Name, null, null, new [] {item.MitigationId}));
                    cells.Add(new Cell(item.ThreatEvent.Severity.Name, item.ThreatEvent.Severity.TextColor, item.ThreatEvent.Severity.BackColor, false, true));
                    cells.Add(new Cell(item.Strength.Name));
                }

                result = cells;
            }

            return result;
        }

        private IEnumerable<Cell> GetCells(IEnumerable<IThreatEvent> threatEvents, [NotNull] IPropertyType propertyType)
        {
            IEnumerable<Cell> result = null;

            var list = threatEvents?.ToArray();
            if (list?.Any() ?? false)
            {
                var cells = new List<Cell>();

                foreach (var item in list)
                {
                    var property = item.GetProperty(propertyType);
                    if (property != null)
                    {
                        var cell = Cell.Create(item, property);
                        if (cell != null)
                        {
                            cells.Add(new Cell($"{item.Parent.Name}",
                                $"[{item.Model.GetIdentityTypeInitial(item.Parent)}] ",
                                null,
                                new[] {item.ParentId}));
                            cells.Add(cell);
                        }
                    }
                }

                result = cells;
            }

            return result;
        }
    }
}
