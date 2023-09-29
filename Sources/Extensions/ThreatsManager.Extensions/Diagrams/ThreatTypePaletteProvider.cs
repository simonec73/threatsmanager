using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using PostSharp.Patterns.Contracts;
using System.Drawing;

namespace ThreatsManager.Extensions.Diagrams
{
    [Extension("EBF57601-6795-4C9A-855D-58306CB5DC0C", "Threat Type Palette Provider", 10, ExecutionMode.Simplified)]
    public class ThreatTypePaletteProvider : IPaletteProvider
    {
        public string Name => "Threat Types";

        public Image Icon => Icons.Resources.threat_types;

        public IEnumerable<PaletteItem> GetPaletteItems([NotNull] IThreatModel model, string filter)
        {
            IEnumerable<ThreatTypePaletteItem> result;

            if (string.IsNullOrWhiteSpace(filter))
            {
                result = model?.ThreatTypes?
                    .OrderBy(x => x.Name)
                    .Select(x => new ThreatTypePaletteItem(x.Name)
                    {
                        Tag = x
                    });
            }
            else
            {
                result = model?.SearchThreatTypes(filter)
                    .Select(x => new ThreatTypePaletteItem(x.Name)
                    {
                        Tag = x
                    });
            }

            return result;
        }
    }
}
