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
    [Extension("22CC8EDC-2559-44FD-9C1F-AA21CCBB8F1C", "Item Template Names List Placeholder", 55, ExecutionMode.Business)]
    public class ListItemTemplateNamesPlaceholderFactory : IPlaceholderFactory
    {
        public string Qualifier => "ListItemTemplateNames";

        public IPlaceholder Create(string parameters = null)
        {
            return new ListItemTemplateNamesPlaceholder();
        }
    }

    public class ListItemTemplateNamesPlaceholder : ITextEnumerationPlaceholder
    {
        public string Name => "ItemTemplateNames";
        public string Label => "Item Template Names";
        public PlaceholderSection Section => PlaceholderSection.List;
        public Bitmap Image => null;

        public IEnumerable<string> GetTextEnumeration([NotNull] IThreatModel model)
        {
            IEnumerable<string> result = null;

            var list = new List<string>();

            var externalInteractors = model.EntityTemplates?
                .Where(x => x.EntityType == EntityType.ExternalInteractor)
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .ToArray();
            if (externalInteractors?.Any() ?? false)
                list.AddRange(externalInteractors);

            var processes = model.EntityTemplates?
                .Where(x => x.EntityType == EntityType.Process)
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .ToArray();
            if (processes?.Any() ?? false)
                list.AddRange(processes);

            var dataStores = model.EntityTemplates?
                .Where(x => x.EntityType == EntityType.DataStore)
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .ToArray();
            if (dataStores?.Any() ?? false)
                list.AddRange(dataStores);

            var flows = model.FlowTemplates?
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .ToArray();
            if (flows?.Any() ?? false)
                list.AddRange(flows);

            var trustBoundaries = model.TrustBoundaryTemplates?
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .ToArray();
            if (trustBoundaries?.Any() ?? false)
                list.AddRange(trustBoundaries);

            if (list.Any())
                result = list;

            return result;
        }
    }
}
