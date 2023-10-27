using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Newtonsoft.Json.Linq;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class GraphLink : GoLabeledLink, IDisposable
    {
        private ILink _link;
        private int _markerSize;
        private AssociatedPanelItemMarker _threatsMarker;
        private bool _alternativeTextLocation;

        public event Action<ILink> SelectedLink;
        
        public event Action<IThreatEvent> SelectedThreatEvent;

        public GraphLink()
        {
            Style = GoStrokeStyle.Bezier;
            Editable = true;
            Copyable = false;
            Curviness = 10.0f;
            Orthogonal = false;
            ToArrow = true;
            ToArrowWidth = 6.5f * Dpi.Factor.Width;
            Reshapable = true;
            Selectable = true;
            DragsNode = false;
            Relinkable = false;
            PenWidth = 1.5f * Dpi.Factor.Width;

            var textControl = new GraphText(this);
            textControl.Movable = false;
            textControl.Copyable = true;
            textControl.AddObserver(this);
            MidLabel = textControl;

            var source = new GoText();
            source.Text = "R";
            source.Selectable = false;
            source.Visible = false;
            source.FontSize = 7;
            FromLabel = source;

            var target = new GoText();
            target.Text = "W/C";
            target.Selectable = false;
            target.Visible = false;
            target.FontSize = 7;
            ToLabel = target;

            AddObserver(this);
        }

        public GraphLink([NotNull] ILink link, [StrictlyPositive] float dpiFactor, [Range(8, 128)] int markerSize) : this()
        {
            AssignLink(link, markerSize, dpiFactor);
        }

        public void Dispose()
        {
            if (_link?.DataFlow is IDataFlow flow)
            {
                ((INotifyPropertyChanged)flow).PropertyChanged -= OnPropertyChanged;
                _link.PropertyValueChanged -= OnLinkPropertyValueChanged;
                flow.ThreatEventAdded -= ThreatEventsChanged;
                flow.ThreatEventRemoved -= ThreatEventsChanged;
                flow.Flipped -= Flipped;
                _threatsMarker.PanelItemClicked -= OnPanelItemClicked;
                _threatsMarker.Dispose();
            }
        }

        private void AssignLink(ILink link, int markerSize, float dpiFactor = 1.0f)
        {
            if (link != null && link.DataFlow is IDataFlow flow)
            {
                if (_link != null && _link.DataFlow is IDataFlow oldFlow)
                {
                    ((INotifyPropertyChanged)oldFlow).PropertyChanged -= OnPropertyChanged;
                    _link.PropertyValueChanged -= OnLinkPropertyValueChanged;
                    oldFlow.ThreatEventAdded -= ThreatEventsChanged;
                    oldFlow.ThreatEventRemoved -= ThreatEventsChanged;
                    oldFlow.Flipped -= Flipped;
                }

                _link = link;
                if (MidLabel is GraphText textControl)
                {
                    textControl.Copyable = true;
                    textControl.Movable = false;
                    textControl.Text = _link.DataFlow.Name;
                }
                ((INotifyPropertyChanged)_link.DataFlow).PropertyChanged += OnPropertyChanged;
                _link.PropertyValueChanged += OnLinkPropertyValueChanged;

                _threatsMarker = new AssociatedPanelItemMarker(_link.DataFlow);
                Add(_threatsMarker);

                _threatsMarker.PanelItemClicked += OnPanelItemClicked;
                flow.ThreatEventAdded += ThreatEventsChanged;
                flow.ThreatEventRemoved += ThreatEventsChanged;
                flow.Flipped += Flipped;

                _markerSize = markerSize;
                UpdateParameters(markerSize, dpiFactor);

                RefreshFlowType();
            }
        }

        public void UpdateParameters([Range(8, 128)] int markerSize, [StrictlyPositive] float dpiFactor = 1.0f)
        {
            if (_link is IThreatModelChild child && child.Model != null)
            {
                var schemaManager = new DiagramPropertySchemaManager(child.Model);
                var propertyType = schemaManager.GetTextLocationPropertyType();
                if (propertyType != null && _link.GetProperty(propertyType) is IPropertyBool propertyBool &&
                    propertyBool.Value)
                {
                    _alternativeTextLocation = propertyBool.Value;
                }

                var pointsPropertyType = schemaManager.GetLinksSchema()?.GetPropertyType("Points");
                var property = _link.GetProperty(pointsPropertyType);
                if (property is IPropertyArray propertyArray)
                {
                    List<string> points = new List<string>();
                    var array = propertyArray.Value?.ToArray();
                    var count = array?.Length ?? 0;
                    if (count > 0)
                    {
                        RealLink.ClearPoints();
                        for (int i = 0; i < count / 2; i++)
                        {
                            var x = float.Parse(array[i * 2], NumberFormatInfo.InvariantInfo) * dpiFactor;
                            var y = float.Parse(array[i * 2 + 1], NumberFormatInfo.InvariantInfo) * dpiFactor;
                            RealLink.AddPoint(new PointF(x, y));
                            if (dpiFactor != 1.0f)
                            {
                                points.Add(x.ToString(NumberFormatInfo.InvariantInfo));
                                points.Add(y.ToString(NumberFormatInfo.InvariantInfo));
                            }
                        }

                        if (points.Any())
                            propertyArray.Value = points;
                    }
                }
            }

            if (markerSize != _markerSize)
                _markerSize = markerSize;

            if (_threatsMarker != null)
            {
                _threatsMarker.Position = new PointF(MidLabel.Position.X + MidLabel.Width + 2.0f, MidLabel.Position.Y + MidLabel.Height + 2.0f);
                _threatsMarker.Size = new SizeF(markerSize, markerSize);
            }
        }

        public ILink Link
        {
            get => _link;
            set
            {
                AssignLink(value, _markerSize);
            }
        }

        private void OnLinkPropertyValueChanged(IPropertiesContainer container, IProperty property)
        {
            var schemaManager = new DiagramPropertySchemaManager(property.Model);
            if (schemaManager.GetTextLocationPropertyType()?.Id == property.PropertyTypeId &&
                property is IPropertyBool propertyBool)
            {
                _alternativeTextLocation = propertyBool.Value;
                LayoutChildren(this);
            }
        }

        protected override void PositionMidLabel(GoObject lab, PointF a, PointF b)
        {
            if (_alternativeTextLocation)
            {
                PointF newp = new PointF((float) (((double) a.X + (double) b.X) / 2.0), (float) (((double) a.Y + (double) b.Y) / 2.0));
                int spot = 1;
                if (!MidLabelCentered)
                    spot = (double) b.X >= (double) a.X ? (!IsApprox(b.Y, a.Y) ? ((double) b.Y >= (double) a.Y ? 4 : 2) : 32) : (!IsApprox(b.Y, a.Y) ? ((double) b.Y >= (double) a.Y ? 8 : 16) : 128);
                lab.SetSpotLocation(spot, newp);
            }
            else
            {
                base.PositionMidLabel(lab, a, b);
            }
        }

        internal bool IsApprox(float x, float y)
        {
            float num1 = 0.5f;
            GoDocument document = this.Document;
            if (document != null)
                num1 = 0.5f;
            float num2 = x - y;
            return (double) num2 < (double) num1 && (double) num2 > -(double) num1;
        }

        private void OnPanelItemClicked(object item)
        {
            if (item is IThreatEvent threatEvent)
                SelectedThreatEvent?.Invoke(threatEvent);
        }

        private void ThreatEventsChanged(IThreatEventsContainer arg1, IThreatEvent arg2)
        {
            _threatsMarker.Visible = arg1.ThreatEvents?.Any() ?? false;
        }

        private void Flipped(IDataFlow flow)
        {
            var realLink = RealLink as GraphRealLink;

            if (realLink != null)
            {
                var pointCount = realLink.PointsCount;

                if (pointCount > 0)
                {
                    using (var scope = UndoRedoManager.OpenScope("Flip direction in diagram"))
                    {
                        var schemaManager = new DiagramPropertySchemaManager(_link.DataFlow.Model);
                        var pointsPropertyType = schemaManager.GetLinksSchema()?.GetPropertyType("Points");
                        var property = _link.GetProperty(pointsPropertyType) ??
                                       _link.AddProperty(pointsPropertyType, null);

                        if (property is IPropertyArray propertyArray)
                        {
                            var list = new List<string>();
                            for (int i = pointCount - 1; i >= 0; i--)
                            {
                                var point = realLink.GetPoint(i);
                                list.Add(point.X.ToString(NumberFormatInfo.InvariantInfo));
                                list.Add(point.Y.ToString(NumberFormatInfo.InvariantInfo));
                            }
                            propertyArray.Value = list;
                            scope?.Complete();
                        }
                    }
                }
            }

            AssignLink(_link, _markerSize);
        }

        public string Text
        {
            get
            {
                string result = null;

                if (MidLabel is GraphText textControl)
                {
                    result = textControl.Text;
                }

                return result;
            }
        }

        public bool Loading { get; set; }

        public override void OnGotSelection(GoSelection sel)
        {
            base.OnGotSelection(sel);
            RaiseSelectedEvent();
        }

        internal void RaiseSelectedEvent()
        {
            if (Link != null)
                SelectedLink?.Invoke(Link);
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI,
            object newVal, RectangleF newRect)
        {
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);

            if (observed is GraphText)
            {
                if (subhint == GoText.ChangedText && string.CompareOrdinal(_link.DataFlow.Name, (string)newVal) != 0)
                {
                    if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
                    {
                        using (var scope = UndoRedoManager.OpenScope("Change Flow name"))
                        {
                            _link.DataFlow.Name = (string)newVal;
                            scope?.Complete();
                        }
                    }
                }
                else if (_threatsMarker != null && subhint == ChangedBounds)
                {
                    _threatsMarker.Position = new PointF(observed.Position.X + observed.Width + 2.0f,
                        observed.Position.Y + observed.Height + 2.0f);
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    if (MidLabel is GraphText textControl)
                    {
                        textControl.Text = _link.DataFlow.Name;
                    }
                    break;
                case "FlowType":
                    RefreshFlowType();
                    break;
            }
        }

        public void ShowThreats(GoView view, PointF point)
        {
            _threatsMarker.ShowPanelItemListForm(view, point);
        }

        private void RefreshFlowType()
        {
            var text = ToLabel as GoText;

            switch (_link.DataFlow.FlowType)
            {
                case FlowType.ReadWriteCommand:
                    FromLabel.Visible = true;
                    if (text != null)
                        text.Text = "W/C";
                    ToLabel.Visible = true;
                    break;
                case FlowType.Read:
                    FromLabel.Visible = true;
                    ToLabel.Visible = false;
                    break;
                case FlowType.WriteCommand:
                    FromLabel.Visible = false;
                    if (text != null)
                        text.Text = "W/C";
                    ToLabel.Visible = true;
                    break;
            }
        }

        public override GoLink CreateRealLink() {
            return new GraphRealLink();
        }

        #region Context menu.
        private List<string> _buckets;
        private Dictionary<string, List<IContextAwareAction>> _actions;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            Scope scope = Scope.DataFlow | Scope.Link;

            var effective = actions.Where(x => (x.Scope & scope) != 0 &&
                                               (x is IIdentityContextAwareAction || x is ILinkContextAwareAction)).ToArray();
            if (effective.Any())
            {
                _buckets = new List<string>();
                _actions = new Dictionary<string, List<IContextAwareAction>>();

                foreach (var action in effective)
                {
                    var group = action.Group ?? string.Empty;
                    if (!_actions.TryGetValue(group, out var actionsInGroup))
                    {
                        actionsInGroup = new List<IContextAwareAction>();
                        _buckets.Add(group);
                        _actions.Add(group, actionsInGroup);
                    }
                    actionsInGroup.Add(action);
                }
            }
        }

        public override GoContextMenu GetContextMenu(GoView view)
        {
            if (view is GoOverview) return null;
            var cm = new GoContextMenu(view);

            if (_buckets?.Any() ?? false)
            {
                foreach (var bucket in _buckets)
                {
                    AddMenu(cm, _actions[bucket]);
                }
            }

            return cm;
        }

        private void AddMenu([NotNull] GoContextMenu menu, [NotNull] List<IContextAwareAction> actions)
        {
            bool separator = false;

            foreach (var action in actions)
            {
                if (action.IsVisible(_link.DataFlow))
                {
                    if (!separator)
                    {
                        separator = true;

                        if (menu.Items.Count > 0)
                            menu.Items.Add(new ToolStripSeparator());
                    }

                    menu.Items.Add(new ToolStripMenuItem(action.Label, action.SmallIcon, DoAction)
                    {
                        Tag = action
                    });
                }
            }
        }

        private void DoAction(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem &&
                menuItem.Tag is IContextAwareAction action)
            {
                if (action is IIdentityContextAwareAction identityContextAwareAction)
                    identityContextAwareAction.Execute(_link.DataFlow);
                else if (action is ILinkContextAwareAction linkContextAwareAction)
                    linkContextAwareAction.Execute(_link);
            }
        }
        #endregion
    }
}