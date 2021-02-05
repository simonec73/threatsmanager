using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class GraphEntity : GoIconicNode, IDisposable
    {
        private DpiState _dpiState = DpiState.Ok;
        private readonly IEntityShape _shape;
        private List<string> _buckets;
        private Dictionary<string, List<IContextAwareAction>> _actions;
        private readonly AssociatedThreatsMarker _threatsMarker;
        private readonly AssociatedDiagramMarker _diagramMarker;
        private readonly WarningMarker _warningMarker;
        private static readonly ImageSize _imageSize;

        public event Action<IEntityShape> SelectedShape;
        public event Action<IThreatEvent> SelectedThreatEvent;
        public event Action<IDiagram> OpenDiagram;
        public static event Action<IIdentity> ParentChanged;

        public IEntityShape EntityShape => _shape;

        public bool Deactivated { get; set; }

        static GraphEntity()
        {
            _imageSize = Dpi.Factor.Height >= 1.5 ? ImageSize.Big : ImageSize.Medium;
        }

        public void Dispose()
        {
            if (_shape?.Identity is IEntity entity)
            {
                entity.ImageChanged -= OnEntityImageChanged;

                _diagramMarker.DiagramClicked -= OnDiagramClicked;
                _diagramMarker.Dispose();

                _threatsMarker.ThreatEventClicked -= OnThreatEventClicked;
                _threatsMarker.Dispose();

                ((INotifyPropertyChanged)entity).PropertyChanged -= OnPropertyChanged;
            }

        }

        #region Initialization.
        public GraphEntity([NotNull] IEntityShape shape, DpiState dpiState) : base()
        {
            _shape = shape;
            _dpiState = dpiState;
            var identity = shape.Identity;
            if (identity is IEntity entity)
            {
                Initialize(Icons.Resources.ResourceManager, GetIconName(entity), entity.Name);
                if (_imageSize == ImageSize.Big)
                {
                    if (entity.BigImage != null && Icon is GoImage icon)
                        icon.Image = entity.BigImage;
                    Icon.Size = new SizeF(64.0f, 64.0f);
                } 
                else
                {
                    if (entity.Image != null && Icon is GoImage icon)
                        icon.Image = entity.Image;
                    Icon.Size = new SizeF(32.0f, 32.0f);
                }
                entity.ImageChanged += OnEntityImageChanged;

                //InPort.ToSpot = GoObject.NoSpot;
                //InPort.IsValidSelfNode = false;
                //InPort.IsValidDuplicateLinks = false;
                //OutPort.FromSpot = GoObject.NoSpot;
                //OutPort.IsValidSelfNode = false;
                //OutPort.IsValidDuplicateLinks = false;

                Port.Size = new SizeF(15f,15f);

                Icon.Resizable = false;
                Reshapable = false;

                _threatsMarker = new AssociatedThreatsMarker(entity)
                {
                    Position = new PointF(Icon.Size.Width / 2.0f, Icon.Size.Height / 2.0f),
                };
                _threatsMarker.ThreatEventClicked += OnThreatEventClicked;
                Add(_threatsMarker);

                _diagramMarker = new AssociatedDiagramMarker(entity)
                {
                    Position = new PointF(16.0f * Dpi.Factor.Width, 0),
                };
                _diagramMarker.DiagramClicked += OnDiagramClicked;
                Add(_diagramMarker);

                _warningMarker = new WarningMarker()
                {
                    Position = new PointF(0.0f, 16.0f * Dpi.Factor.Height),
                };
                Add(_warningMarker);

                switch (_dpiState)
                {
                    case DpiState.TooSmall:
                        Location = new PointF(_shape.Position.X * 2, _shape.Position.Y * 2);
                        break;
                    case DpiState.TooBig:
                        Location = new PointF(_shape.Position.X / 2, _shape.Position.Y / 2);
                        break;
                    default:
                        Location = _shape.Position;
                        break;
                }
                Copyable = false;

                AddObserver(this);

                // ReSharper disable once SuspiciousTypeConversion.Global
                ((INotifyPropertyChanged) identity).PropertyChanged += OnPropertyChanged;
            }
            else
                throw new ArgumentException(Properties.Resources.ShapeNotEntityError);
        }

        private void OnThreatEventClicked(IThreatEvent threatEvent)
        {
            SelectedThreatEvent?.Invoke(threatEvent);
        }

        private void OnDiagramClicked(IDiagram diagram)
        {
            OpenDiagram?.Invoke(diagram);
        }

        private static string GetIconName([NotNull] IEntity entity)
        {
            string result;

            if (entity is IExternalInteractor)
                result = "external_big";
            else if (entity is IProcess)
                result = "process_big";
            else if (entity is IDataStore)
                result = "storage_big";
            else
                result = "undefined_big";

            return result;
        }

        protected override GoText CreateLabel(string name)
        {
            GoText l = null;
            if (name != null)
            {
                l = new GoText()
                {
                    Text = name,
                    Selectable = false,
                    Alignment = MiddleTop,
                    Editable = true
                };
                l.AddObserver(this); // keep this.Text up-to-date with this.Label.Text
            }
            Editable = true;

            return l;
        }

        //protected override GoPort CreatePort(bool input)
        //{
        //    var p = base.CreatePort(input);
        //    p.Size = new SizeF(10, 10);
        //    return p;
        //}

        public override PointF Location {
            get => GetSpotLocation(MiddleCenter);
            set => SetSpotLocation(MiddleCenter, value);
        }
        #endregion

        public bool ThreatsMarker
        {
            get => _threatsMarker.Visible;
            set => _threatsMarker.Visible = value;
        }

        public void ShowThreats(GoView view)
        {
            _threatsMarker.ShowThreatsDialog(view, Location);
        }

        #region Status change.
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    Text = _shape.Identity.Name;
                    break;
            }
        }

        private void OnEntityImageChanged([NotNull] IEntity entity, ImageSize imageSize)
        {
            if (Icon is GoImage icon)
            {
                if (Dpi.Factor.Height >= 1.5)
                {
                    if (imageSize == ImageSize.Big)
                    {
                        icon.Image = entity.BigImage;
                        icon.Size = new SizeF(64.0f, 64.0f);
                    }
                } else
                {
                    if (imageSize == ImageSize.Medium)
                    {
                        icon.Image = entity.Image;
                        icon.Size = new SizeF(32.0f, 32.0f);
                    }
                }  
            }
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI,
            object newVal, RectangleF newRect)
        {
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);

            if (observed.Equals(Label) && subhint == 1501)
            {
                _shape.Identity.Name = Text;
            }

            if (observed.Equals(this) && subhint == 1001)
            {
                float centerX = newRect.X + (newRect.Width / 2f);
                float centerY = newRect.Y + (newRect.Height / 2f);

                if (centerX != _shape.Position.X || centerY != _shape.Position.Y)
                {
                    _shape.Position = new PointF(centerX, centerY);
                }
            }
        }

        public override void OnGotSelection(GoSelection sel)
        {
            base.OnGotSelection(sel);

            SelectedShape?.Invoke(_shape);
        }

        protected override void OnParentChanged(GoGroup oldgroup, GoGroup newgroup)
        {
            base.OnParentChanged(oldgroup, newgroup);

            if (!Deactivated && _shape?.Identity is IGroupElement groupElement)
            {
                if (newgroup is GraphGroup graphGroup &&
                    graphGroup.GroupShape.Identity is IGroup group)
                {
                    groupElement.SetParent(group);
                }
                else
                {
                    groupElement.SetParent(null);
                }

                ParentChanged?.Invoke(_shape.Identity);
            }
        }

        public void Validate()
        {
            bool valid = false;
            _warningMarker.Tooltip = null;

            if (_shape?.Identity is IEntity entity)
            {
                if (Parent is GraphGroup graphGroup)
                {
                    valid = graphGroup.GroupShape?.Identity == entity.Parent;
                }
                else
                {
                    valid = entity.Parent == null;
                }

                if (!valid)
                    _warningMarker.Tooltip = Resources.ParentAlignmentWarning;
            }

            _warningMarker.Visible = !valid;
        }
        #endregion

        #region Context menu.
        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            Scope scope = Scope.EntityShape;

            if (_shape.Identity is IExternalInteractor)
                scope |= Scope.ExternalInteractor;
            else if (_shape.Identity is IProcess)
                scope |= Scope.Process;
            else if (_shape.Identity is IDataStore)
                scope |= Scope.DataStore;

            var effective = actions
                .Where(x => (x.Scope & scope) != 0 && 
                    (x is IIdentityContextAwareAction || x is IShapeContextAwareAction))
                .ToArray();
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
                if (action.IsVisible(_shape.Identity))
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
                    identityContextAwareAction.Execute(_shape.Identity);
                else if (action is IShapeContextAwareAction shapeContextAwareAction)
                    shapeContextAwareAction.Execute(_shape);
            }
        }
        #endregion
    }
}