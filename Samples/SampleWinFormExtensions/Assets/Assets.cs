using Newtonsoft.Json;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Recording;
using System.Collections.Generic;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    [JsonObject(MemberSerialization.OptIn)]
    [Recordable(AutoRecord = false)]
    class Assets
    {
        public Assets()
        {

        }

        [JsonProperty("assets")]
        [Child]
        private AdvisableCollection<Asset> _items { get; set; }

        public IEnumerable<Asset> Items => _items?.AsEnumerable();

        public void AddAsset([NotNull] Asset asset, [NotNull] IThreatModel model)
        {
            if (!(_items?.Any(x => string.CompareOrdinal(x.Name, asset.Name) == 0) ?? false)) 
            {
                using (var scope = UndoRedoManager.OpenScope("Add an asset"))
                {
                    if (_items == null)
                        _items = new AdvisableCollection<Asset>();

                    var newAsset = asset.Clone() as Asset;
                    UndoRedoManager.Attach(newAsset, model);
                    _items.Add(newAsset);
                    scope?.Complete();
                }
            }
        }

        public void RemoveAsset([Required] string name)
        {
            var asset = _items?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
            if (asset != null)
            {
                using (var scope = UndoRedoManager.OpenScope("Remove an asset"))
                {
                    UndoRedoManager.Detach(asset);
                    _items.Remove(asset);
                    scope?.Complete();
                }
            }
        }

        public Asset Get([Required] string name)
        {
            return _items?.FirstOrDefault(x => string.CompareOrdinal(x.Name, name) == 0);
        }
    }
}
