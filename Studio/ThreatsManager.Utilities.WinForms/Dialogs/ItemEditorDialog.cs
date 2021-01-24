using DevComponents.DotNetBar;
using System;
using System.Drawing;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class ItemEditorDialog : Form, IExecutionModeSupport
    {
        private bool _loading;
        private static FormWindowState _windowState = FormWindowState.Normal;
        private static Size _persistentSize;
        private static Point _persistentLocation;

        public ItemEditorDialog()
        {
            _loading = true;

            InitializeComponent();
            
            if (_persistentSize.IsEmpty)
            {
                _persistentSize = this.Size;
            }
            if (_persistentLocation.IsEmpty)
            {
                _persistentLocation = new Point((int)(100 * Dpi.Factor.Width), (int)(100 * Dpi.Factor.Height));
            }
            else
            {
                StartPosition = FormStartPosition.Manual;
            }

            _loading = false;
        }

        public object Item
        {
            get => _item.Item;
            set
            {
                _item.Item = value;
            }
        }

        public bool ReadOnly
        {
            get => _item.ReadOnly;
            set => _item.ReadOnly = value;
        }
        
        private void ItemEditorDialog_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            if (_windowState == FormWindowState.Maximized)
            {
                Location = _persistentLocation;
                WindowState = _windowState;
            }
            else
            {
                Size = _persistentSize;
                Location = _persistentLocation;
            }
            this.ResumeLayout();
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void ItemEditorDialog_Resize(object sender, EventArgs e)
        {
            if (!_loading)
            {
                _windowState = WindowState;
            }
        }

        private void ItemEditorDialog_LocationChanged(object sender, EventArgs e)
        {
            if (!_loading && WindowState != FormWindowState.Maximized)
                _persistentLocation = Location;
        }

        private void ItemEditorDialog_SizeChanged(object sender, EventArgs e)
        {
            if (!_loading && WindowState != FormWindowState.Maximized)
                _persistentSize = Size;
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            _item.SetExecutionMode(mode);
        }
    }
}
