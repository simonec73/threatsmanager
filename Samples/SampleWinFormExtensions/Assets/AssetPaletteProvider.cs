using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    [Extension("F22ABB5B-01C9-4A06-B191-B6F58C858F8F", "Asset Palette Provider", 100, ExecutionMode.Simplified)]
    public class AssetPaletteProvider : IPaletteProvider
    {
        public string Name => "Asset";

        public Image Icon => Icons.Resources.undefined;

        public IEnumerable<PaletteItem> GetPaletteItems([NotNull] IThreatModel model, string filter)
        {
            IEnumerable<AssetPaletteItem> result = null;

            var schemaManager = new AssetPropertySchemaManager(model);
            var propertyType = schemaManager.AssetPropertyType;
            if (propertyType != null) 
            {
                var property = model.GetProperty(propertyType);
                if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is Assets assets)
                {
                    if (string.IsNullOrWhiteSpace(filter))
                    {
                        result = assets.Items?
                            .OrderBy(x => x.Name)
                            .Select(x => new AssetPaletteItem(x.Name)
                        {
                            Tag = x
                        });
                    }
                    else
                    {
                        result = assets.Items?
                            .Where(x => Matches(x, filter))
                            .OrderBy(x => x.Name)
                            .Select(x => new AssetPaletteItem(x.Name)
                            {
                                Tag = x
                            });
                    }
                }
            }

            return result;
        }

        private bool Matches([NotNull] Asset asset, [Required] string filter)
        {
            return (asset.Name?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
                (asset.Description?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0;
        }
    }
}
