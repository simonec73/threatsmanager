using System;
using System.Drawing;
using System.IO;
using Northwoods.Go;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    /// <summary>
    /// Summary description for GraphDoc.
    /// </summary>
    [Serializable]
    public class GraphDoc : GoDocument
    {
        public GraphDoc()
        {
            Name = "GraphDoc " + NextDocumentId();
            //LinksLayer = Layers.CreateNewLayerBefore(Layers.Default);
            LinksLayer = Layers.CreateNewLayerAfter(Layers.Default);
            LinksLayer.Identifier = "Links";
            LinksLayer.AllowSelect = true;
            //Layers.CreateNewLayerAfter(Layers.Default).Identifier = "bottom";
            Layers.CreateNewLayerBefore(Layers.Default).Identifier = "bottom";
            MaintainsPartID = true;
            IsModified = false;
            //UndoManager = new GoUndoManager();
        }

        public string Location
        {
            get { return _myLocation; }
            set
            {
                var old = _myLocation;
                if (old != value)
                {
                    _myLocation = value;
                    RaiseChanged(ChangedLocation, 0, null, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        public GoComment InsertComment()
        {
            var comment = new GoComment();
            comment.Text = "Enter your comment here,\r\non multiple lines.";
            comment.Position = NextNodePosition();
            comment.Label.Multiline = true;
            comment.Label.Editable = true;
            StartTransaction();
            Add(comment);
            FinishTransaction("Insert Comment");
            return comment;
        }

        public GoLink MakeRelationship(GoTextNode a, GoTextNode b)
        {
            var l = new GoLink();
            l.Orthogonal = true;
            l.Style = GoStrokeStyle.RoundedLine;
            l.Brush = null;
            l.FromPort = a.BottomPort;
            l.ToPort = b.TopPort;
            return l;
        }

        public PointF NextNodePosition()
        {
            var next = _myNextNodePos;
            _myNextNodePos.X += 50;
            if (_myNextNodePos.X > 400)
            {
                _myNextNodePos.X = 40;
                _myNextNodePos.Y += 50;
                if (_myNextNodePos.Y > 300)
                    _myNextNodePos.Y = 40;
            }
            return next;
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.Hint)
            {
                case ChangedLocation:
                {
                    Location = (string) e.GetValue(undo);
                    break;
                }
                default:
                    base.ChangeValue(e, undo);
                    return;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                if (Location == "") return false;
                var info = new FileInfo(Location);
                var ro = ((info.Attributes & FileAttributes.ReadOnly) != 0);
                var oldskips = SkipsUndoManager;
                SkipsUndoManager = true;
                // take out the following statement if you want the user to be able
                // to modify the graph even though the file is read-only
                SetModifiable(!ro);
                SkipsUndoManager = oldskips;
                return ro;
            }
        }

        public static int NextDocumentId()
        {
            return _myDocCounter++;
        }

        public const int ChangedLocation = LastHint + 23;

        private static int _myDocCounter = 1;
        private string _myLocation = "";
        private PointF _myNextNodePos = new PointF(30, 30);
    }
}