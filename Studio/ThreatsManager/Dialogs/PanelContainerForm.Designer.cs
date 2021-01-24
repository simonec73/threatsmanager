using System.Linq;
using DevComponents.DotNetBar;

namespace ThreatsManager.Dialogs
{
    partial class PanelContainerForm
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
            foreach (var bar in _ribbonBars)
            {
                var buttons = bar.Items.OfType<ButtonItem>().ToArray();
                if (buttons.Any())
                {
                    foreach (var button in buttons)
                    {
                        button.Click -= ButtonOnClick;
                        _superTooltip.SetSuperTooltip(button, null);
                    }
                }
            }

            _ribbonBars.Clear();

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
            this._ribbonMerge = new DevComponents.DotNetBar.RibbonBarMergeContainer();
            this._superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this.SuspendLayout();
            // 
            // _ribbonMerge
            // 
            this._ribbonMerge.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this._ribbonMerge.Location = new System.Drawing.Point(33, 12);
            this._ribbonMerge.Name = "_ribbonMerge";
            this._ribbonMerge.Size = new System.Drawing.Size(647, 100);
            // 
            // 
            // 
            this._ribbonMerge.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonMerge.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this._ribbonMerge.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this._ribbonMerge.TabIndex = 0;
            this._ribbonMerge.Visible = false;
            // 
            // _superTooltip
            // 
            this._superTooltip.DefaultTooltipSettings = new DevComponents.DotNetBar.SuperTooltipInfo("", "", "", null, null, DevComponents.DotNetBar.eTooltipColor.Gray);
            this._superTooltip.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            // 
            // PanelContainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(727, 518);
            this.Controls.Add(this._ribbonMerge);
            this.Name = "PanelContainerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PanelContainerForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PanelContainerForm_FormClosed);
            this.Load += new System.EventHandler(this.PanelContainerForm_Load);
            this.TextChanged += new System.EventHandler(this.PanelContainerForm_TextChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.RibbonBarMergeContainer _ribbonMerge;
        private DevComponents.DotNetBar.SuperTooltip _superTooltip;
    }
}