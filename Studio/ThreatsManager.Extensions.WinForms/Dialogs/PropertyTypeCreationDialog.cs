using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Extensions.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class PropertyTypeCreationDialog : Form, IInitializableObject
    {
        private IPropertySchema _schema;

        public PropertyTypeCreationDialog()
        {
            InitializeComponent();

            _propertyType.Items.AddRange(EnumExtensions.GetUIVisible<PropertyValueType>()
                .Select(x => x.GetEnumLabel()).ToArray());
        }

        public PropertyTypeCreationDialog([NotNull] IPropertySchema schema) : this()
        {
            _schema = schema;
        }

        public bool IsInitialized => _schema != null;

        public string PropertyName => _name.Text;

        public string Description => _description.Text;

        public PropertyValueType PropertyValueType =>
            ((string) _propertyType.SelectedItem).GetEnumValue<PropertyValueType>();

        private bool IsValid()
        {
            return IsInitialized && !string.IsNullOrWhiteSpace(_name.Text) && 
                   _propertyType.SelectedItem != null;
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _propertyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _layoutDescription_MarkupLinkClick(object sender, DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs e)
        {
            try
            {
                //_spellAsYouType.CheckAsYouType = false;

                using (var dialog = new TextEditorDialog
                {
                    Text = _description.Text, 
                    Multiline = true, 
                    ReadOnly = _description.ReadOnly
                })
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        _description.Text = dialog.Text;
                }
            }
            finally
            {
                //_spellAsYouType.CheckAsYouType = true;
            }
        }
    }
}
