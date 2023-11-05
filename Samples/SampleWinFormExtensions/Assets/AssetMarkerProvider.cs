using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    public class AssetMarkerProvider : IMarkerProvider
    {
        private readonly IEntity _entity;
        private List<AssetPanelItem> _items;

        public AssetMarkerProvider([NotNull] IEntity entity)
        {
            _entity = entity;
            if (entity is IUndoable undoable)
                undoable.Undone += HandleOperationUndone;
        }

        public bool Visible => GetAssets()?.Any() ?? false;

        public event Action<IMarkerProvider> StatusUpdated;

        public void Dispose()
        {
            if (_entity is IUndoable undoable)
                undoable.Undone -= HandleOperationUndone;

            if (_items?.Any() ?? false)
            {
                foreach (var item in _items)
                {
                    item.Updated -= Item_Updated;
                    item.Removed -= Item_Removed;
                }

                _items.Clear();
            }
        }

        public Image GetIcon(int size)
        {
            Image result = null;

            if (Visible)
            {
                result = Icons.Resources.undefined_big;

                if (result.Width != size)
                {
                    try
                    {
                        var temp = new Bitmap(size, size);
                        using (Graphics g = Graphics.FromImage(temp))
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(result, 0, 0, size, size);
                        }

                        result = temp;
                    }
                    catch
                    {
                        // Ignore and return the not resized bitmap.
                    }
                }
            }

            return result;
        }

        public IEnumerable<PanelItem> GetPanelItems()
        {
            var oldItems = _items?.ToArray();
            if (oldItems?.Any() ?? false)
            {
                foreach (var item in oldItems)
                {
                    item.Updated -= Item_Updated;
                    item.Removed -= Item_Removed;
                }

                _items = null;
            }

            _items = GetAssets()?.OrderBy(x => x.Name)
                .Select(x => new AssetPanelItem(x))?.ToList();
            if (_items?.Any() ?? false)
            {
                foreach (var item in _items)
                {
                    item.Updated += Item_Updated;
                    item.Removed += Item_Removed;
                }
            }

            return _items;
        }

        private void Item_Removed(PanelItem item)
        {
            if (item is AssetPanelItem assetPanelItem)
            {
                _items?.Remove(assetPanelItem);
                assetPanelItem.Updated -= Item_Updated;
                assetPanelItem.Removed -= Item_Removed;
                StatusUpdated?.Invoke(this);
            }
        }

        private void Item_Updated(PanelItem item)
        {
            StatusUpdated?.Invoke(this);
        }

        #region Private members to manage received events.
        private void HandleOperationUndone(object undoableObject, bool removed)
        {
            StatusUpdated?.Invoke(this);
        }
        #endregion

        #region Other member functions.
        private IEnumerable<Asset> GetAssets()
        {
            IEnumerable<Asset> result = null;

            if (_entity != null)
            {
                var schemaManager = new AssetPropertySchemaManager(_entity.Model);
                var propertyType = schemaManager.AssetPropertyType;
                if (propertyType != null)
                {
                    var property = _entity.GetProperty(propertyType);
                    if (property is IPropertyJsonSerializableObject jsonSerializableObject &&
                        jsonSerializableObject.Value is Assets assets)
                    {
                        result = assets.Items;
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
