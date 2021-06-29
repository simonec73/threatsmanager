using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("BA09D71A-9617-466F-B998-3763D9C010C9", "Trust Boundary Placeholder", 43, ExecutionMode.Business)]
    public class ListTrustBoundariesPlaceholder : IListPlaceholder
    {
        public string Qualifier => "ListTrustBoundaries";
        public bool Tabular => true;

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
                    var properties = boundary.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && !x.PropertyType.DoNotPrint &&
                                    (model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false))
                        .OrderBy(x => model.GetSchema(x.PropertyType.SchemaId).Priority)
                        .ThenBy(x => x.PropertyType.Priority)
                        .Select(ItemRow.Create)
                        .ToArray();
                    if (properties?.Any() ?? false)
                        items.AddRange(properties);
                }

                result = list;
            }

            return result;
        }
    }
}
