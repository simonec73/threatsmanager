﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Layout;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.Editors;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.WinForms.Dialogs;
using IProperty = ThreatsManager.Interfaces.ObjectModel.Properties.IProperty;
using MarkupLinkClickEventArgs = DevComponents.DotNetBar.Layout.MarkupLinkClickEventArgs;
using Padding = System.Windows.Forms.Padding;

namespace ThreatsManager.Utilities.WinForms
{
    public partial class ItemEditor
    {
        private Dictionary<Control, EventHandler> _changeActions = new Dictionary<Control, EventHandler>();

        #region Resource releasing.
        private interface IResourceReleaser
        {
            void Release();
        }

        private class ResourceReleaser<T> : IResourceReleaser where T : class
        {
            private Action<T> _action;
            private T _value;
            private Action<T, EventHandler> _eventHandlerAction;
            private EventHandler _handler;

            public ResourceReleaser([NotNull] Action<T> action, [NotNull] T value) 
            {
                _action = action;
                _value = value;
            }

            public ResourceReleaser([NotNull] Action<T, EventHandler> action, [NotNull] T value,
                [NotNull] EventHandler handler)
            {
                _eventHandlerAction = action;
                _value = value;
                _handler = handler;
            }

            public void Release()
            {
                if (_value != null)
                {
                    if (_action != null)
                    {
                        _action.Invoke(_value);
                    }
                    else if (_eventHandlerAction != null && _handler != null)
                    {
                        _eventHandlerAction.Invoke(_value, _handler);
                    }
                }

                _action = null;
                _value = null;
                _eventHandlerAction = null;
                _handler = null;
            }
        }

        private readonly List<IResourceReleaser> _releasers = new List<IResourceReleaser>();
        #endregion

        #region Label.
        private static Label AddSingleLineLabel([NotNull] LayoutControl container,
            string label, string text, Bitmap image = null)
        {
            return AddSingleLineLabel(container, label, text, 101, image);
        }

        private static Label AddSingleLineLabel([NotNull] LayoutControl container,
            string label, string text, int widthPerc, Bitmap image = null)
        {
            int height = (int) (21 * Dpi.Factor.Height);
            if (image != null)
            {
                height = image.Height + 10;
                text = "      " + text;
            }
            var control = new Label()
            {
                Text = text,
                UseMnemonic = false,
                Padding = new Padding(4, 0, 4, 0),
                TextAlign = ContentAlignment.TopLeft,
                Image = image,
                ImageAlign = ContentAlignment.TopLeft,
            };

            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = height,
                HeightType = eLayoutSizeType.Absolute,
                Width = widthPerc,
                WidthType = eLayoutSizeType.Percent,
                Padding = new Padding(4)
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private static Label AddLabel([NotNull] LayoutControl container,
            string label, string text, Bitmap image = null)
        {
            int height = (int) (105 * Dpi.Factor.Height);
            if (image != null)
            {
                height = image.Height + 10;
                text = "      " + text;
            }
            var control = new Label()
            {
                Text = text,
                UseMnemonic = false,
                Padding = new Padding(4, 0, 4, 0),
                TextAlign = ContentAlignment.TopLeft,
                Image = image,
                ImageAlign = ContentAlignment.TopLeft,
            };

            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = height,
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Padding = new Padding(4)
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }
        #endregion

        #region Hyperlink.
        private LinkLabel AddHyperlink([NotNull] LayoutControl container,
            string label, IIdentity identity, Bitmap image = null)
        {
            int height = (int) (25 * Dpi.Factor.Height);
            var text = identity?.Name;
            var padding = 4;
            if (image != null)
            {
                height = image.Height + 10;
                padding += 20;
            }
            var control = new LinkLabel()
            {
                Text = text,
                UseMnemonic = false,
                Padding = new Padding(padding, 0, 4, 0),
                Image = image,
                ImageAlign = ContentAlignment.TopLeft,
                Tag = identity
            };
            control.LinkClicked += OnHyperlinkClicked;
            _releasers.Add(new ResourceReleaser<LinkLabel>(ReleaseLinkClicked, control));

            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = height,
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Padding = new Padding(4)
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private void OnHyperlinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender is LinkLabel linkLabel && linkLabel.Tag is IIdentity identity)
            {
                var dialog = new ItemEditorDialog();
                dialog.SetExecutionMode(_executionMode);
                dialog.ReadOnly = ReadOnly;
                dialog.Item = identity;
                dialog.ShowDialog(Form.ActiveForm);
            }
        }

        private void ReleaseLinkClicked(LinkLabel control)
        {
            control.LinkClicked -= OnHyperlinkClicked;
        }
        #endregion

        #region Text and String properties.
        private TextBox AddSingleLineText([NotNull] LayoutControl container,
            IPropertySingleLineString property, bool readOnly)
        {
            var control = AddSingleLineText(container, property?.PropertyType?.Name,
                property?.StringValue, null,
                property?.PropertyType?.Description, readOnly);
            control.Tag = property;
            return control;
        }

