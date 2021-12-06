using System;
using System.Collections.Generic;
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

namespace ThreatsManager.Extensions.Panels.KnownMitigationList
{
    public partial class KnownMitigationListPanel : UserControl, IShowThreatModelPanel<Form>, ICustomRibbonExtension, 
        IInitializableObject, IContextAwareExtension, IDesktopAlertAwareExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;

        public KnownMitigationListPanel()
        {
            InitializeComponent();
 
            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<MitigationListFilter>().ToArray());
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

            var threatTypes = _model.ThreatTypes?.ToArray();
            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    threatType.ThreatTypeMitigationAdded += ThreatTypeMitigationAdded;
                    threatType.ThreatTypeMitigationRemoved += ThreatTypeMitigationRemoved;
                }
            }

            _model.ChildCreated += ModelChildCreated;
            _model.ChildRemoved += ModelChildRemoved;

            InitializeGrid();
            LoadModel();
        }

        [Dispatched]
        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is IMitigation mitigation)
            {
                var row = GetRow(mitigation);
                if (row != null)
                {
                    RemoveEventSubscriptions(row);
                    _grid.PrimaryGrid.Rows.Remove(row);
                }
            }

            if (identity is IThreatType threatType)
            {
                threatType.ThreatTypeMitigationAdded -= ThreatTypeMitigationAdded;
                threatType.ThreatTypeMitigationRemoved -= ThreatTypeMitigationRemoved;
            }
        }

        [Dispatched]
        private void ModelChildCreated(IIdentity identity)
        {
            var filter = _filter.Text;
            var filterSpecial = EnumExtensions.GetEnumValue<MitigationListFilter>((string)_specialFilter.SelectedItem);
            if (identity is IMitigation mitigation && IsSelected(mitigation, filter, filterSpecial))
            {
                AddGridRow(mitigation, _grid.PrimaryGrid);
            }
 
            if (identity is IThreatType threatType)
            {
                threatType.ThreatTypeMitigationAdded += ThreatTypeMitigationAdded;
                threatType.ThreatTypeMitigationRemoved += ThreatTypeMitigationRemoved;
            }
        }

        [Dispatched]
        private void ThreatTypeMitigationAdded([NotNull] IThreatTypeMitigationsContainer container, [NotNull] IThreatTypeMitigation threatTypeMitigation)
        {
            var row = GetRow(threatTypeMitigation);
            if (row == null)
            {
                var mitigationRow = GetRow(threatTypeMitigation.Mitigation);
                if (mitigationRow != null)
                {
                    var panel = mitigationRow.Rows.OfType<GridPanel>()
                        .FirstOrDefault(x => string.CompareOrdinal("ThreatTypes", x.Name) == 0);
                    AddGridRow(threatTypeMitigation, panel);
                }
            }
        }

        [Dispatched]
        private void ThreatTypeMitigationRemoved([NotNull] IThreatTypeMitigationsContainer container, [NotNull] IThreatTypeMitigation threatTypeMitigation)
        {
            var row = GetRow(threatTypeMitigation);
            if (row != null)
            {
                RemoveEventSubscriptions(row);
                row.GridPanel.Rows.Remove(row);
            }
        }
        #endregion

        public bool IsInitialized => _model != null;

        #region Known Mitigation level.
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
                    HeaderText = "Name",
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
  
                panel.Columns.Add(new GridColumn("ControlType")
                {
                    HeaderText = "Control Type",
                    DataType = typeof(string),
                    EditorType = typeof(EnumComboBox),
                    EditorParams = new object[] { EnumExtensions.GetEnumLabels<SecurityControlType>() },
                    AllowEdit = true,
                    Width = 75
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
                var panel = _grid.PrimaryGrid;
                panel.Rows.Clear();

                var items = _model.Mitigations?
                    .OrderBy(x => x.Name)
                    .ToArray();
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<MitigationListFilter>((string)_specialFilter.SelectedItem);

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

        private void AddGridRow([NotNull] IMitigation mitigation, [NotNull] GridPanel panel)
        {
            var row = new GridRow(
                mitigation.Name,
                mitigation.ControlType);
            ((INotifyPropertyChanged) mitigation).PropertyChanged += OnEntityPropertyChanged;
            row.Tag = mitigation;
            row.Cells[0].CellStyles.Default.Image = mitigation.GetImage(ImageSize.Small);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnPropertyChanged;
            panel.Rows.Add(row);

            var subPanel = CreateThreatTypesPanel(mitigation);
            if (subPanel != null)
                row.Rows.Add(subPanel);
        }
        
        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IMitigation mitigation)
            {
                ((INotifyPropertyChanged) mitigation).PropertyChanged -= OnEntityPropertyChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnPropertyChanged;

                var panel = row.Rows.OfType<GridPanel>().FirstOrDefault();
                var children = panel?.Rows.OfType<GridRow>().ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                        RemoveEventSubscriptions(child);
                }
            } else if (row?.Tag is IThreatTypeMitigation ttm)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnThreatTypeCellChanged;
                ((INotifyPropertyChanged) ttm).PropertyChanged -= OnThreatTypeMitigationPropertyChanged;
                ((INotifyPropertyChanged) ttm.ThreatType).PropertyChanged -= OnThreatTypePropertyChanged;
            }
        }

        [Dispatched]
        private void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs e)
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
        }
        #endregion

        #region Threat Type level.
        private GridPanel CreateThreatTypesPanel([NotNull] IMitigation mitigation)
        {
            GridPanel result = null;

            if (!string.IsNullOrWhiteSpace(mitigation.Name))
            {
                result = new GridPanel
                {
                    Name = "ThreatTypes",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowTreeButtons = false,
                    ShowTreeLines = false,
                    ShowRowHeaders = false,
                    InitialSelection = RelativeSelection.None
                };

                result.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Threat Type Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    AllowEdit = false
                });
 
                result.Columns.Add(new GridColumn("Severity")
                {
                    HeaderText = "Severity",
                    DataType = typeof(ISeverity),
                    EditorType = typeof(SeverityComboBox),
                    EditorParams = new object[] { _model.Severities?.Where(x => x.Visible) },
                    AllowEdit = true,
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

                var threatTypes = _model.ThreatTypes?
                    .Where(x => x.Mitigations?.Any(y => y.MitigationId == mitigation.Id) ?? false)
                    .Select(x => x.Mitigations.First(y => y.MitigationId == mitigation.Id))
                    .OrderBy(x => x.ThreatType.Name)
                    .ToArray();
                if (threatTypes?.Any() ?? false)
                {
                    foreach (var threatType in threatTypes)
                    {
                        AddGridRow(threatType, result);
                    }
                }
            }

            return result;
        }

        private void AddGridRow([NotNull] IThreatTypeMitigation ttm, [NotNull] GridPanel panel)
        {
            GridRow row = new GridRow(
                ttm.ThreatType.Name,
                ttm.ThreatType.Severity,
                ttm.Strength)
            {
                Tag = ttm
            };
            UpdateMitigationLevel(ttm.ThreatType, row);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnThreatTypeCellChanged;
            panel.Rows.Add(row);

            ((INotifyPropertyChanged) ttm).PropertyChanged += OnThreatTypeMitigationPropertyChanged;
            ((INotifyPropertyChanged) ttm.ThreatType).PropertyChanged += OnThreatTypePropertyChanged;
        }

        private void OnThreatTypePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatType threatType)
            {
                var rows = GetRows(threatType)?.ToArray();
                if (rows?.Any() ?? false)
                {
                    foreach (var row in rows)
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
        }

        private void OnThreatTypeMitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatTypeMitigation threatTypeMitigation)
            {
                var row = GetRow(threatTypeMitigation);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Strength":
                            row["Strength"].Value = threatTypeMitigation.Strength;
                            break;
                    }
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
                    if (cell.GridRow.Tag is IThreatTypeMitigation threatTypeMitigation)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Severity":
                                if (cell.Value is ISeverity severity)
                                    threatTypeMitigation.ThreatType.Severity = severity;
                                break;
                            case "Strength":
                                if (cell.Value is IStrength strength)
                                {
                                    threatTypeMitigation.Strength = strength;
                                    UpdateMitigationLevel(threatTypeMitigation.ThreatType, cell.GridRow);
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

        private IEnumerable<GridRow> GetRows([NotNull] IThreatType threatType)
        {
            List<GridRow> result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var panel = row.Rows.OfType<GridPanel>()
                    .FirstOrDefault(x => string.CompareOrdinal("ThreatTypes", x.Name) == 0);
                var current = panel?.Rows.OfType<GridRow>().FirstOrDefault(x =>
                    (x.Tag is IThreatTypeMitigation ttm) && (ttm.ThreatTypeId == threatType.Id));
                if (current != null)
                {
                    if (result == null)
                        result = new List<GridRow>();
                    result.Add(current);
                }
            }

            return result;
        }

        private GridRow GetRow([NotNull] IThreatTypeMitigation threatTypeMitigation)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var panel = row.Rows.OfType<GridPanel>()
                    .FirstOrDefault(x => string.CompareOrdinal("ThreatTypes", x.Name) == 0);
                var current = panel?.Rows.OfType<GridRow>().FirstOrDefault(x =>
                    (x.Tag is IThreatTypeMitigation ttm) && (ttm == threatTypeMitigation));
                if (current != null)
                {
                    result = current;
                    break;
                }
            }

            return result;
        }
        #endregion

        #region Auxiliary private members.
        private GridRow GetRow([NotNull] IMitigation mitigation)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == mitigation)
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
                    if (row.Tag is IMitigation entity)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                entity.Name = (string) cell.Value;
                                break;
                            case "ControlType":
                                entity.ControlType = ((string) cell.Value).GetEnumValue<SecurityControlType>();
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

        private bool IsSelected([NotNull] IMitigation item, string filter, MitigationListFilter filterSpecial)
        {
            bool result;

            if (string.IsNullOrWhiteSpace(filter))
                result = true;
            else
            {
                result = (!string.IsNullOrWhiteSpace(item.Name) &&
                              item.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                             (!string.IsNullOrWhiteSpace(item.Description) &&
                              item.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);
                if (!result && (item.Properties?.Any() ?? false))
                {
                    var properties = item.Properties.ToArray();
                    foreach (var property in properties)
                    {
                        var stringValue = property.StringValue;
                        if ((!string.IsNullOrWhiteSpace(stringValue) &&
                             stringValue.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            
            if (result)
            {
                switch (filterSpecial)
                {
                    case MitigationListFilter.NoThreatTypes:
                        result = !(_model.ThreatTypes?.Any(x => x.Mitigations?.Any(y => y.MitigationId == item.Id) ?? false) ?? false);
                        break;
                    case MitigationListFilter.NoThreatEvents:
                        result = !((_model.ThreatEvents?.Any(x => x.Mitigations?.Any(y => y.MitigationId == item.Id) ?? false) ?? false) ||
                            (_model.Entities?.Any(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.MitigationId == item.Id) ?? false) ?? false) ?? false) ||
                            (_model.DataFlows?.Any(x => x.ThreatEvents?.Any(y => y.Mitigations?.Any(z => z.MitigationId == item.Id) ?? false) ?? false) ?? false));
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

                if (row?.Tag is IMitigation)
                {
                    MenuDefinition.UpdateVisibility(_mitigationMenu, row.Tag);
                    _mitigationMenu?.Show(_grid.PointToScreen(e.Location));
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

            ChangeCustomActionStatus?.Invoke("RemoveMitigation", _currentRow.Tag is IMitigation);
            ChangeCustomActionStatus?.Invoke("AddThreatType", _currentRow.Tag is IMitigation);
            ChangeCustomActionStatus?.Invoke("RemoveThreatType", !(_currentRow.Tag is IMitigation));
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
