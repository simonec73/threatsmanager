using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Diagrams.ThreatEventsMaker
{
    public class ThreatEventMarkerProvider : IMarkerProvider
    {
        private readonly IThreatEventsContainer _container;
        private List<ThreatEventPanelItem> _items;

        public ThreatEventMarkerProvider([NotNull] IThreatEventsContainer container)
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
                    ((INotifyPropertyChanged)threatEvent).PropertyChanged += ThreatEventPropertyChanged;
                    if (threatEvent is IUndoable undoableTE)
                        undoableTE.Undone += HandleOperationUndone;
                }
            }
        }

        private void ThreatEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEvent && string.CompareOrdinal(e.PropertyName, "Severity") == 0)
            {

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
                        ((INotifyPropertyChanged)threatEvent).PropertyChanged -= ThreatEventPropertyChanged;
                        if (threatEvent is IUndoable undoableTE)
                            undoableTE.Undone -= HandleOperationUndone;
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

            var severity = _container?.ThreatEvents?
                .OrderByDescending(x => x.Severity, new SeverityComparer())
                .FirstOrDefault()?.Severity;

            if (severity != null)
            {
                try
                {
                    var temp = new Bitmap(size, size);
                    using (Graphics g = Graphics.FromImage(temp))
                    {
                        using (var brush = new SolidBrush(Color.FromKnownColor(severity.BackColor)))
                        {
                            g.FillEllipse(brush, 0, 0, size, size);
                        }
                    }

                    result = temp;
                }
                catch
                {
                    // Ignore and return the not resized bitmap.
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

            _items = _container?.ThreatEvents?.OrderBy(x => x.Name)
                .Select(x => new ThreatEventPanelItem(x))?.ToList();
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
            ((INotifyPropertyChanged)threatEvent).PropertyChanged += ThreatEventPropertyChanged;
            if (threatEvent is IUndoable undoableTE)
                undoableTE.Undone += HandleOperationUndone;
        }

        private void ThreatRemoved(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            StatusUpdated?.Invoke(this);
            ((INotifyPropertyChanged)threatEvent).PropertyChanged -= ThreatEventPropertyChanged;
            if (threatEvent is IUndoable undoableTE)
                undoableTE.Undone -= HandleOperationUndone;
        }
        #endregion
    }
}
