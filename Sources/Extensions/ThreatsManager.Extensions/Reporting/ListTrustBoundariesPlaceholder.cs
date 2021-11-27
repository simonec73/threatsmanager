using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("BA09D71A-9617-466F-B998-3763D9C010C9", "Trust Boundary Placeholder", 43, ExecutionMode.Business)]
    public class ListTrustBoundariesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListTrustBoundaries";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListTrustBoundariesPlaceholder();
        }
    }

    public class ListTrustBoundariesPlaceholder : IListPlaceholder
    {
        public string Name => "TrustBoundaries";
        public string Label => "Trust Boundaries";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.trust_boundary_small;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var trustBoundaries = model.Groups?.OfType<ITrustBoundary>().OrderBy(x => x.Name).ToArray();

            if (trustBoundaries?.Any() ?? false)
            {
                var dict = new Dictionary<string, IPropertyType>();

                foreach (var trustBoundary in trustBoundaries)
                {
                    var properties = trustBoundary.Properties?
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

            var boundaries = model.Groups?.OfType<ITrustBoundary>().OrderBy(x => x.Name).ToArray();

            if (boundaries?.Any() ?? false)
            {
                var list = new List<ListItem>();

                foreach (var boundary in boundaries)
                {
                    var items = new List<ItemRow>();
                    items.Add(new TextRow("Description", boundary.Description));
                    var itemRows = boundary.GetItemRows()?.ToArray();
                    if (itemRows?.Any() ?? false)
                        items.AddRange(itemRows);

                    list.Add(new ListItem(boundary.Name, boundary.Id, items));
                }

                result = list;
            }

            return result;
        }
    }
}