        private TextBox AddSingleLineText([NotNull] LayoutControl container,
            string name, string text, Action<TextBox> changeAction,
            string tooltip, bool readOnly)
        {
            var control = new TextBox()
            {
                Text = text,
                ReadOnly = readOnly,
                ShortcutsEnabled = true
            };
            if (changeAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    if (sender is TextBox textBox)
                        changeAction(textBox);
                }
                _changeActions.Add(control, handler);
                control.TextChanged += handler;
                _releasers.Add(new ResourceReleaser<TextBox>(ReleaseTextChanged, control, handler));
            }
            else
            {
                control.TextChanged += TextPropertyChanged;
                _releasers.Add(new ResourceReleaser<TextBox>(ReleaseTextChanged, control, TextPropertyChanged));
            }
            var item = new LayoutControlItem()
            {
                Text = $"<a href=\"SingleLineText\">{name?.Replace("&", "&&")}</a>",
                Control = control,
                Height = (int) (20 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Tooltip = tooltip,
                Padding = new Padding(4)
            };
            item.MarkupLinkClick += OnMarkupLinkClick;
            _releasers.Add(new ResourceReleaser<LayoutControlItem>(ReleaseMarkupLinkClick, item));
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private void OnMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            if (sender is LayoutControlItem layoutControlItem)
            {
                try
                {
                    //_spellAsYouType.CheckAsYouType = false;
                    switch (e.HRef)
                    {
                        case "SingleLineText":
                            if (layoutControlItem.Control is TextBox textBox)
                            {
                                using (var dialog = new TextEditorDialog
                                {
                                    Multiline = false, 
                                    Text = textBox.Text,
                                    ReadOnly = textBox.ReadOnly
                                })
                                {
                                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                        textBox.Text = dialog.Text;
                                }
                            }
                            break;
                        case "Text":
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
                            break;
                    }
                }
                finally
                {
                    //_spellAsYouType.CheckAsYouType = true;
                }
            }
        }

        private RichTextBox AddText([NotNull] LayoutControl container,
            IPropertyString property, bool readOnly)
        {
            var control = AddText(container, property?.PropertyType?.Name,
                property?.StringValue, null,
                property?.PropertyType?.Description, readOnly);
            control.Tag = property;
            return control;
        }

        private RichTextBox AddText([NotNull] LayoutControl container,
            string name, string text, Action<RichTextBox> changeAction,
            string tooltip, bool readOnly)
        {
            var control = new RichTextBox()
            {
                Text = text,
                ReadOnly = readOnly,
                ShortcutsEnabled = true
            };
            if (changeAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    if (sender is RichTextBox textBox)
                        changeAction(textBox);
                }
                _changeActions.Add(control, handler);
                control.TextChanged += handler;
                _releasers.Add(new ResourceReleaser<RichTextBox>(ReleaseTextChanged, control, handler));
            }
            else
            {
                control.TextChanged += TextPropertyChanged;
                _releasers.Add(new ResourceReleaser<RichTextBox>(ReleaseTextChanged, control, TextPropertyChanged));
            }
            var item = new LayoutControlItem()
            {
                Text = $"<a href=\"Text\">{name?.Replace("&", "&&")}</a>",
                Control = control,
                Height = 150,
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Tooltip = tooltip
            };
            item.MarkupLinkClick += OnMarkupLinkClick;
            _releasers.Add(new ResourceReleaser<LayoutControlItem>(ReleaseMarkupLinkClick, item));
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private static void TextPropertyChanged(object sender, EventArgs e)
        {
            if (sender is RichTextBox textBox &&
                textBox.Tag is IPropertyString property)
            {
                property.StringValue = textBox.Text;
            } else if (sender is TextBox textBox2 &&
                textBox2.Tag is IPropertySingleLineString property2)
            {
                property2.StringValue = textBox2.Text;
            } else if (sender is RichTextBox textBox3 && textBox3.Tag is IPropertyViewerBlock propertyViewerBlock)
            {
                propertyViewerBlock.Text = textBox3.Text;
            } else if (sender is TextBox textBox4 && textBox4.Tag is IPropertyViewerBlock propertyViewerBlock2)
            {
                propertyViewerBlock2.Text = textBox4.Text;
            }
        }

        private void ReleaseTextChanged(TextBox control, EventHandler handler)
        {
            control.TextChanged -= handler;
        }

        private void ReleaseTextChanged(RichTextBox control, EventHandler handler)
        {
            control.TextChanged -= handler;
        }

        private void ReleaseMarkupLinkClick(LayoutControlItem item)
        {
            item.MarkupLinkClick -= OnMarkupLinkClick;
        }
        #endregion

