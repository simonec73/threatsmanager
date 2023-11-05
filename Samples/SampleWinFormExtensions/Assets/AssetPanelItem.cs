using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using System.Drawing;
using System.Text;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    public class AssetPanelItem : PanelItem
    {
        public AssetPanelItem([Required] Asset asset) : base(asset.Name) 
        {
            Tag = asset;
        }

        public override Image Icon => Icons.Resources.undefined_small;

        public override ClickAction ActionOnClick => ClickAction.ShowTooltip;

        public override string TooltipText
        {
            get
            {
                string result = null;

                if (Tag is Asset asset)
                {
                    var builder = new StringBuilder();
                    builder.AppendLine(asset.Description);
                    builder.AppendLine($"Value: {asset.Value}");

                    result = builder.ToString();
                }

                return result;
            }
        }

        public override TooltipAction TooltipAction => TooltipAction.Edit | TooltipAction.Remove;

        public override void EditAction(object target)
        {
            if (target is IEntity entity && Tag is Asset asset)
            {
                var dialog = new CreateAssetDialog(entity.Model, asset);
                dialog.Show();

                base.EditAction(target);
            }
        }

        public override void RemoveAction(object target)
        {
            if (target is IEntity entity && Tag is Asset asset)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove assigned Asset"))
                {
                    var schemaManager = new AssetPropertySchemaManager(entity.Model);
                    var propertyType = schemaManager.AssetPropertyType;
                    if (propertyType != null)
                    {
                        var property = entity.GetProperty(propertyType);
                        if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                        {
                            var assets = jsonSerializableObject.Value as Assets;
                            if (assets != null)
                            {
                                assets.RemoveAsset(asset.Name);
                            }
                        }

                        scope?.Complete();
                    }
                }

                base.RemoveAction(target);
            }
        }
    }
}
