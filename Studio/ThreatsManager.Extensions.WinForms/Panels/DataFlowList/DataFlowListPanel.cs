using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.DataFlowList
{
    public partial class DataFlowListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, 
        IDesktopAlertAwareExtension, IPanelOpenerExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private GridCell _lastMouseOverCell;

        public DataFlowListPanel()
        {
            InitializeComponent();

            _properties.OpenDiagram += OpenDiagram;

            InitializeGrid();
 
            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<DataFlowListFilter>().ToArray());
            _specialFilter.SelectedIndex = 0;
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;
        public event Action<IPanelFactory, IIdentity> OpenPanel;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;
            _model.ChildCreated += ModelChildCreated;
            _model.ChildRemoved += ModelChildRemoved;
            _model.LinkAdded += LinkAdded;
            _model.LinkRemoved += LinkRemoved;

            LoadModel();
        }

        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is IDataFlow dataFlow)
            {
                var row = GetRow(dataFlow);
                if (row != null)
                {
                    RemoveEventSubscriptions(row);
                    _grid.PrimaryGrid.Rows.Remove(row);
                    ChangeCustomActionStatus?.Invoke("RemoveDataFlow", false);
                    ChangeCustomActionStatus?.Invoke("FindDataFlow", false);
                }
            }
        }

        private void ModelChildCreated(IIdentity identity)
        {
            var filter = _filter.Text;
            var filterSpecial = EnumExtensions.GetEnumValue<DataFlowListFilter>((string)_specialFilter.SelectedItem);
            if (identity is IDataFlow dataFlow && IsSelected(dataFlow, filter, filterSpecial))
            {
                AddGridRow(dataFlow, _grid.PrimaryGrid);
            }
        }

        private void LinkAdded(ILinksContainer container, ILink link)
        {
            if (link?.DataFlow is IDataFlow dataFlow)
            {
                HandleLinkEvent(dataFlow);
            }
        }

        private void LinkRemoved(ILinksContainer container, IDataFlow dataFlow)
        {
            if (dataFlow != null)
            {
                HandleLinkEvent(dataFlow);
            }
        }

        private void HandleLinkEvent([NotNull] IDataFlow dataFlow)
        {
            var row = GetRow(dataFlow);
            if (row == null)
            {
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<DataFlowListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(dataFlow, filter, filterSpecial))
                {
                    AddGridRow(dataFlow, _grid.PrimaryGrid);
                }
            }
            else
            {
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<DataFlowListFilter>((string)_specialFilter.SelectedItem);
                if (!IsSelected(dataFlow, filter, filterSpecial))
                {
                    row.GridPanel.Rows.Remove(row);
                }
            }

        }
        #endregion

        public bool IsInitialized => _model != null;

        private void OpenDiagram(Guid diagramId)
        {
            var diagram = _model.GetDiagram(diagramId);
            var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
            if (factory != null && diagram != null)
                OpenPanel?.Invoke(factory, diagram);
        }

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

                panel.Columns.Add(new GridColumn("Source")
                {
                    HeaderText = "Source",
                    DataType = typeof(string),
                    AllowEdit = false,
                    Width = 250
                });

                panel.Columns.Add(new GridColumn("Target")
                {
                    HeaderText = "Target",
                    DataType = typeof(string),
                    AllowEdit = false,
                    Width = 250
                });

                panel.Columns.Add(new GridColumn("FlowType")
                {
                    HeaderText = "Flow Type",
                    DataType = typeof(string),
                    EditorType = typeof(EnumComboBox),
                    EditorParams = new object[] { EnumExtensions.GetEnumLabels<FlowType>() },
                    AllowEdit = true,
                    Width = 150
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
                RemoveEventSubscriptions();
                var panel = _grid.PrimaryGrid;
                panel.Rows.Clear();

                var items = _model.DataFlows?
                    .OrderBy(x => x.Name)
                    .ToArray();
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<DataFlowListFilter>((string)_specialFilter.SelectedItem);

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

        private void AddGridRow([NotNull] IDataFlow dataFlow, [NotNull] GridPanel panel)
        {
            var row = new GridRow(
                dataFlow.Name,
                dataFlow.Source?.Name ?? string.Empty,
                dataFlow.Target?.Name ?? string.Empty,
                dataFlow.FlowType.GetEnumLabel());
            ((INotifyPropertyChanged)dataFlow).PropertyChanged += OnFlowPropertyChanged;
            if (dataFlow.Source != null)
            {
                ((INotifyPropertyChanged) dataFlow.Source).PropertyChanged += OnEntityPropertyChanged;
                dataFlow.Source.ImageChanged += OnImageChanged;
            }

            if (dataFlow.Target != null)
            {
                ((INotifyPropertyChanged) dataFlow.Target).PropertyChanged += OnEntityPropertyChanged;
                dataFlow.Target.ImageChanged += OnImageChanged;
            }

            row.Tag = dataFlow;
            row.Cells[0].CellStyles.Default.Image = dataFlow.GetImage(ImageSize.Small);
            row.Cells[1].CellStyles.Default.Image = dataFlow.Source?.GetImage(ImageSize.Small);
            row.Cells[2].CellStyles.Default.Image = dataFlow.Target?.GetImage(ImageSize.Small);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnPropertyChanged;
            AddSuperTooltipProvider(dataFlow, row.Cells[0]);
            AddSuperTooltipProvider(dataFlow.Source, row.Cells[1]);
            AddSuperTooltipProvider(dataFlow.Target, row.Cells[2]);

            panel.Rows.Add(row);
        }

        private void RemoveEventSubscriptions()
        {
            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
                RemoveEventSubscriptions(row);
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IDataFlow dataFlow)
            {
                ((INotifyPropertyChanged)dataFlow).PropertyChanged -= OnFlowPropertyChanged;
                if (dataFlow.Source != null)
                {
                    ((INotifyPropertyChanged) dataFlow.Source).PropertyChanged -= OnEntityPropertyChanged;
                    dataFlow.Source.ImageChanged -= OnImageChanged;
                }
                if (dataFlow.Target != null)
                {
                    ((INotifyPropertyChanged) dataFlow.Target).PropertyChanged -= OnEntityPropertyChanged;
                    dataFlow.Target.ImageChanged -= OnImageChanged;
                }
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnPropertyChanged;
                RemoveSuperTooltipProvider(row.Cells[0]);
                RemoveSuperTooltipProvider(row.Cells[1]);
                RemoveSuperTooltipProvider(row.Cells[2]);
            }
        }

        private void OnFlowPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IDataFlow dataFlow)
            {
                var row = GetRow(dataFlow);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = dataFlow.Name;
                            AddSuperTooltipProvider(dataFlow, row["Name"]);
                            break;
                        case "FlowType":
                            row["FlowType"].Value = dataFlow.FlowType.GetEnumLabel();
                            break;
                    }
                }
            }
        }

        private void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IEntity entity)
            {
                var sourceRows = GetRowsForSource(entity)?.ToArray();
                if (sourceRows?.Any() ?? false)
                {
                    foreach (var row in sourceRows)
                    {
                        switch (e.PropertyName)
                        {
                            case "Name":
                                row["Source"].Value = entity.Name;
                                AddSuperTooltipProvider(entity, row["Source"]);
                                break;
                        }
                    }
                }

                var targetRows = GetRowsForTarget(entity)?.ToArray();
                if (targetRows?.Any() ?? false)
                {
                    foreach (var row in targetRows)
                    {
                        switch (e.PropertyName)
                        {
                            case "Name":
                                row["Target"].Value = entity.Name;
                                AddSuperTooltipProvider(entity, row["Target"]);
                                break;
                        }
                    }
                }
            }
        }

        private void OnImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            var sourceRows = GetRowsForSource(entity)?.ToArray();
            if (sourceRows?.Any() ?? false)
            {
                foreach (var row in sourceRows)
                {
                    row["Source"].CellStyles.Default.Image = entity.GetImage(ImageSize.Small);
                }
            }

            var targetRows = GetRowsForTarget(entity)?.ToArray();
            if (targetRows?.Any() ?? false)
            {
                foreach (var row in targetRows)
                {
                    row["Target"].CellStyles.Default.Image = entity.GetImage(ImageSize.Small);
                }
            }
        }

        private GridRow GetRow([NotNull] IDataFlow dataFlow)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == dataFlow)
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

        private IEnumerable<GridRow> GetRowsForSource([NotNull] IEntity source)
        {
            List<GridRow> result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag is IDataFlow dataFlow && dataFlow.SourceId == source.Id)
                {
                    if (result == null)
                        result = new List<GridRow>();
                    result.Add(row);
                }
            }

            return result;
        }

        private IEnumerable<GridRow> GetRowsForTarget([NotNull] IEntity target)
        {
            List<GridRow> result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag is IDataFlow dataFlow && dataFlow.TargetId == target.Id)
                {
                    if (result == null)
                        result = new List<GridRow>();
                    result.Add(row);
                }
            }

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
                    if (row.Tag is IDataFlow dataFlow)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                dataFlow.Name = (string) cell.Value;
                                break;
                            case "FlowType":
                                dataFlow.FlowType = ((string)cell.Value).GetEnumValue<FlowType>();
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

        private bool IsSelected([NotNull] IDataFlow item, string filter, DataFlowListFilter filterSpecial)
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
                    case DataFlowListFilter.NoDiagram:
                        result = _model.Diagrams?.All(x => x.Links?.All(y => y.AssociatedId != item.Id) ?? true) ?? true;
                        break;
                    case DataFlowListFilter.NoThreatEvents:
                        result = !(item.ThreatEvents?.Any() ?? false);
                        break;
                    case DataFlowListFilter.MissingMitigations:
                        result = item.ThreatEvents?.Any(x => !(x.Mitigations?.Any() ?? false)) ?? false;
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

                if (row?.Tag != null)
                {
                    MenuDefinition.UpdateVisibility(_contextMenu, row.Tag);
                    _contextMenu?.Show(_grid.PointToScreen(e.Location));
                }
            }
        }

        private void ShowCurrentRow()
        {
            if (_currentRow?.Tag is IDataFlow dataFlow)
            {
                _properties.Item = dataFlow;
                ChangeCustomActionStatus?.Invoke("RemoveDataFlow", true);
                ChangeCustomActionStatus?.Invoke("FindDataFlow", true);
            }
            else
            {
                ChangeCustomActionStatus?.Invoke("RemoveDataFlow", false);
                ChangeCustomActionStatus?.Invoke("FindDataFlow", false);
            }
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
        
        #region Tooltip Management.
        private void AddSuperTooltipProvider([NotNull] IIdentity identity, [NotNull] GridCell cell)
        {
            var provider = new GridCellSuperTooltipProvider(cell);
            cell.Tag = provider;
            _superTooltip.SetSuperTooltip(provider, _model.GetSuperTooltipInfo(identity));
        }
        
        private void RemoveSuperTooltipProvider([NotNull] GridCell cell)
        {
            if (cell.Tag is GridCellSuperTooltipProvider provider)
            {
                _superTooltip.SetSuperTooltip(provider, null);
            }
        }

        private void _tooltipTimer_Tick(object sender, EventArgs e)
        {
            _tooltipTimer.Stop();
            ShowCellTooltip();
        }

        private void ShowCellTooltip()
        {
            if (_lastMouseOverCell?.Tag is GridCellSuperTooltipProvider provider)
            {
                provider.Show();
            }
        }

        private void HideTooltip()
        {
            if (_lastMouseOverCell?.Tag is GridCellSuperTooltipProvider provider)
            {
                provider.Hide();
                _lastMouseOverCell = null;
            }
        }

        private void _grid_CellMouseMove(object sender, GridCellMouseEventArgs e)
        {
            if(e.GridCell != _lastMouseOverCell && e.GridCell.Tag is GridCellSuperTooltipProvider provider)
            {
                //HideTooltip();
                _lastMouseOverCell = e.GridCell;
                _tooltipTimer.Start();
            }
        }

        private void _grid_CellMouseDown(object sender, GridCellMouseEventArgs e)
        {
            HideTooltip();
        }

        private void _grid_CellMouseLeave(object sender, GridCellEventArgs e)
        {
            //HideTooltip();
            _tooltipTimer.Stop();
        }

        private void _superTooltip_MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
        {
            if (Guid.TryParse(e.HRef, out var id))
                OpenDiagram(id);
        }
        #endregion

        public void SetExecutionMode(ExecutionMode mode)
        {
            _properties.SetExecutionMode(mode);
        }
    }
}
