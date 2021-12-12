using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.AutoGenRules.Engine;
using ThreatsManager.AutoThreatGeneration.Actions;
using ThreatsManager.AutoThreatGeneration.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.AutoThreatGeneration.Panels.ThreatTypeList
{
    public partial class AutoThreatTypeListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, IDesktopAlertAwareExtension
    {
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private SelectionRule _filteringRule;

        public AutoThreatTypeListPanel()
        {
            InitializeComponent();

            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<AutoThreatTypeListFilter>().ToArray());
            _specialFilter.SelectedIndex = 0;
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;
            _model.ChildCreated += ModelChildCreated;
            _model.ChildRemoved += ModelChildRemoved;

            InitializeGrid();
            LoadModel();
        }

        [Dispatched]
        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is IThreatType threatType)
            {
                var row = GetRow(threatType);
                if (row != null)
                {
                    RemoveEventSubscriptions(row);
                    _grid.PrimaryGrid.Rows.Remove(row);
                }
            }
        }

        [Dispatched]
        private void ModelChildCreated(IIdentity identity)
        {
            if (identity is IThreatType threatType)
            {
                AddGridRow(threatType, _grid.PrimaryGrid);
            }
        }
        #endregion

        public bool IsInitialized => _model != null;

        public IActionDefinition ActionDefinition => new ActionDefinition(Id, "AutoGenRules", "Auto Gen Rules", 
            Properties.Resources.threat_types_big_gearwheel, Properties.Resources.threat_types_gearwheel);

        #region Threat Type level.
        private void InitializeGrid()
        {
            lock (_grid)
            {
                GridPanel panel = _grid.PrimaryGrid;
                panel.ShowTreeButtons = true;
                panel.ShowTreeLines = true;
                panel.AllowRowDelete = false;
                panel.AllowRowInsert = false;
                panel.AllowRowResize = true;
                panel.ShowRowDirtyMarker = false;
                panel.ShowRowHeaders = false;
                panel.InitialActiveRow = RelativeRow.None;

                panel.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Threat Type Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    EditorType = typeof(GridTextBoxDropDownEditControl),
                    AllowEdit = true
                });
                var ddc = panel.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }

                panel.Columns.Add(new GridColumn("Top")
                {
                    HeaderText = "Top",
                    DataType = typeof(bool),
                    Width = 75,
                    EditorType = typeof(GridSwitchButtonEditControl),
                    AllowEdit = true
                });
 
                panel.Columns.Add(new GridColumn("AutoGenRule")
                {
                    HeaderText = "Automatic Threat Generation Rule",
                    DataType = typeof(string),
                    Width = 200,
                    EditorType = typeof(GridButtonXEditControl)
                });
                var bc = panel.Columns["AutoGenRule"].EditControl as GridButtonXEditControl;
                if (bc != null)
                {
                    bc.Click += BcButtonClick;
                }
            }
        }

        private void BcButtonClick(object sender, EventArgs e)
        {
            if (sender is GridButtonXEditControl bc && bc.EditorCell.GridRow.Tag is IThreatType threatType)
            {
                using (var dialog = new RuleEditDialog())
                {
                    dialog.Initialize(threatType);

                    dialog.Rule = threatType.GetRule();

                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        threatType.SetRule(dialog.Rule);
                    }
                }
            }
        }

        private void LoadModel()
        {
            try
            {
                _grid.SuspendLayout();
                _loading = true;
                var panel = _grid.PrimaryGrid;
                panel.Rows.Clear();

                var items = _model.ThreatTypes?
                    .OrderBy(x => x.Name)
                    .ToArray();
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<AutoThreatTypeListFilter>((string)_specialFilter.SelectedItem);
                var filterRule = _filteringRule?.Root.ToString();

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (IsSelected(item, filter, filterSpecial, filterRule))
                        {
                            AddGridRow(item, panel);
                        }
                    }
                }
            }
            finally
            {
                _loading = false;
                _grid.ResumeLayout(true);
            }
        }

        private GridRow AddGridRow([NotNull] IThreatType threatType, [NotNull] GridPanel panel)
        {
            bool rule = HasSelectionRule(threatType);

            var row = new GridRow(
                threatType.Name,
                threatType.HasTop(),
                rule ? "Edit Rule" : "Create Rule");
            ((INotifyPropertyChanged) threatType).PropertyChanged += OnThreatTypePropertyChanged;
            threatType.PropertyValueChanged += OnThreatTypePropertyValueChanged;
            row.Tag = threatType;
            UpdateMitigationLevel(threatType, row);
            panel.Rows.Add(row);
            row.Cells[0].PropertyChanged += OnThreatTypeCellChanged;
            row.Cells[1].PropertyChanged += OnThreatTypeCellChanged;

            threatType.ThreatTypeMitigationAdded += OnThreatTypeMitigationAdded;
            threatType.ThreatTypeMitigationRemoved += OnThreatTypeMitigationRemoved;

            if (threatType.Mitigations?.Any() ?? false)
            {
                var subPanel = CreateMitigationsPanel(threatType);
                if (subPanel != null)
                    row.Rows.Add(subPanel);
            }

            return row;
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IThreatType threatType)
            {
                ((INotifyPropertyChanged) threatType).PropertyChanged -= OnThreatTypePropertyChanged;
                threatType.PropertyValueChanged -= OnThreatTypePropertyValueChanged;
                row.Cells[0].PropertyChanged -= OnThreatTypeCellChanged;

                threatType.ThreatTypeMitigationAdded -= OnThreatTypeMitigationAdded;
                threatType.ThreatTypeMitigationRemoved -= OnThreatTypeMitigationRemoved;

                var panel = row.Rows.OfType<GridPanel>().FirstOrDefault();
                var children = panel?.Rows.OfType<GridRow>().ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                        RemoveEventSubscriptions(child);
                }
            } else if (row?.Tag is IThreatTypeMitigation mitigation)
            {
                ((INotifyPropertyChanged)mitigation).PropertyChanged -= OnMitigationPropertyChanged;
                ((INotifyPropertyChanged)mitigation.Mitigation).PropertyChanged -= OnMitigationPropertyChanged;
                mitigation.PropertyValueChanged -= OnMitigationPropertyValueChanged;
            }
        }

        [Dispatched]
        private void OnThreatTypeMitigationAdded(IThreatTypeMitigationsContainer container, IThreatTypeMitigation mitigation)
        {
            if (container is IThreatType threatType)
            {
                var filterSpecial = EnumExtensions.GetEnumValue<AutoThreatTypeListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(threatType, _filter.Text, filterSpecial, _filteringRule?.Root.ToString()))
                {
                    var row = GetRow(threatType);
                    if (row == null)
                    {
                        row = AddGridRow(threatType, _grid.PrimaryGrid);
                    }

                    var scenarioPanel = row.Rows.OfType<GridPanel>()
                        .FirstOrDefault(x => string.CompareOrdinal(x.Name, "Mitigations") == 0);
                    if (scenarioPanel != null)
                    {
                        AddGridRow(mitigation, scenarioPanel);
                    }
                    else
                    {
                        scenarioPanel = CreateMitigationsPanel(threatType);
                        if (scenarioPanel != null)
                            row.Rows.Add(scenarioPanel);
                    }

                    UpdateMitigationLevel(threatType, row);
                }
            }
        }

        [Dispatched]
        private void OnThreatTypeMitigationRemoved(IThreatTypeMitigationsContainer container, IThreatTypeMitigation mitigation)
        {
            if (container is IThreatType threatType)
            {
                var row = GetRow(threatType);

                var panel = row?.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Mitigations") == 0);
                var scenarioRow = panel?.Rows.OfType<GridRow>()
                    .FirstOrDefault(x =>
                        (x.Tag is IThreatTypeMitigation typeMitigation) && typeMitigation == mitigation);
                if (scenarioRow != null)
                {
                    panel.Rows.Remove(scenarioRow);

                    if (panel.Rows.Count == 0)
                        row.Rows.Remove(panel);

                    UpdateMitigationLevel(threatType, row);
                }
            }
        }

        [Dispatched]
        private void OnThreatTypePropertyValueChanged(IPropertiesContainer container, IProperty property)
        {
            if (container is IThreatType threatType && property is IPropertyJsonSerializableObject jsonProperty &&
                jsonProperty.Value is SelectionRule selectionRule)
            {
                var row = GetRow(threatType);
                if (row != null)
                    row.Cells["AutoGenRule"].Value = selectionRule.Root != null ? "Edit Rule" : "Create Rule";
            }
        }

        private void OnThreatTypeCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_loading && sender is GridCell cell)
            {
                try
                {
                    _loading = true;
                    if (cell.GridRow.Tag is IThreatType threatType)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                threatType.Name = (string) cell.Value;
                                break;
                            case "Top":
                                threatType.SetTop((bool)(cell.Value ?? false));
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

        [Dispatched]
        private void OnThreatTypePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatType threatType)
            {
                var row = GetRow(threatType);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = threatType.Name;
                            break;
                    }
                }
            }
        }

        private GridRow GetRow([NotNull] IThreatType threatType)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == threatType)
                {
                    result = row;
                    break;
                }
            }

            return result;
        }
        #endregion

        #region Standard Mitigation level.
        private GridPanel CreateMitigationsPanel([NotNull] IThreatType threatType)
        {
            GridPanel result = null;

            if (!string.IsNullOrWhiteSpace(threatType.Name))
            {
                result = new GridPanel
                {
                    Name = "Mitigations",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowTreeButtons = false,
                    ShowTreeLines = false,
                    ShowRowHeaders = false,
                    InitialSelection = RelativeSelection.None,
                };

                result.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Mitigation Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                result.Columns.Add(new GridColumn("Top")
                {
                    HeaderText = "Top",
                    DataType = typeof(bool),
                    Width = 75,
                    EditorType = typeof(GridSwitchButtonEditControl),
                    AllowEdit = true
                });

                result.Columns.Add(new GridColumn("AutoGenRule")
                {
                    HeaderText = "Automatic Threat Generation Rule",
                    DataType = typeof(string),
                    Width = 200,
                    EditorType = typeof(GridButtonXEditControl)
                });
                var bc = result.Columns["AutoGenRule"].EditControl as GridButtonXEditControl;
                if (bc != null)
                {
                    bc.Click += BcMitigationButtonClick;
                }

                var mitigations = threatType.Mitigations?
                    .OrderBy(x => x.Mitigation.Name)
                    .ToArray();

                if (mitigations?.Any() ?? false)
                {
                    foreach (var mitigation in mitigations)
                    {
                        AddGridRow(mitigation, result);
                    }
                }
            }

            return result;
        }

        private void BcMitigationButtonClick(object sender, EventArgs e)
        {
            if (sender is GridButtonXEditControl bc && bc.EditorCell.GridRow.Tag is IThreatTypeMitigation mitigation)
            {
                using (var dialog = new MitigationRuleEditDialog())
                {
                    if (dialog.Initialize(mitigation))
                    {
                        dialog.Rule = mitigation.GetRule();

                        if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                        {
                            mitigation.SetRule(dialog.Rule);
                        }
                    }
                    else
                    {
                        ShowWarning?.Invoke(
                            "Threat Event Generation Rule for the related Threat Type has not been set.");
                    }
                }
            }
        }

        private void AddGridRow([NotNull] IThreatTypeMitigation mitigation, [NotNull] GridPanel panel)
        {
            bool rule = HasSelectionRule(mitigation);

            GridRow row = new GridRow(
                mitigation.Mitigation.Name,
                mitigation.HasTop(),
                rule ? "Edit Rule" : "Create Rule")
            {
                Tag = mitigation
            };
            panel.Rows.Add(row);
            row.Cells[1].PropertyChanged += OnMitigationCellChanged;

            ((INotifyPropertyChanged)mitigation).PropertyChanged += OnMitigationPropertyChanged;
            ((INotifyPropertyChanged)mitigation.Mitigation).PropertyChanged += OnMitigationPropertyChanged;
            mitigation.PropertyValueChanged += OnMitigationPropertyValueChanged;
            mitigation.PropertyAdded += OnMitigationPropertyValueChanged;
            mitigation.Mitigation.PropertyValueChanged += OnMitigationPropertyValueChanged;
            mitigation.Mitigation.PropertyAdded += OnMitigationPropertyValueChanged;
        }

        private void OnMitigationCellChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_loading && sender is GridCell cell)
            {
                try
                {
                    _loading = true;
                    if (cell.GridRow.Tag is IThreatTypeMitigation mitigation)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Top":
                                mitigation.SetTop((bool)(cell.Value ?? false));
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

        [Dispatched]
        private void OnMitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IMitigation mitigation)
            {
                var row = GetRow(mitigation);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = mitigation.Name;
                            break;
                    }
                }
            }
        }

        [Dispatched]
        private void OnMitigationPropertyValueChanged(IPropertiesContainer container, IProperty property)
        {
            if (property is IPropertyJsonSerializableObject jsonProperty &&
                jsonProperty.Value is SelectionRule selectionRule)
            {
                if (container is IThreatTypeMitigation ttm)
                {
                    var row = GetRow(ttm);
                    if (row != null)
                        row.Cells["AutoGenRule"].Value = selectionRule.Root != null ? "Edit Rule" : "Create Rule";
                }

                if (container is IMitigation mitigation)
                {
                    var row = GetRow(mitigation);
                    if (row != null)
                        row.Cells["AutoGenRule"].Value = selectionRule.Root != null ? "Edit Rule" : "Create Rule";
                }
            }
        }

        private GridRow GetRow([NotNull] IThreatTypeMitigation mitigation)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var panel = row.Rows.OfType<GridPanel>()
                    .FirstOrDefault(x => string.CompareOrdinal("Mitigations", x.Name) == 0);
                if (panel != null)
                {
                    result = panel.Rows.OfType<GridRow>().FirstOrDefault(x =>
                        (x.Tag is IThreatTypeMitigation threatTypeMitigation) &&
                        (threatTypeMitigation.MitigationId == mitigation.MitigationId) &&
                        (threatTypeMitigation.ThreatTypeId == mitigation.ThreatTypeId));
                    if (result != null)
                        break;
                }
            }

            return result;
        }

        private GridRow GetRow([NotNull] IMitigation mitigation)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var panel = row.Rows.OfType<GridPanel>()
                    .FirstOrDefault(x => string.CompareOrdinal("Mitigations", x.Name) == 0);
                if (panel != null)
                {
                    result = panel.Rows.OfType<GridRow>().FirstOrDefault(x =>
                        (x.Tag is IThreatTypeMitigation threatTypeMitigation) &&
                        threatTypeMitigation.MitigationId == mitigation.Id);
                    if (result != null)
                        break;
                }
            }

            return result;
        }
        #endregion

        #region Auxiliary private members.
        private GridRow GetRow(Point position)
        {
            GridRow result = null;

            GridElement item = _grid.GetElementAt(position);

            if (item is GridCell cell)
                result = cell.GridRow;

            return result;
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

        private bool IsSelected([NotNull] IThreatType item, string filter, 
            AutoThreatTypeListFilter filterSpecial, string filterRule)
        {
            bool result;

            var mitigations = item.Mitigations?.Where(x => x != null).ToArray();

            if (string.IsNullOrWhiteSpace(filter))
                result = true;
            else
            {
                result = item.Filter(filter);

                if (!result && (mitigations?.Any() ?? false))
                {
                    foreach (var mitigation in mitigations)
                    {
                        result = mitigation.Mitigation.Filter(filter);
                        if (result)
                            break;
                    }
                }
            }

            if (result)
            {
                switch (filterSpecial)
                {
                    case AutoThreatTypeListFilter.NoGenRule:
                        result = !HasSelectionRule(item);
                        break;
                    case AutoThreatTypeListFilter.NoMitigationGenRule:
                        result = !(item.Mitigations?.All(HasSelectionRule) ?? true);
                        break;
                }
            }

            if (result && _ruleFilter.Checked && !string.IsNullOrWhiteSpace(filterRule))
            {
                result = FilterRule(item, filterRule);
                if (!result && (mitigations?.Any() ?? false))
                {
                    foreach (var mitigation in mitigations)
                    {
                        result = FilterRule(mitigation.Mitigation, filterRule);
                        if (result)
                            break;
                    }
                }
            }

            return result;
        }

        private bool FilterRule([NotNull] IPropertiesContainer container, [Required] string filterRule)
        {
            var result = false;

            var rule = container.GetRule();
            if (rule != null)
            {
                var ruleText = rule.ToString();
                if (ruleText.ToLower().Contains(filterRule.ToLower()))
                {
                    result = true;
                }
            }

            return result;
        }

        private bool HasSelectionRule(IPropertiesContainer container)
        {
            var rule = container?.GetRule();
            return rule?.Root != null;
        }

        private void _filter_ButtonCustomClick(object sender, EventArgs e)
        {
            _filter.Text = null;
        }

        private void _apply_Click(object sender, EventArgs e)
        {
            LoadModel();
        }

        private void _grid_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                var row = GetRow(e.Location);

                if (row?.Tag is IThreatType)
                {
                    MenuDefinition.UpdateVisibility(_threatTypeMenu, row.Tag);
                    _threatTypeMenu?.Show(_grid.PointToScreen(e.Location));
                }
                if (row?.Tag is IThreatTypeMitigation)
                {
                    MenuDefinition.UpdateVisibility(_threatTypeMitigationMenu, row.Tag);
                    _threatTypeMitigationMenu?.Show(_grid.PointToScreen(e.Location));
                }
            }
        }

        private void ShowCurrentRow()
        {
            _properties.Item = _currentRow?.Tag;
        }

        private void _filter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                LoadModel();
            } else if (e.KeyChar == (char) Keys.Escape)
            {
                e.Handled = true;
                _filter.Text = string.Empty;
            }
        }

        private static void UpdateMitigationLevel([NotNull] IThreatType threatType, [NotNull] GridRow row)
        {
            try
            {
                switch (threatType.GetMitigationLevel())
                {
                    case MitigationLevel.NotMitigated:
                        row.Cells[0].CellStyles.Default.Image = Resources.threat_circle_small;
                        break;
                    case MitigationLevel.Partial:
                        row.Cells[0].CellStyles.Default.Image = Resources.threat_circle_orange_small;
                        break;
                    case MitigationLevel.Complete:
                        row.Cells[0].CellStyles.Default.Image = Resources.threat_circle_green_small;
                        break;
                }
            }
            catch
            {
                // Ignore
            }
        }
        #endregion

        private void _grid_CellActivated(object sender, GridCellActivatedEventArgs e)
        {
            if (!_loading)
            {
                _currentRow = e.NewActiveCell.GridRow;
                ShowCurrentRow();
            }
        }

        private void _grid_RowActivated(object sender, GridRowActivatedEventArgs e)
        {
            if (!_loading)
            {
                if (e.NewActiveRow is GridRow gridRow)
                {
                    _currentRow = gridRow;
                    ShowCurrentRow();
                }
            }
        }

        private void _ruleFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (_ruleFilter.Checked)
            {
                using (var dialog = new RuleFilterDialog())
                {
                    dialog.Initialize(_model);
                    dialog.Rule = _filteringRule;

                    if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                    {
                        _filteringRule = dialog.Rule;
                        _ruleFilter.Image = Properties.Resources.checkbox_small;
                    }
                    else
                    {
                        _ruleFilter.Checked = false;
                        _ruleFilter.Image = null;
                    }
                }
            }
            else
            {
                _ruleFilter.Image = null;
            }
        }
    }
}
