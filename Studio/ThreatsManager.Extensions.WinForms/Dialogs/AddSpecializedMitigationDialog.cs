using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class AddSpecializedMitigationDialog : Form
    {
        private readonly IMitigation _mitigation;
        private ISpecializedMitigation _specialized;
        private RichTextBoxSpellAsYouTypeAdapter _spellName;
        private RichTextBoxSpellAsYouTypeAdapter _spellDescription;

        public AddSpecializedMitigationDialog()
        {
            InitializeComponent();

            try
            {
                _spellAsYouType.UserDictionaryFile = SpellCheckConfig.UserDictionary;
            }
            catch
            {
                // User Dictionary File is optional. If it is not possible to create it, then let's simply block it.
                _spellAsYouType.UserDictionaryFile = null;
            }

            _spellName = _spellAsYouType.AddSpellCheck(_name);
            _spellDescription = _spellAsYouType.AddSpellCheck(_description);
            _spellAsYouType.SetRepaintTimer(500);
        }

        public AddSpecializedMitigationDialog([NotNull] IMitigation mitigation) : this()
        {
            _mitigation = mitigation;
            _mitigationName.Text = mitigation.Name;

            _templates.ItemHeight = (int)(16 * Dpi.Factor.Height);

            _mitigation = mitigation;
            var specialized = mitigation.Specialized?.ToArray();

            IEnumerable<IItemTemplate> templates = mitigation.Model?.EntityTemplates?
                .Where(x => !(specialized?.Any(y => y.TargetId == x.Id) ?? false))
                .OrderBy(x => x.Name)
                .ToArray();         
            if (templates?.Any() ?? false)
            {
                _templates.Items.AddRange(templates.Select(x => new ComboBoxItem(x, x.GetImage(ImageSize.Small))).ToArray());
            }

            templates = mitigation.Model?.FlowTemplates?
                .Where(x => !(specialized?.Any(y => y.TargetId == x.Id) ?? false))
                .OrderBy(x => x.Name)
                .ToArray();
            if (templates?.Any() ?? false)
            {
                _templates.Items.AddRange(templates.Select(x => new ComboBoxItem(x, x.GetImage(ImageSize.Small))).ToArray());
            }
        }

        public ISpecializedMitigation SpecializedMitigation => _specialized;

        private void _templates_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsOkEnabled();
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsOkEnabled();
        }

        private void _description_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsOkEnabled();
        }

        private bool IsOkEnabled()
        {
            return _templates.SelectedItem != null &&
                (!string.IsNullOrWhiteSpace(_name.Text) || !string.IsNullOrWhiteSpace(_description.Text));
        }


        private void _ok_Click(object sender, EventArgs e)
        {
            var template = ((ComboBoxItem)_templates.SelectedItem)?.Value as IItemTemplate;
            if (template != null)
            {
                _specialized = _mitigation.AddSpecializedMitigation(template, _name.Text, _description.Text);
            }
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
