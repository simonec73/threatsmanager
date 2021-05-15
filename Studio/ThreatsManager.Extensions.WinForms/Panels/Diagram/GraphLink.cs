using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
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

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class GraphLink : GoLabeledLink, IDisposable
    {
        private DpiState _dpiState = DpiState.Ok;
        private ILink _link;
        private AssociatedThreatsMarker _threatsMarker;
        private bool _alternativeTextLocation;

        public event Action<ILink> SelectedLink;
        
        public event Action<IThreatEvent> SelectedThreatEvent;

        private List<string> _buckets;
        private Dictionary<string, List<IContextAwareAction>> _actions;

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

        public void Dispose()
        {
            if (_link?.DataFlow is IDataFlow flow)
            {
                ((INotifyPropertyChanged)flow).PropertyChanged -= OnPropertyChanged;
                _link.PropertyValueChanged -= OnLinkPropertyValueChanged;
                _link.DataFlow.ThreatEventAdded -= ThreatEventsChanged;
                _link.DataFlow.ThreatEventRemoved -= ThreatEventsChanged;
                _threatsMarker.ThreatEventClicked -= OnThreatEventClicked;
                _threatsMarker.Dispose();
            }
        }

        public GraphLink([NotNull] ILink link, DpiState dpiState) : this()
        {
            _dpiState = dpiState;
            Link = link;
        }

        public ILink Link
        {
            get => _link;
            set
            {
                _link = value;
                if (MidLabel is GraphText textControl)
                {
                    textControl.Text = _link.DataFlow.Name;
                }
                ((INotifyPropertyChanged) _link.DataFlow).PropertyChanged += OnPropertyChanged;

                if (value is IThreatModelChild child && child.Model != null)
                {
                    var schemaManager = new DiagramPropertySchemaManager(child.Model);
                    var propertyType = schemaManager.GetTextLocationPropertyType();
                    if (propertyType != null && value.GetProperty(propertyType) is IPropertyBool propertyBool &&
                        propertyBool.Value)
                    {
                        _alternativeTextLocation = propertyBool.Value;
                    }
                }
                _link.PropertyValueChanged += OnLinkPropertyValueChanged;

                _threatsMarker = new AssociatedThreatsMarker(_link.DataFlow)
                {
                    Position = new PointF(MidLabel.Position.X + MidLabel.Width + 2.0f, MidLabel.Position.Y + MidLabel.Height + 2.0f)
                };
                Add(_threatsMarker);
                _threatsMarker.ThreatEventClicked += OnThreatEventClicked;
                _link.DataFlow.ThreatEventAdded += ThreatEventsChanged;
                _link.DataFlow.ThreatEventRemoved += ThreatEventsChanged;

                RefreshFlowType();
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
                if (!this.MidLabelCentered)
                    spot = (double) b.X >= (double) a.X ? (!this.IsApprox(b.Y, a.Y) ? ((double) b.Y >= (double) a.Y ? 4 : 2) : 32) : (!this.IsApprox(b.Y, a.Y) ? ((double) b.Y >= (double) a.Y ? 8 : 16) : 128);
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

        private void OnThreatEventClicked(IThreatEvent threatEvent)
        {
            SelectedThreatEvent?.Invoke(threatEvent);
        }

        public DpiState DpiState
        {
            get => _dpiState;
            set => _dpiState = value;
        }

        private void ThreatEventsChanged(IThreatEventsContainer arg1, IThreatEvent arg2)
        {
            _threatsMarker.Visible = arg1.ThreatEvents?.Any() ?? false;
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
                if (subhint == 1501 && string.CompareOrdinal(_link.DataFlow.Name, (string) newVal) != 0)
                {
                    _link.DataFlow.Name = (string) newVal;
                } else if (_threatsMarker != null && subhint == 1001)
                    _threatsMarker.Position = new PointF(observed.Position.X + observed.Width + 2.0f,
                        observed.Position.Y + observed.Height + 2.0f);
            } else if (observed is GraphLink graphLink && subhint == 1001 &&
                       graphLink.FromNode != null && graphLink.ToNode != null &&
                       _link != null && !Loading)
            {
                var pointCount = graphLink.RealLink.PointsCount;
                if (pointCount > 0)
                {
                    var schemaManager = new DiagramPropertySchemaManager(_link.DataFlow.Model);
                    var pointsPropertyType = schemaManager.GetLinksSchema()?.GetPropertyType("Points");
                    var property = _link.GetProperty(pointsPropertyType) ?? 
                                   _link.AddProperty(pointsPropertyType, null);

                    if (property is IPropertyArray propertyArray)
                    {
                        var list = new List<string>();
                        for (int i = 0; i < pointCount; i++)
                        {
                            var point = graphLink.RealLink.GetPoint(i);
                            switch (_dpiState)
                            {
                                case DpiState.TooSmall:
                                    point = new PointF(point.X / 2, point.Y / 2);
                                    break;
                                case DpiState.TooBig:
                                    point = new PointF(point.X * 2, point.Y * 2);
                                    break;
                            }

                            list.Add(point.X.ToString(NumberFormatInfo.InvariantInfo));
                            list.Add(point.Y.ToString(NumberFormatInfo.InvariantInfo));
                        }
                        propertyArray.Value = list;
                    }
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
            _threatsMarker.ShowThreatsDialog(view, point);
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

                        if (menu.MenuItems.Count > 0)
                            menu.MenuItems.Add(new MenuItem("-"));
                    }

                    menu.MenuItems.Add(new MenuItem(action.Label, DoAction)
                    {
                        Tag = action
                    });
                }
            }
        }

        private void DoAction(object sender, EventArgs e)
        {
            if (sender is MenuItem menuItem &&
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