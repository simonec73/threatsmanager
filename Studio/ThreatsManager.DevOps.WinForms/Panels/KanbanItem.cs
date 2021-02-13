using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Dialogs;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Panels
{
    public class KanbanItem : GoBoxNode
    {
        private Color _color;
        private const float DefaultWidth = 350f;
        private readonly int _originalContainer;
        private int _container;
        private IDevOpsConnector _connector;

        public KanbanItem([Required] string name, Color color, int originalContainer)
        {
            PortBorderMargin = SizeF.Empty;
            
            _color = color;
            _originalContainer = originalContainer;

            Header.BrushColor = color;
            Label.Text = name;

            ToolTipText = name;

            AddItem("Roadmap", Icons.Resources.threat_model);
            AddItem("Assigned To", Properties.Resources.businesswoman);
        }

        public KanbanItem([NotNull] IMitigation mitigation, int originalContainer) : 
            this(mitigation.Name, ThreatModelManager.StandardColor, originalContainer)
        {
            Item = mitigation;

            var model = mitigation.Model;
            if (model != null)
            {
                var schemaManager = new RoadmapPropertySchemaManager(model);
                SetInfo("Roadmap", schemaManager.GetStatus(mitigation).GetEnumLabel());

                _connector = DevOpsManager.GetConnector(model);
                if (_connector != null)
                {
                    var devOpsSchemaManager = new DevOpsPropertySchemaManager(model);
                    var info = devOpsSchemaManager.GetDevOpsInfo(mitigation, _connector);
                    if ((info?.Id ?? -1) >= 0)
                    {
                        DevOpsId = info.Id;
                    }
                }
            }
        }

        public event Action<KanbanItem> MoveRequired;

        public object Item { get; private set; }

        public int Container
        {
            get
            {
                return _container;
            }

            set
            {
                _container = value;
                if (Header[1] is GoImage image)
                {
                    image.Visible = (_container != _originalContainer);
                    Header.LayoutChildren(Header[0]);
                    Header.LayoutChildren(Header[1]);
                }
            }
        }

        public int DevOpsId { get; }

        private void AddItem([Required] string key, Bitmap image = null)
        {
            var list = Items;
            var recordItem = new KanbanSubItem();
            recordItem.Label.Width = (DefaultWidth / 2f) - 50f;
            recordItem.Label.WrappingWidth = (DefaultWidth / 2f) - 50f;
            recordItem.Label.Text = key;
            recordItem.Value.WrappingWidth = DefaultWidth / 2f;
            if (image != null)
                recordItem.Image.Image = image;
            list.Add(recordItem);
        }

        private void AddHyperlink([Required] string key, Bitmap image = null)
        {
            var list = Items;
            var recordItem = new KanbanHyperlink();
            recordItem.Label.Width = (DefaultWidth / 2f) - 50f;
            recordItem.Label.WrappingWidth = (DefaultWidth / 2f) - 50f;
            recordItem.Label.Text = key;
            recordItem.Value.Width = DefaultWidth / 2f - 50f;
            if (image != null)
                recordItem.Image.Image = image;
            list.Add(recordItem);
        }

        public void SetInfo([Required] string key, string value)
        {
            var item = Items.OfType<KanbanSubItem>()
                .FirstOrDefault(x => string.CompareOrdinal(x.Label.Text, key) == 0);
            if (item != null)
                item.Value.Text = value;
        }

        public void SetHyperlinkInfo([Required] string key, string label, string url)
        {
            var item = Items.OfType<KanbanHyperlink>()
                .FirstOrDefault(x => string.CompareOrdinal(x.Label.Text, key) == 0);
            if (item != null)
            {
                item.HyperlinkLabel = label;
                item.Hyperlink = url;
            }
        }

        private GoListGroup Header => (GoListGroup)((GoGroup)this.Body)[0];

        private GoListGroup Items => (GoListGroup)((GoGroup)this.Body)[1];

        protected override GoObject CreateBody() 
        {
            var container = new GoListGroup
            {
                Selectable = false,
                BrushColor = Color.White,
                //BorderPenColor = Color.Transparent,
                //LinePenColor = Color.Transparent,
                //LinePenWidth = 0.5f,
                //BorderPenWidth = 0f,
                Spacing = 2,
                TopLeftMargin = new SizeF(0, 0),
                BottomRightMargin = new SizeF(0, 2),
                Width = DefaultWidth
            };

            GoListGroup header = new GoListGroup
            {
                Orientation = Orientation.Horizontal,
                Selectable = false,
                AutoRescales = false,
                Width = DefaultWidth,
                Spacing = 2
            };

            GoText headerText = new GoText
            {
                DragsNode = true,
                Selectable = false,
                Editable = false,
                Wrapping = false,
                AutoRescales = false,
                AutoResizes = false,
                StringTrimming = StringTrimming.EllipsisCharacter,
                FontSize = 9,
                Width = DefaultWidth - 50,
                TextColor = Color.White,
                Height = 16 * Dpi.Factor.Height
            };
            header.Add(headerText);

            GoImage img = new GoImage
            {
                Selectable = false,
                AutoRescales = false,
                AutoResizes = false,
                Size = new SizeF(16 * Dpi.Factor.Width, 16 * Dpi.Factor.Height),
                Image = Properties.Resources.pencil,
                Visible = false
            };
            header.Add(img);

            container.Add(header);

            GoListGroup items = new GoListGroup();
            items.Selectable = false;
            container.Add(items);

            return container;
        }

        public override GoContextMenu GetContextMenu(GoView view)
        {
            if (view is GoOverview) return null;
            var result = new GoContextMenu(view);
            result.MenuItems.Add(new MenuItem("Associate to DevOps object", AssociateToDevOps));

            return result;
        }

        private void AssociateToDevOps(object sender, EventArgs e)
        {
            if (Item is IMitigation mitigation)
            {
                var dialog = new DevOpsWorkItemAssociationDialog(mitigation);
                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                {
                    var itemInfo = dialog.SelectedItemInfo;
                    //if (itemInfo != null)
                    //    SetHyperlinkInfo("Link", itemInfo.Id.ToString(), itemInfo.Url);
                    MoveRequired?.Invoke(this);
                }
            }
        }

        public void SetWidth(int width)
        {
            Header.Width = width;
            Header[0].Width = width - 50;
            Items.Width = width;
            var items = Items.OfType<KanbanSubItem>().ToArray();
            foreach (var item in items)
            {
                item.Label.Width = (width / 2f) - 50f;
                item.Label.WrappingWidth = (width / 2f) - 50f;
                item.Value.WrappingWidth = width / 2f;
            }
        }
    }
}
