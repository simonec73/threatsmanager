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
    [Extension("F7289F51-D54E-4574-A193-5DE1D2368DFF", "External Interactor List Placeholder", 39, ExecutionMode.Business)]
    public class ListExternalInteractorsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListExternalInteractors";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListExternalInteractorsPlaceholder();
        }
    }

    public class ListExternalInteractorsPlaceholder : IListPlaceholder
    {
        public string Name => "External Interactors";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.external_small;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var entities = model.Entities?.OfType<IExternalInteractor>().OrderBy(x => x.Name).ToArray();

            if (entities?.Any() ?? false)
            {
                var dict = new Dictionary<string, IPropertyType>();

                foreach (var entity in entities)
                {
                    var properties = entity.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && !x.PropertyType.DoNotPrint &&
                                    (model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false))
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

        public IEnumerable<ListItem> GetList(IThreatModel model)
        {
            IEnumerable<ListItem> result = null;

            var entities = model.Entities?.OfType<IExternalInteractor>().OrderBy(x => x.Name).ToArray();

            if (entities?.Any() ?? false)
            {
                var list = new List<ListItem>();

                foreach (var entity in entities)
                {
                    List<ItemRow> items = new List<ItemRow>();
                    items.Add(new TextRow("Description", entity.Description));
                    var properties = entity.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && !x.PropertyType.DoNotPrint &&
                                    (model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false))
                        .OrderBy(x => model.GetSchema(x.PropertyType.SchemaId).Priority)
                        .ThenBy(x => x.PropertyType.Priority)
                        .ThenBy(x => x.PropertyType.Name)
                        .Select(x => ItemRow.Create(entity, x))
                        .ToArray();
                    if (properties?.Any() ?? false)
                        items.AddRange(properties);

                    list.Add(new ListItem(entity.Name, entity.Id, items));
                }

                result = list;
            }

            return result;
        }
    }
}
