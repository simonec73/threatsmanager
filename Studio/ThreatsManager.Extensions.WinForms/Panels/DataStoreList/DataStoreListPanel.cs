using System;
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

namespace ThreatsManager.Extensions.Panels.DataStoreList
{
    public partial class DataStoreListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, 
        IDesktopAlertAwareExtension, IPanelOpenerExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private GridCell _lastMouseOverCell;

        public DataStoreListPanel()
        {
            InitializeComponent();

            InitializeGrid();
 
            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<DataStoreListFilter>().ToArray());
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
            _model.EntityShapeAdded += EntityShapeAdded;
            _model.EntityShapeRemoved += EntityShapeRemoved;

            LoadModel();
        }

        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is IDataStore dataStore)
            {
                var row = GetRow(dataStore);
                if (row != null)
                {
                    RemoveEventSubscriptions(row);
                    _grid.PrimaryGrid.Rows.Remove(row);
                    ChangeCustomActionStatus?.Invoke("RemoveDataStore", false);
                    ChangeCustomActionStatus?.Invoke("FindDataStore", false);
                }
            }
        }

        private void ModelChildCreated(IIdentity identity)
        {
            var filter = _filter.Text;
            var filterSpecial = EnumExtensions.GetEnumValue<DataStoreListFilter>((string)_specialFilter.SelectedItem);
            if (identity is IDataStore dataStore && IsSelected(dataStore, filter, filterSpecial))
            {
                AddGridRow(dataStore, _grid.PrimaryGrid);
            }
        }

        private void EntityShapeAdded(IEntityShapesContainer container, IEntityShape entityShape)
        {
            if (entityShape?.Identity is IDataStore dataStore)
            {
                HandleEntityShapeEvent(dataStore);
            }
        }

        private void EntityShapeRemoved(IEntityShapesContainer container, IEntity entity)
        {
            if (entity is IDataStore dataStore)
            {
                HandleEntityShapeEvent(dataStore);
            }
        }

        private void HandleEntityShapeEvent([NotNull] IDataStore dataStore)
        {
            var row = GetRow(dataStore);
            if (row == null)
            {
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<DataStoreListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(dataStore, filter, filterSpecial))
                {
                    AddGridRow(dataStore, _grid.PrimaryGrid);
                }
            }
            else
            {
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<DataStoreListFilter>((string)_specialFilter.SelectedItem);
                if (!IsSelected(dataStore, filter, filterSpecial))
                {
                    row.GridPanel.Rows.Remove(row);
                }
            }

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

                panel.Columns.Add(new GridColumn("Parent")
                {
                    HeaderText = "Parent",
                    DataType = typeof(string),
                    AllowEdit = false,
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

                var items = _model.Entities?.OfType<IDataStore>()
                    .OrderBy(x => x.Name)
                    .ToArray();
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<DataStoreListFilter>((string)_specialFilter.SelectedItem);

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

        private void AddGridRow([NotNull] IEntity entity, [NotNull] GridPanel panel)
        {
            var row = new GridRow(
                entity.Name,
                entity.Parent?.Name ?? string.Empty);
            ((INotifyPropertyChanged) entity).PropertyChanged += OnEntityPropertyChanged;
            entity.ParentChanged += OnEntityParentChanged;
            row.Tag = entity;
            row.Cells[0].CellStyles.Default.Image = entity.GetImage(ImageSize.Small);
            entity.ImageChanged += OnImageChanged;
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnPropertyChanged;
            AddSuperTooltipProvider(entity, row.Cells[0]);

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
            if (row?.Tag is IEntity entity)
            {
                ((INotifyPropertyChanged) entity).PropertyChanged -= OnEntityPropertyChanged;
                entity.ParentChanged -= OnEntityParentChanged;
                entity.ImageChanged -= OnImageChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnPropertyChanged;
                RemoveSuperTooltipProvider(row.Cells["Name"]);
            }
        }

        private void OnImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            var row = GetRow(entity);
            if (row != null)
            {
                row.Cells[0].CellStyles.Default.Image = entity.GetImage(ImageSize.Small);
            }
        }

        private void OnEntityParentChanged(IGroupElement item, IGroup oldParent, IGroup newParent)
        {
            if (item is IEntity entity)
            {
                var row = GetRow(entity);
                if (row != null)
                {
                    row["Parent"].Value = newParent?.Name ?? string.Empty;
                }
            }
        }

        private void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IEntity entity)
            {
                var row = GetRow(entity);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = entity.Name;
                            AddSuperTooltipProvider(entity, row["Name"]);
                            break;
                    }
                }
            }
        }

        private GridRow GetRow([NotNull] IEntity entity)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == entity)
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
                    if (row.Tag is IEntity entity)
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

        private bool IsSelected([NotNull] IDataStore item, string filter, DataStoreListFilter filterSpecial)
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
                    case DataStoreListFilter.NoDiagram:
                        result = _model.Diagrams?.All(x => x.GetEntityShape(item.Id) == null) ?? true;
                        break;
                    case DataStoreListFilter.NoThreatEvents:
                        result = !(item.ThreatEvents?.Any() ?? false);
                        break;
                    case DataStoreListFilter.MissingMitigations:
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
            if (_currentRow?.Tag is IDataStore entity)
            {
                _properties.Item = entity;
                ChangeCustomActionStatus?.Invoke("RemoveDataStore", true);
                ChangeCustomActionStatus?.Invoke("FindDataStore", true);
            }
            else
            {
                ChangeCustomActionStatus?.Invoke("RemoveDataStore", false);
                ChangeCustomActionStatus?.Invoke("FindDataStore", false);
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
            {
                var diagram = _model.GetDiagram(id);
                var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
                if (factory != null && diagram != null)
                    OpenPanel?.Invoke(factory, diagram);
            }
        }
        #endregion

        public void SetExecutionMode(ExecutionMode mode)
        {
            _properties.SetExecutionMode(mode);
        }
    }
}
