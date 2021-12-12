using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.PropertySchemaList
{
    public partial class PropertySchemaListPanel : UserControl, IShowThreatModelPanel<Form>, ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, IDesktopAlertAwareExtension
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private bool _loading;
        private bool _promoted;
        private static IEnumerable<IPropertySchemasUpdater> _updaters;

        static PropertySchemaListPanel()
        {
            _updaters = ExtensionUtils.GetExtensions<IPropertySchemasUpdater>()?.ToArray();
        }

        public PropertySchemaListPanel()
        {
            InitializeComponent();
            _appliesTo.EditTextBox.KeyPress += EditTextBoxOnKeyPress;
            InitializeGrid();

            var scopes = EnumExtensions.GetUIVisible<Scope>()?.ToArray();
            if (scopes?.Any() ?? false)
            {
                foreach (var scope in scopes)
                {
                    _appliesTo.Tokens.Add(new EditToken(scope.ToString(), scope.GetEnumLabel()));
                }
            }

            var executionModes = EnumExtensions.GetUIVisible<ExecutionMode>()?.ToArray();
            if (executionModes?.Any() ?? false)
            {
                foreach (var executionMode in executionModes)
                {
                    _requiredExecutionMode.Items.Add(executionMode.GetEnumLabel());
                }
            }
        }

        private void EditTextBoxOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                e.KeyChar = ';';
            }
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;
        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;

            LoadModel();
        }
        #endregion

        public bool IsInitialized => _model != null;

        private void InitializeGrid()
        {
            lock (_grid)
            {
                // TODO: Add here the new columns.

                GridPanel panel = _grid.PrimaryGrid;
                panel.ShowTreeButtons = false;
                panel.AllowRowDelete = false;
                panel.AllowRowInsert = false;
                panel.ShowRowDirtyMarker = false;
                panel.ShowRowHeaders = false;
                panel.InitialActiveRow = RelativeRow.None;

                panel.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Name",
                    Width = 175,
                    DataType = typeof(string),
                    EditorType = typeof(GridTextBoxDropDownEditControl),
                    AllowEdit = true
                });
                GridTextBoxDropDownEditControl ddc = panel.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }

                panel.Columns.Add(new GridColumn("Description")
                {
                    HeaderText = "Description",
                    Width = 350,
                    DataType = typeof(string),
                    EditorType = typeof(GridTextBoxDropDownEditControl),
                    AllowEdit = true
                });
                panel.Columns["Description"].CellStyles.Default.AllowMultiLine = Tbool.True;
                ddc = panel.Columns["Description"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }

                panel.Columns.Add(new GridColumn("Type")
                {
                    HeaderText = "Property Type",
                    Width = 150,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                panel.Columns.Add(new GridColumn("Priority")
                {
                    HeaderText = "Priority",
                    Width = 75,
                    DataType = typeof(int),
                    EditorType = typeof(GridIntegerInputEditControl),
                    AllowEdit = true
                });

                panel.Columns.Add(new GridColumn("Visible")
                {
                    HeaderText = "Visible",
                    Width = 75,
                    DataType = typeof(bool),
                    EditorType = typeof(GridSwitchButtonEditControl),
                    AllowEdit = true
                });

                panel.Columns.Add(new GridColumn("Printable")
                {
                    HeaderText = "Printable",
                    Width = 75,
                    DataType = typeof(bool),
                    EditorType = typeof(GridSwitchButtonEditControl),
                    AllowEdit = true
                });

                panel.Columns.Add(new GridColumn("ReadOnly")
                {
                    HeaderText = "Read Only",
                    Width = 75,
                    DataType = typeof(bool),
                    EditorType = typeof(GridSwitchButtonEditControl),
                    AllowEdit = true
                });

                panel.Columns.Add(new GridColumn("Values")
                {
                    HeaderText = "List of Values",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    EditorType = typeof(GridTokenEditControl),
                    AllowEdit = true
                });
            }
        }

        void DdcButtonClearClick(object sender, CancelEventArgs e)
        {
            GridTextBoxDropDownEditControl ddc =
                sender as GridTextBoxDropDownEditControl;

            if (ddc != null)
            {
                ddc.Text = null;
                e.Cancel = true;
            }
        }

        private void LoadModel()
        {
            try
            {
                _grid.SuspendLayout();
                _loading = true;

                _schemas.Items.Clear();
                Reset();

                var schemas = _model.Schemas?.OrderBy(x => x.Name).ToArray();
                if (schemas?.Any() ?? false)
                {
                    _schemas.Items.AddRange(schemas);
                }
            }
            finally
            {
                _loading = false;
                _grid.ResumeLayout(true);
            }
        }

        private void Reset()
        {
            _name.Enabled = false;
            _name.Clear();
            _namespace.Enabled = false;
            _namespace.Clear();
            _description.Enabled = false;
            _description.Clear();
            _appliesTo.Enabled = false;
            _appliesTo.SelectedTokens.Clear();
            _autoApply.Checked = false;
            _autoApply.Enabled = false;
            _requiredExecutionMode.Enabled = false;
            _requiredExecutionMode.SelectedItem = null;
            _priority.Text = null;
            _priority.Enabled = false;
            _system.Checked = false;
            _visible.Checked = false;
            _notExportable.Checked = false;

            var panel = _grid.PrimaryGrid;
            var rows = panel.Rows.OfType<GridRow>().ToArray();
            if (rows?.Any() ?? false)
            {
                foreach (var row in rows)
                    RemoveEventSubscriptions(row);
                panel.Rows.Clear();
            }
        }

        private void AddGridRow([NotNull] IPropertyType propertyType, [NotNull] GridPanel panel)
        {
            if (_schemas.SelectedItem is IPropertySchema schema)
            {
                string context = null;
                bool isList = false;
                if (propertyType is IListPropertyType listPt)
                {
                    context = listPt.Context;
                    isList = true;
                } else if (propertyType is IListMultiPropertyType listMultiPt)
                {
                    context = listMultiPt.Context;
                    isList = true;
                }

                var row = new GridRow(
                    propertyType.Name,
                    propertyType.Description,
                    propertyType.GetType().GetEnumFromType<PropertyValueType>().GetEnumLabel(),
                    propertyType.Priority,
                    propertyType.Visible,
                    !propertyType.DoNotPrint,
                    propertyType.ReadOnly,
                    context)
                {
                    Tag = propertyType,
                    AllowEdit = !schema.System || _promoted
                };

                row.Cells[7].AllowEdit = isList;

                ((INotifyPropertyChanged) propertyType).PropertyChanged += OnPropertyTypePropertyChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged += OnPropertyChanged;
                panel.Rows.Add(row);
            }
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IPropertyType propertyType)
            {
                ((INotifyPropertyChanged) propertyType).PropertyChanged -= OnPropertyTypePropertyChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnPropertyChanged;
            }
        }

        private void OnPropertyTypePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IPropertyType propertyType)
            {
                var row = GetRow(propertyType);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = propertyType.Name;
                            break;
                        case "Description":
                            row["Description"].Value = propertyType.Description;
                            break;
                        case "Priority":
                            row["Priority"].Value = propertyType.Priority;
                            break;
                        case "Visible":
                            row["Visible"].Value = propertyType.Visible;
                            break;
                        case "DoNotPrint":
                            row["Printable"].Value = !propertyType.DoNotPrint;
                            break;
                        case "ReadOnly":
                            row["ReadOnly"].Value = propertyType.ReadOnly;
                            break;
                    }
                }
            }
        }

        private GridRow GetRow([NotNull] IPropertyType propertyType)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == propertyType)
                {
                    result = row;
                    break;
                }
            }

            return result;
        }

        private GridRow GetRow(Point position)
        {
            GridRow result = null;

            GridElement item = _grid.GetElementAt(position);

            if (item is GridCell cell)
                result = cell.GridRow;

            return result;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var cell = sender as GridCell;
            var propertyName = propertyChangedEventArgs.PropertyName;

            if (!_loading && cell != null)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    if (row.Tag is IPropertyType propertyType)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                if (_schemas.SelectedItem is IPropertySchema schema)
                                {
                                    if (_updaters?.Any() ?? false)
                                    {
                                        foreach (var updater in _updaters)
                                        {
                                            updater.UpdatePropertyTypeName(_model, schema.Name,
                                                schema.Namespace, _name.Text, schema.Namespace);
                                        }
                                    }
                                }

                                propertyType.Name = (string) cell.Value;
                                break;
                            case "Description":
                                propertyType.Description = (string) cell.Value;
                                break;
                            case "Priority":
                                propertyType.Priority = (int) cell.Value;
                                break;
                            case "Visible":
                                propertyType.Visible = (bool) cell.Value;
                                break;
                            case "Printable":
                                propertyType.DoNotPrint = !((bool) cell.Value);
                                break;
                            case "ReadOnly":
                                propertyType.ReadOnly = (bool) cell.Value;
                                break;
                            case "Values":
                                if (propertyType is IListPropertyType listPt)
                                {
                                    listPt.Context = (string) cell.Value;
                                } else if (propertyType is IListMultiPropertyType listMultiPt)
                                {
                                    listMultiPt.Context = (string) cell.Value;
                                }
                                break;
                        }
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private void _schemas_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSchema();
        }

        private void RefreshSchema()
        {
            if (_schemas.SelectedItem is IPropertySchema schema)
            {
                _loading = true;

                _name.Text = schema.Name;
                _name.Enabled = !schema.System || _promoted;
                _namespace.Text = schema.Namespace;
                _namespace.Enabled = !schema.System || _promoted;
                _description.Text = schema.Description;
                _description.Enabled = !schema.System || _promoted;
                _appliesTo.SelectedTokens.Clear();
                var flags = schema.AppliesTo.GetFlags()?.ToArray();
                if (flags?.Any() ?? false)
                {
                    foreach (var flag in flags)
                    {
                        var token = _appliesTo.Tokens.FirstOrDefault(x => string.CompareOrdinal(x.Value, flag.ToString()) == 0);
                        if (token != null)
                            _appliesTo.SelectedTokens.Add(token);
                    }
                }

                _appliesTo.Enabled = !schema.System || _promoted;
                _priority.Value = schema.Priority;
                _priority.Enabled = !schema.System || _promoted;
                _autoApply.Checked = schema.AutoApply;
                _autoApply.Enabled = !schema.System || _promoted;
                _requiredExecutionMode.SelectedItem = schema.RequiredExecutionMode.GetEnumLabel();
                _requiredExecutionMode.Enabled = !schema.System || _promoted;
                _system.Checked = schema.System;
                _visible.Checked = schema.Visible;
                _visible.Enabled = !schema.System || _promoted;
                _notExportable.Checked = schema.NotExportable;
                _notExportable.Enabled = !schema.System || _promoted;

                var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
                if (rows.Any())
                {
                    foreach (var row in rows)
                        RemoveEventSubscriptions(row);
                }

                _grid.PrimaryGrid.Rows.Clear();
                var propertyTypes = schema.PropertyTypes?.ToArray();
                if (propertyTypes?.Any() ?? false)
                {
                    var panel = _grid.PrimaryGrid;
                    foreach (var propertyType in propertyTypes)
                    {
                        AddGridRow(propertyType, panel);
                    }
                }

                _loading = false;
            }
            else
            {
                Reset();
            }
        }

        private void _appliesTo_ValidateToken(object sender, ValidateTokenEventArgs ea)
        {
            ea.IsValid = !ea.IsNewToken;
        }

        private void _name_TextChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                if (string.IsNullOrWhiteSpace(_name.Text))
                {
                    _loading = true;
                    _name.Text = schema.Name;
                    _loading = false;

                    ShowWarning?.Invoke("The Schema name cannot be empty.");
                }
                else
                {
                    if (_updaters?.Any() ?? false)
                    {
                        foreach (var updater in _updaters)
                        {
                            updater.UpdateSchemaName(_model, schema.Name,
                                schema.Namespace, _name.Text, schema.Namespace);
                        }
                    }

                    schema.Name = _name.Text;
                }
            }
        }

        private void _namespace_TextChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                if (string.IsNullOrWhiteSpace(_namespace.Text))
                {
                    _loading = true;
                    _namespace.Text = schema.Namespace;
                    _loading = false;

                    ShowWarning?.Invoke("The Schema namespace cannot be empty.");
                }
                else
                {
                    if (_updaters?.Any() ?? false)
                    {
                        foreach (var updater in _updaters)
                        {
                            updater.UpdateSchemaName(_model, schema.Name,
                                schema.Namespace, schema.Name, _namespace.Text);
                        }
                    }

                    schema.Namespace = _namespace.Text;
                }
            }
        }

        private void _description_TextChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                schema.Description = _description.Text;
            }
        }

        private void _appliesTo_SelectedTokensChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                var selectedTokens = _appliesTo.SelectedTokens?.ToArray();
                var newScope = Scope.Undefined;

                if (selectedTokens?.Any() ?? false)
                {
                    foreach (var token in selectedTokens)
                    {
                        var scope = token.Value.GetEnum(Scope.Undefined);
                        if (scope != Scope.Undefined)
                        {
                            newScope |= scope;
                        }
                    }
                }

                schema.AppliesTo = newScope;
            }
        }

        private void _priority_ValueChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                schema.Priority = _priority.Value;
            }
        }

        private void _autoApply_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                schema.AutoApply = _autoApply.Checked;
            }
        }

        private void _requiredExecutionMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                schema.RequiredExecutionMode =
                    _requiredExecutionMode.SelectedItem?.ToString()?.GetEnumValue<ExecutionMode>() ?? ExecutionMode.Business;
            }
        }

        private void _visible_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                schema.Visible = _visible.Checked;
            }
        }

        private void _notExportable_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loading && _schemas.SelectedItem is IPropertySchema schema && (!schema.System || _promoted))
            {
                schema.NotExportable = _notExportable.Checked;
            }
        }
    }
}
