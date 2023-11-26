namespace ThreatsManager.Extensions.Dialogs
{
    partial class UndoRedoOperationsDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this._close = new System.Windows.Forms.Button();
            this._refresh = new System.Windows.Forms.Button();
            this.layoutControl1 = new DevComponents.DotNetBar.Layout.LayoutControl();
            this._nextRedo = new System.Windows.Forms.Button();
            this._previousRedo = new System.Windows.Forms.Button();
            this._nextUndo = new System.Windows.Forms.Button();
            this._previousUndo = new System.Windows.Forms.Button();
            this._countRedo = new System.Windows.Forms.Label();
            this._countUndo = new System.Windows.Forms.Label();
            this._countRedoFirst = new System.Windows.Forms.Label();
            this._countUndoFirst = new System.Windows.Forms.Label();
            this._redo = new System.Windows.Forms.RichTextBox();
            this._undo = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.layoutControlItem1 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem2 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutSpacerItem1 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem4 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem6 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem8 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem9 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem3 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutSpacerItem2 = new DevComponents.DotNetBar.Layout.LayoutSpacerItem();
            this.layoutControlItem5 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem7 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem10 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.layoutControlItem11 = new DevComponents.DotNetBar.Layout.LayoutControlItem();
            this.panel1.SuspendLayout();
            this.layoutControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._close);
            this.panel1.Controls.Add(this._refresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 305);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 48);
            this.panel1.TabIndex = 3;
            this.panel1.TabStop = true;
            // 
            // _close
            // 
            this._close.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._close.Location = new System.Drawing.Point(299, 13);
            this._close.Name = "_close";
            this._close.Size = new System.Drawing.Size(75, 23);
            this._close.TabIndex = 1;
            this._close.Text = "Close";
            this._close.UseVisualStyleBackColor = true;
            this._close.Click += new System.EventHandler(this._close_Click);
            // 
            // _refresh
            // 
            this._refresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._refresh.Location = new System.Drawing.Point(218, 13);
            this._refresh.Name = "_refresh";
            this._refresh.Size = new System.Drawing.Size(75, 23);
            this._refresh.TabIndex = 0;
            this._refresh.Text = "Refresh";
            this._refresh.UseVisualStyleBackColor = true;
            this._refresh.Click += new System.EventHandler(this._refresh_Click);
            // 
            // layoutControl1
            // 
            this.layoutControl1.BackColor = System.Drawing.Color.White;
            this.layoutControl1.Controls.Add(this._nextRedo);
            this.layoutControl1.Controls.Add(this._previousRedo);
            this.layoutControl1.Controls.Add(this._nextUndo);
            this.layoutControl1.Controls.Add(this._previousUndo);
            this.layoutControl1.Controls.Add(this._countRedo);
            this.layoutControl1.Controls.Add(this._countUndo);
            this.layoutControl1.Controls.Add(this._countRedoFirst);
            this.layoutControl1.Controls.Add(this._countUndoFirst);
            this.layoutControl1.Controls.Add(this._redo);
            this.layoutControl1.Controls.Add(this._undo);
            this.layoutControl1.Controls.Add(this.label1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            // 
            // 
            // 
            this.layoutControl1.RootGroup.Items.AddRange(new DevComponents.DotNetBar.Layout.LayoutItemBase[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutSpacerItem1,
            this.layoutControlItem4,
            this.layoutControlItem6,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem3,
            this.layoutSpacerItem2,
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem10,
            this.layoutControlItem11});
            this.layoutControl1.Size = new System.Drawing.Size(592, 305);
            this.layoutControl1.TabIndex = 4;
            // 
            // _nextRedo
            // 
            this._nextRedo.Location = new System.Drawing.Point(505, 277);
            this._nextRedo.Margin = new System.Windows.Forms.Padding(0);
            this._nextRedo.Name = "_nextRedo";
            this._nextRedo.Size = new System.Drawing.Size(75, 24);
            this._nextRedo.TabIndex = 12;
            this._nextRedo.Text = "Next";
            this._nextRedo.UseVisualStyleBackColor = true;
            this._nextRedo.Click += new System.EventHandler(this._nextRedo_Click);
            // 
            // _previousRedo
            // 
            this._previousRedo.Location = new System.Drawing.Point(422, 277);
            this._previousRedo.Margin = new System.Windows.Forms.Padding(0);
            this._previousRedo.Name = "_previousRedo";
            this._previousRedo.Size = new System.Drawing.Size(75, 24);
            this._previousRedo.TabIndex = 11;
            this._previousRedo.Text = "Previous";
            this._previousRedo.UseVisualStyleBackColor = true;
            this._previousRedo.Click += new System.EventHandler(this._previousRedo_Click);
            // 
            // _nextUndo
            // 
            this._nextUndo.Location = new System.Drawing.Point(505, 135);
            this._nextUndo.Margin = new System.Windows.Forms.Padding(0);
            this._nextUndo.Name = "_nextUndo";
            this._nextUndo.Size = new System.Drawing.Size(75, 24);
            this._nextUndo.TabIndex = 6;
            this._nextUndo.Text = "Next";
            this._nextUndo.UseVisualStyleBackColor = true;
            this._nextUndo.Click += new System.EventHandler(this._nextUndo_Click);
            // 
            // _previousUndo
            // 
            this._previousUndo.Location = new System.Drawing.Point(422, 135);
            this._previousUndo.Margin = new System.Windows.Forms.Padding(0);
            this._previousUndo.Name = "_previousUndo";
            this._previousUndo.Size = new System.Drawing.Size(75, 24);
            this._previousUndo.TabIndex = 5;
            this._previousUndo.Text = "Previous";
            this._previousUndo.UseVisualStyleBackColor = true;
            this._previousUndo.Click += new System.EventHandler(this._previousUndo_Click);
            // 
            // _countRedo
            // 
            this._countRedo.AutoSize = true;
            this._countRedo.Location = new System.Drawing.Point(307, 277);
            this._countRedo.Margin = new System.Windows.Forms.Padding(0);
            this._countRedo.Name = "_countRedo";
            this._countRedo.Size = new System.Drawing.Size(107, 24);
            this._countRedo.TabIndex = 10;
            this._countRedo.Text = "label5";
            // 
            // _countUndo
            // 
            this._countUndo.AutoSize = true;
            this._countUndo.Location = new System.Drawing.Point(307, 135);
            this._countUndo.Margin = new System.Windows.Forms.Padding(0);
            this._countUndo.Name = "_countUndo";
            this._countUndo.Size = new System.Drawing.Size(107, 24);
            this._countUndo.TabIndex = 4;
            this._countUndo.Text = "label4";
            // 
            // _countRedoFirst
            // 
            this._countRedoFirst.AutoSize = true;
            this._countRedoFirst.Location = new System.Drawing.Point(129, 277);
            this._countRedoFirst.Margin = new System.Windows.Forms.Padding(0);
            this._countRedoFirst.Name = "_countRedoFirst";
            this._countRedoFirst.Size = new System.Drawing.Size(107, 24);
            this._countRedoFirst.TabIndex = 9;
            this._countRedoFirst.Text = "label3";
            // 
            // _countUndoFirst
            // 
            this._countUndoFirst.AutoSize = true;
            this._countUndoFirst.Location = new System.Drawing.Point(129, 135);
            this._countUndoFirst.Margin = new System.Windows.Forms.Padding(0);
            this._countUndoFirst.Name = "_countUndoFirst";
            this._countUndoFirst.Size = new System.Drawing.Size(107, 24);
            this._countUndoFirst.TabIndex = 3;
            this._countUndoFirst.Text = "label2";
            // 
            // _redo
            // 
            this._redo.Location = new System.Drawing.Point(67, 167);
            this._redo.Margin = new System.Windows.Forms.Padding(0);
            this._redo.Name = "_redo";
            this._redo.ReadOnly = true;
            this._redo.Size = new System.Drawing.Size(521, 102);
            this._redo.TabIndex = 7;
            this._redo.Text = "";
            this._redo.WordWrap = false;
            // 
            // _undo
            // 
            this._undo.Location = new System.Drawing.Point(67, 25);
            this._undo.Margin = new System.Windows.Forms.Padding(0);
            this._undo.Name = "_undo";
            this._undo.ReadOnly = true;
            this._undo.Size = new System.Drawing.Size(521, 102);
            this._undo.TabIndex = 1;
            this._undo.Text = "";
            this._undo.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(584, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This tool is for debugging purposes only. It will not be visible in the official " +
    "release.";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.label1;
            this.layoutControlItem1.Height = 21;
            this.layoutControlItem1.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Text = "Label:";
            this.layoutControlItem1.TextVisible = false;
            this.layoutControlItem1.Width = 100;
            this.layoutControlItem1.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this._undo;
            this.layoutControlItem2.Height = 50;
            this.layoutControlItem2.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem2.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Text = "Undo\r\nOperations";
            this.layoutControlItem2.Width = 100;
            this.layoutControlItem2.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem1
            // 
            this.layoutSpacerItem1.Height = 32;
            this.layoutSpacerItem1.Name = "layoutSpacerItem1";
            this.layoutSpacerItem1.Width = 62;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this._countUndoFirst;
            this.layoutControlItem4.Height = 21;
            this.layoutControlItem4.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Text = "First level";
            this.layoutControlItem4.Width = 49;
            this.layoutControlItem4.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this._countUndo;
            this.layoutControlItem6.Height = 21;
            this.layoutControlItem6.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Text = "Total";
            this.layoutControlItem6.Width = 49;
            this.layoutControlItem6.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this._previousUndo;
            this.layoutControlItem8.Height = 31;
            this.layoutControlItem8.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Width = 83;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this._nextUndo;
            this.layoutControlItem9.Height = 31;
            this.layoutControlItem9.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Width = 83;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this._redo;
            this.layoutControlItem3.Height = 50;
            this.layoutControlItem3.HeightType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            this.layoutControlItem3.MinSize = new System.Drawing.Size(120, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Text = "Redo\r\nOperations";
            this.layoutControlItem3.Width = 100;
            this.layoutControlItem3.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutSpacerItem2
            // 
            this.layoutSpacerItem2.Height = 32;
            this.layoutSpacerItem2.Name = "layoutSpacerItem2";
            this.layoutSpacerItem2.Width = 62;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this._countRedoFirst;
            this.layoutControlItem5.Height = 21;
            this.layoutControlItem5.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Text = "First level";
            this.layoutControlItem5.Width = 49;
            this.layoutControlItem5.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this._countRedo;
            this.layoutControlItem7.Height = 21;
            this.layoutControlItem7.MinSize = new System.Drawing.Size(64, 18);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Text = "Total";
            this.layoutControlItem7.Width = 49;
            this.layoutControlItem7.WidthType = DevComponents.DotNetBar.Layout.eLayoutSizeType.Percent;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this._previousRedo;
            this.layoutControlItem10.Height = 31;
            this.layoutControlItem10.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Width = 83;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this._nextRedo;
            this.layoutControlItem11.Height = 31;
            this.layoutControlItem11.MinSize = new System.Drawing.Size(32, 20);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Width = 83;
            // 
            // UndoRedoOperationsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this._close;
            this.ClientSize = new System.Drawing.Size(592, 353);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.panel1);
            this.Name = "UndoRedoOperationsDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Undo/Redo Operations";
            this.Load += new System.EventHandler(this.UndoRedoOperationsDialog_Load);
            this.panel1.ResumeLayout(false);
            this.layoutControl1.ResumeLayout(false);
            this.layoutControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button _close;
        private System.Windows.Forms.Button _refresh;
        private DevComponents.DotNetBar.Layout.LayoutControl layoutControl1;
        private System.Windows.Forms.RichTextBox _redo;
        private System.Windows.Forms.RichTextBox _undo;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem1;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem2;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Label _countRedo;
        private System.Windows.Forms.Label _countUndo;
        private System.Windows.Forms.Label _countRedoFirst;
        private System.Windows.Forms.Label _countUndoFirst;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem4;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem6;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem5;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem7;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem1;
        private DevComponents.DotNetBar.Layout.LayoutSpacerItem layoutSpacerItem2;
        private System.Windows.Forms.Button _nextRedo;
        private System.Windows.Forms.Button _previousRedo;
        private System.Windows.Forms.Button _nextUndo;
        private System.Windows.Forms.Button _previousUndo;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem8;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem9;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem10;
        private DevComponents.DotNetBar.Layout.LayoutControlItem layoutControlItem11;
    }
}