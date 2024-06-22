﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;
using Northwoods.Go.Layout;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public sealed class GraphView : GoView, IInitializableObject
    {
        private IDiagram _diagram;

        public GraphView()
        {
            // let users move objects rather than an outline of the selected objects
            DragsRealtime = true;
            // user created links will be instances of GraphLink
            NewLinkClass = typeof(GraphLink);

            GridSnapDrag = GoViewSnapStyle.None;
            //GridCellSize = new SizeF(10,10);
            GridStyle = GoViewGridStyle.None;

            ObjectResized += GraphView_ObjectResized;
        }

        private void GraphView_ObjectResized(object sender, GoSelectionEventArgs e)
        {
            if (!UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
            {
                if (e.GoObject is GraphGroup group)
                {
                    using (var scope = UndoRedoManager.OpenScope("Resize Group"))
                    {
                        group.LayoutChildren();

                        SaveGroupData(group);

                        var groups = group.OfType<GraphGroup>().ToArray();
                        if (groups?.Any() ?? false)
                        {
                            foreach (var child in groups)
                            {
                                SaveGroupData(child);
                            }
                        }

                        scope?.Complete();
                    }
                }
                else if (e.GoObject is GraphRealLink realLink)
                {
                    var pointCount = realLink.PointsCount;
                    var link = realLink.AbstractLink as GraphLink;
                    var tmLink = link?.Link;

                    if (pointCount > 0 && tmLink != null)
                    {
                        using (var scope = UndoRedoManager.OpenScope("Change Link points"))
                        {
                            var schemaManager = new DiagramPropertySchemaManager(tmLink.DataFlow.Model);
                            var pointsPropertyType = schemaManager.GetLinksSchema()?.GetPropertyType("Points");
                            var property = tmLink.GetProperty(pointsPropertyType) ??
                                           tmLink.AddProperty(pointsPropertyType, null);

                            if (property is IPropertyArray propertyArray)
                            {
                                var list = new List<string>();
                                for (int i = 0; i < pointCount; i++)
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
            }
        }

        private static void SaveGroupData([NotNull] GraphGroup group)
        {
            var shape = group.GroupShape;
            var border = group.Border;

            if (shape.Size.Width != border.Size.Width ||
                shape.Size.Height != border.Size.Height)
                shape.Size = new SizeF(border.Size.Width, border.Size.Height);

            var location = group.Location;
            float centerX = location.X;
            float centerY = location.Y + group.Label.Height / 2f - 8.0f;

            if (centerX != shape.Position.X || centerY != shape.Position.Y)
            {
                shape.Position = new PointF(centerX, centerY);
            }
        }

        private static void SaveEntityData([NotNull] GraphEntity entity)
        {
            var shape = entity.EntityShape;
            var location = entity.Location;

            if (location.X != shape.Position.X || location.Y != shape.Position.Y)
            {
                shape.Position = new PointF(location.X, location.Y);
            }
        }

        public event Action<PointF, GraphGroup> CreateExternalInteractor;
        
        public event Action<PointF, GraphGroup> CreateProcess;
        
        public event Action<PointF, GraphGroup> CreateDataStore;
        
        public event Action<PointF, GraphGroup> CreateTrustBoundary;
        
        public event Action<Guid, PointF, GraphGroup> CreateIdentity;

        public void Initialize([NotNull] IDiagram diagram)
        {
            _diagram = diagram;
            Document = CreateDocument();
        }

        public bool IsInitialized => _diagram != null;

        public override GoDocument CreateDocument()
        {
            return new GraphDoc();
        }

        public GraphDoc Doc
        {
            get { return Document as GraphDoc; }
        }

        #region Context Menu.
        private List<string> _buckets;
        private Dictionary<string, List<IContextAwareAction>> _actions;

        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var effective = actions
                .OfType<IIdentityContextAwareAction>()
                .Where(x => (x.Scope & Scope.Diagram) != 0).ToArray();
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

        protected override void OnBackgroundContextClicked(GoInputEventArgs evt)
        {
            base.OnBackgroundContextClicked(evt);
            GoContextMenu cm = new GoContextMenu(this);

            if (_buckets?.Any() ?? false)
            {
                foreach (var bucket in _buckets)
                {
                    AddMenu(cm, _actions[bucket]);
                }
            }
            cm.Show(this, evt.ViewPoint);
        }

        private void AddMenu([NotNull] GoContextMenu menu, [NotNull] List<IContextAwareAction> actions)
        {
            bool separator = false;

            foreach (var action in actions)
            {
                if (action.IsVisible(_diagram))
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
                action.Execute(_diagram);
            }
        }

        /// <summary>
        /// Called when the user clicks on the background context menu Paste menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This calls <see cref="GoView.EditPaste"/> and selects all of the newly pasted objects.
        /// </remarks>
        public void Paste_Command(Object sender, EventArgs e)
        {
            PointF docpt = LastInput.DocPoint;
            StartTransaction();
            Selection.Clear();
            EditPaste(); // selects all newly pasted objects
            RectangleF copybounds = GoDocument.ComputeBounds(Selection, this);
            SizeF offset = new SizeF(docpt.X - copybounds.X, docpt.Y - copybounds.Y);
            MoveSelection(Selection, offset, true);
            FinishTransaction("Context Paste");
        }
        #endregion

        #region Drag & Drop.
        protected override void OnBackgroundSelectionDropped(GoInputEventArgs evt)
        {
            bool wasinbox = false;
            foreach (GoObject obj in Selection)
            {
                if (obj.Parent is GraphGroup)
                {
                    wasinbox = true;
                    break;
                }
            }
            if (wasinbox)
            {
                Document.DefaultLayer.AddCollection(Selection, true);
            }
            base.OnBackgroundSelectionDropped(evt);
        }

        protected override void OnExternalObjectsDropped(GoInputEventArgs evt)
        {
            PointF point = evt.DocPoint;

            if (Selection.Primary is GoSimpleNode simpleNode)
            {
                using (var scope = UndoRedoManager.OpenScope("Dropped a new standard item to the Diagram"))
                {
                    var parent = GetParentGroup(point);
                    simpleNode.Remove();
                    switch (simpleNode.Text)
                    {
                        case "ExternalInteractor":
                            CreateExternalInteractor?.Invoke(point, parent);
                            break;
                        case "Process":
                            CreateProcess?.Invoke(point, parent);
                            break;
                        case "DataStore":
                            CreateDataStore?.Invoke(point, parent);
                            break;
                        case "TrustBoundary":
                            CreateTrustBoundary?.Invoke(point, parent);
                            break;
                        default:
                            if (Guid.TryParse(simpleNode.Text, out Guid id))
                            {
                                CreateIdentity?.Invoke(id, point, parent);

                                var entity = _diagram.Model?.GetEntity(id);
                                IGroup p = null;
                                if (entity != null)
                                {
                                    p = entity.Parent;
                                }
                                else
                                {
                                    var group = _diagram.Model?.GetGroup(id);
                                    if (group is IGroupElement element)
                                    {
                                        p = element.Parent;
                                    }
                                }

                                if (p != null)
                                {
                                    parent = Doc.GetGroup(p.Id);
                                }
                            }
                            break;
                    }

                    RecurseLayoutParent(parent);

                    scope?.Complete();
                }
            } 
            else if (Selection.Primary is GraphPaletteItemNode paletteItemNode)
            {
                using (var scope = UndoRedoManager.OpenScope("Added an advanced item to the Diagram"))
                {
                    var item = paletteItemNode.Item;
                    paletteItemNode.Remove();

                    var nodes = Document.PickObjects(point, false, null, 100);
                    foreach (var node in nodes)
                    {
                        var parentNode = node?.ParentNode;
                        if (parentNode is GraphEntity gnode)
                        {
                            var entity = _diagram.Model?.GetEntity(gnode.EntityShape.AssociatedId);
                            if (entity != null)
                            {
                                item.Apply(entity);
                                gnode.ShowThreats(this);
                            }
                        }
                        else if (parentNode is GraphLink glink)
                        {
                            var dataFlow = _diagram.Model?.GetDataFlow(glink.Link.AssociatedId);
                            if (dataFlow != null)
                            {
                                item.Apply(dataFlow);

                                glink.ShowThreats(this, point);
                            }
                        }
                        else if (parentNode is GraphGroup ggroup)
                        {
                            var group = _diagram.Model?.GetGroup(ggroup.GroupShape.AssociatedId);
                            if (group != null)
                            {
                                item.Apply(group);
                                //ggroup.ShowThreats(this);
                                ggroup.LayoutChildren();
                            }
                        }
                    }

                    scope?.Complete();
                }
            }
            else
            {
            }
        }

        private void RecurseLayoutParent(GraphGroup group)
        {
            if (group != null)
            {
                group.LayoutChildren();

                if (group.Parent is GraphGroup parent)
                {
                    RecurseLayoutParent(parent);
                }
            }
        }

        private UndoRedoScope _moveScope;

        protected override void DoInternalDrag(DragEventArgs evt)
        {
            if (_moveScope == null && !UndoRedoManager.IsUndoing && !UndoRedoManager.IsRedoing)
                _moveScope = UndoRedoManager.OpenScope("Moving objects");

            base.DoInternalDrag(evt);
        }

        protected override void DoInternalDrop(DragEventArgs evt)
        {
            base.DoInternalDrop(evt);

            var groups = Doc.GetGroups();
            if (groups?.Any() ?? false)
            {
                foreach (var group in groups)
                {
                    group.LayoutChildren();
                    SaveGroupData(group);
                }
            }

            var entities = Doc.GetEntities();
            if (entities?.Any() ?? false)
            {
                foreach (var entity in entities)
                {
                    SaveEntityData(entity);
                }
            }

            //var groups = Doc.OfType<GraphGroup>();
            //if (groups?.Any() ?? false)
            //{
            //    foreach (var group in groups)
            //    {
            //        group.LayoutChildren();
            //    }
            //}

            _moveScope?.Complete();
            _moveScope = null;
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs evt)
        {
            if (_moveScope != null && evt.Action == DragAction.Cancel)
            {
                _moveScope.Dispose();
                _moveScope = null;
            }
        }

        private GraphGroup GetParentGroup(PointF point)
        {
            GraphGroup result = null;

            var nodes = Document.PickObjects(point, false, null, 100);
            if (nodes?.Any() ?? false)
            {
                foreach (var node in nodes)
                {
                    if (node is GoRoundedRectangle rectangle &&
                        rectangle.Parent is GraphGroup group)
                    {
                        result = group;
                        break;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Layout functions.
        public void AlignLeftSides()
        {
            GoObject obj = GetPrimaryNode();
            if (obj != null && !(obj is IGoLink))
            {
                using (var scope = UndoRedoManager.OpenScope("Diagram Layout - Align left sides"))
                {
                    StartTransaction();
                    float X = obj.SelectionObject.Left;
                    foreach (GoObject temp in Selection)
                    {
                        GoObject t = temp.SelectionObject;
                        if (t.Parent is GraphEntity ge)
                        {
                            ge.Left = X;
                        }
                    }
                    FinishTransaction("Align Left Sides");
                    scope?.Complete();
                }
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a node.");
            }
        }

        public void AlignHorizontally()
        {
            GoObject obj = GetPrimaryNode();
            if (obj != null && !(obj is IGoLink))
            {
                using (var scope = UndoRedoManager.OpenScope("Diagram Layout - Align horizontally"))
                {
                    StartTransaction();
                    float Y = obj.SelectionObject.Center.Y;
                    foreach (GoObject temp in Selection)
                    {
                        GoObject t = temp.SelectionObject;
                        if (t.Parent is GraphEntity ge)
                        {
                            ge.Center = new PointF(t.Center.X, Y);
                        }
                    }
                    FinishTransaction("Align Vertical Centers");
                    scope?.Complete();
                }
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a node.");
            }
        }

        public void AlignRightSides()
        {
            GoObject obj = GetPrimaryNode();
            if (obj != null && !(obj is IGoLink))
            {
                using (var scope = UndoRedoManager.OpenScope("Diagram Layout - Align right sides"))
                {
                    StartTransaction();
                    float X = obj.SelectionObject.Right;
                    foreach (GoObject temp in Selection)
                    {
                        GoObject t = temp.SelectionObject;
                        if (t.Parent is GraphEntity ge)
                        {
                            ge.Right = X;
                        }
                    }
                    FinishTransaction("Align Right Sides");
                    scope?.Complete();
                }
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a node.");
            }
        }

        public void AlignTops()
        {
            GoObject obj = GetPrimaryNode();
            if (obj != null && !(obj is IGoLink))
            {
                using (var scope = UndoRedoManager.OpenScope("Diagram Layout - Align tops"))
                {
                    StartTransaction();
                    float Y = obj.SelectionObject.Top;
                    foreach (GoObject temp in Selection)
                    {
                        GoObject t = temp.SelectionObject;
                        if (t.Parent is GraphEntity ge)
                        {
                            ge.Top = Y;
                        }
                    }
                    FinishTransaction("Align Tops");
                    scope?.Complete();
                }
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a node.");
            }
        }

        public void AlignVertically()
        {
            GoObject obj = GetPrimaryNode();
            if (obj != null && !(obj is IGoLink))
            {
                using (var scope = UndoRedoManager.OpenScope("Diagram Layout - Align vertically"))
                {
                    StartTransaction();
                    float X = obj.SelectionObject.Center.X;
                    foreach (GoObject temp in Selection)
                    {
                        GoObject t = temp.SelectionObject;
                        if (t.Parent is GraphEntity ge)
                        {
                            ge.Center = new PointF(X, t.Center.Y);
                        }
                    }
                    FinishTransaction("Align Horizontal Centers");
                    scope?.Complete();
                }
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a node.");
            }
        }

        public void AlignBottoms()
        {
            GoObject obj = GetPrimaryNode();
            if (obj != null && !(obj is IGoLink))
            {
                using (var scope = UndoRedoManager.OpenScope("Diagram Layout - Align bottoms"))
                {
                    StartTransaction();
                    float Y = obj.SelectionObject.Bottom;
                    foreach (GoObject temp in Selection)
                    {
                        GoObject t = temp.SelectionObject;
                        if (t.Parent is GraphEntity ge)
                        {
                            ge.Bottom = Y;
                        }
                    }
                    FinishTransaction("Align Bottoms");
                    scope?.Complete();
                }
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a node.");
            }
        }

        public void DoLayout([NotNull] GraphGroup group, int spacingHorizontal, int spacingVertical)
        {
            using (var scope = UndoRedoManager.OpenScope("Diagram Layout"))
            {
                var layout = new GoLayoutLayeredDigraph()
                {
                    Document = Doc,
                    DirectionOption = GoLayoutDirection.Right,
                    LayerSpacing = spacingHorizontal,
                    ColumnSpacing = spacingVertical,
                    Iterations = 5,
                    Network = new GoLayoutLayeredDigraphNetwork(),
                };
                layout.Network.AddNodesAndLinksFromCollection(group, true);
                layout.PerformLayout();
                scope?.Complete();
            }
        }

        public void DoLayout(int spacingHorizontal, int spacingVertical)
        {
            using (var scope = UndoRedoManager.OpenScope("Diagram Layout"))
            {
                var layout = new GoLayoutLayeredDigraph()
                {
                    Document = Doc,
                    DirectionOption = GoLayoutDirection.Right,
                    LayerSpacing = spacingHorizontal,
                    ColumnSpacing = spacingVertical,
                    Iterations = 5
                };

                // NOTE: this piece of code does not perform the right layout inside the group.
                //if (Selection.Count == 1 && Selection.Primary is GraphGroup group)
                //{
                //    layout.Network = new GoLayoutLayeredDigraphNetwork();
                //    layout.Network.AddNodesAndLinksFromCollection(group, true);
                //    layout.PerformLayout();
                //    group.RefreshBorder();

                //    var globalLayout = new GoLayoutLayeredDigraph()
                //    {
                //        Document = Doc, 
                //        DirectionOption = GoLayoutDirection.Right,
                //        LayerSpacing = spacingHorizontal,
                //        ColumnSpacing = spacingVertical,
                //        Iterations = 5
                //    };
                //    globalLayout.PerformLayout();
                //}
                //else
                layout.PerformLayout();
                scope?.Complete();
            }
        }

        private GoObject GetPrimaryNode()
        {
            GoObject result = Selection.Primary;
            if (result is IGoLink)
            {
                foreach (var item in Selection)
                {
                    if (item is IGoNode)
                    {
                        result = item;
                        break;
                    }
                }
            }

            return result;
        }

        private GraphGroup GetPrimaryGroup()
        {
            GraphGroup result = Selection.Primary as GraphGroup;
            if (result == null)
            {
                foreach (var item in Selection)
                {
                    if (item is GraphGroup group)
                    {
                        result = group;
                        break;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Zooming.
        private bool _originalScale = true;
        private PointF _originalDocPosition = new PointF();
        private float _originalDocScale = 1.0f;

        public void Zoom(float zoomFactor)
        {
            _originalScale = true;
            DocScale = zoomFactor;
        }

        public void ZoomIn()
        {
            _originalScale = true;
            float newscale = (float) (Math.Round(DocScale / 0.9f * 100) / 100);
            DocScale = newscale;
        }

        public void ZoomOut()
        {
            _originalScale = true;
            float newscale = (float) (Math.Round(DocScale * 0.9f * 100) / 100);
            DocScale = newscale;
        }

        public void ZoomNormal()
        {
            _originalScale = true;
            DocScale = 1;
        }

        public void ZoomToFit()
        {
            if (_originalScale)
            {
                _originalDocPosition = DocPosition;
                _originalDocScale = DocScale;
                RescaleToFit();
            }
            else
            {
                DocPosition = _originalDocPosition;
                DocScale = _originalDocScale;
            }
            _originalScale = !_originalScale;
        }
        #endregion

        #region Drawing to bitmap.
        public void ToClipboard()
        {
            var selection = Selection;
            SelectAll();
            var bitmap = GetBitmapFromCollection(Selection);
            Selection.Clear();
            Selection.AddRange(selection);
            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Bitmap, true, bitmap);
            Clipboard.SetDataObject(dataObject);
        }

        public void ToFile([Required] string fileName)
        {
            var selection = Selection;
            SelectAll();
            var bitmap = GetBitmapFromCollection(Selection);
            Selection.Clear();
            Selection.AddRange(selection);
            bitmap.Save(fileName);
        }

        public Bitmap ToBitmap()
        {
            var selection = Selection;
            SelectAll();
            var result = GetBitmapFromCollection(Selection);
            Selection.Clear();
            Selection.AddRange(selection);
            return result;
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}