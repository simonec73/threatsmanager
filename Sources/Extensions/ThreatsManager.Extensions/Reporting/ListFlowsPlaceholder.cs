using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    [Extension("4ABFFA6B-D690-4124-9299-D730D32633D9", "Flow List Placeholder", 42, ExecutionMode.Business)]
    public class ListFlowsPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListFlows";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListFlowsPlaceholder();
        }
    }

    public class ListFlowsPlaceholder : IListPlaceholder
    {
        public string Name => "Flows";
        public string Label => "Flows";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => Icons.Resources.flow_small;

        public bool Tabular => true;

        public IEnumerable<KeyValuePair<string, IPropertyType>> GetProperties([NotNull] IThreatModel model)
        {
            IEnumerable<KeyValuePair<string, IPropertyType>> result = null;

            var flows = model.DataFlows?.OrderBy(x => x.Name).ToArray();

            if (flows?.Any() ?? false)
            {
                var dict = new Dictionary<string, IPropertyType>();

                foreach (var flow in flows)
                {
                    var properties = flow.Properties?
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

            var flows = model.DataFlows?.OrderBy(x => x.Name).ToArray();

            if (flows?.Any() ?? false)
            {
                var list = new List<ListItem>();

                foreach (var flow in flows)
                {
                    var items = new List<ItemRow>();
                    items.Add(new TextRow("Description", flow.Description));
                    if (flow.Source != null)
                        items.Add(new TextRow("Source", $"[{flow.Model.GetIdentityTypeInitial(flow.Source)}] {flow.Source.Name}", 
                            null, null, new []{flow.SourceId}));
                    if (flow.Target != null)
                        items.Add(new TextRow("Target", $"[{flow.Model.GetIdentityTypeInitial(flow.Target)}] {flow.Target.Name}",
                            null, null, new []{flow.TargetId}));
                    items.Add(new TextRow("Flow Type", flow.FlowType.GetEnumLabel()));
                    var itemRows = flow.GetItemRows()?.ToArray();
                    if (itemRows?.Any() ?? false)
                        items.AddRange(itemRows);

                    list.Add(new ListItem(flow.Name, flow.Id, items));
                }

                result = list;
            }

            return result;
        }
    }
}
