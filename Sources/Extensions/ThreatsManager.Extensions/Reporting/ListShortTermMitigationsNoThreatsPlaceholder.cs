using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("70B4F20B-EF59-4DA1-ADAB-E7385B443F55", "Short Term Mitigation List (no context) Placeholder", 44, ExecutionMode.Business)]
    public class ListShortTermMitigationsNoThreatsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListShortTermMitigationsNoThreats";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListShortTermMitigationsNoThreatsPlaceholder();
        }
    }

    public class ListShortTermMitigationsNoThreatsPlaceholder : IListPlaceholder
    {
        public string Name => "ShortTermMitigationsNoThreats";
        public string Label => "Short Term Mitigations (no context)";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.mitigations_small;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var schema = new RoadmapPropertySchemaManager(model);
            var mitigations = model.GetUniqueMitigations()?
                .Where(x => schema.GetStatus(x) == RoadmapStatus.ShortTerm)
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
                .Where(x => schema.GetStatus(x) == RoadmapStatus.ShortTerm)
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
    }
}