        #region Boolean properties.
        private SwitchButton AddBool([NotNull] LayoutControl container,
            string label, bool value, string description,
            Action<bool> changeAction, bool readOnly)
        {
            var control = new SwitchButton() {Value = value, Width = 100, IsReadOnly = readOnly};
            if (changeAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    if (sender is SwitchButton switchButton)
                        changeAction(switchButton.Value);
                }
                _changeActions.Add(control, handler);
                control.ValueChanged += handler;
                _releasers.Add(new ResourceReleaser<SwitchButton>(ReleaseValueChanged, control, handler));
            }
            else
            {
                control.ValueChanged += BoolPropertyChanged;
                _releasers.Add(new ResourceReleaser<SwitchButton>(ReleaseValueChanged, control, BoolPropertyChanged));
            }
            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = 20 + (int)(10 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 100,
                WidthType = eLayoutSizeType.Percent,
                Tooltip = description
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private SwitchButton AddBool([NotNull] LayoutControl container,
            IPropertyBool property, bool readOnly)
        {
            var control = AddBool(container, property?.PropertyType?.Name, property?.Value ?? false,
                property?.PropertyType?.Description, null, readOnly);
            control.Tag = property;
            return control;
        }

        private static void BoolPropertyChanged(object sender, EventArgs e)
        {
            if (sender is SwitchButton switchButton &&
                switchButton.Tag is IPropertyBool property)
            {
                property.Value = switchButton.Value;
            }
        }

        private void ReleaseValueChanged(SwitchButton control, EventHandler handler)
        {
            control.ValueChanged -= handler;
        }
        #endregion

        #region Integer properties.
        private IntegerInput AddInteger([NotNull] LayoutControl container,
            string label, int value, string description,
            Action<int> changeAction, bool readOnly)
        {
            var control = new IntegerInput() {Value = value, Width = 100, IsInputReadOnly = readOnly};
            if (changeAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    if (sender is IntegerInput integerInput)
                        changeAction(integerInput.Value);
                }
                _changeActions.Add(control, handler);
                control.ValueChanged += handler;
                _releasers.Add(new ResourceReleaser<IntegerInput>(ReleaseValueChanged, control, handler));
            }
            else
            {
                control.ValueChanged += IntegerPropertyChanged;
                _releasers.Add(new ResourceReleaser<IntegerInput>(ReleaseValueChanged, control, IntegerPropertyChanged));
            }
            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = (int) (20 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 100,
                WidthType = eLayoutSizeType.Percent,
                Tooltip = description
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private IntegerInput AddInteger([NotNull] LayoutControl container,
            IPropertyInteger property, bool readOnly)
        {
            var control = AddInteger(container, property?.PropertyType?.Name, property?.Value ?? 0,
                property?.PropertyType?.Description, null, readOnly);
            control.Tag = property;
            return control;
        }

        private static void IntegerPropertyChanged(object sender, EventArgs e)
        {
            if (sender is IntegerInput control &&
                control.Tag is IPropertyInteger property)
            {
                property.Value = control.Value;
            }
        }

        private void ReleaseValueChanged(IntegerInput control, EventHandler handler)
        {
            control.ValueChanged -= handler;
        }
        #endregion

        #region Decimal properties.
        private DoubleInput AddDecimal([NotNull] LayoutControl container,
            string label, decimal value, string description,
            Action<decimal> changeAction, bool readOnly)
        {
            var control = new DoubleInput() {Value = Convert.ToDouble(value), Width = 100, DisplayFormat = "F2", IsInputReadOnly = readOnly};
            if (changeAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    if (sender is DoubleInput doubleInput)
                        changeAction(Convert.ToDecimal(doubleInput.Value));
                }
                _changeActions.Add(control, handler);
                control.ValueChanged += handler;
                _releasers.Add(new ResourceReleaser<DoubleInput>(ReleaseValueChanged, control, handler));
            }
            else
            {
                control.ValueChanged += DecimalPropertyChanged;
                _releasers.Add(new ResourceReleaser<DoubleInput>(ReleaseValueChanged, control, DecimalPropertyChanged));
            }
            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = (int) (20 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = (int) (125 * Dpi.Factor.Height),
                WidthType = eLayoutSizeType.Absolute,
                Tooltip = description
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private DoubleInput AddDecimal([NotNull] LayoutControl container,
            IPropertyDecimal property, bool readOnly)
        {
            var control = AddDecimal(container, property?.PropertyType?.Name, property?.Value ?? 0,
                property?.PropertyType?.Description, null, readOnly);
            control.Tag = property;
            return control;
        }

        private static void DecimalPropertyChanged(object sender, EventArgs e)
        {
            if (sender is DoubleInput control &&
                control.Tag is IPropertyDecimal property)
            {
                property.Value = Convert.ToDecimal(control.Value);
            }
        }

        private void ReleaseValueChanged(DoubleInput control, EventHandler handler)
        {
            control.ValueChanged -= handler;
        }
        #endregion

        #region Tokens properties.
        private TokenEditor AddTokens([NotNull] LayoutControl container,
            IPropertyTokens property, bool readOnly)
        {
            var control = new TokenEditor()
            {
                Tag = property,
                ReadOnly = readOnly
            };
            control.EditTextBox.KeyPress += OnKeywordsKeyPress;
            _releasers.Add(new ResourceReleaser<TokenEditor>(ReleaseKeyPress, control));
            var values = property?.Value?.ToArray();
            if (values != null)
            {
                foreach (var value in values)
                {
                    var token = new EditToken(value);
                    control.Tokens.Add(token);
                    control.SelectedTokens.Add(token);
                }
            }
            control.SelectedTokensChanged += TokensPropertyChanged;
            _releasers.Add(new ResourceReleaser<TokenEditor>(ReleaseSelectedTokensChanged, control));

            var item = new LayoutControlItem()
            {
                Text = property?.PropertyType?.Name.Replace("&", "&&"),
                Control = control,
                Height = (int) (20 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                //MinSize = new Size(500, 30),
                Tooltip = property?.PropertyType?.Description
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private static void TokensPropertyChanged(object sender, EventArgs e)
        {
            if (sender is TokenEditor tokenEditor &&
                tokenEditor.Tag is IPropertyTokens property)
            {
                property.Value = tokenEditor.SelectedTokens.Select(x => x.Value);
            }
        }
        
        private static void OnKeywordsKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                e.KeyChar = ';';
            }
        }

        private static void ReleaseKeyPress(TokenEditor control)
        {
            control.EditTextBox.KeyPress -= OnKeywordsKeyPress;
        }

        private static void ReleaseSelectedTokensChanged(TokenEditor control)
        {
            control.SelectedTokensChanged -= TokensPropertyChanged;
        }
        #endregion

        #region Combo Box properties.
        private ComboBox AddCombo([NotNull] LayoutControl container,
            IPropertyList property, bool readOnly)
        {
            ComboBox control = null;

            if (property?.PropertyType is IListPropertyType listPropertyType)
            {
                var items = listPropertyType.Values?.ToArray();
                if (items?.Any() ?? false)
                {
                    control = new ComboBox()
                    {
                        Tag = property,
                        Enabled = !readOnly,
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };

                    control.Items.Add(string.Empty);
                    control.Items.AddRange(items);
                    var selected = control.Items.OfType<IListItem>()
                        .FirstOrDefault(x => string.CompareOrdinal(x.Id, property.Value?.Id) == 0);
                    control.SelectedItem = selected;
                    control.SelectedIndexChanged += ComboPropertyChanged;
                    _releasers.Add(new ResourceReleaser<ComboBox>(ReleaseSelectedIndexChanged, control, ComboPropertyChanged));

                    var item = new LayoutControlItem()
                    {
                        Text = listPropertyType.Name.Replace("&", "&&"),
                        Control = control,
                        Height = (int) (25 * Dpi.Factor.Height),
                        HeightType = eLayoutSizeType.Absolute,
                        Width = 101,
                        WidthType = eLayoutSizeType.Percent
                    };
                    container.Controls.Add(control);
                    container.RootGroup.Items.Add(item);
                }
            }

            return control;
        }

        private static void ComboPropertyChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.Tag is IPropertyList propertyList)
            {
                propertyList.Value = comboBox.SelectedItem as IListItem;
            }
        }

        private ComboBox AddCombo([NotNull] LayoutControl container,
            string label, string selected,
            IEnumerable<string> values, Action<string> changeAction, bool readOnly)
        {
            var control = new ComboBox()
            {
                Enabled = !readOnly,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            if (values?.Any() ?? false)
            {
                control.Items.AddRange(values?.ToArray());
                control.SelectedItem = selected;
            }
            if (changeAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    if (sender is ComboBox comboBox)
                        changeAction((string) control.SelectedItem);
                }
                _changeActions.Add(control, handler);
                control.SelectedIndexChanged += handler;
                _releasers.Add(new ResourceReleaser<ComboBox>(ReleaseSelectedIndexChanged, control, handler));
            }
            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = (int) (25 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private void ReleaseSelectedIndexChanged(ComboBox control, EventHandler handler)
        {
            control.SelectedIndexChanged -= handler;
        }
        #endregion

        #region List Box.
        private ListBox AddListBox([NotNull] LayoutControl container,
            string label, IEnumerable<object> values,
            EventHandler addItemEventHandler, bool readOnly)
        {
            var control = new ListBox()
            {
                SelectionMode = SelectionMode.MultiExtended
            };
            var items = values?.ToArray();
            if (items?.Any() ?? false)
                control.Items.AddRange(items.ToArray());
            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = (int) (150 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Padding = new Padding(4)
            };
            if (string.IsNullOrWhiteSpace(label))
            {
                item.TextVisible = false;
            }
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            if (!readOnly)
            {
                var addButton = new Button()
                {
                    Text = "Add",
                    Tag = control
                };
                if (addItemEventHandler != null)
                {
                    _changeActions.Add(addButton, addItemEventHandler);
                    addButton.Click += addItemEventHandler;
                    _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, addButton, addItemEventHandler));
                }

                var addButtonItem = new LayoutControlItem()
                {
                    Control = addButton,
                    Height = 20 + (int) (15 * Dpi.Factor.Height),
                    HeightType = eLayoutSizeType.Absolute,
                    Width = 33,
                    WidthType = eLayoutSizeType.Percent
                };
                container.Controls.Add(addButton);
                container.RootGroup.Items.Add(addButtonItem);

                var removeButton = new Button()
                {
                    Text = "Remove",
                    Tag = control
                };
                removeButton.Click += ClickedListBoxRemove;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, removeButton, ClickedListBoxRemove));
                var removeButtonItem = new LayoutControlItem()
                {
                    Control = removeButton,
                    Height = 20 + (int) (15 * Dpi.Factor.Height),
                    HeightType = eLayoutSizeType.Absolute,
                    Width = 33,
                    WidthType = eLayoutSizeType.Percent
                };
                container.Controls.Add(removeButton);
                container.RootGroup.Items.Add(removeButtonItem);

                var clearButton = new Button()
                {
                    Text = "Clear",
                    Tag = control
                };
                clearButton.Click += ClickedListBoxClear;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, clearButton, ClickedListBoxClear));
                var clearButtonItem = new LayoutControlItem()
                {
                    Control = clearButton,
                    Height = 20 + (int) (15 * Dpi.Factor.Height),
                    HeightType = eLayoutSizeType.Absolute,
                    Width = 33,
                    WidthType = eLayoutSizeType.Percent
                };
                container.Controls.Add(clearButton);
                container.RootGroup.Items.Add(clearButtonItem);
            }

