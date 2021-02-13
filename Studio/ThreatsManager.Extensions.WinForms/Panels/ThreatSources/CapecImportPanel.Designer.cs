namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    partial class CapecImportPanel
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
            this._catalog = new ThreatsManager.Extensions.Panels.ThreatSources.MitreCatalogControl();
            this.SuspendLayout();
            // 
            // _catalog
            // 
            this._catalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._catalog.FileName = "capec.xml";
            this._catalog.Location = new System.Drawing.Point(0, 0);
            this._catalog.Model = null;
            this._catalog.Name = "_catalog";
            this._catalog.Size = new System.Drawing.Size(892, 531);
            this._catalog.SourceUrl = null;
            this._catalog.TabIndex = 0;
            // 
            // CapecImportPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._catalog);
            this.Name = "CapecImportPanel";
            this.Size = new System.Drawing.Size(892, 531);
            this.ResumeLayout(false);

        }

        #endregion

        private MitreCatalogControl _catalog;
    }
}
