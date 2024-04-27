using System;
using System.Collections.Generic;
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
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ItemTemplateList
{
    public partial class ItemTemplateListPanel : UserControl, IShowThreatModelPanel<Form>, ICustomRibbonExtension, 
        IInitializableObject, IContextAwareExtension, IDesktopAlertAwareExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;

        public ItemTemplateListPanel()
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

            if (_model is IUndoable undoable && undoable.IsUndoEnabled)
            {
                undoable.Undone += ModelUndone;
            }

            LoadModel();
        }

        private void ModelUndone(object item, bool removed)
        {
            if (removed)
            {
                this.ParentForm?.Close();
            }
            else
            {
                if (item is IThreatModel model)
                {
                    var list = new List<IItemTemplate>();
                    var entities = model.EntityTemplates?.ToArray();
                    if (entities?.Any() ?? false)
                    {
                        list.AddRange(entities);
                    }
                    var flows = model.FlowTemplates?.ToArray();
                    if (flows?.Any() ?? false)
                    {
                        list.AddRange(flows);
                    }
                    var trustBoundaries = model.TrustBoundaryTemplates?.ToArray();
                    if (trustBoundaries?.Any() ?? false)
                    {
                        list.AddRange(trustBoundaries);
                    }

                    var grid = _grid.PrimaryGrid;
                    var rows = grid.Rows.OfType<GridRow>().ToArray();
                    if (rows.Any())
                    {
                        foreach (var row in rows)
                        {
                            if (row.Tag is IEntityTemplate entity)
                            {
                                if (model.GetEntityTemplate(entity.Id) == null)
                                {
                                    RemoveEventSubscriptions(row);
                                    _grid.PrimaryGrid.Rows.Remove(row);
                                }
                                else
                                {
                                    list.Remove(entity);
                                }
                            } else if (row.Tag is IFlowTemplate flow)
                            {
                                if (model.GetFlowTemplate(flow.Id) == null)
                                {
                                    RemoveEventSubscriptions(row);
                                    _grid.PrimaryGrid.Rows.Remove(row);
                                }
                                else
                                {
                                    list.Remove(flow);
                                }
                            } else if (row.Tag is ITrustBoundaryTemplate trustBoundary)
                            {
                                if (model.GetEntityTemplate(trustBoundary.Id) == null)
                                {
                                    RemoveEventSubscriptions(row);
                                    _grid.PrimaryGrid.Rows.Remove(row);
                                }
                                else
                                {
                                    list.Remove(trustBoundary);
                                }
                            }
                        }
                    }

                    if (list.Any())
                    {
                        foreach (var i in list)
                        {
                            AddGridRow(i, grid);
                        }
                    }
                }
            }
        }

        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is IItemTemplate template)
            {
                var row = GetRow(template);
                if (row != null)
                {
                    RemoveEventSubscriptions(row);
                    _grid.PrimaryGrid.Rows.Remove(row);
                }
            }
        }

        private void ModelChildCreated(IIdentity identity)
        {
            if (identity is IItemTemplate template)
            {
                AddGridRow(template, _grid.PrimaryGrid);
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

                panel.Columns.Add(new GridColumn("ItemType")
                {
                    HeaderText = "Item Type",
                    Width = 150,
                    DataType = typeof(string),
                    AllowEdit = false
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

                AddGridRows(_model.EntityTemplates?
                    .OrderBy(x => x.EntityType)
                    .ThenBy(x => x.Name));
                AddGridRows(_model.FlowTemplates?
                    .OrderBy(x => x.Name));
                AddGridRows(_model.TrustBoundaryTemplates?
                    .OrderBy(x => x.Name));
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

        private void AddGridRows(IEnumerable<IItemTemplate> items)
        {
            var templates = items?.ToArray();
            if (templates?.Any() ?? false)
            {
                var filter = _filter.Text;
                foreach (var template in templates)
                {
                    if (string.IsNullOrWhiteSpace(filter) || IsSelected(template, filter))
                    {
                        AddGridRow(template, _grid.PrimaryGrid);
                    }
                }
            }
        }

        private void AddGridRow([NotNull] IItemTemplate template, [NotNull] GridPanel panel)
        {
            string itemType = null;
            if (template is IEntityTemplate entityTemplate)
            {
                itemType = entityTemplate.EntityType.GetEnumLabel();
                entityTemplate.ImageChanged += OnImageChanged;
            }
            else if (template is IFlowTemplate)
                itemType = "Flow";
            else if (template is ITrustBoundaryTemplate)
                itemType = "Trust Boundary";

            var row = new GridRow(template.Name, itemType);
            ((INotifyPropertyChanged) template).PropertyChanged += OnTemplatePropertyChanged;
            row.Tag = template;
            row.Cells[0].CellStyles.Default.Image = template.GetImage(ImageSize.Small);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnPropertyChanged;
            panel.Rows.Add(row);

            if (template is IUndoable undoable && undoable.IsUndoEnabled)
                undoable.Undone += TemplateUndone;
        }

        private void TemplateUndone(object item, bool removed)
        {
            if (item is IItemTemplate itemTemplate)
            {
                var row = GetRow(itemTemplate);
                if (row != null)
                {
                    if (removed)
                    {
                        RemoveEventSubscriptions(row);
                        _grid.PrimaryGrid.Rows.Remove(row);
                    }
                    else
                    {
                        try
                        {
                            _loading = true;
                            row.Cells["Name"].Value = itemTemplate.Name;
                            if (itemTemplate is IEntityTemplate entityTemplate)
                                row.Cells[0].CellStyles.Default.Image = entityTemplate.GetImage(ImageSize.Small);
                        }
                        finally
                        {
                            _loading = false;
                        }
                    }
                }
            }
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IItemTemplate template)
            {
                ((INotifyPropertyChanged) template).PropertyChanged -= OnTemplatePropertyChanged;
                if (template is IEntityTemplate entityTemplate)
                    entityTemplate.ImageChanged -= OnImageChanged;
                if (template is IUndoable undoable && undoable.IsUndoEnabled)
                    undoable.Undone -= TemplateUndone;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnPropertyChanged;
            }
        }

        private void OnImageChanged([NotNull] IEntityTemplate template, ImageSize size)
        {
            var row = GetRow(template);
            if (row != null)
            {
                row.Cells[0].CellStyles.Default.Image = template.GetImage(ImageSize.Small);
            }
        }

        private void OnTemplatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IItemTemplate template)
            {
                var row = GetRow(template);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = template.Name;
                            break;
                    }
                }
            }
        }

        private GridRow GetRow([NotNull] IItemTemplate template)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == template)
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
            if (!_loading && sender is GridCell cell)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    if (row.Tag is IItemTemplate template)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                template.Name = (string) cell.Value;
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

        private bool IsSelected([NotNull] IItemTemplate item, [Required] string filter)
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
            if (_currentRow?.Tag is IItemTemplate template)
            {
                _properties.Item = template;
                ChangeCustomActionStatus?.Invoke("RemoveItemTemplates", true);
                ChangeActionsStatus(true);
            }
            else
            {
                ChangeCustomActionStatus?.Invoke("RemoveItemTemplates", false);
                ChangeActionsStatus(false);
            }
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
                                    else if (action.Tag is IPropertiesContainersContextAwareAction containersContextAwareAction &&
                                        (containersContextAwareAction.Scope & SupportedScopes) != 0)
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

        private void _grid_CellActivated(object sender, GridCellActivatedEventArgs e)
        {
            if (!_loading)
            {
                var row = e.NewActiveCell?.GridRow;
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