            return control;
        }

        private void ClickedListBoxClear(object sender, EventArgs e)
        {
            if (sender is Button button &&
                button.Tag is ListBox listBox &&
                listBox.Items.Count > 0)
            {
                if (_item is IThreatEventsContainer teContainer)
                {
                    if (MessageBox.Show(Form.ActiveForm,
                            "Are you sure you want to remove all Threat Events?",
                            "Remove all Threat Events", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var list = teContainer.ThreatEvents?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var item in list)
                                teContainer.RemoveThreatEvent(item.Id);
                        }

                        listBox.Items.Clear();
                    }
                }
                else if (_item is IThreatTypesContainer ttContainer)
                {
                    if (MessageBox.Show(Form.ActiveForm,
                            "Are you sure you want to remove all Threat Types?",
                            "Remove all Threat Types", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var list = ttContainer.ThreatTypes?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var item in list)
                                ttContainer.RemoveThreatType(item.Id);
                        }

                        listBox.Items.Clear();
                    }
                }
                else if (_item is IThreatEventMitigationsContainer temContainer)
                {
                    if (MessageBox.Show(Form.ActiveForm,
                            "Are you sure you want to remove all Mitigations for the current Threat Event?",
                            "Remove all Mitigations", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var list = temContainer.Mitigations?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var item in list)
                                temContainer.RemoveMitigation(item.MitigationId);
                        }

                        listBox.Items.Clear();
                    }
                }
                else if (_item is IThreatTypeMitigationsContainer ttmContainer)
                {
                    if (MessageBox.Show(Form.ActiveForm,
                            "Are you sure you want to remove all Mitigations for the current Threat Type?",
                            "Remove all Mitigations", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var list = ttmContainer.Mitigations?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var item in list)
                                ttmContainer.RemoveMitigation(item.MitigationId);
                        }

                        listBox.Items.Clear();
                    }
                }
            }
        }

        private void ClickedListBoxRemove(object sender, EventArgs e)
        {
            if (sender is Button button &&
                button.Tag is ListBox listBox &&
                listBox.SelectedItems.Count > 0)
            {
                if (_item is IThreatEventsContainer container)
                {
                    if (MessageBox.Show(Form.ActiveForm,
                            "Are you sure you want to remove the selected Threat Events?",
                            "Remove the selected Threat Events", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var items = listBox.SelectedItems.OfType<IThreatEvent>().ToArray();
                        if (items.Any())
                        {
                            foreach (var item in items)
                            {
                                if (!container.RemoveThreatEvent(item.Id))
                                    listBox.Items.Remove(item);
                            }
                        }
                    }
                }
                else if (_item is IThreatTypeMitigationsContainer ttmContainer)
                {
                    if (MessageBox.Show(Form.ActiveForm,
                            "Are you sure you want to remove the selected Mitigations from the list of Standard Mitigations for the current Threat Type?",
                            "Remove the selected Mitigations", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var items = listBox.SelectedItems.OfType<IThreatTypeMitigation>().ToArray();
                        if (items.Any())
                        {
                            foreach (var item in items)
                            {
                                if (!ttmContainer.RemoveMitigation(item.MitigationId))
                                    listBox.Items.Remove(item);
                            }
                        }
                    }
                }
                else if (_item is IThreatEventMitigationsContainer temContainer)
                {
                    if (MessageBox.Show(Form.ActiveForm,
                            "Are you sure you want to remove the selected Mitigations from the associations with the current Threat Event?",
                            "Remove the selected Mitigations", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        var items = listBox.SelectedItems.OfType<IThreatEventMitigation>().ToArray();
                        if (items.Any())
                        {
                            foreach (var item in items)
                            {
                                if (!temContainer.RemoveMitigation(item.MitigationId))
                                    listBox.Items.Remove(item);
                            }
                        }
                    }
                }
            }
        }

        private void ReleaseClick(Button control, EventHandler handler)
        {
            control.Click -= handler;
        }
        #endregion

        #region List View.
        private ListView AddListView([NotNull] LayoutControl container,
            string label, IEnumerable<IThreatEvent> threatEvents)
        {
            var smallImageList = new ImageList();
            var imageList = new ImageList();
            var control = new ListView()
            {
                HeaderStyle = ColumnHeaderStyle.None,
                View = View.Details,
                SmallImageList = smallImageList,
                LargeImageList = imageList,
                UseCompatibleStateImageBehavior = false,
                HideSelection = false,
                MultiSelect = false
            };

            control.Columns.AddRange(new [] {new ColumnHeader()});
            var values = threatEvents?.ToArray();
            if (values?.Any() ?? false)
            {
                int i = 0;
                foreach (var value in values)
                {
                    smallImageList.Images.Add(value.Parent.GetImage(ImageSize.Small));
                    imageList.Images.Add(value.Parent.GetImage(ImageSize.Medium));
                    control.Items.Add(new ListViewItem(value.Parent.Name, i)
                    {
                        Tag = value
                    });
                    i++;
                }
                control.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            control.MouseDoubleClick += OnListViewDoubleClick;
            _releasers.Add(new ResourceReleaser<ListView>(ReleaseMouseDoubleClick, control));

            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = (int) (150 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Padding = new Padding(4)
            };
            if (string.IsNullOrWhiteSpace(label))
            {
                item.TextVisible = false;
            }
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private ListView AddListView([NotNull] LayoutControl container,
            string label, IEnumerable<IVulnerability> vulnerabilities)
        {
            var smallImageList = new ImageList();
            var imageList = new ImageList();
            var control = new ListView()
            {
                HeaderStyle = ColumnHeaderStyle.None,
                View = View.Details,
                SmallImageList = smallImageList,
                LargeImageList = imageList,
                UseCompatibleStateImageBehavior = false,
                HideSelection = false,
                MultiSelect = false
            };

            control.Columns.AddRange(new[] { new ColumnHeader() });
            var values = vulnerabilities?.ToArray();
            if (values?.Any() ?? false)
            {
                int i = 0;
                foreach (var value in values)
                {
                    var parent = value.Parent as IIdentity;
                    if (parent != null)
                    {
                        smallImageList.Images.Add(parent.GetImage(ImageSize.Small));
                        imageList.Images.Add(parent.GetImage(ImageSize.Medium));
                        control.Items.Add(new ListViewItem(parent.Name, i)
                        {
                            Tag = value
                        });
                        i++;
                    }
                }
                control.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            control.MouseDoubleClick += OnListViewDoubleClick;
            _releasers.Add(new ResourceReleaser<ListView>(ReleaseMouseDoubleClick, control));

            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = (int)(150 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Padding = new Padding(4)
            };
            if (string.IsNullOrWhiteSpace(label))
            {
                item.TextVisible = false;
            }
            container.Controls.Add(control);
            container.RootGroup.Items.Add(item);

            return control;
        }

        private void OnListViewDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender is ListView listView)
            {
                ListViewHitTestInfo hit = listView.HitTest(e.Location);

                if (hit.Item?.Tag is IIdentity identity)
                {
                    var dialog = new ItemEditorDialog();
                    dialog.SetExecutionMode(_executionMode);
                    dialog.ReadOnly = ReadOnly;
                    dialog.Item = identity;
                    dialog.ShowDialog(Form.ActiveForm);
                }
            }
        }

        private void ReleaseMouseDoubleClick(ListView control)
        {
            control.MouseDoubleClick -= OnListViewDoubleClick;
        }
        #endregion

        #region List properties.
        private SuperGridControl AddList([NotNull] LayoutControl container, IPropertyArray property, bool readOnly)
        {
            var control = AddList(container, property?.PropertyType?.Name, property?.Value, readOnly);
            control.Tag = new WinForms.ItemEditor.Actions<IPropertyArray>(property)
            {
                Created = CreatePropertyArrayItem,
                Changed = ChangePropertyArrayItem,
                Removed = RemovePropertyArrayItem,
                Cleared = ClearPropertyArrayItem
            };

            if (property != null)
            {
                property.Changed += PropertyChanged;
                _releasers.Add(new ResourceReleaser<IPropertyArray>(ReleaseChanged, property));
            }

            return control;

        }

        private void PropertyChanged(IProperty obj)
        {
            if (obj is IPropertyArray array)
            {
                var schema = _model?.GetSchema(array.PropertyType?.SchemaId ?? Guid.Empty);
                if (schema != null)
                {
                    var control = GetControl(schema.Name, array.PropertyType.Name);
                    if (control is SuperGridControl grid)
                    {
                        grid.SuspendLayout();
                        grid.PrimaryGrid.Rows.Clear();
                        var list = array.Value?.ToArray();
                        if (list?.Any() ?? false)
                        {
                            foreach (var item in list)
                            {
                                var row = new GridRow(item);
                                row.Cells[0].Tag = item;
                                row.Cells[0].PropertyChanged += RowCellChanged;
                                grid.PrimaryGrid.Rows.Add(row);
                            }
                        }
                        grid.ResumeLayout();
                    }
                }
            }
        }

        private bool CreatePropertyArrayItem(Control control, string text, IPropertyArray property)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(text))
            {
                var current = property?.Value?.ToArray();
                if (!(current?.Contains(text) ?? false))
                {
                    var list = new List<string>();
                    if (current?.Any() ?? false)
                    {
                        list.AddRange(current);
                    }

                    list.Add(text);

                    property.Value = list;
                    result = true;
                }
            }

            return result;
        }

        private bool ChangePropertyArrayItem(Control control, string oldText, string newText, IPropertyArray property)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(oldText) && !string.IsNullOrWhiteSpace(newText))
            {
                var current = property?.Value?.ToArray();
                if (current?.Contains(oldText) ?? false)
                {
                    var list = new List<string>(current);
                    var index = list.IndexOf(oldText);
                    list[index] = newText;

                    property.Value = list;

                    result = true;
                }
            }

            return result;
        }

        private bool RemovePropertyArrayItem(Control control, string text, IPropertyArray property)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(text))
            {
                var current = property?.Value?.ToArray();
                if (current?.Contains(text) ?? false)
                {
                    var list = new List<string>(current);
                    list.Remove(text);

                    property.Value = list;
                    result = true;
                }
            }

            return result;
        }

        private void ClearPropertyArrayItem(Control control, IPropertyArray property)
        {
            property.Value = null;
        }

        private SuperGridControl AddList([NotNull] LayoutControl container,
            string label, IEnumerable<string> values, bool readOnly)
        {
            var control = new SuperGridControl();
            control.LicenseKey = "PUT_YOUR_LICENSE_HERE";
            InitializeGrid(control);

            var items = values?.ToArray();
            if (items?.Any() ?? false)
            {
                foreach (var item in items)
                {
                    var row = new GridRow(item);
                    row.Cells[0].Tag = item;
                    row.Cells[0].PropertyChanged += RowCellChanged;
                    control.PrimaryGrid.Rows.Add(row);
                }
            }

            var controlItem = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                Control = control,
                Height = (int) (150 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Padding = new Padding(4)
            };
            container.Controls.Add(control);
            container.RootGroup.Items.Add(controlItem);

            if (!readOnly)
            {
                var addButton = new Button()
                {
                    Text = "Add",
                    Tag = control
                };
                addButton.Click += ClickedListAdd;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, addButton, ClickedListAdd));

                var addButtonItem = new LayoutControlItem()
                {
                    Control = addButton,
                    Height = 20 + (int) (15 * Dpi.Factor.Height),
                    HeightType = eLayoutSizeType.Absolute,
                    Width = 33,
                    WidthType = eLayoutSizeType.Percent
                };
                container.Controls.Add(addButton);
                container.RootGroup.Items.Add(addButtonItem);

                var removeButton = new Button()
                {
                    Text = "Remove",
                    Tag = control
                };
                removeButton.Click += ClickedListRemove;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, removeButton, ClickedListRemove));

                var removeButtonItem = new LayoutControlItem()
                {
                    Control = removeButton,
                    Height = 20 + (int) (15 * Dpi.Factor.Height),
                    HeightType = eLayoutSizeType.Absolute,
                    Width = 33,
                    WidthType = eLayoutSizeType.Percent
                };
                container.Controls.Add(removeButton);
                container.RootGroup.Items.Add(removeButtonItem);

                var clearButton = new Button()
                {
                    Text = "Clear",
                    Tag = control
                };
                clearButton.Click += ClickedListClear;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, clearButton, ClickedListClear));

                var clearButtonItem = new LayoutControlItem()
                {
                    Control = clearButton,
                    Height = 20 + (int) (15 * Dpi.Factor.Height),
                    HeightType = eLayoutSizeType.Absolute,
                    Width = 33,
                    WidthType = eLayoutSizeType.Percent
                };
                container.Controls.Add(clearButton);
                container.RootGroup.Items.Add(clearButtonItem);
            }

            return control;
        }

        private void RowCellChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is GridCell cell && cell.Tag is string oldText &&
                cell.SuperGrid.Tag is WinForms.ItemEditor.IActions actions)
            {
                cell.Tag = cell.Value;
                if (!actions.RaiseChanged(cell.SuperGrid, oldText, (string) cell.Value) &&
                    !string.IsNullOrWhiteSpace(oldText))
                {
                    MessageBox.Show(Form.ActiveForm, "Please do not clear the text. Empty text is not supported.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cell.Value = oldText;
                }
            }
        }

        private void InitializeGrid([NotNull] SuperGridControl grid)
        {
            var panel = grid.PrimaryGrid;
            panel.ShowTreeButtons = false;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.ShowRowDirtyMarker = false;
            panel.ShowColumnHeader = false;
            panel.ShowRowHeaders = false;

            panel.Columns.Add(new GridColumn("Text")
            {
                DataType = typeof(string),
                EditorType = typeof(GridTextBoxDropDownEditControl),
                AllowEdit = true,
                AutoSizeMode = ColumnAutoSizeMode.Fill
            });
        }

        private void ClickedListClear(object sender, EventArgs e)
        {
            if (sender is Button button &&
                button.Tag is SuperGridControl grid &&
                grid.PrimaryGrid.Rows.Count > 0 &&
                grid.Tag is WinForms.ItemEditor.IActions actions)
            {
                if (MessageBox.Show(Form.ActiveForm,
                        "Are you sure you want to remove all items from the list?",
                        "Remove all items", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    actions.RaiseCleared(grid);
                    grid.PrimaryGrid.Rows.Clear();
                }
            }
        }

        private void ClickedListRemove(object sender, EventArgs e)
        {
            if (sender is Button button &&
                button.Tag is SuperGridControl grid &&
                (grid.PrimaryGrid.SelectedRows.OfType<GridRow>().Any() || grid.PrimaryGrid.SelectedCellCount > 0) &&
                grid.Tag is WinForms.ItemEditor.IActions actions)
            {
                if (MessageBox.Show(Form.ActiveForm,
                        "Are you sure you want to remove the selected items from the list?",
                        "Remove the selected items", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    var rows = grid.PrimaryGrid.SelectedRows.OfType<GridRow>().ToArray();
                    foreach (var item in rows)
                    {
                        if (item.Cells[0].Value is string text)
                            actions.RaiseRemoved(grid, text);
                    }

                    var cells = grid.PrimaryGrid.SelectedCells.OfType<GridCell>().ToArray();
                    foreach (var cell in cells)
                    {
                        if (cell.Value is string text)
                        {
                            //grid.PrimaryGrid.Rows.Remove(cell.GridRow);
                            actions.RaiseRemoved(grid, text);
                        }
                    }
                }
            }
        }

        private void ClickedListAdd(object sender, EventArgs e)
        {
            if (sender is Button button &&
                button.Tag is SuperGridControl grid &&
                grid.Tag is WinForms.ItemEditor.IActions actions)
            {
                //var row = new GridRow("New");
                //row.Cells[0].Tag = "New";
                //row.Cells[0].PropertyChanged += RowCellChanged;
                //grid.PrimaryGrid.Rows.Add(row);
                actions.RaiseCreated(grid, "New");
            }
        }

        private void ReleaseChanged(IPropertyArray property)
        {
            property.Changed -= PropertyChanged;
        }
        #endregion

        #region Property Viewer.
        private void AddPropertyViewerBlocks([NotNull] LayoutControl container, 
            [NotNull] IEnumerable<IPropertyViewerBlock> blocks, bool readOnly)
        {
            if (blocks?.Any() ?? false)
            {
                Control control;

                foreach (var block in blocks)
                {
                    control = null;

                    switch (block.BlockType)
                    {
                        case PropertyViewerBlockType.Label:
                            AddLabel(container, block.Label, block.Text);
                            break;
                        case PropertyViewerBlockType.SingleLineLabel:
                            AddSingleLineLabel(container, block.Label, block.Text);
                            break;
                        case PropertyViewerBlockType.String:
                            control = AddText(container, block.Label, block.Text, null, null, readOnly);
                            break;
                        case PropertyViewerBlockType.SingleLineString:
                            control = AddSingleLineText(container, block.Label, block.Text, null, null, readOnly);
                            break;
                        case PropertyViewerBlockType.ImageButton:
                            control = AddCompactButton(container, block.Label, block.Image, null, readOnly);
                            break;
                        case PropertyViewerBlockType.Button:
                            control = AddButton(container, block.Label, block.Text, block.Image, null, readOnly);
                            break;
                        case PropertyViewerBlockType.DateTimePicker:
                            if (DateTime.TryParse(block.Text, out var dateTime))
                                control = AddDateTimePicker(container, block.Label, dateTime, null, readOnly);
                            else
                                control = AddDateTimePicker(container, block.Label, null, null, readOnly);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (control != null)
                        control.Tag = block;
                }
            }
        }

        private Button AddCompactButton([NotNull] LayoutControl container, string label, 
            Bitmap image, Action<Button> clickAction, bool readOnly)
        {
            var picture = image;
            if (image != null)
            {
                picture = new Bitmap(image, (int) (32 * Dpi.Factor.Width),
                    (int) (32 * Dpi.Factor.Height));
            }

            var result = new Button()
            {
                Image = picture,
                Enabled = !readOnly,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = {BorderSize = 0}
            };
            if (clickAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    clickAction(result);
                }
                _changeActions.Add(result, handler);
                result.Click += handler;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, result, handler));
            }
            else
            {
                result.Click += ButtonClicked;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, result, ButtonClicked));
            }
            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                TextVisible = false,
                Control = result,
                Height = (int) (48 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = (int) (48 * Dpi.Factor.Width),
                WidthType = eLayoutSizeType.Absolute
            };
            _superTooltip.SetSuperTooltip(result, new SuperTooltipInfo()
            {
                HeaderVisible = false,
                BodyText = label?.Replace("\n", "<br/>"),
                FooterVisible = false
            });
            container.Controls.Add(result);
            container.RootGroup.Items.Add(item);

            return result;
        }

        private Button AddButton([NotNull] LayoutControl container, string label, string description,
            Bitmap image, Action<Button> clickAction, bool readOnly)
        {
            var result = new Button() {Text = label, Image = image, Enabled = !readOnly};
            if (clickAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    clickAction(result);
                }
                _changeActions.Add(result, handler);
                result.Click += handler;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, result, handler));
            }
            else
            {
                result.Click += ButtonClicked;
                _releasers.Add(new ResourceReleaser<Button>(ReleaseClick, result, ButtonClicked));
            }
            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                TextVisible = false,
                Control = result,
                Height = (int) (36 * Dpi.Factor.Height),
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent,
                Tooltip = description
            };
            container.Controls.Add(result);
            container.RootGroup.Items.Add(item);

            return result;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is IPropertyViewerBlock propertyViewerBlock)
            {
                if (propertyViewerBlock.Execute() && button.Parent is LayoutControl layoutControl &&
                        layoutControl.Tag is IPropertyViewer viewer)
                {
                    layoutControl.SuspendLayout();
                    layoutControl.Controls.Clear();
                    layoutControl.RootGroup.Items.Clear();

                    var blocks = viewer.Blocks;
                    if (blocks?.Any() ?? false)
                    {
                        AddPropertyViewerBlocks(layoutControl, blocks, _readonly);
                    }

                    FinalizeSection(layoutControl);
                    layoutControl.ResumeLayout();
                }
            }
        }

        private DateTimePicker AddDateTimePicker([NotNull] LayoutControl container, string label, DateTime? dateTime,
            Action<DateTimePicker> changeAction, bool readOnly)
        {
            var result = new DateTimePicker() {
                Text = label, 
                Value = dateTime ?? DateTime.Now, 
                Enabled = !readOnly

            };
            if (changeAction != null)
            {
                void handler(object sender, EventArgs args)
                {
                    changeAction(result);
                }
                _changeActions.Add(result, handler);
                result.ValueChanged += handler;
                _releasers.Add(new ResourceReleaser<DateTimePicker>(ReleaseValueChanged, result, handler));
            }
            else
            {
                result.ValueChanged += DateTimePickerValueChanged;
                _releasers.Add(new ResourceReleaser<DateTimePicker>(ReleaseValueChanged, result, DateTimePickerValueChanged));
            }
            var item = new LayoutControlItem()
            {
                Text = label?.Replace("&", "&&"),
                TextVisible = false,
                Control = result,
                Height = 36,
                HeightType = eLayoutSizeType.Absolute,
                Width = 101,
                WidthType = eLayoutSizeType.Percent
            };
            container.Controls.Add(result);
            container.RootGroup.Items.Add(item);

            return result;
        }

        private void DateTimePickerValueChanged(object sender, EventArgs e)
        {
            if (sender is DateTimePicker picker && picker.Tag is IPropertyViewerBlock block)
            {
                block.Text = picker.Value.ToString();
            }
        }

        private void ReleaseValueChanged(DateTimePicker control, EventHandler handler)
        {
            control.ValueChanged -= handler;
        }
        #endregion
    }
}