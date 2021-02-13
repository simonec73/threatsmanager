using System.Linq;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Layout;
using ThreatsManager.Utilities;
using ItemEditor = ThreatsManager.Utilities.WinForms.ItemEditor;

namespace ThreatsManager.Extensions.Panels.Diagram
{
    partial class ModelPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            _properties.OpenDiagram -= OpenDiagram;
            GraphEntity.ParentChanged -= GraphEntityParentChanged;
            GraphGroup.ParentChanged -= GraphGroupParentChanged;

            _graph.SelectionDeleting -= _graph_SelectionDeleting;
            _graph.ObjectGotSelection -= _graph_ObjectGotSelection;
            _graph.ObjectLostSelection -= _graph_ObjectLostSelection;
            _graph.LinkCreated -= _graph_LinkCreated;

            _properties.Item = null;

            if (_diagram != null)
            {
                _diagram.Model.ChildCreated -= OnModelChildCreated;
                _diagram.Model.ChildRemoved -= OnModelChildRemoved;
            }

            var shapes = _entities.Values.OfType<GraphEntity>().ToArray();
            if (shapes.Any())
            {
                foreach (var shape in shapes)
                    RemoveRegisteredEvents(shape);
            }

            var links = _links.Values.OfType<GraphLink>().ToArray();
            if (links.Any())
            {
                foreach (var link in links)
                    RemoveRegisteredEvents(link);
            }

            var groups = _groups.Values.OfType<GraphGroup>().ToArray();
            if (groups.Any())
            {
                foreach (var group in groups)
                    group.Dispose();
            }

