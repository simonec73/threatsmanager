using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class AssociatedThreatsMarker : GoImage, IDisposable
    {
        private IThreatEventsContainer _container;
        private bool _visible = false;

        private AssociatedThreatsMarker() {
            Printable = false;
            Resizable = false;
            Deletable = false;
            Copyable = false;
            Selectable = false;

            MarkerStatusTrigger.MarkerStatusUpdated += MarkerStatusTriggerOnMarkerStatusUpdated;
        }

        public void Dispose()
        {
            MarkerStatusTrigger.MarkerStatusUpdated -= MarkerStatusTriggerOnMarkerStatusUpdated;
            if (_container != null)
            {
                _container.ThreatEventAdded -= ThreatAdded;
                _container.ThreatEventRemoved -= ThreatRemoved;

                var threatEvents = _container.ThreatEvents?.ToArray();
                if (threatEvents?.Any() ?? false)
                {
                    foreach (var threatEvent in threatEvents)
                    {
                        threatEvent.ThreatEventMitigationAdded -= MitigationAdded;
                        threatEvent.ThreatEventMitigationRemoved -= MitigationRemoved;

                        var mitigations = threatEvent.Mitigations?.ToArray();
                        if (mitigations?.Any() ?? false)
                        {
                            foreach (var mitigation in mitigations)
                            {
                                ((INotifyPropertyChanged)mitigation).PropertyChanged -= MitigationPropertyChanged;
                            }
                        }
                    }
                }
            }
        }

        private void MarkerStatusTriggerOnMarkerStatusUpdated(MarkerStatus status)
        {
            Visible = _visible;
        }

        public AssociatedThreatsMarker([NotNull] IEntity entity) : this()
        {
            UpdateContainer(entity);
            Visible = entity.ThreatEvents?.Any() ?? false;
        }

        public AssociatedThreatsMarker([NotNull] IDataFlow dataFlow) : this()
        {
            UpdateContainer(dataFlow);
            Visible = dataFlow.ThreatEvents?.Any() ?? false;
        }

        public Action<IThreatEvent> ThreatEventClicked; 

        private void UpdateContainer([NotNull] IThreatEventsContainer container)
        {
            // TODO: Release event handlers when marker is not necessary anymore.

            _container = container;
            _container.ThreatEventAdded += ThreatAdded;
            _container.ThreatEventRemoved += ThreatRemoved;

            var threatEvents = container.ThreatEvents?.ToArray();
            if (threatEvents?.Any() ?? false)
            {
                foreach (var threatEvent in threatEvents)
                {
                    threatEvent.ThreatEventMitigationAdded += MitigationAdded;
                    threatEvent.ThreatEventMitigationRemoved += MitigationRemoved;

                    var mitigations = threatEvent.Mitigations?.ToArray();
                    if (mitigations?.Any() ?? false)
                    {
                        foreach (var mitigation in mitigations)
                        {
                            ((INotifyPropertyChanged)mitigation).PropertyChanged += MitigationPropertyChanged;
                        }
                    }
                }
            }

            UpdateMitigationLevel();
        }

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                _visible = value;
                base.Visible = value && (MarkerStatusTrigger.CurrentStatus == MarkerStatus.Full);
                UpdateMitigationLevel();
            }
        }

        // can't get any selection handles
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj) { }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            if (!evt.Alt && !evt.Control && !evt.DoubleClick && !evt.Shift && (evt.Buttons == MouseButtons.Left))
            {
                ShowThreatsDialog(view, evt.DocPoint);
            }

            return base.OnSingleClick(evt, view);
        }

        public void ShowThreatsDialog([NotNull] GoView view, PointF point)
        {
            IEnumerable<IThreatEvent> threatEvents = null;
            
            if (_container is IEntity entity && (entity.ThreatEvents?.Any() ?? false))
            {
                threatEvents = entity.ThreatEvents;
            } else if (_container is IDataFlow dataFlow && (dataFlow.ThreatEvents?.Any() ?? false))
            {
                threatEvents = dataFlow.ThreatEvents;
            }

            if (threatEvents != null)
            {
                Visible = true;

                var form = new ThreatEventListForm
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = view.PointToScreen(view.ConvertDocToView(Point.Round(point)))
                };
                form.ThreatEventClicked += threatEvent => ThreatEventClicked?.Invoke(threatEvent);
                form.ShowThreatEvents(threatEvents);
                form.Show(view.FindForm());
            }
        }
      
        private void ThreatAdded(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            UpdateMitigationLevel();
            threatEvent.ThreatEventMitigationAdded += MitigationAdded;
            threatEvent.ThreatEventMitigationRemoved += MitigationRemoved;
        }
       
        private void ThreatRemoved(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            UpdateMitigationLevel();
            threatEvent.ThreatEventMitigationAdded -= MitigationAdded;
            threatEvent.ThreatEventMitigationRemoved -= MitigationRemoved;
        }

        private void MitigationAdded(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            UpdateMitigationLevel();
            ((INotifyPropertyChanged)mitigation).PropertyChanged += MitigationPropertyChanged;
        }

        private void MitigationRemoved(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            UpdateMitigationLevel();
            ((INotifyPropertyChanged)mitigation).PropertyChanged -= MitigationPropertyChanged;
        }

        private void MitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEventMitigation mitigation && string.CompareOrdinal(e.PropertyName, "Strength") == 0)
                UpdateMitigationLevel();
        }

        private void UpdateMitigationLevel()
        {
            var threatEvents = _container.ThreatEvents?.ToArray();
            var notMitigated = 0;
            var partial = 0;
            var complete = 0;

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
            }

            if (notMitigated == 0 && partial == 0)
            {
                Image = Resources.threats_circle_green_big;
            }
            else
            {
                if (partial == 0)
                {
                    if (complete == 0)
                    {
                        // Only not mitigated
                        Image = Resources.threats_circle_big;
                    }
                    else
                    {
                        // Some complete and some non mitigated.
                        Image = Resources.threats_circle_orange_big;
                    }
                }
                else
                {
                    // Only partial
                    Image = Resources.threats_circle_orange_big;
                }
            }

            if (Dpi.Factor.Width >= 1.5)
                Size = new SizeF(32.0f, 32.0f);
            else
                Size = new SizeF(16.0f, 16.0f);
        }
    }
}