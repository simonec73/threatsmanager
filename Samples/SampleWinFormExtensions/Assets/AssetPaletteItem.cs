using ThreatsManager.Utilities;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    internal class AssetPaletteItem : PaletteItem
    {
        public AssetPaletteItem(string name) : base(name) { }

        public override void Apply(object target)
        {
            if (target is IEntity entity && Tag is Asset asset)
            {
                using (var scope = UndoRedoManager.OpenScope("Create Asset on entity"))
                {
                    var schemaManager = new AssetPropertySchemaManager(entity.Model);
                    var propertyType = schemaManager.AssetPropertyType;
                    if (propertyType != null)
                    {
                        var property = entity.GetProperty(propertyType);
                        if (property == null)
                        {
                            property = entity.AddProperty(propertyType, null);
                        }

                        if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                        {
                            var assets = jsonSerializableObject.Value as Assets;
                            if (assets == null)
                            {
                                assets = new Assets();
                                jsonSerializableObject.Value = assets;
                            }

                            assets.AddAsset(asset, entity.Model);
                        }

                        scope?.Complete();
                    }
                }
            }
        }
    }
}
