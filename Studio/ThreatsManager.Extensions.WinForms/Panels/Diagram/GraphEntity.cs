using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Properties;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public sealed class GraphEntity : GoIconicNode, IDisposable
    {
        private readonly IEntityShape _shape;
        private ImageSize _sourceSize;

        private readonly AssociatedPanelItemMarker _panelItemMarker;
        private readonly AssociatedDiagramMarker _diagramMarker;
        private readonly WarningMarker _warningMarker;

        public event Action<IEntityShape> SelectedShape;
        public event Action<IThreatEvent> SelectedThreatEvent;
        public event Action<IDiagram> OpenDiagram;
        public static event Action<IIdentity> ParentChanged;

        public IEntityShape EntityShape => _shape;

        public bool Deactivated { get; set; }

        public void Dispose()
        {
            if (_shape?.Identity is IEntity entity)
            {
                entity.ImageChanged -= OnEntityImageChanged;

                _diagramMarker.DiagramClicked -= OnDiagramClicked;
                _diagramMarker.Dispose();

                _panelItemMarker.PanelItemClicked -= OnPanelItemClicked;
                _panelItemMarker.Dispose();

                ((INotifyPropertyChanged)entity).PropertyChanged -= OnPropertyChanged;
            }

            UndoRedoManager.Undone -= UndoRedoManager_UndoneRedone;
            UndoRedoManager.Redone -= UndoRedoManager_UndoneRedone;
        }

        #region Initialization.
        public GraphEntity([NotNull] IEntityShape shape,
            [StrictlyPositive] float dpiFactor, [Range(32, 256)] int iconSize,
            [Range(8, 248)] int iconCenterSize, ImageSize sourceSize, [Range(8, 128)] int markerSize) : base()
        {
            _shape = shape;
            var identity = shape.Identity;
            if (identity is IEntity entity)
            {
                Initialize(Icons.Resources.ResourceManager, GetIconName(entity), entity.Name);
                entity.ImageChanged += OnEntityImageChanged;

                //InPort.ToSpot = GoObject.NoSpot;
                //InPort.IsValidSelfNode = false;
                //InPort.IsValidDuplicateLinks = false;
                //OutPort.FromSpot = GoObject.NoSpot;
                //OutPort.IsValidSelfNode = false;
                //OutPort.IsValidDuplicateLinks = false;

                Icon.Resizable = false;
                Reshapable = false;
                Copyable = false;

                _panelItemMarker = new AssociatedPanelItemMarker(entity);
                _panelItemMarker.PanelItemClicked += OnPanelItemClicked;
                Add(_panelItemMarker);

                _diagramMarker = new AssociatedDiagramMarker(entity);
                _diagramMarker.DiagramClicked += OnDiagramClicked;
                Add(_diagramMarker);

                _warningMarker = new WarningMarker();
                Add(_warningMarker);

                UpdateParameters(iconSize, iconCenterSize, sourceSize, markerSize, dpiFactor);

                AddObserver(this);

                // ReSharper disable once SuspiciousTypeConversion.Global
                ((INotifyPropertyChanged) identity).PropertyChanged += OnPropertyChanged;
            }
            else
                throw new ArgumentException(Properties.Resources.ShapeNotEntityError);

            UndoRedoManager.Undone += UndoRedoManager_UndoneRedone;
            UndoRedoManager.Redone += UndoRedoManager_UndoneRedone;
        }

        private void UndoRedoManager_UndoneRedone(string obj)
        {
            var shape = _shape;
            var location = Location;
        }

        public void UpdateParameters([Range(32, 256)] int iconSize, [Range(8, 248)] int iconCenterSize,
            ImageSize sourceSize, [Range(8, 128)] int markerSize, [StrictlyPositive] float dpiFactor = 1.0f)
        {
            using (var scope = UndoRedoManager.OpenScope("Update parameters"))
            {
                _sourceSize = sourceSize;
                ApplySourceSize();

                Icon.Size = new SizeF(iconSize, iconSize);
                Port.Size = new SizeF(iconCenterSize, iconCenterSize);
                _panelItemMarker.Position = new PointF(iconSize - markerSize, iconSize - markerSize);
                _panelItemMarker.Size = new SizeF(markerSize, markerSize);
                _diagramMarker.Position = new PointF(iconSize - markerSize, 0f);
                _diagramMarker.Size = new SizeF(markerSize, markerSize);
                _warningMarker.Position = new PointF(0f, iconSize - markerSize);
                _warningMarker.Size = new SizeF(markerSize, markerSize);
                if (dpiFactor != 1.0f)
                    _shape.Position = new PointF(_shape.Position.X * dpiFactor, _shape.Position.Y * dpiFactor);
                Location = new PointF(_shape.Position.X, _shape.Position.Y);

                scope?.Complete();
            }
        }

        private void ApplySourceSize()
        {
            var identity = _shape.Identity;
            if (identity is IEntity entity)
            {
                if (Icon is GoImage icon)
                {
                    switch (_sourceSize)
                    {
                        case ImageSize.Small:
                            if (entity.SmallImage != null)
                                icon.Image = entity.SmallImage;
                            else if (entity.Image != null)
                                icon.Image = entity.Image;
                            else if (entity.BigImage != null)
                                icon.Image = entity.BigImage;
                            break;
                        case ImageSize.Medium:
                            if (entity.Image != null)
                                icon.Image = entity.Image;
                            else if (entity.BigImage != null)
                                icon.Image = entity.BigImage;
                            else if (entity.SmallImage != null)
                                icon.Image = entity.SmallImage;
                            break;
                        case ImageSize.Big:
                            if (entity.BigImage != null)
                                icon.Image = entity.BigImage;
                            else if (entity.Image != null)
                                icon.Image = entity.Image;
                            else if (entity.SmallImage != null)
                                icon.Image = entity.SmallImage;
                            break;
                    }
                }
            }
            else
                throw new ArgumentException(Properties.Resources.ShapeNotEntityError);
        }

        private void OnPanelItemClicked(object item)
        {
            if (item is IThreatEvent threatEvent)
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
                    Movable = false,
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
            get => _panelItemMarker.Visible;
            set => _panelItemMarker.Visible = value;
        }

        public void ShowThreats(GoView view)
        {
            _panelItemMarker.ShowPanelItemListForm(view, Location);
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
                if (_sourceSize == imageSize)
                {
                    ApplySourceSize();
                }
            }
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI,
            object newVal, RectangleF newRect)
        {
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
            
            if (observed.Equals(Label) &&  subhint == GoText.ChangedText)
            {
                using (var scope = UndoRedoManager.OpenScope("Update Identity name"))
                {
                    _shape.Identity.Name = Text;
                    scope?.Complete();
                }
            }

            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing && observed.Equals(this) && subhint == ChangedBounds)
            {
                float centerX = newRect.X + (newRect.Width / 2f);
                float centerY = newRect.Y + (newRect.Height / 2f);

                if (centerX != _shape.Position.X || centerY != _shape.Position.Y)
                {
                    using (var scope = UndoRedoManager.OpenScope("Reposition Entity"))
                    {
                        _shape.Position = new PointF(centerX, centerY);
                        scope?.Complete();
                    }
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
                    using (var scope = UndoRedoManager.OpenScope("Update Identity name"))
                    {
                        valid = graphGroup.GroupShape?.Identity == entity.Parent;
                        scope?.Complete();
                    }
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
        private List<string> _buckets;
        private Dictionary<string, List<IContextAwareAction>> _actions;

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
                    identityContextAwareAction.Execute(_shape.Identity);
                else if (action is IShapeContextAwareAction shapeContextAwareAction)
                    shapeContextAwareAction.Execute(_shape);
            }
        }
        #endregion
    }
}