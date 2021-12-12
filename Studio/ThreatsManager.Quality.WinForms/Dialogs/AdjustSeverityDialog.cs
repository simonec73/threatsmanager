using System;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Layout;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.CalculatedSeverity;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Quality.Dialogs
{
    public partial class AdjustSeverityDialog : Form
    {
        private IThreatEvent _threatEvent;
        private bool _adjusting;

        public AdjustSeverityDialog()
        {
            InitializeComponent();

            AddSpellCheck(_reason);

            var values = EnumExtensions.GetEnumLabels<DeltaValue>()?.ToArray();
            if (values?.Any() ?? false)
                _adjustment.Items.AddRange(values);
        }

        public AdjustSeverityDialog([NotNull] IThreatEvent threatEvent) : this()
        {
            _threatEvent = threatEvent;

            _threatEventName.Text = "      " + threatEvent.Name;
            _threatEventName.Image = threatEvent.GetImage(ImageSize.Small);
            if (threatEvent.Parent != null)
            {
                _associatedTo.Text = "      " + threatEvent.Parent.Name;
                _associatedTo.Image = threatEvent.Parent.GetImage(ImageSize.Small);
            }

            if (threatEvent.Model is IThreatModel model)
            {
                var schemaManager = new CalculatedSeverityPropertySchemaManager(model);
                var config = schemaManager.GetSeverityCalculationConfig(threatEvent);
                if (config != null)
                {
                    _points.Value = config.Delta;
                    _reason.Text = config.DeltaReason;
                    _ok.Enabled = !string.IsNullOrWhiteSpace(_reason.Text);
                }
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

        private void _reason_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = !string.IsNullOrWhiteSpace(_reason.Text);
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (_threatEvent.Model is IThreatModel model)
            {
                var schemaManager = new CalculatedSeverityPropertySchemaManager(model);
                schemaManager.SetSeverityCalculationConfig(_threatEvent, _points.Value, _reason.Text);
            }
        }

        private void _reasonContainer_MarkupLinkClick(object sender, DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs e)
        {
            if (sender is LayoutControlItem layoutControlItem)
            {
                if (layoutControlItem.Control is RichTextBox richTextBox)
                {
                    using (var dialog = new TextEditorDialog
                    {
                        Multiline = true,
                        Text = richTextBox.Text,
                        ReadOnly = richTextBox.ReadOnly
                    })
                    {
                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            richTextBox.Text = dialog.Text;
                    }
                }
            }

        }

        private void _points_ValueChanged(object sender, EventArgs e)
        {
            if (!_adjusting)
            {
                _adjusting = true;

                try
                {
                    if (Enum.IsDefined(typeof(DeltaValue), _points.Value))
                    {
                        _adjustment.SelectedItem = ((DeltaValue)_points.Value).GetEnumLabel();
                    }
                    else
                    {
                        _adjustment.SelectedItem = null;
                    }
                }
                finally
                {
                    _adjusting = false;
                }
            }
        }

        private void _adjustment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_adjusting)
            {
                _adjusting = true;

                try
                {
                    _points.Value = (int) (_adjustment.Text?.GetEnumValue<DeltaValue>() ?? 0);
                }
                finally
                {
                    _adjusting = false;
                }
            }
        }
    }
}
