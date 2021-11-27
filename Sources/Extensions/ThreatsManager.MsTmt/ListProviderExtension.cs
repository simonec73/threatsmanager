using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Extensions
{
    [Extension("2D9A45F4-86D1-4573-BEED-EF32B0A5D1AB", "MS TMT List Provider", 25, ExecutionMode.Simplified)]
    public class ListProviderExtension : IListProviderExtension
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