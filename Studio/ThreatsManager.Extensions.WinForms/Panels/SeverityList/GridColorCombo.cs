using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.SeverityList
{
    public class GridColorCombo : GridComboBoxExEditControl
    {
        public GridColorCombo()
        {
            DisableInternalDrawing = true;
            DropDownStyle = ComboBoxStyle.DropDownList;

            Items.AddRange(EnumExtensions.GetEnumLabels<KnownColor>().ToArray());

            DrawItem += GridColorComboDrawItem;
        }

        public override void CellRender(Graphics g)
        {
            Rectangle r = EditorCell.Bounds;
            Rectangle t = EditorCell.UnMergedBounds;

            r.Y += (r.Height - t.Height) / 2;
            r.Height = t.Height - 1;
            r.Width--;

            RenderItem(g, r, Color.FromName(Text));
        }

        public override Size GetProposedSize(
            Graphics g, GridCell cell, CellVisualStyle style, Size constraintSize)
        {
            Size size = base.GetProposedSize(g, cell, style, constraintSize);

            size.Width += 40;

            return (size);
        }

        void GridColorComboDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();

                var name = (string) Items[e.Index];

                RenderItem(e.Graphics, e.Bounds, Color.FromName(name));
            }
        }

        private void RenderItem(Graphics g, Rectangle bounds, Color color)
        {
            using (Brush br = new SolidBrush(color))
            {
                bounds.Width--;
                bounds.Height--;

                Rectangle r = bounds;
                r.Width = 30;
                r.Inflate(-2, -2);

                g.FillRectangle(br, r);
                g.DrawRectangle(Pens.Black, r);

                r = bounds;
                r.X += 30;
                r.Width -= 30;

                using (StringFormat sf = new StringFormat())
                {
                    sf.LineAlignment = StringAlignment.Center;

                    g.DrawString(color.Name, Font, Brushes.Black, r, sf);
                }
            }
        }
    }
}