using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Extensions.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class CreateEntityTemplateDialog : Form
    {
        private IEntity _entity;

        public CreateEntityTemplateDialog()
        {
            InitializeComponent();

            _entityType.Items.AddRange(EnumExtensions.GetEnumLabels<EntityType>().ToArray());
            _entityType.SelectedIndex = 0;

            try
            {
                _spellAsYouType.UserDictionaryFile = SpellCheckConfig.UserDictionary;
            }
            catch
            {
                // User Dictionary File is optional. If it is not possible to create it, then let's simply block it.
                _spellAsYouType.UserDictionaryFile = null;
            }

            AddSpellCheck(_name);
            AddSpellCheck(_description);
            _spellAsYouType.SetRepaintTimer(500);
        }

        public CreateEntityTemplateDialog([NotNull] IEntity entity) : this()
        {
            _entity = entity;
            _name.Text = entity.Name;
            _entityType.SelectedItem = entity.GetEntityType().GetEnumLabel();
            _entityType.Enabled = false;
            _description.Text = entity.Description;
            _imageSelector.Initialize(entity);
        }

        public string EntityName => _name.Text;

        public EntityType EntityType
        {
            get
            {
                var selectedItem = _entityType.SelectedItem;
                if (selectedItem is string selectedItemString)
                {
                    return selectedItemString.GetEnumValue<EntityType>();
                }
                else
                    throw new InvalidOperationException();
            }
        }

        public string EntityDescription => _description.Text;

        public Bitmap BigImage => _imageSelector.BigImage;

        public Bitmap Image => _imageSelector.Image;

        public Bitmap SmallImage => _imageSelector.SmallImage;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_name.Text);
        }

        private void _name_TextChanged(object sender, EventArgs e)
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
        
        private void AddSpellCheck([NotNull] TextBoxBase control)
        {
            try
            {
                if (control is RichTextBox richTextBox)
                {
                    _spellAsYouType.AddTextComponent(new RichTextBoxSpellAsYouTypeAdapter(richTextBox, 
                        _spellAsYouType.ShowCutCopyPasteMenuOnTextBoxBase));
                }
                else
                {
                    _spellAsYouType.AddTextBoxBase(control);
                }
            }
            catch
            {
            }
        }

        private void CreateEntityTemplateDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
