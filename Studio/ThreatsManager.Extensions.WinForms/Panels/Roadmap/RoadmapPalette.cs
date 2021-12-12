using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
    public sealed class RoadmapPalette : GoView
    {
        public RoadmapPalette() : base()
        {
            AllowDragOut = true;
            AllowDrop = true;
            AllowDelete = false;
            AllowEdit = false;
            AllowInsert = false;
            AllowLink = false;
            AllowMove = false;
            AllowReshape = false;
            AllowResize = false;
            ArrowMoveLarge = 10F;
            ArrowMoveSmall = 1F;
            AutoScrollRegion = new System.Drawing.Size(0, 0);
            BackColor = System.Drawing.Color.White;
            Dock = System.Windows.Forms.DockStyle.Fill;
            DragsRealtime = true;
            GridCellSizeHeight = 58F;
            GridCellSizeWidth = 52F;
            GridOriginX = 20F;
            GridOriginY = 5F;
            Location = new System.Drawing.Point(3, 3);
            ShowsNegativeCoordinates = false;
            Size = new System.Drawing.Size(185, 376);
            HidesSelection = true;
            ShowHorizontalScrollBar = GoViewScrollBarVisibility.Hide;
            ShowVerticalScrollBar = GoViewScrollBarVisibility.IfNeeded;

            RemoveMitigation += OnRemoveMitigation;
        }

        public event Action<IMitigation> MitigationDropped;

        private static event Action<IMitigation> RemoveMitigation;

        private void OnRemoveMitigation(IMitigation obj)
        {
            var node = Document.OfType<RoadmapItem>()
                .FirstOrDefault(x => x.Mitigation == obj);

            if (node != null)
            {
                RemoveNode(node);
                RefreshNodes();
            }
        }

        public void AddNode([NotNull] RoadmapItem node)
        {
            Document.Add(node);
            MitigationDropped?.Invoke(node.Mitigation);
        }

        public void RemoveNode([NotNull] RoadmapItem node)
        {
            Document.Remove(node);
        }

        public void Clear()
        {
            Document.Clear();
        }

        public void RefreshNodes()
        {
            var nodes = Document.OfType<RoadmapItem>()
                .Where(x => x.Label != null)
                .OrderBy(x => x.Label.Text).ToArray();
            if (nodes.Any())
            {
                float y = 10;

                foreach (var node in nodes)
                {
                    node.Position = new PointF(10, y);
                    y += node.Height + 10;
                }
            }

            var bounds = this.ComputeDocumentBounds();
            this.Document.Size = bounds.Size;

            OnResize(EventArgs.Empty);
        }

        protected override void DoExternalDrag(DragEventArgs evt)
        {
            base.DoExternalDrag(evt);
            evt.Effect = DragDropEffects.Move;
        }

        protected override void OnExternalObjectsDropped(GoInputEventArgs evt)
        {
            if (Selection.Primary is RoadmapItem node &&
                node.Document is GoDocument doc)
            {
                RemoveMitigation?.Invoke(node.Mitigation);

                AddNode(node);
                RefreshNodes();
            }
        }

        protected override void Dispose(bool disposing)
        {
            RemoveMitigation -= OnRemoveMitigation;

            base.Dispose(disposing);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            var nodes = Document.OfType<RoadmapItem>().ToArray();
            if (nodes?.Any() ?? false)
            {
                var width = this.Width;
                foreach (var node in nodes)
                {
                    node.SetWidth(width);
                }
            }
        }
    }
}
