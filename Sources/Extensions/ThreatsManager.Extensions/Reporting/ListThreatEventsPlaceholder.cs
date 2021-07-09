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
    [Extension("875E2340-FB13-4990-B9B8-700D4F340BCD", "Threat Event List Placeholder", 44, ExecutionMode.Business)]
    public class ListThreatEventsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListThreatEvents";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListThreatEventsPlaceholder();
        }
    }

    public class ListThreatEventsPlaceholder : IListPlaceholder
    {
        public string Name => "ThreatEvents";
        public string Label => "Threat Events";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.threat_event_small;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var threatEvents = model.GetThreatEvents()?
                .OrderByDescending(x => x.Severity, new SeverityComparer())
                .ToArray();

            if (threatEvents?.Any() ?? false)
            {
                var dict = new Dictionary<string, IPropertyType>();

                foreach (var threatEvent in threatEvents)
                {
                    var properties = threatEvent.Properties?
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

            var threatEvents = model.GetThreatEvents()?
                .OrderByDescending(x => x.Severity, new SeverityComparer())
                .ThenBy(x => x.Name)
                .ThenBy(x => x.Parent.Name)
                .ToArray();

            if (threatEvents?.Any() ?? false)
            {
                var list = new List<ListItem>();

                foreach (var threatEvent in threatEvents)
                {
                    var items = new List<ItemRow>();
                    items.Add(new TextRow("Severity", threatEvent.Severity.Name,
                        threatEvent.Severity.TextColor, threatEvent.Severity.BackColor, true, true, 75));
                    items.Add(new TextRow("Threat Type", threatEvent.ThreatType.Name, null, null, new [] {threatEvent.ThreatTypeId}));
                    items.Add(new TextRow("Description", threatEvent.Description));
                    items.Add(new TextRow("Associated To", 
                        $"{threatEvent.Parent.Name}",
                        $"[{model.GetIdentityTypeInitial(threatEvent.Parent)}] ",
                        null,
                        new [] {threatEvent.ParentId}));
                    items.Add(new TableRow("Approved Mitigations", new []
                    {
                        new TableColumn("Mitigation", 350),
                        new TableColumn("Severity", 75),
                        new TableColumn("Strength", 75)
                    }, GetCells(threatEvent.Mitigations?.Where(x => x.Status == MitigationStatus.Approved))));
                    items.Add(new TableRow("Existing Mitigations", new[]
                    {
                        new TableColumn("Mitigation", 350),
                        new TableColumn("Severity", 75),
                        new TableColumn("Strength", 75)
                    }, GetCells(threatEvent.Mitigations?.Where(x => x.Status == MitigationStatus.Existing))));
                    items.Add(new TableRow("Implemented Mitigations", new[]
                    {
                        new TableColumn("Mitigation", 350),
                        new TableColumn("Severity", 75),
                        new TableColumn("Strength", 75)
                    }, GetCells(threatEvent.Mitigations?.Where(x => x.Status == MitigationStatus.Implemented))));
                    items.Add(new TableRow("Planned Mitigations", new[]
                    {
                        new TableColumn("Mitigation", 350),
                        new TableColumn("Severity", 75),
                        new TableColumn("Strength", 75)
                    }, GetCells(threatEvent.Mitigations?.Where(x => x.Status == MitigationStatus.Planned))));
                    items.Add(new TableRow("Proposed Mitigations", new[]
                    {
                        new TableColumn("Mitigation", 350),
                        new TableColumn("Severity", 75),
                        new TableColumn("Strength", 75)
                    }, GetCells(threatEvent.Mitigations?.Where(x => x.Status == MitigationStatus.Proposed))));

                    var itemRows = threatEvent.GetItemRows()?.ToArray();
                    if (itemRows?.Any() ?? false)
                        items.AddRange(itemRows);

                    list.Add(new ListItem(threatEvent.Name, threatEvent.Id, items));
                }

                result = list;
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
                    cells.Add(new Cell(item.Mitigation.Name, null, null, new [] {item.MitigationId}));
                    cells.Add(new Cell(item.ThreatEvent.Severity.Name, item.ThreatEvent.Severity.TextColor, item.ThreatEvent.Severity.BackColor, false, true));
                    cells.Add(new Cell(item.Strength.Name));
                }

                result = cells;
            }

            return result;
        }
    }
}
