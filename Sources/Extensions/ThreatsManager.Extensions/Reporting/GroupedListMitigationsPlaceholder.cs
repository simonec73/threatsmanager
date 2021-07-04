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
    [Extension("168C7073-44EA-41C8-8885-830B2FD804B4", "Mitigation List Placeholder", 44, ExecutionMode.Business)]
    public class GroupedListMitigationsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "GroupedListMitigations";

        public IPlaceholder Create(string parameters = null)
        {
            var placeholder = new GroupedListMitigationsPlaceholder();
            var split = parameters?.Split('#');
            if ((split?.Any() ?? false) && split.Length == 3)
            {
                placeholder.SchemaNamespace = split[0];
                placeholder.SchemaName = split[1];
                placeholder.PropertyName = split[2];
            }

            return placeholder;
        }
    }

    public class GroupedListMitigationsPlaceholder : IGroupedListPlaceholder
    {
        public string Name => "Mitigations (grouped)";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.mitigations_small;

        public bool Tabular => true;

        public string SchemaNamespace { get; set; }
        public string SchemaName { get; set; }
        public string PropertyName { get; set; }

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var mitigations = model.GetUniqueMitigations()?
                .OrderBy(x => x.Name)
                .ToArray();

            if (mitigations?.Any() ?? false)
            {
                var dict = new Dictionary<string, IPropertyType>();

                foreach (var mitigation in mitigations)
                {
                    var properties = mitigation.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && !x.PropertyType.DoNotPrint &&
                                    model.GetSchema(x.PropertyType.SchemaId) is IPropertySchema schema && schema.Visible &&
                                    (string.CompareOrdinal(schema.Namespace, SchemaNamespace) != 0 ||
                                     string.CompareOrdinal(schema.Name, SchemaName) != 0 ||
                                     string.CompareOrdinal(x.PropertyType.Name, PropertyName) != 0))
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
                    .ThenBy(x => x.Value.Priority)
                    .ThenBy(x => x.Key)
                    .ToArray();
            }

            return result;
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<ListItem>>> GetList(IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IEnumerable<ListItem>>> result = null;

            var mitigations = model.GetUniqueMitigations()?
                .OrderBy(x => x.Name)
                .ToArray();

            if ((mitigations?.Any() ?? false) && !string.IsNullOrWhiteSpace(SchemaNamespace) &&
                !string.IsNullOrWhiteSpace(SchemaName) && !string.IsNullOrWhiteSpace(PropertyName))
            {
                var list = new List<KeyValuePair<string, IEnumerable<ListItem>>>();

                var propertyType = model.GetSchema(SchemaName, SchemaNamespace)?.GetPropertyType(PropertyName);
                if (propertyType != null)
                {
                    var possibleValues = mitigations
                        .Where(x => x.HasProperty(propertyType))
                        .Select(x => x.GetProperty(propertyType)?.StringValue)
                        .OrderBy(x => x)
                        .ToArray();

                    if (possibleValues.Any())
                    {
                        foreach (var possibleValue in possibleValues)
                        {
                            var selectedMitigations = mitigations
                                .Where(x => x.HasProperty(propertyType) &&
                                            string.CompareOrdinal(x.GetProperty(propertyType).StringValue, possibleValue) == 0)
                                .ToArray();

                            var listItems = GetListItems(model, selectedMitigations)?.ToArray();
                            if (listItems?.Any() ?? false)
                            {
                                list.Add(new KeyValuePair<string, IEnumerable<ListItem>>(possibleValue, listItems));
                            }
                        }
                    }

                    var remainingMitigations = mitigations.Where(x => !x.HasProperty(propertyType)).ToArray();
                    var remainingListItems = GetListItems(model, remainingMitigations)?.ToArray();
                    if (remainingListItems?.Any() ?? false)
                    {
                        list.Add(new KeyValuePair<string, IEnumerable<ListItem>>("<undefined>", remainingListItems));
                    }
                }

                result = list;
            }

            return result;
        }

        private IEnumerable<ListItem> GetListItems([NotNull] IThreatModel model, IEnumerable<IMitigation> mitigations)
        {
            IEnumerable<ListItem> result = null;

            if (mitigations?.Any() ?? false)
            {
                var list = new List<ListItem>();

                foreach (var mitigation in mitigations)
                {
                    var items = new List<ItemRow>();

                    items.Add(new TextRow("Control Type", mitigation.ControlType.GetEnumLabel()));
                    items.Add(new TextRow("Description", mitigation.Description));

                    var properties = mitigation.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && !x.PropertyType.DoNotPrint &&
                                    model.GetSchema(x.PropertyType.SchemaId) is IPropertySchema schema && schema.Visible &&
                                    (string.CompareOrdinal(schema.Namespace, SchemaNamespace) != 0 ||
                                     string.CompareOrdinal(schema.Name, SchemaName) != 0 ||
                                     string.CompareOrdinal(x.PropertyType.Name, PropertyName) != 0))
                        .OrderBy(x => model.GetSchema(x.PropertyType.SchemaId).Priority)
                        .ThenBy(x => x.PropertyType.Priority)
                        .ThenBy(x => x.PropertyType.Name)
                        .Select(x => ItemRow.Create(mitigation, x))
                        .ToArray();
                    if (properties?.Any() ?? false)
                        items.AddRange(properties);

                    list.Add(new ListItem(mitigation.Name, mitigation.Id, items));
                }

                result = list;
            }

            return result;
        }
    }
}
