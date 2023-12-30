using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Diagrams;
using ThreatsManager.Extensions.Panels.DiagramConfiguration;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class AssociatedPanelItemMarker : GoImage, IDisposable
    {
        private readonly IMarkerProvider _markerProvider;
        private readonly object _referenceObject;
        private readonly int _iconSize;
        private bool _visible = false;

        private AssociatedPanelItemMarker() {
            Printable = false;
            Resizable = false;
            Deletable = false;
            Copyable = false;
            Selectable = false;

            MarkerStatusTrigger.MarkerStatusUpdated += MarkerStatusTriggerOnMarkerStatusUpdated;
            PanelItemListFormTrigger.PanelStatusUpdated += PanelItemListFormTrigger_ShowPanels;
        }

        public AssociatedPanelItemMarker([NotNull] object referenceObj) : this()
        {
            if (referenceObj is IThreatModelChild child)
            {
                _referenceObject = referenceObj;

                var configuration = new DiagramConfigurationManager(child.Model);
                if (configuration != null)
                {
                    _iconSize = configuration.DiagramMarkerSize;
                    var extensionId = configuration.MarkerExtension;
                    var factory = ExtensionUtils.GetExtension<IMarkerProviderFactory>(extensionId);
                    _markerProvider = factory?.Create(referenceObj);

                    if (_markerProvider != null)
                    {
                        _markerProvider.StatusUpdated += MarkerStatusUpdated;
                        Image = _markerProvider.GetIcon(_iconSize);
                        Visible = _markerProvider.Visible;
                    }
                }
            }
        }

        public void Dispose()
        {
            MarkerStatusTrigger.MarkerStatusUpdated -= MarkerStatusTriggerOnMarkerStatusUpdated;
            if (_markerProvider != null)
            {
                _markerProvider.StatusUpdated -= MarkerStatusUpdated;
                _markerProvider.Dispose();
            }
            PanelItemListFormTrigger.PanelStatusUpdated -= PanelItemListFormTrigger_ShowPanels;
        }

        private void MarkerStatusUpdated(IMarkerProvider provider)
        {
            if (_markerProvider != null)
            {
                Image = _markerProvider.GetIcon(_iconSize);
                Visible = _markerProvider.Visible;
            }
        }

        private void MarkerStatusTriggerOnMarkerStatusUpdated(MarkerStatus status)
        {
            Visible = _visible;
        }

        private void PanelItemListFormTrigger_ShowPanels(PanelsStatus status, GoView view)
        {
            if (status == PanelsStatus.Visible)
            {
                ShowPanelItemListForm(view, this.Location);
            }
        }

        /// <summary>
        /// Event raised when a panel item is clicked.
        /// </summary>
        /// <remarks>The returned object is the Tag associated with the Panel Item.</remarks>
        public Action<object> PanelItemClicked; 

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                _visible = value;
                base.Visible = value && (MarkerStatusTrigger.CurrentStatus == MarkerStatus.Full);
                if (_markerProvider != null)
                    Image = _markerProvider.GetIcon(_iconSize);
            }
        }

        // can't get any selection handles
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj) { }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            if (!evt.Alt && !evt.Control && !evt.DoubleClick && !evt.Shift && (evt.Buttons == MouseButtons.Left))
            {
                ShowPanelItemListForm(view, evt.DocPoint);
            }

            return base.OnSingleClick(evt, view);
        }

        public void ShowPanelItemListForm([NotNull] GoView view, PointF point)
        {
            var panelItems = _markerProvider?.GetPanelItems()?.ToArray();

            if (panelItems?.Any() ?? false)
            {
                Visible = true;

                var form = new PanelItemListForm(_referenceObject)
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = view.PointToScreen(view.ConvertDocToView(Point.Round(point)))
                };
                form.PanelItemClicked += item => PanelItemClicked?.Invoke(item);
                form.ShowPanelItems(panelItems);
                form.Show(view.FindForm());
            }
        }
    }
}