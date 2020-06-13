using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Extensions
{
    [Export(typeof(IListProviderExtension))]
    [ExportMetadata("Id", "2D9A45F4-86D1-4573-BEED-EF32B0A5D1AB")]
    [ExportMetadata("Label", "MS TMT List Provider")]
    [ExportMetadata("Priority", 25)]
    [ExportMetadata("Parameters", null)]
    [ExportMetadata("Mode", ExecutionMode.Simplified)]
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