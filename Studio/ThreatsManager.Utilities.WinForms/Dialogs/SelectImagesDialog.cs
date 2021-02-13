using System;
using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class SelectImagesDialog : Form
    {
        public SelectImagesDialog()
        {
            InitializeComponent();
        }

        public SelectImagesDialog([NotNull] IEntity entity) : this()
        {
            _name.Text = entity.Name;
            _selector.Initialize(entity);
        }

        public SelectImagesDialog([NotNull] IEntityTemplate template) : this()
        {
            _name.Text = template.Name;
            _selector.Initialize(template);
        }

        public Bitmap BigImage => _selector.BigImage;

        public Bitmap Image => _selector.Image;

        public Bitmap SmallImage => _selector.SmallImage;

        private void _selector_Ready()
        {
            _ok.Enabled = true;
        }
    }
}
