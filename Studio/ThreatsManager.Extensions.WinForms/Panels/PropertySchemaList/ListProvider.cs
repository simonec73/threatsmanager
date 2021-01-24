using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.PropertySchemaList
{
    [Extension("69F3A397-6FD2-4FBF-9E77-94A33ABA439D", "List Provider for user-defined properties", 25, ExecutionMode.Simplified)]
    public class ListProvider : IListProviderExtension
    {
        public IEnumerable<IListItem> GetAvailableItems(string context)
        {
            IEnumerable<IListItem> result = null;

            var items = context?.TagSplit();
            if (items?.Any() ?? false)
                result = new List<IListItem>(items.Select(x => new ListItem(x)));

            return result;
        }
    }
}