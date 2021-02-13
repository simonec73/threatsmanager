using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.AdvTree;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Extensions.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class DiagramSortDialog : Form
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public DiagramSortDialog()
        {
            InitializeComponent();
        }

        public DiagramSortDialog([NotNull] IThreatModel model) : this()
        {
            var diagrams = model.Diagrams?.OrderBy(x => x.Order).ToArray();
            if (diagrams?.Any() ?? false)
            {
                foreach (var diagram in diagrams)
                {
                    var node = new Node(diagram.Name)
                    {
                        Tooltip = diagram.Description, 
                        Tag = diagram,
                        Image = Resources.model
                    };
                    _diagrams.Nodes.Add(node);
                }
            }
        }

        private void _diagrams_BeforeNodeDrop(object sender, TreeDragDropEventArgs e)
        {
            e.Cancel = e.NewParentNode != null;
        }

        private void _diagrams_NodeDragFeedback(object sender, TreeDragFeedbackEventArgs e)
        {
            // Get mouse position relative to tree control
            Point mousePos = _diagrams.PointToClient(Control.MousePosition);

            // Get node mouse is over
            Node mouseOverNode = _diagrams.GetNodeAt(mousePos.Y);

            e.AllowDrop = (e.ParentNode == null) && (mouseOverNode == null);
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            var diagrams = _diagrams.Nodes.OfType<Node>().Select(x => x.Tag).OfType<IDiagram>().ToArray();
            if (diagrams.Any())
            {
                for (int i = 0; i < diagrams.Length; i++)
                {
                    diagrams[i].Order = i + 1;
                }
            }
        }
    }
}