            RemoveActionEvents();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelPanel));
            this._palettePanel = new System.Windows.Forms.Panel();
            this._tabContainer = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel4 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this._templatesPalette = new ThreatsManager.Extensions.Panels.Diagram.GraphPalette();
            this._templateToolsPanel = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._templateRefresh = new System.Windows.Forms.Button();
            this._templateTypes = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this._templateExternalInteractor = new DevComponents.Editors.ComboItem();
            this._templateProcess = new DevComponents.Editors.ComboItem();
            this._templateDataStore = new DevComponents.Editors.ComboItem();
            this._templateTrustBoundary = new DevComponents.Editors.ComboItem();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._templates = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this._standardPalette = new ThreatsManager.Extensions.Panels.Diagram.GraphPalette();
            this._standard = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this._existingPalette = new ThreatsManager.Extensions.Panels.Diagram.GraphPalette();
            this._existingToolsPanel = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._existingRefresh = new System.Windows.Forms.Button();
            this._existingTypes = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this._existingExternalInteractor = new DevComponents.Editors.ComboItem();
            this._existingProcess = new DevComponents.Editors.ComboItem();
            this._existingDataStore = new DevComponents.Editors.ComboItem();
            this._existingTrustBoundary = new DevComponents.Editors.ComboItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._existing = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel3 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this._threatsPalette = new ThreatsManager.Extensions.Panels.Diagram.GraphPalette();
            this._threatsToolsPanel = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._threatsSearch = new System.Windows.Forms.Button();
            this._threatsFilter = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this._threats = new DevComponents.DotNetBar.SuperTabItem();
            this._leftSplitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this._rightSplitter = new DevComponents.DotNetBar.ExpandableSplitter();
            this._properties = new ThreatsManager.Utilities.WinForms.ItemEditor();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this._graph = new ThreatsManager.Extensions.Panels.Diagram.GraphView();
            this._palettePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._tabContainer)).BeginInit();
            this._tabContainer.SuspendLayout();
            this.superTabControlPanel4.SuspendLayout();
            this._templateToolsPanel.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            this._existingToolsPanel.SuspendLayout();
            this.superTabControlPanel3.SuspendLayout();
            this._threatsToolsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _palettePanel
            // 
            this._palettePanel.BackColor = System.Drawing.Color.White;
            this._palettePanel.Controls.Add(this._tabContainer);
            this._palettePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this._palettePanel.Location = new System.Drawing.Point(0, 0);
            this._palettePanel.Name = "_palettePanel";
            this._palettePanel.Size = new System.Drawing.Size(179, 494);
            this._palettePanel.TabIndex = 2;
            // 
            // _tabContainer
            // 
            this._tabContainer.AutoCloseTabs = false;
            this._tabContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._tabContainer.CloseButtonOnTabsAlwaysDisplayed = false;
            // 
            // 
            // 
            // 
            // 
            // 
            this._tabContainer.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this._tabContainer.ControlBox.MenuBox.Name = "";
            this._tabContainer.ControlBox.Name = "";
            this._tabContainer.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._tabContainer.ControlBox.MenuBox,
            this._tabContainer.ControlBox.CloseBox});
            this._tabContainer.Controls.Add(this.superTabControlPanel4);
            this._tabContainer.Controls.Add(this.superTabControlPanel1);
            this._tabContainer.Controls.Add(this.superTabControlPanel2);
            this._tabContainer.Controls.Add(this.superTabControlPanel3);
            this._tabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabContainer.ForeColor = System.Drawing.Color.Black;
            this._tabContainer.Location = new System.Drawing.Point(0, 0);
            this._tabContainer.Name = "_tabContainer";
            this._tabContainer.ReorderTabsEnabled = false;
            this._tabContainer.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this._tabContainer.SelectedTabIndex = 0;
            this._tabContainer.Size = new System.Drawing.Size(179, 494);
            this._tabContainer.TabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._tabContainer.TabHorizontalSpacing = 0;
            this._tabContainer.TabIndex = 0;
            this._tabContainer.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._standard,
            this._templates,
            this._existing,
            this._threats});
            this._tabContainer.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.VisualStudio2008Dock;
            this._tabContainer.TabVerticalSpacing = 2;
            // 
            // superTabControlPanel4
            // 
            this.superTabControlPanel4.Controls.Add(this._templatesPalette);
            this.superTabControlPanel4.Controls.Add(this._templateToolsPanel);
            this.superTabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel4.Location = new System.Drawing.Point(0, 40);
            this.superTabControlPanel4.Name = "superTabControlPanel4";
            this.superTabControlPanel4.Size = new System.Drawing.Size(179, 454);
            this.superTabControlPanel4.TabIndex = 0;
            this.superTabControlPanel4.TabItem = this._templates;
            // 
            // _templatesPalette
            // 
            this._templatesPalette.AllowDelete = false;
            this._templatesPalette.AllowEdit = false;
            this._templatesPalette.AllowInsert = false;
            this._templatesPalette.AllowLink = false;
            this._templatesPalette.AllowMove = false;
            this._templatesPalette.AllowReshape = false;
            this._templatesPalette.AllowResize = false;
            this._templatesPalette.ArrowMoveLarge = 10F;
            this._templatesPalette.ArrowMoveSmall = 1F;
            this._templatesPalette.AutomaticLayout = false;
            this._templatesPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._templatesPalette.BackColor = System.Drawing.Color.White;
            this._templatesPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._templatesPalette.GridCellSizeHeight = 58F;
            this._templatesPalette.GridCellSizeWidth = 52F;
            this._templatesPalette.GridOriginX = 20F;
            this._templatesPalette.GridOriginY = 5F;
            this._templatesPalette.HidesSelection = true;
            this._templatesPalette.Location = new System.Drawing.Point(0, 35);
            this._templatesPalette.Name = "_templatesPalette";
            this._templatesPalette.ShowHorizontalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Hide;
            this._templatesPalette.ShowsNegativeCoordinates = false;
            this._templatesPalette.ShowVerticalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Show;
            this._templatesPalette.Size = new System.Drawing.Size(179, 419);
            this._templatesPalette.TabIndex = 3;
            this._templatesPalette.Text = "graphPalette1";
            // 
            // _templateToolsPanel
            // 
            this._templateToolsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._templateToolsPanel.Controls.Add(this._templateRefresh);
            this._templateToolsPanel.Controls.Add(this._templateTypes);
            this._templateToolsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._templateToolsPanel.ForeColor = System.Drawing.Color.Black;
            this._templateToolsPanel.Location = new System.Drawing.Point(0, 0);
            this._templateToolsPanel.Name = "_templateToolsPanel";
            // 
            // 
            // 
            this._templateToolsPanel.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this._templateToolsPanel.Size = new System.Drawing.Size(179, 35);
            this._templateToolsPanel.TabIndex = 2;
            this._templateToolsPanel.Text = "_templateToolsPanel";
            // 
            // _templateRefresh
            // 
            this._templateRefresh.Image = global::ThreatsManager.Extensions.Properties.Resources.nav_refresh_small;
            this._templateRefresh.Location = new System.Drawing.Point(147, 4);
            this._templateRefresh.Margin = new System.Windows.Forms.Padding(0);
            this._templateRefresh.Name = "_templateRefresh";
            this._templateRefresh.Size = new System.Drawing.Size(28, 26);
            this._templateRefresh.TabIndex = 1;
            this._templateRefresh.UseVisualStyleBackColor = true;
            this._templateRefresh.Click += new System.EventHandler(this._templatesRefresh_Click);
            // 
            // _templateTypes
            // 
            this._templateTypes.DisplayMember = "Text";
            this._templateTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._templateTypes.DropDownHeight = 300;
            this._templateTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._templateTypes.ForeColor = System.Drawing.Color.Black;
            this._templateTypes.FormattingEnabled = true;
            this._templateTypes.IntegralHeight = false;
            this._templateTypes.ItemHeight = 20;
            this._templateTypes.Items.AddRange(new object[] {
            this._templateExternalInteractor,
            this._templateProcess,
            this._templateDataStore,
            this._templateTrustBoundary});
            this._templateTypes.Location = new System.Drawing.Point(4, 4);
            this._templateTypes.Margin = new System.Windows.Forms.Padding(0);
            this._templateTypes.Name = "_templateTypes";
            this._templateTypes.PreventEnterBeep = true;
            this._templateTypes.Size = new System.Drawing.Size(135, 26);
            this._templateTypes.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._templateTypes.TabIndex = 0;
            this._templateTypes.WatermarkText = "Please select the Type";
            this._templateTypes.SelectedIndexChanged += new System.EventHandler(this._templateTypes_SelectedIndexChanged);
            // 
            // _templateExternalInteractor
            // 
            this._templateExternalInteractor.Text = "External Interactor";
            this._templateExternalInteractor.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // _templateProcess
            // 
            this._templateProcess.Text = "Process";
            this._templateProcess.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // _templateDataStore
            // 
            this._templateDataStore.Text = "Data Store";
            this._templateDataStore.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // _templateTrustBoundary
            // 
            this._templateTrustBoundary.Text = "Trust Boundary";
            this._templateTrustBoundary.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this._templateTypes;
            this.layoutControlItem1.Height = 34;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Label:";
            this.layoutControlItem1.TextVisible = false;
            this.layoutControlItem1.Width = 80;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._templateRefresh;
            this.layoutControlItem2.Height = 31;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Width = 20;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _templates
            // 
            this._templates.AttachedControl = this.superTabControlPanel4;
            this._templates.CloseButtonVisible = false;
            this._templates.GlobalItem = false;
            this._templates.Image = global::ThreatsManager.Extensions.Properties.Resources.rubber_stamp;
            this._templates.Name = "_templates";
            this._templates.Text = " ";
            this._templates.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Near;
            this._templates.Tooltip = "Item Templates";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this._standardPalette);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 40);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(179, 454);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this._standard;
            // 
            // _standardPalette
            // 
            this._standardPalette.AllowDelete = false;
            this._standardPalette.AllowEdit = false;
            this._standardPalette.AllowInsert = false;
            this._standardPalette.AllowLink = false;
            this._standardPalette.AllowMove = false;
            this._standardPalette.AllowReshape = false;
            this._standardPalette.AllowResize = false;
            this._standardPalette.ArrowMoveLarge = 10F;
            this._standardPalette.ArrowMoveSmall = 1F;
            this._standardPalette.AutomaticLayout = false;
            this._standardPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._standardPalette.BackColor = System.Drawing.Color.White;
            this._standardPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._standardPalette.GridCellSizeHeight = 58F;
            this._standardPalette.GridCellSizeWidth = 52F;
            this._standardPalette.GridOriginX = 20F;
            this._standardPalette.GridOriginY = 5F;
            this._standardPalette.HidesSelection = true;
            this._standardPalette.Location = new System.Drawing.Point(0, 0);
            this._standardPalette.Name = "_standardPalette";
            this._standardPalette.ShowHorizontalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Hide;
            this._standardPalette.ShowsNegativeCoordinates = false;
            this._standardPalette.ShowVerticalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Show;
            this._standardPalette.Size = new System.Drawing.Size(179, 454);
            this._standardPalette.TabIndex = 0;
            this._standardPalette.Text = "graphPalette1";
            this._standardPalette.MouseEnter += new System.EventHandler(this._standardPalette_MouseEnter);
            // 
            // _standard
            // 
            this._standard.AttachedControl = this.superTabControlPanel1;
            this._standard.CloseButtonVisible = false;
            this._standard.GlobalItem = false;
            this._standard.Image = global::ThreatsManager.Extensions.Properties.Resources.shapes;
            this._standard.Name = "_standard";
            this._standard.Text = " ";
            this._standard.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Near;
            this._standard.Tooltip = "Basic Objects";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this._existingPalette);
            this.superTabControlPanel2.Controls.Add(this._existingToolsPanel);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 40);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(179, 454);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this._existing;
            // 
            // _existingPalette
            // 
            this._existingPalette.AllowDelete = false;
            this._existingPalette.AllowEdit = false;
            this._existingPalette.AllowInsert = false;
            this._existingPalette.AllowLink = false;
            this._existingPalette.AllowMove = false;
            this._existingPalette.AllowReshape = false;
            this._existingPalette.AllowResize = false;
            this._existingPalette.ArrowMoveLarge = 10F;
            this._existingPalette.ArrowMoveSmall = 1F;
            this._existingPalette.AutomaticLayout = false;
            this._existingPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._existingPalette.BackColor = System.Drawing.Color.White;
            this._existingPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._existingPalette.GridCellSizeHeight = 58F;
            this._existingPalette.GridCellSizeWidth = 52F;
            this._existingPalette.GridOriginX = 20F;
            this._existingPalette.GridOriginY = 5F;
            this._existingPalette.HidesSelection = true;
            this._existingPalette.Location = new System.Drawing.Point(0, 35);
            this._existingPalette.Name = "_existingPalette";
            this._existingPalette.ShowHorizontalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Hide;
            this._existingPalette.ShowsNegativeCoordinates = false;
            this._existingPalette.ShowVerticalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Show;
            this._existingPalette.Size = new System.Drawing.Size(179, 419);
            this._existingPalette.TabIndex = 1;
            this._existingPalette.Text = "graphPalette1";
            this._existingPalette.MouseEnter += new System.EventHandler(this._existingPalette_MouseEnter);
            // 
            // _existingToolsPanel
            // 
            this._existingToolsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._existingToolsPanel.Controls.Add(this._existingRefresh);
            this._existingToolsPanel.Controls.Add(this._existingTypes);
            this._existingToolsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._existingToolsPanel.ForeColor = System.Drawing.Color.Black;
            this._existingToolsPanel.Location = new System.Drawing.Point(0, 0);
            this._existingToolsPanel.Name = "_existingToolsPanel";
            // 
            // 
            // 
            this._existingToolsPanel.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem3,
            this.layoutControlItem4});
            this._existingToolsPanel.Size = new System.Drawing.Size(179, 35);
            this._existingToolsPanel.TabIndex = 0;
            this._existingToolsPanel.Text = "_existingToolsPanel";
            // 
            // _existingRefresh
            // 
            this._existingRefresh.Image = global::ThreatsManager.Extensions.Properties.Resources.nav_refresh_small;
            this._existingRefresh.Location = new System.Drawing.Point(147, 4);
            this._existingRefresh.Margin = new System.Windows.Forms.Padding(0);
            this._existingRefresh.Name = "_existingRefresh";
            this._existingRefresh.Size = new System.Drawing.Size(28, 26);
            this._existingRefresh.TabIndex = 1;
            this._existingRefresh.UseVisualStyleBackColor = true;
            this._existingRefresh.Click += new System.EventHandler(this._existingRefresh_Click);
            // 
            // _existingTypes
            // 
            this._existingTypes.DisplayMember = "Text";
            this._existingTypes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._existingTypes.DropDownHeight = 300;
            this._existingTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._existingTypes.ForeColor = System.Drawing.Color.Black;
            this._existingTypes.FormattingEnabled = true;
            this._existingTypes.IntegralHeight = false;
            this._existingTypes.ItemHeight = 20;
            this._existingTypes.Items.AddRange(new object[] {
            this._existingExternalInteractor,
            this._existingProcess,
            this._existingDataStore,
            this._existingTrustBoundary});
            this._existingTypes.Location = new System.Drawing.Point(4, 4);
            this._existingTypes.Margin = new System.Windows.Forms.Padding(0);
            this._existingTypes.Name = "_existingTypes";
            this._existingTypes.PreventEnterBeep = true;
            this._existingTypes.Size = new System.Drawing.Size(135, 26);
            this._existingTypes.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._existingTypes.TabIndex = 0;
            this._existingTypes.WatermarkText = "Please select the Type";
            this._existingTypes.SelectedIndexChanged += new System.EventHandler(this._existingTypes_SelectedIndexChanged);
            // 
            // _existingExternalInteractor
            // 
            this._existingExternalInteractor.Text = "External Interactor";
            this._existingExternalInteractor.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // _existingProcess
            // 
            this._existingProcess.Text = "Process";
            this._existingProcess.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // _existingDataStore
            // 
            this._existingDataStore.Text = "Data Store";
            this._existingDataStore.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // _existingTrustBoundary
            // 
            this._existingTrustBoundary.Text = "Trust Boundary";
            this._existingTrustBoundary.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._existingTypes;
            this.layoutControlItem3.Height = 34;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Label:";
            this.layoutControlItem3.TextVisible = false;
            this.layoutControlItem3.Width = 80;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._existingRefresh;
            this.layoutControlItem4.Height = 31;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Width = 20;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _existing
            // 
            this._existing.AttachedControl = this.superTabControlPanel2;
            this._existing.CloseButtonVisible = false;
            this._existing.GlobalItem = false;
            this._existing.Image = global::ThreatsManager.Extensions.Properties.Resources.shelf_full;
            this._existing.Name = "_existing";
            this._existing.Text = " ";
            this._existing.TextAlignment = DevComponents.DotNetBar.eItemAlignment.Near;
            this._existing.Tooltip = "Existing Objects";
            // 
            // superTabControlPanel3
            // 
            this.superTabControlPanel3.Controls.Add(this._threatsPalette);
            this.superTabControlPanel3.Controls.Add(this._threatsToolsPanel);
            this.superTabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel3.Location = new System.Drawing.Point(0, 40);
            this.superTabControlPanel3.Name = "superTabControlPanel3";
            this.superTabControlPanel3.Size = new System.Drawing.Size(179, 454);
            this.superTabControlPanel3.TabIndex = 0;
            this.superTabControlPanel3.TabItem = this._threats;
            // 
            // _threatsPalette
            // 
            this._threatsPalette.AllowDelete = false;
            this._threatsPalette.AllowEdit = false;
            this._threatsPalette.AllowInsert = false;
            this._threatsPalette.AllowLink = false;
            this._threatsPalette.AllowMove = false;
            this._threatsPalette.AllowReshape = false;
            this._threatsPalette.AllowResize = false;
            this._threatsPalette.ArrowMoveLarge = 10F;
            this._threatsPalette.ArrowMoveSmall = 1F;
            this._threatsPalette.AutomaticLayout = false;
            this._threatsPalette.AutoScrollRegion = new System.Drawing.Size(0, 0);
            this._threatsPalette.BackColor = System.Drawing.Color.White;
            this._threatsPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this._threatsPalette.GridCellSizeHeight = 58F;
            this._threatsPalette.GridCellSizeWidth = 52F;
            this._threatsPalette.GridOriginX = 20F;
            this._threatsPalette.GridOriginY = 5F;
            this._threatsPalette.HidesSelection = true;
            this._threatsPalette.Location = new System.Drawing.Point(0, 35);
            this._threatsPalette.Name = "_threatsPalette";
            this._threatsPalette.ShowHorizontalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Hide;
            this._threatsPalette.ShowsNegativeCoordinates = false;
            this._threatsPalette.ShowVerticalScrollBar = Northwoods.Go.GoViewScrollBarVisibility.Show;
            this._threatsPalette.Size = new System.Drawing.Size(179, 419);
            this._threatsPalette.TabIndex = 2;
            this._threatsPalette.Text = "graphPalette1";
            this._threatsPalette.MouseEnter += new System.EventHandler(this._threatsPalette_MouseEnter);
            // 
            // _threatsToolsPanel
            // 
            this._threatsToolsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._threatsToolsPanel.Controls.Add(this._threatsSearch);
            this._threatsToolsPanel.Controls.Add(this._threatsFilter);
            this._threatsToolsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._threatsToolsPanel.ForeColor = System.Drawing.Color.Black;
            this._threatsToolsPanel.Location = new System.Drawing.Point(0, 0);
            this._threatsToolsPanel.Name = "_threatsToolsPanel";
            // 
            // 
            // 
            this._threatsToolsPanel.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem5,
            this.layoutControlItem6});
            this._threatsToolsPanel.Size = new System.Drawing.Size(179, 35);
            this._threatsToolsPanel.TabIndex = 1;
            this._threatsToolsPanel.Text = "_templateToolsPanel";
            // 
            // _threatsSearch
            // 
            this._threatsSearch.Image = global::ThreatsManager.Extensions.Properties.Resources.nav_refresh_small;
            this._threatsSearch.Location = new System.Drawing.Point(147, 4);
            this._threatsSearch.Margin = new System.Windows.Forms.Padding(0);
            this._threatsSearch.Name = "_threatsSearch";
            this._threatsSearch.Size = new System.Drawing.Size(28, 27);
            this._threatsSearch.TabIndex = 1;
            this._threatsSearch.UseVisualStyleBackColor = true;
            this._threatsSearch.Click += new System.EventHandler(this._threatsSearch_Click);
            // 
            // _threatsFilter
            // 
            this._threatsFilter.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this._threatsFilter.Border.Class = "TextBoxBorder";
            this._threatsFilter.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._threatsFilter.DisabledBackColor = System.Drawing.Color.White;
            this._threatsFilter.ForeColor = System.Drawing.Color.Black;
            this._threatsFilter.Location = new System.Drawing.Point(4, 4);
            this._threatsFilter.Margin = new System.Windows.Forms.Padding(0);
            this._threatsFilter.Name = "_threatsFilter";
            this._threatsFilter.PreventEnterBeep = true;
            this._threatsFilter.Size = new System.Drawing.Size(135, 20);
            this._threatsFilter.TabIndex = 0;
            this._threatsFilter.WatermarkText = "Please specify the text to search";
            this._threatsFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._threatsFilter_KeyPress);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._threatsFilter;
            this.layoutControlItem5.Height = 35;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.TextVisible = false;
            this.layoutControlItem5.Width = 80;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._threatsSearch;
            this.layoutControlItem6.Height = 31;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Width = 20;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // _threats
            // 
            this._threats.AttachedControl = this.superTabControlPanel3;
            this._threats.CloseButtonVisible = false;
            this._threats.GlobalItem = false;
            this._threats.Image = ((System.Drawing.Image)(resources.GetObject("_threats.Image")));
            this._threats.Name = "_threats";
            this._threats.Text = " ";
            this._threats.Tooltip = "Threat Types";
            // 
            // _leftSplitter
            // 
            this._leftSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._leftSplitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._leftSplitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._leftSplitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._leftSplitter.ExpandableControl = this._palettePanel;
            this._leftSplitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._leftSplitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._leftSplitter.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._leftSplitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._leftSplitter.ForeColor = System.Drawing.Color.Black;
            this._leftSplitter.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._leftSplitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._leftSplitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._leftSplitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._leftSplitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this._leftSplitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this._leftSplitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this._leftSplitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this._leftSplitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._leftSplitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._leftSplitter.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._leftSplitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._leftSplitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._leftSplitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._leftSplitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._leftSplitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._leftSplitter.Location = new System.Drawing.Point(179, 0);
            this._leftSplitter.Name = "_leftSplitter";
            this._leftSplitter.Size = new System.Drawing.Size(6, 494);
            this._leftSplitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this._leftSplitter.TabIndex = 3;
            this._leftSplitter.TabStop = false;
            // 
            // _rightSplitter
            // 
            this._rightSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._rightSplitter.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._rightSplitter.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._rightSplitter.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this._rightSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            this._rightSplitter.ExpandableControl = this._properties;
            this._rightSplitter.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._rightSplitter.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._rightSplitter.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._rightSplitter.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._rightSplitter.ForeColor = System.Drawing.Color.Black;
            this._rightSplitter.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._rightSplitter.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._rightSplitter.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._rightSplitter.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._rightSplitter.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this._rightSplitter.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this._rightSplitter.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this._rightSplitter.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this._rightSplitter.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._rightSplitter.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._rightSplitter.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this._rightSplitter.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this._rightSplitter.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this._rightSplitter.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this._rightSplitter.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this._rightSplitter.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this._rightSplitter.Location = new System.Drawing.Point(471, 0);
            this._rightSplitter.Name = "_rightSplitter";
            this._rightSplitter.Size = new System.Drawing.Size(6, 494);
            this._rightSplitter.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this._rightSplitter.TabIndex = 1;
            this._rightSplitter.TabStop = false;
            // 
            // _properties
            // 
            this._properties.BackColor = System.Drawing.Color.White;
            this._properties.Dock = System.Windows.Forms.DockStyle.Right;
            this._properties.Item = null;
            this._properties.Location = new System.Drawing.Point(477, 0);
            this._properties.Name = "_properties";
            this._properties.ReadOnly = false;
            this._properties.Size = new System.Drawing.Size(309, 494);
            this._properties.TabIndex = 0;
            this._properties.Enter += new System.EventHandler(this._properties_Enter);
            this._properties.Leave += new System.EventHandler(this._properties_Leave);
            this._properties.MouseEnter += new System.EventHandler(this._properties_MouseEnter);
            // 
            // comboItem1
            // 
            this.comboItem1.Image = ((System.Drawing.Image)(resources.GetObject("comboItem1.Image")));
            this.comboItem1.Text = "External Interactor";
            this.comboItem1.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // comboItem2
            // 
            this.comboItem2.Image = ((System.Drawing.Image)(resources.GetObject("comboItem2.Image")));
            this.comboItem2.Text = "Process";
            this.comboItem2.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // comboItem3
            // 
            this.comboItem3.Image = ((System.Drawing.Image)(resources.GetObject("comboItem3.Image")));
            this.comboItem3.Text = "Data Store";
            this.comboItem3.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // comboItem4
            // 
            this.comboItem4.Image = ((System.Drawing.Image)(resources.GetObject("comboItem4.Image")));
            this.comboItem4.Text = "Trust Boundary";
            this.comboItem4.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // _graph
            // 
            this._graph.ArrowMoveLarge = 10F;
            this._graph.ArrowMoveSmall = 1F;
            this._graph.BackColor = System.Drawing.Color.White;
            this._graph.Dock = System.Windows.Forms.DockStyle.Fill;
            this._graph.DragsRealtime = true;
            this._graph.GridCellSizeHeight = 10F;
            this._graph.GridCellSizeWidth = 10F;
            this._graph.GridSnapDrag = Northwoods.Go.GoViewSnapStyle.Jump;
            this._graph.Location = new System.Drawing.Point(185, 0);
            this._graph.Name = "_graph";
            this._graph.Size = new System.Drawing.Size(286, 494);
            this._graph.TabIndex = 4;
            this._graph.Text = "graphView1";
            this._graph.SelectionDeleting += new System.ComponentModel.CancelEventHandler(this._graph_SelectionDeleting);
            this._graph.ObjectGotSelection += new Northwoods.Go.GoSelectionEventHandler(this._graph_ObjectGotSelection);
            this._graph.ObjectLostSelection += new Northwoods.Go.GoSelectionEventHandler(this._graph_ObjectLostSelection);
            this._graph.LinkCreated += new Northwoods.Go.GoSelectionEventHandler(this._graph_LinkCreated);
            // 
            // ModelPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._graph);
            this.Controls.Add(this._leftSplitter);
            this.Controls.Add(this._palettePanel);
            this.Controls.Add(this._rightSplitter);
            this.Controls.Add(this._properties);
            this.Name = "ModelPanel";
            this.Size = new System.Drawing.Size(786, 494);
            this._palettePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._tabContainer)).EndInit();
            this._tabContainer.ResumeLayout(false);
            this.superTabControlPanel4.ResumeLayout(false);
            this._templateToolsPanel.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this._existingToolsPanel.ResumeLayout(false);
            this.superTabControlPanel3.ResumeLayout(false);
            this._threatsToolsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ItemEditor _properties;
        private DevComponents.DotNetBar.ExpandableSplitter _rightSplitter;
        private System.Windows.Forms.Panel _palettePanel;
        private DevComponents.DotNetBar.ExpandableSplitter _leftSplitter;
        private GraphView _graph;
        private DevComponents.DotNetBar.SuperTabControl _tabContainer;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private GraphPalette _standardPalette;
        private DevComponents.DotNetBar.SuperTabItem _standard;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem _existing;
        private GraphPalette _existingPalette;
        private DevComponents.DotNetBar.Layout.LayoutControl _existingToolsPanel;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel3;
        private DevComponents.DotNetBar.SuperTabItem _threats;
        private DevComponents.DotNetBar.Layout.LayoutControl _threatsToolsPanel;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem4;
        private GraphPalette _threatsPalette;
        private SuperTabControlPanel superTabControlPanel4;
        private SuperTabItem _templates;
        private GraphPalette _templatesPalette;
        private LayoutControl _templateToolsPanel;
        private System.Windows.Forms.Button _templateRefresh;
        private LayoutControlItem layoutControlItem2;
        private System.Windows.Forms.Button _existingRefresh;
        private LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Controls.TextBoxX _threatsFilter;
        private LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.Button _threatsSearch;
        private LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Controls.ComboBoxEx _templateTypes;
        private DevComponents.Editors.ComboItem _templateExternalInteractor;
        private DevComponents.Editors.ComboItem _templateProcess;
        private DevComponents.Editors.ComboItem _templateDataStore;
        private LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx _existingTypes;
        private DevComponents.Editors.ComboItem _existingExternalInteractor;
        private DevComponents.Editors.ComboItem _existingProcess;
        private DevComponents.Editors.ComboItem _existingDataStore;
        private DevComponents.Editors.ComboItem _existingTrustBoundary;
        private LayoutControlItem layoutControlItem3;
        private DevComponents.Editors.ComboItem _templateTrustBoundary;
    }
}
