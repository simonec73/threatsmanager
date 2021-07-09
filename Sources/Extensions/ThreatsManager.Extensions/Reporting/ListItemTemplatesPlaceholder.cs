using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("C04AB978-519D-42C7-8332-BD2A5914389E", "Item Templates List Placeholder", 54, ExecutionMode.Business)]
    public class ListItemTemplatesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListItemTemplates";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListItemTemplatesPlaceholder();
        }
    }

    public class ListItemTemplatesPlaceholder : IListPlaceholder
    {
        public string Name => "ItemTemplates";
        public string Label => "Item Templates";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => null;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var dict = new Dictionary<string, IPropertyType>();

            var entities = model.EntityTemplates?.ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    var properties = entity.Properties?
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
            }

            var flows = model.FlowTemplates?.ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    var properties = flow.Properties?
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
            }

            var trustBoundaries = model.TrustBoundaryTemplates?.ToArray();
            if (trustBoundaries?.Any() ?? false)
            {
                foreach (var trustBoundary in trustBoundaries)
                {
                    var properties = trustBoundary.Properties?
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
            }

            if (dict.Any())
            {
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

            var list = new List<ListItem>();
            AddEntityTemplates(list, model, EntityType.ExternalInteractor);
            AddEntityTemplates(list, model, EntityType.Process);
            AddEntityTemplates(list, model, EntityType.DataStore);
            AddFlowTemplates(list, model);
            AddTrustBoundaryTemplates(list, model);

            if (list.Any())
                result = list;

            return result;
        }

        private void AddEntityTemplates([NotNull] List<ListItem> list, [NotNull] IThreatModel model, EntityType entityType)
        {
            var entities = model.EntityTemplates?
                .Where(x => x.EntityType == entityType)
                .OrderBy(x => x.Name).ToArray();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    List<ItemRow> items = new List<ItemRow>();
                    items.Add(new TextRow("Description", entity.Description));
                    items.Add(new TextRow("Item Type", entity.EntityType.GetEnumLabel()));
                    var itemRows = entity.GetItemRows()?.ToArray();
                    if (itemRows?.Any() ?? false)
                        items.AddRange(itemRows);
                    list.Add(new ListItem(entity.Name, entity.Id, items));
                }
            }
        }

        private void AddFlowTemplates([NotNull] List<ListItem> list, [NotNull] IThreatModel model)
        {
            var flows = model.FlowTemplates?
                .OrderBy(x => x.Name).ToArray();
            if (flows?.Any() ?? false)
            {
                foreach (var flow in flows)
                {
                    List<ItemRow> items = new List<ItemRow>();
                    items.Add(new TextRow("Description", flow.Description));
                    items.Add(new TextRow("Flow Type", flow.FlowType.GetEnumLabel()));
                    var itemRows = flow.GetItemRows()?.ToArray();
                    if (itemRows?.Any() ?? false)
                        items.AddRange(itemRows);
                    list.Add(new ListItem(flow.Name, flow.Id, items));
                }
            }
        }

        private void AddTrustBoundaryTemplates([NotNull] List<ListItem> list, [NotNull] IThreatModel model)
        {
            var trustBoundaries = model.TrustBoundaryTemplates?
                .OrderBy(x => x.Name).ToArray();
            if (trustBoundaries?.Any() ?? false)
            {
                foreach (var trustBoundary in trustBoundaries)
                {
                    List<ItemRow> items = new List<ItemRow>();
                    items.Add(new TextRow("Description", trustBoundary.Description));
                    var itemRows = trustBoundary.GetItemRows()?.ToArray();
                    if (itemRows?.Any() ?? false)
                        items.AddRange(itemRows);
                    list.Add(new ListItem(trustBoundary.Name, trustBoundary.Id, items));
                }
            }
        }
    }
}
