using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Layout;
using PostSharp.Patterns.Contracts;
using Syncfusion.XlsIO.Parser.Biff_Records;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public sealed class GraphView : GoView, IInitializableObject
    {
        private DpiState _dpiState = DpiState.Ok;
        private IDiagram _diagram;
        private bool _originalScale = true;
        private PointF _originalDocPosition = new PointF();
        private float _originalDocScale = 1.0f;
        private List<string> _buckets;
        private Dictionary<string, List<IContextAwareAction>> _actions;

        public GraphView()
        {
            // let users move objects rather than an outline of the selected objects
            DragsRealtime = true;
            // user created links will be instances of GraphLink
            NewLinkClass = typeof(GraphLink);

            //GridSnapDrag = GoViewSnapStyle.Jump;
            //GridCellSize = new SizeF(10,10);
            GridStyle = GoViewGridStyle.None;

            // replace both standard linking tools with custom ones
            //ReplaceMouseTool(typeof(GoToolLinkingNew), new GraphViewLinkingNewTool(this));
            //ReplaceMouseTool(typeof(GoToolRelinking), new GraphViewRelinkingTool(this));
        }

        public event Action<string> SetStatusMessage;
        
        public event Action<float> SetStatusZoom;

        public event Action<PointF, GraphGroup> CreateExternalInteractor;
        
        public event Action<PointF, GraphGroup> CreateProcess;
        
        public event Action<PointF, GraphGroup> CreateDataStore;
        
        public event Action<PointF, GraphGroup> CreateTrustBoundary;
        
        public event Action<Guid, PointF, GraphGroup> CreateIdentity;

        public void Initialize([NotNull] IDiagram diagram, DpiState dpiState)
        {
            _dpiState = dpiState;
            _diagram = diagram;
            Document = CreateDocument();
        }

        public bool IsInitialized => _diagram != null;

        public override GoDocument CreateDocument()
        {
            GoDocument doc = new GraphDoc();
            //doc.UndoManager = new GoUndoManager();
            return doc;
        }

        public GraphDoc Doc
        {
            get { return Document as GraphDoc; }
        }

        //public String StatusMessage
        //{
        //    get { return myMessage; }
        //    set
        //    {
        //        myMessage = value;
        //        SetStatusMessage?.Invoke(myMessage);
        //    }
        //}

        /// <summary>
        /// This method is responsible for updating all of the view's visible
        /// state outside of the GoView itself--the title bar, status bar, and properties grid.
        /// </summary>
        public void UpdateFormInfo()
        {
            UpdateTitle();
            SetStatusMessage?.Invoke(Doc.Location);
            SetStatusZoom?.Invoke(DocScale);
        }

        /// <summary>
        /// Update the title bar with the view's document's Name, and an indication
        /// of whether the document is read-only and whether it has been modified.
        /// </summary>
        public void UpdateTitle()
        {
            if (Parent is Form win)
            {
                String title = Document.Name;
                if (Doc.IsReadOnly)
                    title += " [Read Only]";
                if (Doc.IsModified)
                    title += "*";
                win.Text = title;
            }
        }

        /// <summary>
        /// Add page numbers to printed pages, in case someone drops the pages and needs to resort them.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="e"></param>
        /// <param name="hpnum"></param>
        /// <param name="hpmax"></param>
        /// <param name="vpnum"></param>
        /// <param name="vpmax"></param>
        protected override void PrintDecoration(Graphics g, PrintPageEventArgs e, int hpnum, int hpmax, int vpnum, int vpmax)
        {
            String msg = hpnum.ToString() + "," + vpnum.ToString();
            Font font = new Font("Verdana", 10);
            SizeF size = g.MeasureString(msg, font);
            PointF pt = new PointF(e.MarginBounds.X + e.MarginBounds.Width / 2 - size.Width / 2,
                e.MarginBounds.Y + e.MarginBounds.Height);
            g.DrawString(msg, font, Brushes.Blue, pt);
            base.PrintDecoration(g, e, hpnum, hpmax, vpnum, vpmax);
        }

        /// <summary>
        /// If the document's name changes, update the title;
        /// if the document's location changes, update the status bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evt"></param>
        protected override void OnDocumentChanged(Object sender, GoChangedEventArgs evt)
        {
            base.OnDocumentChanged(sender, evt);
            if (evt.Object is IGoLink &&
                (evt.SubHint == GoLink.ChangedFromPort || evt.SubHint == GoLink.ChangedToPort))
            {
                SetStatusMessage?.Invoke("Changed a link's port");
            }
            else if (evt.Hint == GoLayer.InsertedObject)
            {
                if (evt.Object is IGoLink)
                {
                    SetStatusMessage?.Invoke("Inserted a link");
                }
            }
            else if (evt.Hint == GoLayer.RemovedObject)
            {
                if (evt.Object is IGoLink)
                {
                    SetStatusMessage?.Invoke("Removed a link");
                }
            }
            else if (evt.Hint == GoDocument.ChangedName ||
                     evt.Hint == GoDocument.RepaintAll || evt.Hint == GoDocument.FinishedUndo ||
                     evt.Hint == GoDocument.FinishedRedo)
            {
                UpdateFormInfo();
            }
            else if (evt.Hint == GraphDoc.ChangedLocation)
            {
                SetStatusMessage?.Invoke(Doc.Location);
            }
        }

        /// <summary>
        /// If the view's document is replaced, update the title;
        /// if the view's scale changes, update the status bar
        /// </summary>
        /// <param name="evt"></param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs evt)
        {
            base.OnPropertyChanged(evt);
            if (evt.PropertyName == "Document")
                UpdateFormInfo();
            else if (evt.PropertyName == "DocScale")
                SetStatusZoom?.Invoke(DocScale);
        }

        #region Context Menu.
        public void SetContextAwareActions([NotNull] IEnumerable<IContextAwareAction> actions)
        {
            var effective = actions.Where(x => (x.Scope & Scope.Diagram) != 0).ToArray();
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

        /// <summary>
        /// Handle a drop from the tree view.
        /// </summary>
        /// <param name="evt"></param>
        //protected override IGoCollection DoExternalDrop(DragEventArgs evt)
        //{
        //    IDataObject data = evt.Data;
        //    Object treenodeobj = data.GetData(typeof(TreeNode));
        //    if (treenodeobj is TreeNode treenode)
        //    {
        //        Point screenPnt = new Point(evt.X, evt.Y);
        //        Point viewPnt = PointToClient(screenPnt);
        //        PointF docPnt = ConvertViewToDoc(viewPnt);
        //        StartTransaction();
        //        Selection.Clear();
        //        Selection.HotSpot = new SizeF(0, 0);
        //        if (treenode.Tag is GoObject tag)
        //        {
        //            var newobj = Document.AddCopy(tag, docPnt);
        //            Selection.Add(newobj);
        //        }
        //        FinishTransaction("Insert from TreeView");
        //        return Selection;
        //    }
        //    else
        //    {
        //        return base.DoExternalDrop(evt);
        //    }
        //}

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
            PointF point;
            switch (_dpiState)
            {
                case DpiState.TooSmall:
                    point = new PointF(evt.DocPoint.X / 2, evt.DocPoint.Y / 2);
                    break;
                case DpiState.TooBig:
                    point = new PointF(evt.DocPoint.X * 2, evt.DocPoint.Y * 2);
                    break;
                default:
                    point = evt.DocPoint;
                    break;
            }

            if (Selection.Primary is GoSimpleNode simpleNode)
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
                        }
                        break;
                }
            } 
            else if (Selection.Primary is GraphThreatTypeNode threatTypeNode)
            {
                var threatType = threatTypeNode.ThreatType;
                threatTypeNode.Remove();

                var nodes = Document.PickObjects(point, false, null, 100);
                foreach (var node in nodes)
                {
                    var parent = node?.ParentNode;
                    if (parent is GraphEntity gnode)
                    {
                        var entity = _diagram.Model?.GetEntity(gnode.EntityShape.AssociatedId);
                        if (entity != null)
                        {
                            entity.AddThreatEvent(threatType);
                            gnode.ShowThreats(this);
                        }
                    }
                    else if (parent is GraphLink glink)
                    {
                        var dataFlow = _diagram.Model?.GetDataFlow(glink.Link.AssociatedId);
                        if (dataFlow != null)
                        {
                            dataFlow.AddThreatEvent(threatType);
                            glink.ShowThreats(this, point);
                        }
                    }
                }
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

        #region Layout functions.
        public void AlignLeftSides()
        {
            GoObject obj = GetPrimaryNode();
            if (obj != null && !(obj is IGoLink))
            {
                StartTransaction();
                float X = obj.SelectionObject.Left;
                foreach (GoObject temp in Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (t.Parent is GraphEntity ge)
                    {
                        ge.Left = X;
                        if (ge.Parent is GraphGroup group)
                            group.RefreshBorder();
                    }
                }
                FinishTransaction("Align Left Sides");
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
                StartTransaction();
                float Y = obj.SelectionObject.Center.Y;
                foreach (GoObject temp in Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (t.Parent is GraphEntity ge)
                    {
                        ge.Center = new PointF(t.Center.X, Y);
                        if (ge.Parent is GraphGroup group)
                            group.RefreshBorder();
                    }
                }
                FinishTransaction("Align Vertical Centers");
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
                StartTransaction();
                float X = obj.SelectionObject.Right;
                foreach (GoObject temp in Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (t.Parent is GraphEntity ge)
                    {
                        ge.Right = X;
                        if (ge.Parent is GraphGroup group)
                            group.RefreshBorder();
                    }
                }
                FinishTransaction("Align Right Sides");
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
                StartTransaction();
                float Y = obj.SelectionObject.Top;
                foreach (GoObject temp in Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (t.Parent is GraphEntity ge)
                    {
                        ge.Top = Y;
                        if (ge.Parent is GraphGroup group)
                            group.RefreshBorder();
                    }
                }
                FinishTransaction("Align Tops");
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
                StartTransaction();
                float X = obj.SelectionObject.Center.X;
                foreach (GoObject temp in Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (t.Parent is GraphEntity ge)
                    {
                        ge.Center = new PointF(X, t.Center.Y);
                        if (ge.Parent is GraphGroup group)
                            group.RefreshBorder();
                    }
                }
                FinishTransaction("Align Horizontal Centers");
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
                StartTransaction();
                float Y = obj.SelectionObject.Bottom;
                foreach (GoObject temp in Selection)
                {
                    GoObject t = temp.SelectionObject;
                    if (t.Parent is GraphEntity ge)
                    {
                        ge.Bottom = Y;
                        if (ge.Parent is GraphGroup group)
                            group.RefreshBorder();
                    }
                }
                FinishTransaction("Align Bottoms");
            }
            else
            {
                MessageBox.Show("Alignment failure: Primary Selection is empty or a link instead of a node.");
            }
        }

        public void DoLayout([NotNull] GraphGroup group, int spacingHorizontal, int spacingVertical)
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
        }

        public void DoLayout(int spacingHorizontal, int spacingVertical)
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

        public Metafile ToMetafile()
        {
            var selection = Selection;
            SelectAll();
            RectangleF bounds = GoDocument.ComputeBounds(Selection, this);

            Graphics gbm = CreateGraphics();
            IntPtr bufdc = gbm.GetHdc();
            MemoryStream str = new MemoryStream();
            Metafile mf = new Metafile(str, bufdc, bounds, MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);

            Graphics gmf = Graphics.FromImage(mf);
            gmf.PageUnit = GraphicsUnit.Pixel;
            gmf.SmoothingMode = this.SmoothingMode;
            gmf.TextRenderingHint = this.TextRenderingHint;
            gmf.InterpolationMode = this.InterpolationMode;
            gmf.CompositingQuality = this.CompositingQuality;
            gmf.PixelOffsetMode = this.PixelOffsetMode;

            RectangleF b = bounds;
            b.Inflate(1, 1);
            PaintPaperColor(gmf, b);

            foreach (GoObject obj in Selection)
            {
                if (!obj.CanView())
                    continue;
                obj.Paint(gmf, this);
            }

            gmf.Dispose();

            gbm.ReleaseHdc(bufdc);
            gbm.Dispose();
            mf.Dispose();

            byte[] data = str.GetBuffer();

            Selection.Clear();
            Selection.AddRange(selection);

            return new Metafile(new MemoryStream(data, false));
        }
        #endregion
    }
}