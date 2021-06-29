using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("F7289F51-D54E-4574-A193-5DE1D2368DFF", "External Interactor List Placeholder", 39, ExecutionMode.Business)]
    public class ListExternalInteractorsPlaceholder : IListPlaceholder
    {
        public string Qualifier => "ListExternalInteractors";
        public bool Tabular => true;

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
                        .Select(ItemRow.Create)
                        .ToArray();
                    if (properties?.Any() ?? false)
                        items.AddRange(properties);

                    list.Add(new ListItem(entity.Name, items));
                }

                result = list;
            }

            return result;
        }
    }
}
