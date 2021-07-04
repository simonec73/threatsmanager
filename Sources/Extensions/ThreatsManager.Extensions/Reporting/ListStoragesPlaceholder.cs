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
    [Extension("BEC6CA39-77F4-4923-8BBD-44405388B8EF", "Data Store List Placeholder", 41, ExecutionMode.Business)]
    public class ListStoragesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListStorages";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListStoragesPlaceholder();
        }
    }

    public class ListStoragesPlaceholder : IListPlaceholder
    {
        public string Name => "Data Stores";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.storage_small;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var dataStores = model.Entities?.OfType<IDataStore>().OrderBy(x => x.Name).ToArray();

            if (dataStores?.Any() ?? false)
            {
                var dict = new Dictionary<string, IPropertyType>();

                foreach (var dataStore in dataStores)
                {
                    var properties = dataStore.Properties?
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

            var entities = model.Entities?.OfType<IDataStore>().OrderBy(x => x.Name).ToArray();

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
