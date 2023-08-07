using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using System.Drawing;
using ThreatsManager.Extensions.Diagrams;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    public class AssetPanelItem : PanelItem
    {
        public AssetPanelItem([Required] Asset asset) : base(asset.Name) 
        {
            Tag = asset;
        }

        public override Image Icon => Icons.Resources.undefined_small;
    }
}
