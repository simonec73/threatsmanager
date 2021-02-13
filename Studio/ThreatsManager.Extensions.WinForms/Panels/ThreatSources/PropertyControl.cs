using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using PostSharp.Patterns.Contracts;
using Process = System.Diagnostics.Process;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
#pragma warning disable S110 // Inheritance tree of classes should not be too deep
    public partial class PropertyControl : UserControl //TableLayoutPanel //LayoutControl
#pragma warning restore S110 // Inheritance tree of classes should not be too deep
    {
        private bool _loading;
        private readonly List<string> _hiddenProperties = new List<string>();
        private ThreatSourceType _threatSourceType = ThreatSourceType.Capec;
        private int _rowCount;

        
        public event Action<string> SelectedKeyword;

        public PropertyControl()
        {
            _loading = true;
            InitializeComponent();
        }

        #region Public members.
        public void Initialize([NotNull] ThreatSourceNode node)
        {
            _threatSourceType = node.ThreatSourceType;

            var properties = node.Properties;

            if (properties != null)
            {
                try
                {
                    _loading = true;

                    SuspendLayout();

                    if (_table.Controls.Count > 0)
                    {
                        _rowCount = 0;
                        _table.Dispose();
                        InitializeComponent();
                    }

                    _table.SuspendLayout();

                    //_properties.Clear();

                    foreach (var property in properties)
                    {
                        if (IsSingleLine(property.Value))
                            AddSingleLineTextBox(property.Key, property.Key, property.Value, true);
                        else
                            AddRichTextBox(property.Key, property.Key, property.Value, true);
                        //_properties.Add(property.Key, property.Value);
                        AddCheckBox("Include property in model", property.Key);
                    }

                    AddDummyRow();
                }
                finally
                {
                    _loading = false;
                    _table.ResumeLayout();
                    ResumeLayout();
                }
            }
        }

        public void Clear()
        {
            if (_table.Controls.Count > 0)
            {
                SuspendLayout();

                _rowCount = 0;
                _table.Dispose();
                InitializeComponent();

                ResumeLayout();
            }
        }

        public void ClearHiddenProperties()
        {
            _hiddenProperties.Clear();
            var checkBoxes = _table.Controls.OfType<CheckBoxX>().ToArray();
            if (checkBoxes.Any())
            {
                foreach (var checkBox in checkBoxes)
                {
                    checkBox.Checked = true;
                }
            }
        }

        public IEnumerable<string> HiddenProperties => _hiddenProperties.AsReadOnly();

        public void SetHiddenProperties([NotNull] IEnumerable<string> hiddenProperties)
        {
            _hiddenProperties.Clear();
            _hiddenProperties.AddRange(hiddenProperties);
        }
        #endregion

        #region Private members.
        private bool IsSingleLine(string text)
        {
            return string.IsNullOrWhiteSpace(text) || 
                   (!text.TrimEnd('\n').Contains("\n") && (TextRenderer.MeasureText(text, RichTextBox.DefaultFont).Width < (Width - 400)));
        }

        private void AddSingleLineTextBox([Required] string label, 
            [Required] string name, string value, bool readOnly = false)
        {
            var labelControl = new Label
            {
                Dock = DockStyle.Fill,
                Text = label,
                TextAlign = System.Drawing.ContentAlignment.TopLeft
            };
            _table.Controls.Add(labelControl, 0, _rowCount);

            var control = new RichTextBox()
            {
                Name = name,
                ShortcutsEnabled = false,
                Multiline = false,
                ReadOnly = readOnly,
                AutoSize = true,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            if (!string.IsNullOrWhiteSpace(value))
                control.Text = value;
            control.LinkClicked += ControlOnLinkClicked;
            control.DoubleClick += ControlOnDoubleClick;
            _table.Controls.Add(control, 1, _rowCount);
            _table.SetColumnSpan(control, 2);

            if (_table.RowStyles.Count > _rowCount)
                _table.RowStyles[_rowCount] = new RowStyle(SizeType.AutoSize);
            else
                _table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            _rowCount++;
        }

        private void ControlOnDoubleClick(object sender, EventArgs e)
        {
            if (sender is RichTextBox textBox)
            {
                var text = textBox.SelectedText;
                SelectedKeyword?.Invoke(text.Trim());
            }
        }

        private void AddRichTextBox([Required] string label, 
            [Required] string name, string value, bool readOnly = false)
        {
            var labelControl = new Label
            {
                Dock = DockStyle.Fill,
                Text = label,
                TextAlign = System.Drawing.ContentAlignment.TopLeft
            };
            _table.Controls.Add(labelControl, 0, _rowCount);

            var control = new RichTextBox()
            {
                Name = name,
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                WordWrap = true,
                ShortcutsEnabled = false,
                ReadOnly = readOnly,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            if (!string.IsNullOrWhiteSpace(value))
                control.Text = value;
            //control.TextChanged += ControlOnTextChanged;
            //control.GotFocus += FocusOnTextTaken;
            //control.LostFocus += FocusOnTextLost;
            control.LinkClicked += ControlOnLinkClicked;
            control.DoubleClick += ControlOnDoubleClick;
            _table.Controls.Add(control, 1, _rowCount);
            _table.SetColumnSpan(control, 2);

            if (_table.RowStyles.Count > _rowCount)
                _table.RowStyles[_rowCount] = new RowStyle(SizeType.Absolute, 150F);
            else
                _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));

            _rowCount++;
        }

        private void ControlOnLinkClicked(object sender, LinkClickedEventArgs linkClickedEventArgs)
        {
            try
            {
                if (Regex.IsMatch(linkClickedEventArgs.LinkText, 
                    @"\b(https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]", 
                    RegexOptions.IgnoreCase))
                {
#pragma warning disable SCS0001 // Command injection possible in {1} argument passed to '{0}'
                    Process.Start(linkClickedEventArgs.LinkText);
#pragma warning restore SCS0001 // Command injection possible in {1} argument passed to '{0}'
                }
            }
            catch
            {
                // Ignore the error because the link is simply not trusted.
            }
        }

        private void AddCheckBox([Required] string label, [Required] string name)
        {
            var control = new CheckBoxX()
            {
                Name = name,
                Text = label,
                Dock = DockStyle.Fill,
                Checked = !_hiddenProperties.Contains(name)
            };
            control.CheckedChangedEx += CheckBoxStatusChanged;
            _table.Controls.Add(control, 1, _rowCount);
            _table.SetColumnSpan(control, 2);

            if (_table.RowStyles.Count > _rowCount)
                _table.RowStyles[_rowCount] = new RowStyle(SizeType.Absolute, 39F);
            else
                _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));

            _rowCount++;
        }

        private void AddDummyRow()
        {
            var label = new Label();
            _table.Controls.Add(label, 0, _rowCount);
            if (_table.RowStyles.Count > _rowCount)
                _table.RowStyles[_rowCount] = new RowStyle(SizeType.Absolute, 0F);
            else
                _table.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));

            _rowCount++;
        }

        private void CheckBoxStatusChanged(object sender, CheckBoxXChangeEventArgs e)
        {
            if (!_loading)
            {
                if (sender is CheckBoxX control)
                {
                    var name = control.Name;
                    if (control.Checked)
                    {
                        if (_hiddenProperties.Contains(name))
                            _hiddenProperties.Remove(name);
                    }
                    else
                    {
                        if (!_hiddenProperties.Contains(name))
                            _hiddenProperties.Add(name);
                    }
                }
            }
        }
        #endregion
    }
}



