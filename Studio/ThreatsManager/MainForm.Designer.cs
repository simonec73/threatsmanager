namespace ThreatsManager
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._ribbon = new DevComponents.DotNetBar.RibbonControl();
            this._ribbonPanelHome = new DevComponents.DotNetBar.RibbonPanel();
            this._ribbonPanelView = new DevComponents.DotNetBar.RibbonPanel();
            this._viewWindow = new DevComponents.DotNetBar.RibbonBar();
            this._windows = new DevComponents.DotNetBar.ButtonItem();
            this._closeWindows = new DevComponents.DotNetBar.ButtonItem();
            this._closeWindow = new DevComponents.DotNetBar.ButtonItem();
            this._ribbonPanelInsert = new DevComponents.DotNetBar.RibbonPanel();
            this._ribbonPanelIntegrate = new DevComponents.DotNetBar.RibbonPanel();
            this._ribbonPanelExport = new DevComponents.DotNetBar.RibbonPanel();
            this._ribbonPanelAnalyze = new DevComponents.DotNetBar.RibbonPanel();
            this._ribbonPanelReview = new DevComponents.DotNetBar.RibbonPanel();
            this._ribbonPanelHelp = new DevComponents.DotNetBar.RibbonPanel();
            this._ribbonPanelConfigure = new DevComponents.DotNetBar.RibbonPanel();
            this._ribbonPanelImport = new DevComponents.DotNetBar.RibbonPanel();
            this._fileButton = new DevComponents.DotNetBar.ApplicationButton();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.itemContainer2 = new DevComponents.DotNetBar.ItemContainer();
            this.itemContainer3 = new DevComponents.DotNetBar.ItemContainer();
            this._new = new DevComponents.DotNetBar.ButtonItem();
            this._open = new DevComponents.DotNetBar.ButtonItem();
            this._save = new DevComponents.DotNetBar.ButtonItem();
            this._saveAs = new DevComponents.DotNetBar.ButtonItem();
            this._close = new DevComponents.DotNetBar.ButtonItem();
            this._extensionsConfig = new DevComponents.DotNetBar.ButtonItem();
            this._about = new DevComponents.DotNetBar.ButtonItem();
            this._recentDocuments = new DevComponents.DotNetBar.GalleryContainer();
            this.labelItem8 = new DevComponents.DotNetBar.LabelItem();
            this.itemContainer4 = new DevComponents.DotNetBar.ItemContainer();
            this._options = new DevComponents.DotNetBar.ButtonItem();
            this._exit = new DevComponents.DotNetBar.ButtonItem();
            this._ribbonTabHome = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabInsert = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabView = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabAnalyze = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabImport = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabExport = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabIntegrate = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabReview = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabConfigure = new DevComponents.DotNetBar.RibbonTabItem();
            this._ribbonTabHelp = new DevComponents.DotNetBar.RibbonTabItem();
            this._title = new DevComponents.DotNetBar.LabelItem();
            this._closeCurrentWindow = new DevComponents.DotNetBar.ButtonItem();
            this._closeAllWindows = new DevComponents.DotNetBar.ButtonItem();
            this._feedback = new DevComponents.DotNetBar.ButtonItem();
            this._controlMinimize = new DevComponents.DotNetBar.ButtonItem();
            this._controlMaximize = new DevComponents.DotNetBar.ButtonItem();
            this._controlExit = new DevComponents.DotNetBar.ButtonItem();
            this.qatCustomizeItem1 = new DevComponents.DotNetBar.QatCustomizeItem();
            this._openFile = new System.Windows.Forms.OpenFileDialog();
            this._saveAsFile = new System.Windows.Forms.SaveFileDialog();
            this._statusBar = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this._addStatusInfoProvider = new DevComponents.DotNetBar.ButtonItem();
            this._lockRequest = new DevComponents.DotNetBar.LabelItem();
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this._autosaveTimer = new System.Windows.Forms.Timer(this.components);
            this._styleManager = new DevComponents.DotNetBar.StyleManager(this.components);
            this._selectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this._ribbon.SuspendLayout();
            this._ribbonPanelView.SuspendLayout();
            this.SuspendLayout();
            // 
            // _ribbon
            // 
            this._ribbon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this._ribbon.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbon.Controls.Add(this._ribbonPanelHome);
            this._ribbon.Controls.Add(this._ribbonPanelInsert);
            this._ribbon.Controls.Add(this._ribbonPanelView);
            this._ribbon.Controls.Add(this._ribbonPanelIntegrate);
            this._ribbon.Controls.Add(this._ribbonPanelExport);
            this._ribbon.Controls.Add(this._ribbonPanelAnalyze);
            this._ribbon.Controls.Add(this._ribbonPanelReview);
            this._ribbon.Controls.Add(this._ribbonPanelHelp);
            this._ribbon.Controls.Add(this._ribbonPanelConfigure);
            this._ribbon.Controls.Add(this._ribbonPanelImport);
            this._ribbon.Dock = System.Windows.Forms.DockStyle.Top;
            this._ribbon.ForeColor = System.Drawing.Color.Black;
            this._ribbon.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._fileButton,
            this._ribbonTabHome,
            this._ribbonTabInsert,
            this._ribbonTabView,
            this._ribbonTabAnalyze,
            this._ribbonTabImport,
            this._ribbonTabExport,
            this._ribbonTabIntegrate,
            this._ribbonTabReview,
            this._ribbonTabConfigure,
            this._ribbonTabHelp,
            this._title,
            this._closeCurrentWindow,
            this._closeAllWindows,
            this._feedback,
            this._controlMinimize,
            this._controlMaximize,
            this._controlExit});
            this._ribbon.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this._ribbon.Location = new System.Drawing.Point(5, 1);
            this._ribbon.Margin = new System.Windows.Forms.Padding(2);
            this._ribbon.MdiSystemItemVisible = false;
            this._ribbon.Name = "_ribbon";
            this._ribbon.QuickToolbarItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.qatCustomizeItem1});
            this._ribbon.Size = new System.Drawing.Size(1078, 140);
            this._ribbon.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbon.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this._ribbon.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this._ribbon.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this._ribbon.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this._ribbon.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this._ribbon.SystemText.QatDialogAddButton = "&Add >>";
            this._ribbon.SystemText.QatDialogCancelButton = "Cancel";
            this._ribbon.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this._ribbon.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this._ribbon.SystemText.QatDialogOkButton = "OK";
            this._ribbon.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this._ribbon.SystemText.QatDialogRemoveButton = "&Remove";
            this._ribbon.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this._ribbon.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this._ribbon.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this._ribbon.TabGroupHeight = 14;
            this._ribbon.TabIndex = 0;
            this._ribbon.Text = "ribbonControl1";
            // 
            // _ribbonPanelHome
            // 
            this._ribbonPanelHome.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelHome.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelHome.Name = "_ribbonPanelHome";
            this._ribbonPanelHome.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelHome.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelHome.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelHome.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelHome.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelHome.TabIndex = 4;
            // 
            // _ribbonPanelView
            // 
            this._ribbonPanelView.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelView.Controls.Add(this._viewWindow);
            this._ribbonPanelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelView.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelView.Margin = new System.Windows.Forms.Padding(2);
            this._ribbonPanelView.Name = "_ribbonPanelView";
            this._ribbonPanelView.Padding = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this._ribbonPanelView.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelView.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelView.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelView.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelView.TabIndex = 2;
            this._ribbonPanelView.Visible = false;
            // 
            // _viewWindow
            // 
            this._viewWindow.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this._viewWindow.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._viewWindow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._viewWindow.ContainerControlProcessDialogKey = true;
            this._viewWindow.Dock = System.Windows.Forms.DockStyle.Left;
            this._viewWindow.DragDropSupport = true;
            this._viewWindow.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._windows,
            this._closeWindows,
            this._closeWindow});
            this._viewWindow.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._viewWindow.Location = new System.Drawing.Point(2, 0);
            this._viewWindow.Name = "_viewWindow";
            this._viewWindow.OverflowButtonImage = global::ThreatsManager.Properties.Resources.cabinet;
            this._viewWindow.Size = new System.Drawing.Size(162, 95);
            this._viewWindow.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._viewWindow.TabIndex = 0;
            this._viewWindow.Text = "Windows";
            // 
            // 
            // 
            this._viewWindow.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._viewWindow.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // _windows
            // 
            this._windows.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._windows.Image = global::ThreatsManager.Properties.Resources.windows;
            this._windows.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this._windows.ImageSmall = global::ThreatsManager.Properties.Resources.windows_small;
            this._windows.Name = "_windows";
            this._windows.SubItemsExpandWidth = 14;
            this._windows.Text = "Iterate Windows";
            this._windows.Click += new System.EventHandler(this._windows_Click);
            // 
            // _closeWindows
            // 
            this._closeWindows.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._closeWindows.Image = global::ThreatsManager.Properties.Resources.windows_close;
            this._closeWindows.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this._closeWindows.ImageSmall = global::ThreatsManager.Properties.Resources.windows_close_small;
            this._closeWindows.Name = "_closeWindows";
            this._closeWindows.ShowSubItems = false;
            this._closeWindows.SubItemsExpandWidth = 14;
            this._closeWindows.Text = "Close All Windows";
            this._closeWindows.Click += new System.EventHandler(this._closeAllWindows_Click);
            // 
            // _closeWindow
            // 
            this._closeWindow.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._closeWindow.Image = global::ThreatsManager.Properties.Resources.window_close;
            this._closeWindow.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this._closeWindow.ImageSmall = global::ThreatsManager.Properties.Resources.window_close_small;
            this._closeWindow.Name = "_closeWindow";
            this._closeWindow.ShowSubItems = false;
            this._closeWindow.SubItemsExpandWidth = 14;
            this._superTooltip.SetSuperTooltip(this._closeWindow, new DevComponents.DotNetBar.SuperTooltipInfo("Close the current window (Ctrl+F4)", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray));
            this._closeWindow.Text = "Close Window";
            this._closeWindow.Click += new System.EventHandler(this._closeCurrentWindow_Click);
            // 
            // _ribbonPanelInsert
            // 
            this._ribbonPanelInsert.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelInsert.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelInsert.Name = "_ribbonPanelInsert";
            this._ribbonPanelInsert.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelInsert.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelInsert.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelInsert.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelInsert.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelInsert.TabIndex = 5;
            this._ribbonPanelInsert.Visible = false;
            // 
            // _ribbonPanelIntegrate
            // 
            this._ribbonPanelIntegrate.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelIntegrate.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelIntegrate.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelIntegrate.Name = "_ribbonPanelIntegrate";
            this._ribbonPanelIntegrate.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelIntegrate.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelIntegrate.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelIntegrate.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelIntegrate.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelIntegrate.TabIndex = 11;
            this._ribbonPanelIntegrate.Visible = false;
            // 
            // _ribbonPanelExport
            // 
            this._ribbonPanelExport.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelExport.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelExport.Name = "_ribbonPanelExport";
            this._ribbonPanelExport.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelExport.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelExport.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelExport.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelExport.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelExport.TabIndex = 3;
            this._ribbonPanelExport.Visible = false;
            // 
            // _ribbonPanelAnalyze
            // 
            this._ribbonPanelAnalyze.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelAnalyze.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelAnalyze.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelAnalyze.Name = "_ribbonPanelAnalyze";
            this._ribbonPanelAnalyze.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelAnalyze.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelAnalyze.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelAnalyze.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelAnalyze.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelAnalyze.TabIndex = 7;
            this._ribbonPanelAnalyze.Visible = false;
            // 
            // _ribbonPanelReview
            // 
            this._ribbonPanelReview.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelReview.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelReview.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelReview.Name = "_ribbonPanelReview";
            this._ribbonPanelReview.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelReview.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelReview.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelReview.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelReview.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelReview.TabIndex = 6;
            this._ribbonPanelReview.Visible = false;
            // 
            // _ribbonPanelHelp
            // 
            this._ribbonPanelHelp.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelHelp.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelHelp.Name = "_ribbonPanelHelp";
            this._ribbonPanelHelp.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelHelp.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelHelp.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelHelp.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelHelp.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelHelp.TabIndex = 10;
            this._ribbonPanelHelp.Visible = false;
            // 
            // _ribbonPanelConfigure
            // 
            this._ribbonPanelConfigure.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelConfigure.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelConfigure.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelConfigure.Name = "_ribbonPanelConfigure";
            this._ribbonPanelConfigure.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelConfigure.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelConfigure.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelConfigure.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelConfigure.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelConfigure.TabIndex = 9;
            this._ribbonPanelConfigure.Visible = false;
            // 
            // _ribbonPanelImport
            // 
            this._ribbonPanelImport.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonPanelImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ribbonPanelImport.Location = new System.Drawing.Point(0, 43);
            this._ribbonPanelImport.Name = "_ribbonPanelImport";
            this._ribbonPanelImport.Padding = new System.Windows.Forms.Padding(3, 0, 3, 2);
            this._ribbonPanelImport.Size = new System.Drawing.Size(1078, 97);
            // 
            // 
            // 
            this._ribbonPanelImport.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelImport.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonPanelImport.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonPanelImport.TabIndex = 8;
            this._ribbonPanelImport.Visible = false;
            // 
            // _fileButton
            // 
            this._fileButton.AutoExpandOnClick = true;
            this._fileButton.CanCustomize = false;
            this._fileButton.HotTrackingStyle = DevComponents.DotNetBar.eHotTrackingStyle.Image;
            this._fileButton.ImageFixedSize = new System.Drawing.Size(16, 16);
            this._fileButton.ImagePaddingHorizontal = 0;
            this._fileButton.ImagePaddingVertical = 1;
            this._fileButton.Name = "_fileButton";
            this._fileButton.ShowSubItems = false;
            this._fileButton.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer1});
            this._fileButton.Text = "&File";
            // 
            // itemContainer1
            // 
            // 
            // 
            // 
            this.itemContainer1.BackgroundStyle.Class = "RibbonFileMenuContainer";
            this.itemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer1.Name = "itemContainer1";
            this.itemContainer1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer2,
            this.itemContainer4});
            // 
            // 
            // 
            this.itemContainer1.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // itemContainer2
            // 
            // 
            // 
            // 
            this.itemContainer2.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemContainer2.BackgroundStyle.BackColor2 = System.Drawing.Color.White;
            this.itemContainer2.BackgroundStyle.Class = "RibbonFileMenuTwoColumnContainer";
            this.itemContainer2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer2.ItemSpacing = 0;
            this.itemContainer2.Name = "itemContainer2";
            this.itemContainer2.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer3,
            this._recentDocuments});
            // 
            // 
            // 
            this.itemContainer2.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer2.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // itemContainer3
            // 
            // 
            // 
            // 
            this.itemContainer3.BackgroundStyle.Class = "RibbonFileMenuColumnOneContainer";
            this.itemContainer3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer3.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer3.MinimumSize = new System.Drawing.Size(120, 0);
            this.itemContainer3.Name = "itemContainer3";
            this.itemContainer3.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._new,
            this._open,
            this._save,
            this._saveAs,
            this._close,
            this._extensionsConfig,
            this._about});
            // 
            // 
            // 
            this.itemContainer3.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer3.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // _new
            // 
            this._new.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._new.Image = global::ThreatsManager.Properties.Resources.document_empty;
            this._new.Name = "_new";
            this._new.SubItemsExpandWidth = 24;
            this._new.Text = "&New";
            this._new.Click += new System.EventHandler(this._new_Click);
            // 
            // _open
            // 
            this._open.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._open.Image = global::ThreatsManager.Properties.Resources.folder_open_document;
            this._open.Name = "_open";
            this._open.SubItemsExpandWidth = 24;
            this._open.Text = "&Open...";
            this._open.Click += new System.EventHandler(this._open_Click);
            // 
            // _save
            // 
            this._save.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._save.Image = global::ThreatsManager.Properties.Resources.floppy_disk;
            this._save.Name = "_save";
            this._save.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlS);
            this._save.SubItemsExpandWidth = 24;
            this._save.Text = "&Save";
            this._save.Click += new System.EventHandler(this._save_Click);
            // 
            // _saveAs
            // 
            this._saveAs.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._saveAs.Image = global::ThreatsManager.Properties.Resources.save_as;
            this._saveAs.Name = "_saveAs";
            this._saveAs.SubItemsExpandWidth = 24;
            this._saveAs.Text = "Save &As...";
            this._saveAs.Click += new System.EventHandler(this._saveAs_Click);
            // 
            // _close
            // 
            this._close.BeginGroup = true;
            this._close.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._close.Image = global::ThreatsManager.Properties.Resources.folder;
            this._close.Name = "_close";
            this._close.SubItemsExpandWidth = 24;
            this._close.Text = "&Close";
            this._close.Click += new System.EventHandler(this._close_Click);
            // 
            // _extensionsConfig
            // 
            this._extensionsConfig.BeginGroup = true;
            this._extensionsConfig.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._extensionsConfig.Image = global::ThreatsManager.Properties.Resources.gearwheel;
            this._extensionsConfig.Name = "_extensionsConfig";
            this._extensionsConfig.Text = "Extensions Configuration...";
            this._extensionsConfig.Click += new System.EventHandler(this._extensionsConfig_Click);
            // 
            // _about
            // 
            this._about.BeginGroup = true;
            this._about.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._about.Image = global::ThreatsManager.Properties.Resources.information;
            this._about.Name = "_about";
            this._about.Text = "About...";
            this._about.Click += new System.EventHandler(this._about_Click);
            // 
            // _recentDocuments
            // 
            // 
            // 
            // 
            this._recentDocuments.BackgroundStyle.Class = "RibbonFileMenuColumnTwoContainer";
            this._recentDocuments.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._recentDocuments.EnableGalleryPopup = false;
            this._recentDocuments.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this._recentDocuments.MinimumSize = new System.Drawing.Size(180, 240);
            this._recentDocuments.MultiLine = false;
            this._recentDocuments.Name = "_recentDocuments";
            this._recentDocuments.PopupUsesStandardScrollbars = false;
            this._recentDocuments.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem8});
            // 
            // 
            // 
            this._recentDocuments.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._recentDocuments.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // labelItem8
            // 
            this.labelItem8.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.labelItem8.BorderType = DevComponents.DotNetBar.eBorderType.Etched;
            this.labelItem8.CanCustomize = false;
            this.labelItem8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelItem8.Name = "labelItem8";
            this.labelItem8.PaddingBottom = 2;
            this.labelItem8.PaddingTop = 2;
            this.labelItem8.Stretch = true;
            this.labelItem8.Text = "Recent Documents";
            // 
            // itemContainer4
            // 
            // 
            // 
            // 
            this.itemContainer4.BackgroundStyle.Class = "RibbonFileMenuBottomContainer";
            this.itemContainer4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer4.HorizontalItemAlignment = DevComponents.DotNetBar.eHorizontalItemsAlignment.Right;
            this.itemContainer4.Name = "itemContainer4";
            this.itemContainer4.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._options,
            this._exit});
            // 
            // 
            // 
            this.itemContainer4.TitleMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.itemContainer4.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // _options
            // 
            this._options.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._options.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this._options.Image = global::ThreatsManager.Properties.Resources.gearwheels;
            this._options.Name = "_options";
            this._options.SubItemsExpandWidth = 24;
            this._options.Text = "Opt&ions";
            this._options.Click += new System.EventHandler(this._options_Click);
            // 
            // _exit
            // 
            this._exit.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._exit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this._exit.Image = global::ThreatsManager.Properties.Resources.door_exit;
            this._exit.Name = "_exit";
            this._exit.SubItemsExpandWidth = 24;
            this._exit.Text = "E&xit";
            this._exit.Click += new System.EventHandler(this._exit_Click);
            // 
            // _ribbonTabHome
            // 
            this._ribbonTabHome.Checked = true;
            this._ribbonTabHome.Name = "_ribbonTabHome";
            this._ribbonTabHome.Panel = this._ribbonPanelHome;
            this._ribbonTabHome.Text = "Home";
            this._ribbonTabHome.Visible = false;
            // 
            // _ribbonTabInsert
            // 
            this._ribbonTabInsert.Name = "_ribbonTabInsert";
            this._ribbonTabInsert.Panel = this._ribbonPanelInsert;
            this._ribbonTabInsert.Text = "Insert";
            this._ribbonTabInsert.Visible = false;
            // 
            // _ribbonTabView
            // 
            this._ribbonTabView.Name = "_ribbonTabView";
            this._ribbonTabView.Panel = this._ribbonPanelView;
            this._ribbonTabView.Text = "View";
            this._ribbonTabView.Visible = false;
            // 
            // _ribbonTabAnalyze
            // 
            this._ribbonTabAnalyze.Name = "_ribbonTabAnalyze";
            this._ribbonTabAnalyze.Panel = this._ribbonPanelAnalyze;
            this._ribbonTabAnalyze.Text = "Analyze";
            this._ribbonTabAnalyze.Visible = false;
            // 
            // _ribbonTabImport
            // 
            this._ribbonTabImport.Name = "_ribbonTabImport";
            this._ribbonTabImport.Panel = this._ribbonPanelImport;
            this._ribbonTabImport.Text = "Import";
            this._ribbonTabImport.Visible = false;
            // 
            // _ribbonTabExport
            // 
            this._ribbonTabExport.Name = "_ribbonTabExport";
            this._ribbonTabExport.Panel = this._ribbonPanelExport;
            this._ribbonTabExport.Text = "Export";
            this._ribbonTabExport.Visible = false;
            // 
            // _ribbonTabIntegrate
            // 
            this._ribbonTabIntegrate.Name = "_ribbonTabIntegrate";
            this._ribbonTabIntegrate.Panel = this._ribbonPanelIntegrate;
            this._ribbonTabIntegrate.Text = "Integrate";
            this._ribbonTabIntegrate.Visible = false;
            // 
            // _ribbonTabReview
            // 
            this._ribbonTabReview.Name = "_ribbonTabReview";
            this._ribbonTabReview.Panel = this._ribbonPanelReview;
            this._ribbonTabReview.Text = "Review";
            this._ribbonTabReview.Visible = false;
            // 
            // _ribbonTabConfigure
            // 
            this._ribbonTabConfigure.Name = "_ribbonTabConfigure";
            this._ribbonTabConfigure.Panel = this._ribbonPanelConfigure;
            this._ribbonTabConfigure.Text = "Configure";
            this._ribbonTabConfigure.Visible = false;
            // 
            // _ribbonTabHelp
            // 
            this._ribbonTabHelp.Name = "_ribbonTabHelp";
            this._ribbonTabHelp.Panel = this._ribbonPanelHelp;
            this._ribbonTabHelp.Text = "Help";
            this._ribbonTabHelp.Visible = false;
            // 
            // _title
            // 
            this._title.CanCustomize = false;
            this._title.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center;
            this._title.Name = "_title";
            this._title.PaddingBottom = 15;
            this._title.PaddingLeft = 50;
            this._title.PaddingRight = 50;
            this._title.TextLineAlignment = System.Drawing.StringAlignment.Near;
            // 
            // _closeCurrentWindow
            // 
            this._closeCurrentWindow.HotTrackingStyle = DevComponents.DotNetBar.eHotTrackingStyle.Image;
            this._closeCurrentWindow.HoverImage = global::ThreatsManager.Properties.Resources.window_close_sel;
            this._closeCurrentWindow.Image = global::ThreatsManager.Properties.Resources.window_close_white;
            this._closeCurrentWindow.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this._closeCurrentWindow.Name = "_closeCurrentWindow";
            this._superTooltip.SetSuperTooltip(this._closeCurrentWindow, new DevComponents.DotNetBar.SuperTooltipInfo("Close the current window (Ctrl+F4)", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray));
            this._closeCurrentWindow.Text = "Close Window";
            this._closeCurrentWindow.Click += new System.EventHandler(this._closeCurrentWindow_Click);
            // 
            // _closeAllWindows
            // 
            this._closeAllWindows.HotTrackingStyle = DevComponents.DotNetBar.eHotTrackingStyle.Image;
            this._closeAllWindows.HoverImage = global::ThreatsManager.Properties.Resources.windows_close_sel;
            this._closeAllWindows.Image = global::ThreatsManager.Properties.Resources.windows_close_white;
            this._closeAllWindows.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this._closeAllWindows.Name = "_closeAllWindows";
            this._superTooltip.SetSuperTooltip(this._closeAllWindows, new DevComponents.DotNetBar.SuperTooltipInfo("Close All Windows", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray));
            this._closeAllWindows.Text = "Close All Windows";
            this._closeAllWindows.Click += new System.EventHandler(this._closeAllWindows_Click);
            // 
            // _feedback
            // 
            this._feedback.HotTrackingStyle = DevComponents.DotNetBar.eHotTrackingStyle.Image;
            this._feedback.HoverImage = global::ThreatsManager.Properties.Resources.user_message_white;
            this._feedback.Image = global::ThreatsManager.Properties.Resources.user_message;
            this._feedback.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this._feedback.Name = "_feedback";
            this._feedback.Text = "Feedback";
            this._feedback.Click += new System.EventHandler(this._feedback_Click);
            // 
            // _controlMinimize
            // 
            this._controlMinimize.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this._controlMinimize.Name = "_controlMinimize";
            this._controlMinimize.Symbol = "57691";
            this._controlMinimize.SymbolSet = DevComponents.DotNetBar.eSymbolSet.Material;
            this._controlMinimize.Text = "Minimize";
            this._controlMinimize.Click += new System.EventHandler(this._controlMinimize_Click);
            // 
            // _controlMaximize
            // 
            this._controlMaximize.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this._controlMaximize.Name = "_controlMaximize";
            this._controlMaximize.Symbol = "59562";
            this._controlMaximize.SymbolSet = DevComponents.DotNetBar.eSymbolSet.Material;
            this._controlMaximize.Text = "Maximize";
            this._controlMaximize.Click += new System.EventHandler(this._controlMaximize_Click);
            // 
            // _controlExit
            // 
            this._controlExit.AutoCollapseOnClick = false;
            this._controlExit.CanCustomize = false;
            this._controlExit.HotTrackingStyle = DevComponents.DotNetBar.eHotTrackingStyle.Color;
            this._controlExit.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this._controlExit.Name = "_controlExit";
            this._controlExit.Symbol = "57676";
            this._controlExit.SymbolSet = DevComponents.DotNetBar.eSymbolSet.Material;
            this._controlExit.Text = "Close";
            this._controlExit.Click += new System.EventHandler(this._exit_Click);
            // 
            // qatCustomizeItem1
            // 
            this.qatCustomizeItem1.Name = "qatCustomizeItem1";
            // 
            // _openFile
            // 
            this._openFile.Title = "Open existing file";
            // 
            // _saveAsFile
            // 
            this._saveAsFile.Title = "Save file as";
            // 
            // _statusBar
            // 
            // 
            // 
            // 
            this._statusBar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._statusBar.ContainerControlProcessDialogKey = true;
            this._statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._statusBar.DragDropSupport = true;
            this._statusBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._statusBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._addStatusInfoProvider,
            this._lockRequest});
            this._statusBar.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._statusBar.Location = new System.Drawing.Point(5, 519);
            this._statusBar.Margin = new System.Windows.Forms.Padding(6);
            this._statusBar.Name = "_statusBar";
            this._statusBar.Size = new System.Drawing.Size(1078, 22);
            this._statusBar.TabIndex = 1;
            this._statusBar.Text = "metroStatusBar1";
            // 
            // _addStatusInfoProvider
            // 
            this._addStatusInfoProvider.Enabled = false;
            this._addStatusInfoProvider.Name = "_addStatusInfoProvider";
            this._addStatusInfoProvider.Symbol = "57671";
            this._addStatusInfoProvider.SymbolColor = System.Drawing.Color.White;
            this._addStatusInfoProvider.SymbolSet = DevComponents.DotNetBar.eSymbolSet.Material;
            this._addStatusInfoProvider.SymbolSize = 10F;
            this._addStatusInfoProvider.Text = "buttonItem1";
            this._addStatusInfoProvider.Click += new System.EventHandler(this._addStatusInfoProvider_Click);
            // 
            // _lockRequest
            // 
            this._lockRequest.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this._lockRequest.Name = "_lockRequest";
            this._lockRequest.Symbol = "59389";
            this._lockRequest.SymbolSet = DevComponents.DotNetBar.eSymbolSet.Material;
            this._lockRequest.Visible = false;
            // 
            // _superTooltip
            // 
            this._superTooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._superTooltip.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            // 
            // _autosaveTimer
            // 
            this._autosaveTimer.Enabled = true;
            this._autosaveTimer.Tick += new System.EventHandler(this._autosaveTimer_Tick);
            // 
            // _styleManager
            // 
            this._styleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2016;
            this._styleManager.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199))))));
            // 
            // _selectFolder
            // 
            this._selectFolder.Description = "Select the folder where to create the copy of the Threat Model";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1088, 543);
            this.Controls.Add(this._statusBar);
            this.Controls.Add(this._ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Threats Manager Platform";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this._ribbon.ResumeLayout(false);
            this._ribbon.PerformLayout();
            this._ribbonPanelView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.RibbonControl _ribbon;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelView;
        private DevComponents.DotNetBar.ApplicationButton _fileButton;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabView;
        private DevComponents.DotNetBar.QatCustomizeItem qatCustomizeItem1;
        private System.Windows.Forms.OpenFileDialog _openFile;
        private System.Windows.Forms.SaveFileDialog _saveAsFile;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelExport;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabExport;
        private DevComponents.DotNetBar.ItemContainer itemContainer1;
        private DevComponents.DotNetBar.ItemContainer itemContainer2;
        private DevComponents.DotNetBar.ItemContainer itemContainer3;
        private DevComponents.DotNetBar.ButtonItem _new;
        private DevComponents.DotNetBar.ButtonItem _open;
        private DevComponents.DotNetBar.ButtonItem _save;
        private DevComponents.DotNetBar.ButtonItem _saveAs;
        private DevComponents.DotNetBar.ButtonItem _close;
        private DevComponents.DotNetBar.GalleryContainer _recentDocuments;
        private DevComponents.DotNetBar.LabelItem labelItem8;
        private DevComponents.DotNetBar.ItemContainer itemContainer4;
        private DevComponents.DotNetBar.ButtonItem _options;
        private DevComponents.DotNetBar.ButtonItem _exit;
        private DevComponents.DotNetBar.ButtonItem _about;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelHome;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabHome;
        private DevComponents.DotNetBar.Metro.MetroStatusBar _statusBar;
        private DevComponents.DotNetBar.ButtonItem _extensionsConfig;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelInsert;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabInsert;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelAnalyze;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelReview;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabAnalyze;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabReview;
        private DevComponents.DotNetBar.SuperTooltip _superTooltip;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelImport;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabImport;
        private DevComponents.DotNetBar.RibbonBar _viewWindow;
        private DevComponents.DotNetBar.ButtonItem _windows;
        private DevComponents.DotNetBar.ButtonItem _closeWindows;
        private System.Windows.Forms.Timer _autosaveTimer;
        private DevComponents.DotNetBar.LabelItem _title;
        private DevComponents.DotNetBar.ButtonItem _controlExit;
        private DevComponents.DotNetBar.ButtonItem _controlMinimize;
        private DevComponents.DotNetBar.ButtonItem _controlMaximize;
        private DevComponents.DotNetBar.ButtonItem _closeWindow;
        private DevComponents.DotNetBar.StyleManager _styleManager;
        private DevComponents.DotNetBar.ButtonItem _closeCurrentWindow;
        private DevComponents.DotNetBar.ButtonItem _closeAllWindows;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelHelp;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabHelp;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelConfigure;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabConfigure;
        private DevComponents.DotNetBar.ButtonItem _addStatusInfoProvider;
        private DevComponents.DotNetBar.ButtonItem _feedback;
        private DevComponents.DotNetBar.RibbonPanel _ribbonPanelIntegrate;
        private DevComponents.DotNetBar.RibbonTabItem _ribbonTabIntegrate;
        private System.Windows.Forms.FolderBrowserDialog _selectFolder;
        private DevComponents.DotNetBar.LabelItem _lockRequest;
    }
}