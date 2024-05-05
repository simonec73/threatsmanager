using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ThreatActorList
{
    public partial class ThreatActorListPanel : UserControl, IShowThreatModelPanel<Form>, ICustomRibbonExtension, 
        IInitializableObject, IContextAwareExtension, IDesktopAlertAwareExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;

        public ThreatActorListPanel()
        {
            InitializeComponent();

            InitializeGrid();

            UndoRedoManager.Undone += RefreshOnUndoRedo;
            UndoRedoManager.Redone += RefreshOnUndoRedo;
        }

        public event Action<string> ShowMessage;
        
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public IIdentity ReferenceObject => null;

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;
            _model.ChildCreated += ModelChildCreated;
            _model.ChildRemoved += ModelChildRemoved;

            LoadModel();
        }

        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is IThreatActor actor)
            {
                var row = GetRow(actor);
                if (row != null)
                {
                    RemoveEventSubscriptions(row);
                    _grid.PrimaryGrid.Rows.Remove(row);
                }
            }
        }

        private void ModelChildCreated(IIdentity identity)
        {
            if (identity is IThreatActor actor)
            {
                AddGridRow(actor, _grid.PrimaryGrid);
            }
        }
        #endregion

        public bool IsInitialized => _model != null;

        //public IActionDefinition ActionDefinition => new ActionDefinition(Id, "ThreatActorList", "Threat Actor List", 
        //    Resources.actor_big, Resources.actor);

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
                var panel = _grid.PrimaryGrid;
                panel.Rows.Clear();

                var items = _model.ThreatActors?.ToArray();
                var filter = _filter.Text;

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (string.IsNullOrWhiteSpace(filter) || IsSelected(item, filter))
                        {
                            AddGridRow(item, panel);
                        }
                    }
                }
            }
            finally
            {
                _loading = false;
                _currentRow = null;
                _grid.ResumeLayout(true);
            }
        }

        private void RefreshOnUndoRedo(string text)
        {
            LoadModel();
        }

        private void AddGridRow([NotNull] IThreatActor actor, [NotNull] GridPanel panel)
        {
            var row = new GridRow(
                actor.Name);
            ((INotifyPropertyChanged) actor).PropertyChanged += OnActorPropertyChanged;
            row.Tag = actor;
            row.Cells[0].CellStyles.Default.Image = actor.GetImage(ImageSize.Small);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnPropertyChanged;
            panel.Rows.Add(row);
        }
        
        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IThreatActor actor)
            {
                ((INotifyPropertyChanged) actor).PropertyChanged -= OnActorPropertyChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnPropertyChanged;
            }
        }

        private void OnActorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatActor actor)
            {
                var row = GetRow(actor);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = actor.Name;
                            break;
                    }
                }
            }
        }

        private GridRow GetRow([NotNull] IThreatActor actor)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == actor)
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
                    if (row.Tag is IThreatActor actor)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                actor.Name = (string) cell.Value;
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

        private bool IsSelected([NotNull] IThreatActor item, [Required] string filter)
        {
            var result = (!string.IsNullOrWhiteSpace(item.Name) &&
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
            _properties.Item = _currentRow?.Tag;
            ChangeCustomActionStatus?.Invoke("RemoveActor", _currentRow?.Tag is IThreatActor);
            ChangeActionsStatus(_currentRow?.Tag is IThreatActor);
        }

        private void ChangeActionsStatus(bool newStatus)
        {
            if (_commandsBarContextAwareActions?.Any() ?? false)
            {
                foreach (var definitions in _commandsBarContextAwareActions.Values)
                {
                    if (definitions.Any())
                    {
                        foreach (var definition in definitions)
                        {
                            var actions = definition.Commands?.ToArray();
                            if (actions?.Any() ?? false)
                            {
                                foreach (var action in actions)
                                {
                                    if (action.Tag is IIdentitiesContextAwareAction identitiesContextAwareAction &&
                                        (identitiesContextAwareAction.Scope & SupportedScopes) != 0)
                                    {
                                        ChangeCustomActionStatus?.Invoke(action.Name, newStatus);
                                    }
                                    else if (action.Tag is IPropertiesContainersContextAwareAction pcContextAwareAction &&
                                        (pcContextAwareAction.Scope & SupportedScopes) != 0)
                                    {
                                        ChangeCustomActionStatus?.Invoke(action.Name, newStatus);
                                    }
                                }
                            }
                        }
                    }
                }
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

        private void _grid_CellClick(object sender, GridCellClickEventArgs e)
        {
            if (!_loading)
            {
                var row = e.GridCell?.GridRow;
                if (row != _currentRow)
                {
                    _currentRow = row;
                    ShowCurrentRow();
                }
            }
        }

        private void _grid_RowActivated(object sender, GridRowActivatedEventArgs e)
        {
            if (!_loading)
            {
                if (e.NewActiveRow is GridRow gridRow && _currentRow != gridRow)
                {
                    _currentRow = gridRow;
                    ShowCurrentRow();
                }
            }
        }

        private void _grid_SelectionChanged(object sender, GridEventArgs e)
        {
            if (!_loading)
            {
                if (!e.GridPanel.SelectedCells.OfType<GridCell>().Any() &&
                    !e.GridPanel.SelectedRows.OfType<GridRow>().Any())
                {
                    _currentRow = null;
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
