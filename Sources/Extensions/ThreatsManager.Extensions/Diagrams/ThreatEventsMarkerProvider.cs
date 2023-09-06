using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Diagrams
{
    public class ThreatEventsMarkerProvider : IMarkerProvider
    {
        private readonly IThreatEventsContainer _container;
        private List<ThreatEventPanelItem> _items;

        public ThreatEventsMarkerProvider([NotNull] IThreatEventsContainer container) 
        {
            _container = container;
            _container.ThreatEventAdded += ThreatAdded;
            _container.ThreatEventRemoved += ThreatRemoved;
            if (_container is IUndoable undoable)
                undoable.Undone += HandleOperationUndone;

            var threatEvents = container.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    threatEvent.ThreatEventMitigationAdded += MitigationAdded;
                    threatEvent.ThreatEventMitigationRemoved += MitigationRemoved;
                    if (threatEvent is IUndoable undoableTE)
                        undoableTE.Undone += HandleOperationUndone;

                    var mitigations = threatEvent.Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var mitigation in mitigations)
                        {
                            ((INotifyPropertyChanged)mitigation).PropertyChanged += MitigationPropertyChanged;
                            if (mitigation is IUndoable undoableM)
                                undoableM.Undone += HandleOperationUndone;
                        }
                    }
                }
            }
        }

        public bool Visible => _container?.ThreatEvents?.Any() ?? false;

        public event Action<IMarkerProvider> StatusUpdated;

        public void Dispose()
        {
            if (_container != null)
            {
                _container.ThreatEventAdded -= ThreatAdded;
                _container.ThreatEventRemoved -= ThreatRemoved;

                if (_container is IUndoable undoable)
                    undoable.Undone -= HandleOperationUndone;

                var threatEvents = _container.ThreatEvents?.ToArray();
                if (threatEvents?.Any() ?? false)
                {
                    foreach (var threatEvent in threatEvents)
                    {
                        threatEvent.ThreatEventMitigationAdded -= MitigationAdded;
                        threatEvent.ThreatEventMitigationRemoved -= MitigationRemoved;
                        if (threatEvent is IUndoable undoableTE)
                            undoableTE.Undone -= HandleOperationUndone;

                        var mitigations = threatEvent.Mitigations?.ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            foreach (var mitigation in mitigations)
                            {
                                ((INotifyPropertyChanged)mitigation).PropertyChanged -= MitigationPropertyChanged;
                                if (mitigation is IUndoable undoableM)
                                    undoableM.Undone -= HandleOperationUndone;
                            }
                        }
                    }
                }
            }

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

            var visible = GetStatistics(out var notMitigated, out var partial, out var complete);

            if (visible)
            {
                if (notMitigated == 0 && partial == 0)
                {
                    result = Icons.Resources.threats_circle_green_big;
                }
                else
                {
                    if (partial == 0)
                    {
                        if (complete == 0)
                        {
                            // Only not mitigated
                            result = Icons.Resources.threats_circle_big;
                        }
                        else
                        {
                            // Some complete and some non mitigated.
                            result = Icons.Resources.threats_circle_orange_big;
                        }
                    }
                    else
                    {
                        // Only partial
                        result = Icons.Resources.threats_circle_orange_big;
                    }
                }

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
            if (item is ThreatEventPanelItem threatEventPanelItem)
            {
                _items?.Remove(threatEventPanelItem);
                threatEventPanelItem.Updated -= Item_Updated;
                threatEventPanelItem.Removed -= Item_Removed;
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

        private void ThreatAdded(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            StatusUpdated?.Invoke(this);
            threatEvent.ThreatEventMitigationAdded += MitigationAdded;
            threatEvent.ThreatEventMitigationRemoved += MitigationRemoved;
            if (threatEvent is IUndoable undoableTE)
                undoableTE.Undone += HandleOperationUndone;
        }

        private void ThreatRemoved(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            StatusUpdated?.Invoke(this);
            threatEvent.ThreatEventMitigationAdded -= MitigationAdded;
            threatEvent.ThreatEventMitigationRemoved -= MitigationRemoved;
            if (threatEvent is IUndoable undoableTE)
                undoableTE.Undone -= HandleOperationUndone;
        }

        private void MitigationAdded(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            StatusUpdated?.Invoke(this);
            ((INotifyPropertyChanged)mitigation).PropertyChanged += MitigationPropertyChanged;
            if (mitigation is IUndoable undoableM)
                undoableM.Undone += HandleOperationUndone;
        }

        private void MitigationRemoved(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            StatusUpdated?.Invoke(this);
            ((INotifyPropertyChanged)mitigation).PropertyChanged -= MitigationPropertyChanged;
            if (mitigation is IUndoable undoableM)
                undoableM.Undone -= HandleOperationUndone;
        }

        private void MitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEventMitigation && string.CompareOrdinal(e.PropertyName, "Strength") == 0)
                StatusUpdated?.Invoke(this);
        }
        #endregion

        #region Other member functions.
        private bool GetStatistics(out int notMitigated, out int partial, out int complete)
        {
            bool result = false;

            var threatEvents = _container.ThreatEvents?.ToArray();
            notMitigated = 0;
            partial = 0;
            complete = 0;

            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    switch (threatEvent.GetMitigationLevel())
                    {
                        case MitigationLevel.NotMitigated:
                            notMitigated++;
                            break;
                        case MitigationLevel.Partial:
                            partial++;
                            break;
                        case MitigationLevel.Complete:
                            complete++;
                            break;
                    }
                }

                result = true;
            }

            return result;
        }
        #endregion
    }
}
