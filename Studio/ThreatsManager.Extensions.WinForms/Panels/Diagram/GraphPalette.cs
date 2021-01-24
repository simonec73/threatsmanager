using Northwoods.Go;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    public sealed class GraphPalette : GoPalette
    {
        public GraphPalette() : base()
        {
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
            GridCellSizeHeight = 58F;
            GridCellSizeWidth = 52F;
            GridOriginX = 20F;
            GridOriginY = 5F;
            Location = new System.Drawing.Point(3, 3);
            ShowsNegativeCoordinates = false;
            Size = new System.Drawing.Size(185, 376);
            HidesSelection = true;
            ShowHorizontalScrollBar = GoViewScrollBarVisibility.Hide;
            ShowVerticalScrollBar = GoViewScrollBarVisibility.Show;
            //ObjectGotSelection += new Northwoods.Go.GoSelectionEventHandler(this.palUserShapes_ObjectGotSelection);
            AutomaticLayout = false;
        }

        //private void palUserShapes_ObjectGotSelection(object sender, GoSelectionEventArgs e)
        //{
        //    Selection.Clear();
        //    if (GetCurrentGoView().Tool is ClickCreateTool)
        //        ((ClickCreateTool)GetCurrentGoView().Tool).Prototype = GetDropObjectFromPaletteObject(palUserShapes.Selection.First.Copy());
        //}

        //    private static GoIconicNode CreatePalletteNode(GoObject obj, string label, float width)
        //    {
        //        GoIconicNode iconNode = new GoIconicNode();
        //        iconNode.Initialize(null, "", AddBreak(label));
        //        if (obj is GoShape shape)
        //        {
        //            shape.BrushColor = Color.White;
        //        }
        //        obj.Selectable = false;
        //        obj.Size = new SizeF(24, 24);
        //        iconNode.Icon = obj;
        //        iconNode.Icon.Shadowed = true;
        //        iconNode.Label.FontSize = 7;

        //        iconNode.Label.WrappingWidth = width;
        //        iconNode.Label.Wrapping = true;
        //        iconNode.Label.Clipping = true;
        //        iconNode.Label.StringTrimming = StringTrimming.EllipsisCharacter;
        //        return iconNode;
        //    }

        //    private static string AddBreak(string label)
        //    {
        //        string s = "";
        //        int lastchar = 0;
        //        // add a space before each upper case char
        //        for (int i = 1; i < label.Length; i++)
        //        {
        //            if (label.Substring(i, 1).ToUpper() == label.Substring(i, 1))
        //            {
        //                if (s.Length > 0) s += " ";
        //                s = s + label.Substring(lastchar, i - lastchar);
        //                lastchar = i;
        //            }
        //        }
        //        if (s.Length > 0) s += " ";
        //        s = s + label.Substring(lastchar, label.Length - lastchar);
        //        return s;
        //    }
    }
}
