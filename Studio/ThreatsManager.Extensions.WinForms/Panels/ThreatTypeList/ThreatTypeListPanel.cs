using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ThreatTypeList
{
    public partial class ThreatTypeListPanel : UserControl, IShowThreatModelPanel<Form>, ICustomRibbonExtension, 
        IInitializableObject, IContextAwareExtension, IDesktopAlertAwareExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;

        public ThreatTypeListPanel()
        {
            InitializeComponent();

            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<ThreatTypeListFilter>().ToArray());
            _specialFilter.SelectedIndex = 0;
        }

        public event Action<string> ShowMessage;
        
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

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
            var filter = _filter.Text;
            var filterSpecial = EnumExtensions.GetEnumValue<ThreatTypeListFilter>((string)_specialFilter.SelectedItem);
            if (identity is IThreatType threatType && IsSelected(threatType, filter, filterSpecial))
            {
                AddGridRow(threatType, _grid.PrimaryGrid);
            }
        }
        #endregion

        public bool IsInitialized => _model != null;

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
                GridTextBoxDropDownEditControl ddc = panel.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }
 
                panel.Columns.Add(new GridColumn("Severity")
                {
                    HeaderText = "Severity",
                    DataType = typeof(ISeverity),
                    EditorType = typeof(SeverityComboBox),
                    EditorParams = new object[] { _model.Severities?.Where(x => x.Visible) },
                    AllowEdit = true,
                    Width = 75
                });
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
                var filterSpecial = EnumExtensions.GetEnumValue<ThreatTypeListFilter>((string)_specialFilter.SelectedItem);

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (IsSelected(item, filter, filterSpecial))
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
            var row = new GridRow(
                threatType.Name,
                threatType.Severity);
            ((INotifyPropertyChanged) threatType).PropertyChanged += OnThreatTypePropertyChanged;
            row.Tag = threatType;
            UpdateMitigationLevel(threatType, row);
            panel.Rows.Add(row);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnThreatTypeCellChanged;

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
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnThreatTypeCellChanged;

                threatType.ThreatTypeMitigationAdded -= OnThreatTypeMitigationAdded;
                threatType.ThreatTypeMitigationRemoved -= OnThreatTypeMitigationRemoved;

                var panel = row.Rows.OfType<GridPanel>().FirstOrDefault();
                var children = panel?.Rows.OfType<GridRow>().ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                        RemoveEventSubscriptions(child);
                }
            }
            else if (row?.Tag is IThreatTypeMitigation mitigation)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnMitigationCellChanged;

                ((INotifyPropertyChanged) mitigation).PropertyChanged -= OnMitigationPropertyChanged;
                ((INotifyPropertyChanged) mitigation.Mitigation).PropertyChanged -= OnMitigationPropertyChanged;
            }
        }

        [Dispatched]
        private void OnThreatTypeMitigationAdded(IThreatTypeMitigationsContainer container, IThreatTypeMitigation mitigation)
        {
            if (container is IThreatType threatType)
            {
                var filterSpecial = EnumExtensions.GetEnumValue<ThreatTypeListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(threatType, _filter.Text, filterSpecial))
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
                            case "Severity":
                                if (cell.Value is ISeverity severity)
                                    threatType.Severity = severity;
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
                        case "Severity":
                            row["Severity"].Value = threatType.Severity;
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

                result.Columns.Add(new GridColumn("ControlType")
                {
                    HeaderText = "Control Type",
                    DataType = typeof(string),
                    EditorType = typeof(EnumComboBox),
                    EditorParams = new object[] { EnumExtensions.GetEnumLabels<SecurityControlType>() },
                    AllowEdit = false,
                    Width = 75
                });

                result.Columns.Add(new GridColumn("Strength")
                {
                    HeaderText = "Strength",
                    DataType = typeof(IStrength),
                    EditorType = typeof(StrengthComboBox),
                    EditorParams = new object[] { _model.Strengths?.Where(x => x.Visible) },
                    AllowEdit = true,
                    Width = 75
                });

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

        private void AddGridRow([NotNull] IThreatTypeMitigation mitigation, [NotNull] GridPanel panel)
        {
            GridRow row = new GridRow(
                mitigation.Mitigation.Name,
                mitigation.Mitigation.ControlType.ToString(),
                mitigation.Strength.ToString())
            {
                Tag = mitigation
            };
            row.Cells[0].CellStyles.Default.Image = mitigation.Mitigation.GetImage(ImageSize.Small);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnMitigationCellChanged;
            panel.Rows.Add(row);

            ((INotifyPropertyChanged) mitigation).PropertyChanged += OnMitigationPropertyChanged;
            ((INotifyPropertyChanged) mitigation.Mitigation).PropertyChanged += OnMitigationPropertyChanged;
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
                        case "ControlType":
                            row["ControlType"].Value = mitigation.ControlType.ToString();
                            break;
                    }
                }
            }

            if (sender is IThreatTypeMitigation ttMitigation)
            {
                var row = GetRow(ttMitigation);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Strength":
                            row["Strength"].Value = ttMitigation.Strength;
                            break;
                    }
                }
            }
        }

        private void OnMitigationCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
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
                            case "Strength":
                                if (cell.Value is IStrength strength)
                                {
                                    mitigation.Strength = strength;
                                    if (cell.GridRow.GridPanel.Parent is GridRow row)
                                        UpdateMitigationLevel(mitigation.ThreatType, row);
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
                        threatTypeMitigation == mitigation);
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
                    var entity = row.Tag as IThreatType;
                    if (entity != null)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                entity.Name = (string) cell.Value;
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

        private bool IsSelected([NotNull] IThreatType item, string filter, ThreatTypeListFilter filterSpecial)
        {
            bool result;

            var mitigations = item.Mitigations?.ToArray();

            if (string.IsNullOrWhiteSpace(filter))
                result = true;
            else
            {
                result = item.Filter(filter);

                if (!result && (mitigations?.Any() ?? false))
                {
                    foreach (var mitigation in mitigations)
                    {
                        result = mitigation.Mitigation?.Filter(filter) ?? false;
                        if (result)
                            break;
                    }
                }
            }

            if (result)
            {
                switch (filterSpecial)
                {
                    case ThreatTypeListFilter.NoMitigations:
                        result = !(item.Mitigations?.Any() ?? false);
                        break;
                    case ThreatTypeListFilter.NoThreatEvents:
                        result = !((_model.ThreatEvents?.Any(x => x.ThreatTypeId == item.Id) ?? false) ||
                            (_model.Entities?.Any(x => x.ThreatEvents?.Any(y => y.ThreatTypeId == item.Id) ?? false ) ?? false) ||
                            (_model.DataFlows?.Any(x => x.ThreatEvents?.Any(y => y.ThreatTypeId == item.Id) ?? false) ?? false));
                        break;
                }
            }

            return result;
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

            ChangeCustomActionStatus?.Invoke("RemoveThreatType", _currentRow.Tag is IThreatType);
            ChangeCustomActionStatus?.Invoke("AddMitigation", _currentRow.Tag is IThreatType);
            ChangeCustomActionStatus?.Invoke("RemoveMitigation", _currentRow.Tag is IThreatTypeMitigation);
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

        public void SetExecutionMode(ExecutionMode mode)
        {
            _properties.SetExecutionMode(mode);
        }
    }
}
