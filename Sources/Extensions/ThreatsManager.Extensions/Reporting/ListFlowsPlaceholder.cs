using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("4ABFFA6B-D690-4124-9299-D730D32633D9", "Flow List Placeholder", 42, ExecutionMode.Business)]
    public class ListFlowsPlaceholder : IListPlaceholder
    {
        public string Qualifier => "ListFlows";
        public bool Tabular => true;

        public IEnumerable<ListItem> GetList(IThreatModel model)
        {
            IEnumerable<ListItem> result = null;

            var flows = model.DataFlows?.OrderBy(x => x.Name).ToArray();

            if (flows?.Any() ?? false)
            {
                var list = new List<ListItem>();

                foreach (var flow in flows)
                {
                    var items = new List<ItemRow>();
                    items.Add(new TextRow("Description", flow.Description));
                    if (flow.Source != null)
                        items.Add(new TextRow("Source", $"[{flow.Model.GetIdentityTypeInitial(flow.Source)}] {flow.Source.Name}"));
                    if (flow.Target != null)
                        items.Add(new TextRow("Target", $"[{flow.Model.GetIdentityTypeInitial(flow.Target)}] {flow.Target.Name}"));
                    items.Add(new TextRow("Flow Type", flow.FlowType.GetEnumLabel()));
                    var properties = flow.Properties?
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
