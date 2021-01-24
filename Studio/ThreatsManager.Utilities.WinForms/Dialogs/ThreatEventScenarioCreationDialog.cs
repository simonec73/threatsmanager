using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Utilities.WinForms.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class ThreatEventScenarioCreationDialog : Form, IInitializableObject
    {
        private IThreatEvent _event;

        private IThreatEventScenario _scenario;

        public ThreatEventScenarioCreationDialog()
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

            AddSpellCheck(_name);
            AddSpellCheck(_description);
            AddSpellCheck(_motivation);
            _spellAsYouType.SetRepaintTimer(500);
        }

        public ThreatEventScenarioCreationDialog([NotNull] IThreatEvent threatEvent) : this()
        {
            _event = threatEvent;
            var model = threatEvent.Model;

            if (model != null)
            {
                var actors = model.ThreatActors?.OrderBy(x => x.Name).ToArray();
                if (actors?.Any() ?? false)
                    _actor.Items.AddRange(actors);

                var severities = model.Severities?.Where(x => x.Visible).ToArray();
                if (severities?.Any() ?? false)
                    _severity.Items.AddRange(severities);
            }
        }

        public bool IsInitialized => _event?.Model != null;

        public IThreatEventScenario Scenario => _scenario;

        [InitializationRequired]
        private void _ok_Click(object sender, EventArgs e)
        {
            if (IsValid() && _actor.SelectedItem is IThreatActor actor &&
                _severity.SelectedItem is ISeverity severity)
            {
                _scenario = _event.AddScenario(actor, severity, _name.Text);
                if (_scenario != null)
                {
                    _scenario.Description = _description.Text;
                    _scenario.Motivation = _motivation.Text;
                }
            }
        }

        private void _actor_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();

            if (string.IsNullOrWhiteSpace(_name.Text) && _actor.SelectedItem is IThreatActor actor)
                _name.Text = actor.Name;
        }

        private void _severity_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        [InitializationRequired(false)]
        private bool IsValid()
        {
            return IsInitialized && !string.IsNullOrWhiteSpace(_name.Text) && 
                   _actor.SelectedItem != null && _severity.SelectedItem != null;
        }

        private void _threatName_TextChanged(object sender, EventArgs e)
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

        private void _layoutMotivation_MarkupLinkClick(object sender, DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs e)
        {
            try
            {
                //_spellAsYouType.CheckAsYouType = false;

                using (var dialog = new TextEditorDialog
                {
                    Text = _motivation.Text, 
                    Multiline = true, 
                    ReadOnly = _motivation.ReadOnly
                })
                {
                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        _motivation.Text = dialog.Text;
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

        private void ThreatEventScenarioCreationDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _spellAsYouType.RemoveAllTextComponents();
        }
    }
}
