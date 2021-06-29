using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Schemas
{
    [Extension("6046F35A-34EF-4EE7-951F-4A3A55EA18CA", "List Provider for Tmt properties", 25, ExecutionMode.Simplified)]
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