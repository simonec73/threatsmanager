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

namespace ThreatsManager.Extensions.Panels.TrustBoundaryList
{
    public partial class TrustBoundaryListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, 
        IDesktopAlertAwareExtension, IPanelOpenerExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();

        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private GridCell _lastMouseOverCell;

        public TrustBoundaryListPanel()
        {
            InitializeComponent();

            _properties.OpenDiagram += OpenDiagram;

            InitializeGrid();
 
            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<TrustBoundaryListFilter>().ToArray());
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
            _model.GroupShapeAdded += GroupShapeAdded;
            _model.GroupShapeRemoved += GroupShapeRemoved;

            LoadModel();
        }

        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is ITrustBoundary tb)
            {
                var row = GetRow(tb);
                if (row != null)
                {
                    RemoveEventSubscriptions(row);
                    _grid.PrimaryGrid.Rows.Remove(row);
                    ChangeCustomActionStatus?.Invoke("RemoveTrustBoundary", false);
                    ChangeCustomActionStatus?.Invoke("FindTrustBoundary", false);
                }
            }
        }

        private void ModelChildCreated(IIdentity identity)
        {
            var filter = _filter.Text;
            var filterSpecial = EnumExtensions.GetEnumValue<TrustBoundaryListFilter>((string)_specialFilter.SelectedItem);
            if (identity is ITrustBoundary tb && IsSelected(tb, filter, filterSpecial))
            {
                AddGridRow(tb, _grid.PrimaryGrid);
            }
        }

        private void GroupShapeAdded(IGroupShapesContainer container, IGroupShape groupShape)
        {
            if (groupShape?.Identity is ITrustBoundary trustBoundary)
            {
                HandleGroupShapeEvent(trustBoundary);
            }
        }

        private void GroupShapeRemoved(IGroupShapesContainer container, IGroup group)
        {
            if (group is ITrustBoundary trustBoundary)
            {
                HandleGroupShapeEvent(trustBoundary);
            }
        }

        private void HandleGroupShapeEvent([NotNull] ITrustBoundary trustBoundary)
        {
            var row = GetRow(trustBoundary);
            if (row == null)
            {
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<TrustBoundaryListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(trustBoundary, filter, filterSpecial))
                {
                    AddGridRow(trustBoundary, _grid.PrimaryGrid);
                }
            }
            else
            {
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<TrustBoundaryListFilter>((string)_specialFilter.SelectedItem);
                if (!IsSelected(trustBoundary, filter, filterSpecial))
                {
                    row.GridPanel.Rows.Remove(row);
                }
            }

        }
        #endregion

        public bool IsInitialized => _model != null;

        //public IActionDefinition ActionDefinition => new ActionDefinition(Id, "TrustBoundaryList", "Trust Boundary List", 
        //    Resources.trust_boundary_big, Resources.trust_boundary);

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

                var items = _model.Groups?
                    .OfType<ITrustBoundary>()
                    .OrderBy(x => x.Name)
                    .ToArray();
                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<TrustBoundaryListFilter>((string)_specialFilter.SelectedItem);

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

        private void AddGridRow([NotNull] ITrustBoundary tb, [NotNull] GridPanel panel)
        {
            var row = new GridRow(
                tb.Name);
            ((INotifyPropertyChanged)tb).PropertyChanged += OnTrustBoundaryPropertyChanged;
            row.Tag = tb;
            row.Cells[0].CellStyles.Default.Image = tb.GetImage(ImageSize.Small);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnPropertyChanged;
            AddSuperTooltipProvider(tb, row.Cells[0]);

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
            if (row?.Tag is ITrustBoundary tb)
            {
                ((INotifyPropertyChanged)tb).PropertyChanged -= OnTrustBoundaryPropertyChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnPropertyChanged;
                RemoveSuperTooltipProvider(row["Name"]);
            }
        }

        private void OnTrustBoundaryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ITrustBoundary tb)
            {
                var row = GetRow(tb);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = tb.Name;
                            AddSuperTooltipProvider(tb, row["Name"]);
                            break;
                    }
                }
            }
        }

        private GridRow GetRow([NotNull] ITrustBoundary tb)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == tb)
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
                    if (row.Tag is ITrustBoundary tb)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                tb.Name = (string) cell.Value;
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

        private bool IsSelected([NotNull] ITrustBoundary item, string filter, TrustBoundaryListFilter filterSpecial)
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
                    case TrustBoundaryListFilter.NoDiagram:
                        result = _model.Diagrams?.All(x => x.Groups?.All(y => y.AssociatedId != item.Id) ?? true) ?? true;
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
            if (_currentRow?.Tag is ITrustBoundary trustBoundary)
            {
                _properties.Item = trustBoundary;
                ChangeCustomActionStatus?.Invoke("RemoveTrustBoundary", true);
                ChangeCustomActionStatus?.Invoke("FindTrustBoundary", true);
            }
            else
            {
                ChangeCustomActionStatus?.Invoke("RemoveTrustBoundary", false);
                ChangeCustomActionStatus?.Invoke("FindTrustBoundary", false);
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
