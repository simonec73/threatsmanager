using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("763A3C06-79FF-4191-8228-4E021AD3A6FB", "Process List Placeholder", 40, ExecutionMode.Business)]
    public class ListProcessesPlaceholder : IListPlaceholder
    {
        public string Qualifier => "ListProcesses";
        public bool Tabular => true;

        public IEnumerable<ListItem> GetList(IThreatModel model)
        {
            IEnumerable<ListItem> result = null;

            var entities = model.Entities?.OfType<IProcess>().OrderBy(x => x.Name).ToArray();

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
                }

                result = list;
            }

            return result;
        }
    }
}
