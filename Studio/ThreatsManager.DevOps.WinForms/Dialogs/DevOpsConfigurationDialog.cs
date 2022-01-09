using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model.TypeAdapters;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.DevOps.Dialogs
{
    public partial class DevOpsConfigurationDialog : Form
    {
        private bool _loading;
        private IThreatModel _model;

        public DevOpsConfigurationDialog()
        {
            InitializeComponent();

            InitializeStatesGrid();
            InitializeFieldsGrid();
        }

        public async Task Initialize([NotNull] IThreatModel model)
        {
            _model = model;
            _tag.Text = null;
            _parents.Items.Clear();
            _itemTypes.Items.Clear();

            var connector = DevOpsManager.GetConnector(model);
            if (connector != null)
            {
                _tag.Text = connector.Tag;

                if (connector.MasterParent != null)
                {
                    _parents.Items.Add(connector.MasterParent);
                    _parents.SelectedIndex = 0;
                    _parentItemType.Text = connector.MasterParent.WorkItemType;
                }

                var itemTypesAsync = await connector.GetWorkItemTypesAsync();
                var itemTypes = itemTypesAsync?.ToArray();
                if (itemTypes?.Any() ?? false)
                {
                    _itemTypes.Items.AddRange(itemTypes);
                    if (!string.IsNullOrEmpty(connector.WorkItemType) &&
                        _itemTypes.Items.Contains(connector.WorkItemType))
                    {
                        _itemTypes.SelectedItem = connector.WorkItemType;
                    }
                }

                var fieldsAsync = await connector.GetWorkItemDevOpsFieldsAsync();
                var fields = fieldsAsync?.ToArray();
                if (fields?.Any() ?? false)
                {
                    var mappings = connector.WorkItemFieldMappings?.ToArray();

                    foreach (var mapping in mappings)
                    {
                        var field = fields.FirstOrDefault(x => string.CompareOrdinal(mapping.Key, x.Id) == 0);
                        if (field != null)
                        {
                            var row = new GridRow(field.Label, mapping.Value)
                            {
                                Tag = field
                            };
                            _gridFields.PrimaryGrid.Rows.Add(row);
                        }
                    }
                }
            }
        }

        private void InitializeStatesGrid()
        {
            var panel = _gridStates.PrimaryGrid;
            panel.ShowTreeButtons = false;
            panel.ShowTreeLines = false;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.AllowRowResize = false;
            panel.ShowRowDirtyMarker = false;
            panel.ShowRowHeaders = false;
            panel.InitialActiveRow = RelativeRow.None;

            panel.Columns.Add(new GridColumn("State")
            {
                HeaderText = "State for Item Type",
                AutoSizeMode = ColumnAutoSizeMode.Fill,
                DataType = typeof(string),
                AllowEdit = false
            });

            panel.Columns.Add(new GridColumn("KnownState")
            {
                HeaderText = "Known State",
                DataType = typeof(string),
                EditorType = typeof(ItemsComboBox),
                EditorParams = new object[] { ThreatsManager.Utilities.EnumExtensions.GetEnumLabels<WorkItemStatus>() },
                AllowEdit = true,
                Width = 150
            });
        }

        private void InitializeFieldsGrid()
        {
            var panel = _gridFields.PrimaryGrid;
            panel.ShowTreeButtons = false;
            panel.ShowTreeLines = false;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.AllowRowResize = false;
            panel.ShowRowDirtyMarker = false;
            panel.ShowRowHeaders = false;
            panel.InitialActiveRow = RelativeRow.None;
            panel.MultiSelect = false;

            panel.Columns.Add(new GridColumn("Field")
            {
                HeaderText = "Field",
                AutoSizeMode = ColumnAutoSizeMode.Fill,
                DataType = typeof(string),
                AllowEdit = false
            });
 
            panel.Columns.Add(new GridColumn("KnownField")
            {
                HeaderText = "Known Field",
                DataType = typeof(string),
                Width = 300,
                EditorType = typeof(GridButtonXEditControl)
            });
            var bc = panel.Columns["KnownField"].EditControl as GridButtonXEditControl;
            if (bc != null)
            {
                bc.Click += FieldButtonClick;
            }
        }

        private async void OnComboBoxTextUpdate(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var connector = DevOpsManager.GetConnector(_model);

                if (connector != null)
                {
                    string filter = comboBox.Text;

                    var itemsAsync = await connector.GetItemsAsync(filter);
                    var items = itemsAsync?.ToArray();
                    if (items?.Any() ?? false)
                    {
                        comboBox.Items.Clear();
                        comboBox.Items.AddRange(items);

                        comboBox.DroppedDown = true;
                        comboBox.IntegralHeight = true;
                        comboBox.SelectedIndex = -1;
                        comboBox.Text = filter;
                        comboBox.SelectionStart = filter.Length;
                        comboBox.SelectionLength = 0;
                    }
                }
            }
        }

        private void _parents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (string.IsNullOrEmpty(_parents.Text))
                {
                    _parents.DroppedDown = false;
                }
                else
                {
                    e.SuppressKeyPress = true;
                    _parents.Text = null;
                    OnComboBoxTextUpdate(_parents, null);
                }
            }
        }

        private void _parents_SelectedIndexChanged(object sender, EventArgs e)
        {
            var connector = DevOpsManager.GetConnector(_model);
            if (connector != null && _parents.SelectedItem is IDevOpsItemInfo item)
            {
                connector.MasterParent = item;
                DevOpsManager.UpdateConfig(_model);
                _parentItemType.Text = item.WorkItemType;
            }
        }

        private async void _itemTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var connector = DevOpsManager.GetConnector(_model);
            if (connector != null && _itemTypes.SelectedItem is string itemType)
            {
                try
                {
                    _loading = true;
                    _gridStates.PrimaryGrid.Rows.Clear();

                    connector.WorkItemType = itemType;
                    DevOpsManager.UpdateConfig(_model);
                    var itemStatesAsync = await connector.GetWorkItemStatesAsync();
                    var itemStates = itemStatesAsync?.ToArray();
                    if (itemStates?.Any() ?? false)
                    {
                        var mappings = connector.WorkItemStateMappings?.ToArray();

                        foreach (var state in itemStates)
                        {
                            var value = mappings?
                                .Where(x => string.CompareOrdinal(x.Key, state) == 0)?
                                .Select(x => x.Value)
                                .FirstOrDefault() ?? WorkItemStatus.Unknown;

                            var row = new GridRow(state, value.GetEnumLabel())
                            {
                                Tag = state
                            };
                            _gridStates.PrimaryGrid.Rows.Add(row);
                            row.Cells["KnownState"].PropertyChanged += KnownStateChanged;
                        }
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private void KnownStateChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_loading && sender is GridCell cell)
            {
                try
                {
                    _loading = true;
                    if (cell.GridRow.Tag is string devOpsState)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "KnownState":
                                var connector = DevOpsManager.GetConnector(_model);
                                if (connector != null && cell.Value is string knownStateString)
                                {
                                    var knownState = knownStateString.GetEnumValue<WorkItemStatus>();
                                    connector.SetWorkItemStateMapping(devOpsState, knownState);
                                    DevOpsManager.UpdateConfig(_model);
                                    if (knownState == WorkItemStatus.Unknown)
                                    {
                                        cell.Value = connector.WorkItemStateMappings?
                                            .Where(x => string.CompareOrdinal(x.Key, devOpsState) == 0)?
                                            .Select(x => x.Value)
                                            .FirstOrDefault() ?? WorkItemStatus.Unknown;
                                    }
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

        private async void FieldButtonClick(object sender, EventArgs e)
        {
            if (sender is GridButtonXEditControl bc &&
                bc.EditorCell is GridCell cell &&
                cell.GridRow is GridRow row &&
                row.Tag is IDevOpsField field &&
                cell.Value is IdentityField identityField)
            {
                var pair = await ManageField(field, identityField);
                if (pair.Key != null && pair.Value != null)
                {
                    row.Cells["Field"].Value = pair.Key.Label;
                    cell.Value = pair.Value;
                    row.Tag = pair.Key;
                }
            }
        }

        private async void _add_Click(object sender, EventArgs e)
        {
            var pair = await ManageField(null, null);
            if (pair.Key != null && pair.Value != null)
            {
                var row = new GridRow(pair.Key.Label, pair.Value)
                {
                    Tag = pair.Key
                };
                _gridFields.PrimaryGrid.Rows.Add(row);
            }
        }

        private void _remove_Click(object sender, EventArgs e)
        {
            var selectedRow = _gridFields.GetSelectedCells()?
                .OfType<GridCell>()
                .Select(x => x.GridRow)
                .FirstOrDefault();
            if (selectedRow.Tag is IDevOpsField field)
            {
                if (MessageBox.Show($"Are you sure you want to remove field '{field.Label}'?",
                    "Remove field", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    var connector = DevOpsManager.GetConnector(_model);
                    if (connector != null)
                    {
                        connector.SetWorkItemFieldMapping(field, null);
                        DevOpsManager.UpdateConfig(_model);
                        _gridFields.PrimaryGrid.Rows.Remove(selectedRow);
                    }
                }
            }
        }

        private async Task<KeyValuePair<IDevOpsField, IdentityField>> ManageField(IDevOpsField field, IdentityField identityField)
        {
            KeyValuePair<IDevOpsField, IdentityField> result = new KeyValuePair<IDevOpsField, IdentityField>(null, null);

            var connector = DevOpsManager.GetConnector(_model);
            if (connector != null)
            {
                var fieldsAsync = await connector.GetWorkItemDevOpsFieldsAsync();
                var fields = fieldsAsync?.ToArray();
                if (fields?.Any() ?? false)
                {
                    var dialog = new FieldAssociationDialog();
                    dialog.Initialize(_model, fields);
                    if (field != null && identityField != null)
                        dialog.SetField(field, identityField);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        field = dialog.Field;
                        identityField = dialog.IdentityField;
                        if (field != null && identityField != null)
                        {
                            connector.SetWorkItemFieldMapping(field, identityField);
                            DevOpsManager.UpdateConfig(_model);
                            result = new KeyValuePair<IDevOpsField, IdentityField>(field, identityField);
                        }
                    }
                }
            }

            return result;
        }
    }
}
