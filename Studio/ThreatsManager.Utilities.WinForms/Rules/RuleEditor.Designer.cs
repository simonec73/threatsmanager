namespace ThreatsManager.Utilities.WinForms.Rules
{
    partial class RuleEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._decisionTreeTools = new DevComponents.DotNetBar.SideBar();
            this._panelBooleanOps = new DevComponents.DotNetBar.SideBarPanelItem();
            this._booleanAnd = new DevComponents.DotNetBar.ButtonItem();
            this._booleanOr = new DevComponents.DotNetBar.ButtonItem();
            this._booleanNot = new DevComponents.DotNetBar.ButtonItem();
            this._booleanTrue = new DevComponents.DotNetBar.ButtonItem();
            this._panelProperties = new DevComponents.DotNetBar.SideBarPanelItem();
            this._panelPropertiesSource = new DevComponents.DotNetBar.SideBarPanelItem();
            this._panelPropertiesTarget = new DevComponents.DotNetBar.SideBarPanelItem();
            this._panelPropertiesAnyTrustBoundary = new DevComponents.DotNetBar.SideBarPanelItem();
            this._decisionTree = new DevComponents.AdvTree.AdvTree();
            this._colName = new DevComponents.AdvTree.ColumnHeader();
            this._colNamespace = new DevComponents.AdvTree.ColumnHeader();
            this._colSchema = new DevComponents.AdvTree.ColumnHeader();
            this._colOperator = new DevComponents.AdvTree.ColumnHeader();
            this._colValue = new DevComponents.AdvTree.ColumnHeader();
            this._contextDecisionTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._contextChangeNodeType = new System.Windows.Forms.ToolStripMenuItem();
            this._contextDeleteNode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._contextClear = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this._validator = new DevComponents.DotNetBar.Validator.SuperValidator();
            this._decisionTreeValidator = new DevComponents.DotNetBar.Validator.CustomValidator();
            this._highlighter = new DevComponents.DotNetBar.Validator.Highlighter();
            this._styleManager = new DevComponents.DotNetBar.StyleManager(this.components);
            this._tooltipErrorProvider = new TooltipErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this._decisionTree)).BeginInit();
            this._contextDecisionTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // _decisionTreeTools
            // 
            this._decisionTreeTools.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this._decisionTreeTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._decisionTreeTools.BorderStyle = DevComponents.DotNetBar.eBorderType.None;
            this._decisionTreeTools.Dock = System.Windows.Forms.DockStyle.Left;
            this._decisionTreeTools.ExpandedPanel = this._panelBooleanOps;
            this._decisionTreeTools.ForeColor = System.Drawing.Color.Black;
            this._highlighter.SetHighlightColor(this._decisionTreeTools, DevComponents.DotNetBar.Validator.eHighlightColor.Orange);
            this._decisionTreeTools.Location = new System.Drawing.Point(0, 0);
            this._decisionTreeTools.Margin = new System.Windows.Forms.Padding(0);
            this._decisionTreeTools.Name = "_decisionTreeTools";
            this._decisionTreeTools.Panels.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._panelBooleanOps,
            this._panelProperties,
            this._panelPropertiesSource,
            this._panelPropertiesTarget,
            this._panelPropertiesAnyTrustBoundary});
            this._decisionTreeTools.Size = new System.Drawing.Size(192, 354);
            this._decisionTreeTools.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._decisionTreeTools.TabIndex = 2;
            this._decisionTreeTools.UseNativeDragDrop = true;
            this._decisionTreeTools.UsingSystemColors = true;
            // 
            // _panelBooleanOps
            // 
            this._panelBooleanOps.FontBold = true;
            this._panelBooleanOps.ForeColor = System.Drawing.Color.White;
            this._panelBooleanOps.HeaderMouseDownStyle.ForeColor.Color = System.Drawing.Color.White;
            this._panelBooleanOps.HeaderSideHotStyle.ForeColor.Color = System.Drawing.Color.White;
            this._panelBooleanOps.HeaderStyle.ForeColor.Color = System.Drawing.Color.White;
            this._panelBooleanOps.HotForeColor = System.Drawing.Color.White;
            this._panelBooleanOps.ItemImageSize = DevComponents.DotNetBar.eBarImageSize.Large;
            this._panelBooleanOps.Name = "_panelBooleanOps";
            this._panelBooleanOps.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this._booleanAnd,
            this._booleanOr,
            this._booleanNot,
            this._booleanTrue});
            this._panelBooleanOps.Text = "Boolean";
            this._panelBooleanOps.ExpandChange += new System.EventHandler(this._panel_ExpandChange);
            // 
            // _booleanAnd
            // 
            this._booleanAnd.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._booleanAnd.Image = global::ThreatsManager.Utilities.WinForms.Properties.Resources.logic_and;
            this._booleanAnd.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this._booleanAnd.Name = "_booleanAnd";
            this._booleanAnd.Text = global::ThreatsManager.Utilities.WinForms.Properties.Resources.LabelAnd;
            // 
            // _booleanOr
            // 
            this._booleanOr.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._booleanOr.Image = global::ThreatsManager.Utilities.WinForms.Properties.Resources.logic_or;
            this._booleanOr.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this._booleanOr.Name = "_booleanOr";
            this._booleanOr.Text = "OR";
            // 
            // _booleanNot
            // 
            this._booleanNot.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._booleanNot.Image = global::ThreatsManager.Utilities.WinForms.Properties.Resources.logic_not;
            this._booleanNot.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this._booleanNot.Name = "_booleanNot";
            this._booleanNot.Text = "NOT";
            // 
            // _booleanTrue
            // 
            this._booleanTrue.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this._booleanTrue.Image = global::ThreatsManager.Utilities.WinForms.Properties.Resources.ok;
            this._booleanTrue.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this._booleanTrue.Name = "_booleanTrue";
            this._booleanTrue.Text = "TRUE";
            this._booleanTrue.Tooltip = "Evaluates to TRUE";
            // 
            // _panelProperties
            // 
            this._panelProperties.FontBold = true;
            this._panelProperties.HotForeColor = System.Drawing.Color.White;
            this._panelProperties.ItemImageSize = DevComponents.DotNetBar.eBarImageSize.Large;
            this._panelProperties.Name = "_panelProperties";
            this._panelProperties.Text = "Object Properties";
            this._panelProperties.ExpandChange += new System.EventHandler(this._panel_ExpandChange);
            // 
            // _panelPropertiesSource
            // 
            this._panelPropertiesSource.FontBold = true;
            this._panelPropertiesSource.HotForeColor = System.Drawing.Color.White;
            this._panelPropertiesSource.ItemImageSize = DevComponents.DotNetBar.eBarImageSize.Large;
            this._panelPropertiesSource.Name = "_panelPropertiesSource";
            this._panelPropertiesSource.Text = "Source Properties";
            this._panelPropertiesSource.ExpandChange += new System.EventHandler(this._panel_ExpandChange);
            // 
            // _panelPropertiesTarget
            // 
            this._panelPropertiesTarget.FontBold = true;
            this._panelPropertiesTarget.HotForeColor = System.Drawing.Color.White;
            this._panelPropertiesTarget.ItemImageSize = DevComponents.DotNetBar.eBarImageSize.Large;
            this._panelPropertiesTarget.Name = "_panelPropertiesTarget";
            this._panelPropertiesTarget.Text = "Target Properties";
            this._panelPropertiesTarget.ExpandChange += new System.EventHandler(this._panel_ExpandChange);
            // 
            // _panelPropertiesAnyTrustBoundary
            // 
            this._panelPropertiesAnyTrustBoundary.FontBold = true;
            this._panelPropertiesAnyTrustBoundary.HotForeColor = System.Drawing.Color.White;
            this._panelPropertiesAnyTrustBoundary.ItemImageSize = DevComponents.DotNetBar.eBarImageSize.Large;
            this._panelPropertiesAnyTrustBoundary.Name = "_panelPropertiesAnyTrustBoundary";
            this._panelPropertiesAnyTrustBoundary.Text = "Any Trust Boundary Properties";
            this._panelPropertiesAnyTrustBoundary.ExpandChange += new System.EventHandler(this._panel_ExpandChange);
            // 
            // _decisionTree
            // 
            this._decisionTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this._decisionTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this._decisionTree.BackgroundStyle.Class = "TreeBorderKey";
            this._decisionTree.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._decisionTree.CellEdit = true;
            this._decisionTree.Columns.Add(this._colName);
            this._decisionTree.Columns.Add(this._colNamespace);
            this._decisionTree.Columns.Add(this._colSchema);
            this._decisionTree.Columns.Add(this._colOperator);
            this._decisionTree.Columns.Add(this._colValue);
            this._decisionTree.ContextMenuStrip = this._contextDecisionTree;
            this._decisionTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._decisionTree.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._decisionTree.Location = new System.Drawing.Point(192, 0);
            this._decisionTree.Margin = new System.Windows.Forms.Padding(0);
            this._decisionTree.Name = "_decisionTree";
            this._decisionTree.NodesConnector = this.nodeConnector1;
            this._decisionTree.NodeStyle = this.elementStyle1;
            this._decisionTree.NodeStyleMouseOver = this.elementStyle2;
            this._decisionTree.NodeStyleSelected = this.elementStyle2;
            this._decisionTree.PathSeparator = ";";
            this._decisionTree.SelectionBox = false;
            this._decisionTree.SelectionFocusAware = false;
            this._decisionTree.Size = new System.Drawing.Size(775, 354);
            this._decisionTree.Styles.Add(this.elementStyle1);
            this._decisionTree.Styles.Add(this.elementStyle2);
            this._decisionTree.TabIndex = 3;
            this._validator.SetValidator1(this._decisionTree, this._decisionTreeValidator);
            this._decisionTree.BeforeNodeDrop += new DevComponents.AdvTree.TreeDragDropEventHandler(this._decisionTree_BeforeNodeDrop);
            this._decisionTree.NodeDragFeedback += new DevComponents.AdvTree.TreeDragFeedbackEventHander(this._decisionTree_NodeDragFeedback);
            this._decisionTree.AfterNodeDrop += new DevComponents.AdvTree.TreeDragDropEventHandler(this._decisionTree_AfterNodeDrop);
            this._decisionTree.DragEnter += new System.Windows.Forms.DragEventHandler(this._decisionTree_DragEnter);
            // 
            // _colName
            // 
            this._colName.CellsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._colName.Editable = false;
            this._colName.Name = "_colName";
            this._colName.Text = "Name";
            this._colName.Width.Absolute = 200;
            // 
            // _colNamespace
            // 
            this._colNamespace.CellsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._colNamespace.Editable = false;
            this._colNamespace.Name = "_colNamespace";
            this._colNamespace.Text = "Namespace";
            this._colNamespace.Width.Absolute = 125;
            // 
            // _colSchema
            // 
            this._colSchema.CellsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this._colSchema.Editable = false;
            this._colSchema.Name = "_colSchema";
            this._colSchema.Text = "Schema";
            this._colSchema.Width.Absolute = 125;
            // 
            // _colOperator
            // 
            this._colOperator.Name = "_colOperator";
            this._colOperator.Text = "Operator";
            this._colOperator.Width.Absolute = 100;
            // 
            // _colValue
            // 
            this._colValue.Name = "_colValue";
            this._colValue.Text = "Value";
            this._colValue.Width.Absolute = 150;
            // 
            // _contextDecisionTree
            // 
            this._contextDecisionTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._contextChangeNodeType,
            this._contextDeleteNode,
            this.toolStripSeparator1,
            this._contextClear});
            this._contextDecisionTree.Name = "contextMenuStrip1";
            this._contextDecisionTree.Size = new System.Drawing.Size(172, 76);
            this._contextDecisionTree.Opening += new System.ComponentModel.CancelEventHandler(this._contextDecisionTree_Opening);
            // 
            // _contextChangeNodeType
            // 
            this._contextChangeNodeType.Image = global::ThreatsManager.Utilities.WinForms.Properties.Resources.arrows_circle;
            this._contextChangeNodeType.Name = "_contextChangeNodeType";
            this._contextChangeNodeType.Size = new System.Drawing.Size(171, 22);
            this._contextChangeNodeType.Text = "Change node type";
            this._contextChangeNodeType.Click += new System.EventHandler(this._contextChangeNodeType_Click);
            // 
            // _contextDeleteNode
            // 
            this._contextDeleteNode.Image = global::ThreatsManager.Utilities.WinForms.Properties.Resources.garbage_can;
            this._contextDeleteNode.Name = "_contextDeleteNode";
            this._contextDeleteNode.Size = new System.Drawing.Size(171, 22);
            this._contextDeleteNode.Text = "Delete node";
            this._contextDeleteNode.Click += new System.EventHandler(this._contextDeleteNode_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
            // 
            // _contextClear
            // 
            this._contextClear.Image = global::ThreatsManager.Utilities.WinForms.Properties.Resources.erase;
            this._contextClear.Name = "_contextClear";
            this._contextClear.Size = new System.Drawing.Size(171, 22);
            this._contextClear.Text = "Clear";
            this._contextClear.Click += new System.EventHandler(this._contextClear_Click);
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle2
            // 
            this.elementStyle2.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderBottomWidth = 2;
            this.elementStyle2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            this.elementStyle2.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderLeftWidth = 2;
            this.elementStyle2.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderRightWidth = 2;
            this.elementStyle2.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderTopWidth = 2;
            this.elementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.Color.Black;
            // 
            // _validator
            // 
            this._validator.ContainerControl = this;
            this._validator.Highlighter = this._highlighter;
            this._validator.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._validator.CustomValidatorValidateValue += new DevComponents.DotNetBar.Validator.ValidateValueEventHandler(this._validator_CustomValidatorValidateValue);
            // 
            // _decisionTreeValidator
            // 
            this._decisionTreeValidator.ErrorMessage = "The decision tree is not valid";
            this._decisionTreeValidator.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // _highlighter
            // 
            this._highlighter.ContainerControl = this._decisionTree;
            this._highlighter.FocusHighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            this._highlighter.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            // 
            // _styleManager
            // 
            this._styleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2016;
            this._styleManager.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199))))));
            // 
            // _tooltipErrorProvider
            // 
            this._tooltipErrorProvider.Caption = null;
            this._tooltipErrorProvider.CheckTooltipPosition = false;
            this._tooltipErrorProvider.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._tooltipErrorProvider.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            this._tooltipErrorProvider.PositionBelowControl = false;
            // 
            // RuleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._decisionTree);
            this.Controls.Add(this._decisionTreeTools);
            this.Name = "RuleEditor";
            this.Size = new System.Drawing.Size(967, 354);
            ((System.ComponentModel.ISupportInitialize)(this._decisionTree)).EndInit();
            this._contextDecisionTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SideBar _decisionTreeTools;
        private DevComponents.DotNetBar.SideBarPanelItem _panelBooleanOps;
        private DevComponents.DotNetBar.ButtonItem _booleanAnd;
        private DevComponents.DotNetBar.ButtonItem _booleanOr;
        private DevComponents.DotNetBar.ButtonItem _booleanNot;
        private DevComponents.DotNetBar.SideBarPanelItem _panelProperties;
        private DevComponents.DotNetBar.SideBarPanelItem _panelPropertiesSource;
        private DevComponents.DotNetBar.SideBarPanelItem _panelPropertiesTarget;
        private DevComponents.DotNetBar.SideBarPanelItem _panelPropertiesAnyTrustBoundary;
        private DevComponents.AdvTree.AdvTree _decisionTree;
        private DevComponents.AdvTree.ColumnHeader _colName;
        private DevComponents.AdvTree.ColumnHeader _colNamespace;
        private DevComponents.AdvTree.ColumnHeader _colSchema;
        private DevComponents.AdvTree.ColumnHeader _colOperator;
        private DevComponents.AdvTree.ColumnHeader _colValue;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private System.Windows.Forms.ContextMenuStrip _contextDecisionTree;
        private System.Windows.Forms.ToolStripMenuItem _contextChangeNodeType;
        private System.Windows.Forms.ToolStripMenuItem _contextDeleteNode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _contextClear;
        private DevComponents.DotNetBar.Validator.SuperValidator _validator;
        private DevComponents.DotNetBar.Validator.Highlighter _highlighter;
        private DevComponents.DotNetBar.Validator.CustomValidator _decisionTreeValidator;
        private TooltipErrorProvider _tooltipErrorProvider;
        private DevComponents.DotNetBar.ButtonItem _booleanTrue;
        private DevComponents.DotNetBar.StyleManager _styleManager;
    }
}
