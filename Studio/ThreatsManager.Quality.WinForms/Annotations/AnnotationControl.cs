using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Layout;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using ThreatsManager.Utilities.WinForms;
using ThreatsManager.Utilities.WinForms.Dialogs;

namespace ThreatsManager.Quality.Annotations
{
    public partial class AnnotationControl : UserControl, IInitializableObject
    {
        private Annotation _annotation;
        private bool _loading;

        public AnnotationControl()
        {
            InitializeComponent();

            AddSpellCheck(_text);
        }

        public event Action AnnotationUpdated;

        public bool IsInitialized => _annotation != null;

        public void SetObject([NotNull] IThreatModel model, [NotNull] object obj)
        {
            if (obj is IIdentity identity)
            {
                _objectContainer.Text = model.GetIdentityTypeName(identity);
                _objectName.Text = "      " + identity.Name;
                _objectName.Image = identity.GetImage(ImageSize.Small);
            } else if (obj is IThreatEventMitigation threatEventMitigation)
            {
                _objectContainer.Text = "Threat Event Mitigation";
                _objectName.Text = "      " + 
                                   $"Mitigation '{threatEventMitigation.Mitigation.Name}' for '{threatEventMitigation.ThreatEvent.Name}' on '{threatEventMitigation.ThreatEvent.Parent.Name}";
                _objectName.Image = Icons.Resources.mitigations_small;
            } else if (obj is IThreatTypeMitigation threatTypeMitigation)
            {
                _objectContainer.Text = "Threat Type Mitigation";
                _objectName.Text = "      " + 
                                   $"Mitigation '{threatTypeMitigation.Mitigation.Name}' for '{threatTypeMitigation.ThreatType.Name}'";
                _objectName.Image = Icons.Resources.standard_mitigations_small;
            } 
        }

        public Annotation Annotation
        {
            get => _annotation;

            set
            {
                _loading = true;

                try
                {
                    _annotation = value;
                    _text.Text = _annotation?.Text;
                    _createdBy.Text = _annotation?.CreatedBy;
                    _createdOn.Text = _annotation?.CreatedOn.ToLongDateString();
                    _modifiedBy.Text = _annotation?.ModifiedBy;
                    _modifiedOn.Text = string.IsNullOrWhiteSpace(_annotation?.ModifiedBy) ? null : _annotation?.ModifiedOn.ToLongDateString();

                    ClearAnswers();

                    if (_annotation is TopicToBeClarified topicToBeClarified)
                    {
                        _askedBy.Text = topicToBeClarified.AskedBy;
                        _askedOn.Value = topicToBeClarified.AskedOn;
                        _askedVia.Text = topicToBeClarified.AskedVia;
                        _answered.Checked = topicToBeClarified.Answered;
                        if (topicToBeClarified.Answers?.Any() ?? false)
                            AddAnswers(topicToBeClarified.Answers);

                        _askedByContainer.Visible = true;
                        _askedOnContainer.Visible = true;
                        _askedViaContainer.Visible = true;
                        _answeredContainer.Visible = true;
                        _answersContainer.Visible = true;
                        _textContainer.Height = 50;
                    }
                    else
                    {
                        _askedByContainer.Visible = false;
                        _askedOnContainer.Visible = false;
                        _askedViaContainer.Visible = false;
                        _answeredContainer.Visible = false;
                        _answersContainer.Visible = false;
                        _textContainer.Height = 99;
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        [InitializationRequired]
        private void AddAnswers([NotNull] IEnumerable<AnnotationAnswer> answers)
        {
            var array = answers?.ToArray();
            if (array?.Any() ?? false)
            {
                foreach (var answer in array)
                {
                    AddAnswer(answer);
                }
            }
        }

        [InitializationRequired]
        private void AddAnswer([NotNull] AnnotationAnswer answer)
        {
            int count = _answers.Tabs.Count;
            var tabs = _answers.Tabs.OfType<SuperTabItem>()
                .Select(x => (int.TryParse(x.Text, out var n)) ? n : 0)
                .ToArray();
            int index = 1;
            if (tabs.Length > 0)
            {
                index = tabs.Max() + 1;
            }

            var superTabItem = new SuperTabItem();
            var superTabControlPanel = new SuperTabControlPanel
            {
                Name = $"tabControlPanelAnswer{index}",
                Dock = DockStyle.Fill,
                Location = new System.Drawing.Point(0, 30),
                Size = new System.Drawing.Size(661, 162),
                TabItem = superTabItem
            };
            superTabItem.Name = $"tabItemAnswer{index}";
            superTabItem.AttachedControl = superTabControlPanel;
            superTabItem.GlobalItem = false;
            superTabItem.Text = index.ToString();
            superTabItem.Tag = answer;

            var answerControl = new AnswerControl()
            {
                Dock = DockStyle.Fill,
                Answer = answer
            };
            superTabControlPanel.Controls.Add(answerControl);

            _answers.Controls.Add(superTabControlPanel);
            _answers.Tabs.Insert(count - 1, superTabItem);
        }

        [InitializationRequired]
        private void ClearAnswers()
        {
            var toRemove = _answers.Tabs.OfType<SuperTabItem>().ToArray();
            if (toRemove.Any())
            {
                foreach (var item in toRemove)
                {
                    if (item.AttachedControl is SuperTabControlPanel panel)
                    {
                        panel.Controls.Clear();
                        _answers.Controls.Remove(panel);
                    }
                    item.AttachedControl = null;
                }
            }
        }

        private void _addAnswer_Click(object sender, EventArgs e)
        {
            if (_annotation is TopicToBeClarified topicToBeClarified)
            {
                AddAnswer(topicToBeClarified.AddAnswer());
            }
        }

        private void _tabContainer_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete answer '{e.Tab.Text}'?",
                "Remove Answer", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                if (_annotation is TopicToBeClarified topicToBeClarified && e.Tab?.Tag is AnnotationAnswer answer)
                {
                    topicToBeClarified.RemoveAnswer(answer);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void _askedBy_ButtonCustomClick(object sender, EventArgs e)
        {
            _askedBy.Text = UserName.GetDisplayName();
        }

        private void _askedOn_ButtonCustomClick(object sender, EventArgs e)
        {
            _askedOn.Value = DateTime.Now;
        }

        [InitializationRequired]
        private void _text_TextChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                _annotation.Text = _text.Text;
            }
        }

        private void _askedBy_TextChanged(object sender, EventArgs e)
        {
            if (!_loading && _annotation is TopicToBeClarified topicToBeClarified)
            {
                topicToBeClarified.AskedBy = _askedBy.Text;
            }
        }

        private void _askedOn_ValueChanged(object sender, EventArgs e)
        {
            if (!_loading && _annotation is TopicToBeClarified topicToBeClarified)
            {
                topicToBeClarified.AskedOn = _askedOn.Value;
            }
        }

        private void _askedVia_TextChanged(object sender, EventArgs e)
        {
            if (!_loading && _annotation is TopicToBeClarified topicToBeClarified)
            {
                topicToBeClarified.AskedVia = _askedVia.Text;
            }
        }

        private void _answered_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loading && _annotation is TopicToBeClarified topicToBeClarified)
            {
                topicToBeClarified.Answered = _answered.Checked;
                AnnotationUpdated?.Invoke();
            }
        }

        private void _askedVia_ButtonCustomClick(object sender, EventArgs e)
        {
            _askedVia.Text = "Email";
        }

        private void _askedVia_ButtonCustom2Click(object sender, EventArgs e)
        {
            _askedVia.Text = "Call";
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

        private void _textContainer_MarkupLinkClick(object sender, DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs e)
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
    }
}
