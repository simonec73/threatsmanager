﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    [Serializable]
    public class GraphGroup : GoSubGraphBase, IDisposable
    {
        private readonly IGroupShape _shape = null;
        private GoShape _border = null;
        private GoText _label = null;
        private static Pen _borderPen;
        private readonly WarningMarker _warningMarker;
        private bool _loading;
        
        public event Action<IGroupShape> SelectedShape;
        public static event Action<IIdentity> ParentChanged;

        #region Public properties.
        public IGroupShape GroupShape => _shape;

        public bool Deactivated { get; set; }

        public GoShape Border
        {
            get { return _border; }
        }

        public override GoText Label
        {
            get { return _label; }
        }

        public override PointF Location {
            get => GetSpotLocation(MiddleCenter);
            set => SetSpotLocation(MiddleCenter, value);
        }
        #endregion

        #region Initialization.
        public GraphGroup([NotNull] IGroupShape shape, [StrictlyPositive] float dpiFactor, [Range(8, 128)] int markerSize)
        {
            try
            {
                _loading = true;
                _shape = shape;
                if (shape.Identity is IGroup group)
                {
                    Initializing = true;
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    ((INotifyPropertyChanged) group).PropertyChanged += OnPropertyChanged;

                    _border = new GoRoundedRectangle()
                    {
                        Selectable = false
                    };
                    if (_borderPen == null)
                    {
                        Pen p = new Pen(ThreatModelManager.StandardColor, 2) {DashStyle = DashStyle.Dash};
                        _borderPen = p;
                    }
                    _border.Pen = _borderPen;
                    _border.FillSimpleGradient(Color.FromArgb(0xB0, 0x87, 0xCE, 0xFF), Color.FromArgb(0xB0, 0x87, 0xCE, 0xFF), GoObject.MiddleTop);
                    _border.AddObserver(this);
                    Add(_border);

                    _label = new GoText
                    {
                        Text = group.Name,
                        Selectable = false,
                        Movable = false,
                        Editable = true,
                        Position = new PointF(0.0f, -16.0f)
                    };
                    _label.AddObserver(this);
                    Add(_label);

                    _warningMarker = new WarningMarker()
                    {
                        Position = new PointF(10.0f, 10.0f)
                    };
                    Add(_warningMarker);

                    Selectable = true;
                    Resizable = true;
                    Copyable = false;
                    Initializing = false;
                    Visible = true;
                    AutoRescales = false;
                    //LayoutChildren(_border);

                    UpdateParameters(markerSize, dpiFactor);
                }
                else
                    throw new ArgumentException(Properties.Resources.ShapeNotEntityError);
            }
            finally
            {
                _loading = false;
            }
        }

        public void UpdateParameters([Range(8, 128)] int markerSize, [StrictlyPositive] float dpiFactor = 1.0f)
        {
            _warningMarker.Size = new SizeF(markerSize, markerSize);

            if (dpiFactor != 1.0f)
            {
                _shape.Size = new SizeF(_shape.Size.Width * dpiFactor, _shape.Size.Height * dpiFactor);
                _shape.Position = new PointF(_shape.Position.X * dpiFactor, _shape.Position.Y * dpiFactor);
            }
            _border.Size = new SizeF(_shape.Size.Width, _shape.Size.Height);
            Location = new PointF(_shape.Position.X, _shape.Position.Y);
        }
        #endregion

        public void Dispose()
        {
            if (_shape.Identity is IGroup group)
            {
                Initializing = true;
                ((INotifyPropertyChanged) group).PropertyChanged -= OnPropertyChanged;
            }
        }

        #region Status change.
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    Text = _shape.Identity?.Name;
                    break;
            }
        }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            var nodes = Document.PickObjects(evt.DocPoint, false, null, 100);
            foreach (var node in nodes)
            {
                var parent = node?.ParentNode;
                if (parent is GraphEntity gnode)
                {
                    //var entity = _shape.Identity.Model?.GetEntity(gnode.EntityShape.AssociatedId);
                    //if (entity != null)
                    //{
                    //}
                }
                else if (parent is GraphLink)
                {
                    if (node is AssociatedPanelItemMarker marker)
                    {
                        marker.OnSingleClick(evt, view);
                    }
                    else if (node is GraphText)
                    {
                        view.Selection.Clear();
                        view.Selection.Add(node);
                    }
                    else
                    {
                        view.Selection.Clear();
                        view.Selection.Add(parent);
                    }
                }
            }
            return base.OnSingleClick(evt, view);
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI,
            object newVal, RectangleF newRect)
        {
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);

            if (!_loading && !UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                using (var scope = UndoRedoManager.OpenScope("Group update"))
                {
                    if (observed.Equals(Label) && subhint == GoText.ChangedText)
                    {
                        var group = _shape.Identity;
                        if (group != null)
                        {
                            group.Name = Text;
                            scope?.Complete();
                        }
                    }
                }
            }
        }
        
        //public override void DoMove(GoView view, PointF origLoc, PointF newLoc)
        //{
        //    base.DoMove(view, origLoc, newLoc);

        //    if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
        //    {
        //        using (var scope = UndoRedoManager.OpenScope("Move Group"))
        //        {
        //            _shape.Position = new PointF(newLoc.X, newLoc.Y);

        //            var children = this.OfType<GraphGroup>().ToArray();
        //            if (children?.Any() ?? false)
        //            {
        //                foreach (var child in children)
        //                {
        //                    child.DoMove(view, origLoc, newLoc);
        //                }
        //            }

        //            scope?.Complete();
        //        }
        //    }
        //}


        public override void OnGotSelection(GoSelection sel)
        {
            base.OnGotSelection(sel);

            SelectedShape?.Invoke(_shape);
        }

        protected override void OnParentChanged(GoGroup oldgroup, GoGroup newgroup)
        {
            base.OnParentChanged(oldgroup, newgroup);

            if (!Deactivated && !UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing &&
                _shape?.Identity is IGroupElement groupElement)
            {
                using (var scope = UndoRedoManager.OpenScope("Reparent Entity"))
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

                    scope?.Complete();
                }

                ParentChanged?.Invoke(_shape.Identity);
            }
        }

        public void Validate()
        {
            bool valid = false;
            _warningMarker.Tooltip = null;

            if (_shape?.Identity is ITrustBoundary trustBoundary)
            {
                if (Parent is GraphGroup graphGroup)
                {
                    valid = graphGroup.GroupShape?.Identity == trustBoundary.Parent;
                }
                else
                {
                    valid = trustBoundary.Parent == null;
                }
 
                if (!valid)
                    _warningMarker.Tooltip = Resources.ParentAlignmentWarning;
            }

            _warningMarker.Visible = !valid;
        }
        #endregion

        #region Group: children management.
        // override to keep fields up-to-date
        protected override void CopyChildren(GoGroup newgroup, GoCopyDictionary env)
        {
            base.CopyChildren(newgroup, env);
            GraphGroup newobj = (GraphGroup)newgroup;
            newobj._border = (GoShape)env[_border];
            newobj._label = (GoText)env[_label];
        }

        // override to keep fields up-to-date
        public override bool Remove(GoObject obj)
        {
            bool result = base.Remove(obj);
            if (obj == _border)
                _border = null;
            else if (obj == _label)
                _label = null;
            return result;
        }

        // Determine the smallest rectangle that encloses all of the other children of this area.
        // This might return null, if there are no other objects besides the Border.
        public RectangleF ComputeBorder()
        {
            RectangleF rect = RectangleF.Empty;
            bool first = true;
            foreach (GoObject obj in this)
            {
                if (obj == Border) continue;
                if (obj == Label) continue;
                if (obj is WarningMarker) continue;
                if (obj is GraphPaletteItemNode) continue;
                if (first)
                {
                    first = false;
                    rect = obj.Bounds;
                }
                else
                {
                    // add the object's bounding rect to this one
                    rect = RectangleF.Union(rect, obj.Bounds);
                }
            }
            if (!first)
            {
                // leave some room as a margin
                rect.Inflate(4, 4);
            }
            return rect;
        }

        // override to keep _border surrounding the other child objects
        public override void LayoutChildren(GoObject childchanged)
        {
            if (Initializing || UndoRedoManager.IsUndoing || UndoRedoManager.IsRedoing) return;

            GoObject border = Border;
            GoObject label = Label;
            if (childchanged == border && border != null && label != null)
            {
                RectangleF rect = border.Bounds;
                label.SetSpotLocation(BottomLeft, new PointF(rect.X, rect.Y - 2));
            }
            else if (!AllowDragInOut)
            {
                LayoutChildren();
            }
        }

        public void LayoutChildren()
        {
            GoObject border = Border;

            // compute the minimum rectangle needed to enclose the children except for the Border
            RectangleF rect = ComputeBorder();
            if (rect != RectangleF.Empty)
            {
                using (var scope = UndoRedoManager.OpenScope("Force Layout Group Child"))
                {
                    // but don't have the box shrink to minimum size continuously
                    rect = RectangleF.Union(rect, border.Bounds);
                    border.Bounds = rect;
                    _shape.Size = new SizeF(border.Bounds.Width, border.Bounds.Height);
                    var location = Location;
                    _shape.Position = new PointF(location.X, location.Y);

                    scope?.Complete();
                }
            }

            var bounds = _border.Bounds;
            _label.SetSpotLocation(BottomLeft, new PointF(bounds.X, bounds.Y - 2f));
            _warningMarker.SetSpotLocation(TopLeft, new PointF(bounds.X + 10f, bounds.Y + 10f));
        }

        public bool AllowDragInOut => true;

        // highlight when dragging
        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view)
        {
            if (from is GraphGroup)
            {
                // unhighlight any BoxArea that we have left
                from.SkipsUndoManager = true;
                ((GraphGroup)from).Border.Pen = _borderPen;
                from.SkipsUndoManager = false;
            }
            if (AllowDragInOut && view.Tool is GoToolDragging)
            {
                // only highlight when we are dragging
                if (to == this)
                {
                    // highlight this BoxArea that we are entering
                    SkipsUndoManager = true;
                    Border.Pen = new Pen(Color.Red, 2);
                    SkipsUndoManager = false;
                    return true; // don't highlight parent BoxAreas, if any
                }
            }
            return false; // continue by calling parent.OnEnterLeave
        }

        public override bool OnSelectionDropped(GoObjectEventArgs evt, GoView view)
        {
            if (AllowDragInOut)
            {
                var links = view.Selection.OfType<GraphLink>().ToArray();
                foreach (var link in links)
                    view.Selection.Remove(link);

                // add all selected objects to this BoxArea
                view.Selection.AddRange(AddCollection(view.Selection, false));
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Context menu.
        private List<string> _buckets;
        private Dictionary<string, List<IContextAwareAction>> _actions;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            Scope scope = Scope.Undefined;

            if (_shape.Identity is IGroup)
                scope = Scope.Group;

            var effective = actions.Where(x => (x.Scope & scope) != 0 && 
                                                (x is IIdentityContextAwareAction || x is IShapeContextAwareAction)).ToArray();
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