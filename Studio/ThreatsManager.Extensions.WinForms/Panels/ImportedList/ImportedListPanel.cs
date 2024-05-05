using System;
using System.Collections.Generic;
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
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ImportedList
{
    public partial class ImportedListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, 
        IDesktopAlertAwareExtension, IPanelOpenerExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private GridCell _lastMouseOverCell;

        public ImportedListPanel()
        {
            InitializeComponent();

            _properties.OpenDiagram += OpenDiagram;

            InitializeGrid();
 
            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<ImportedListFilter>().ToArray());
            _specialFilter.SelectedIndex = 0;
        }

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;
        public event Action<IPanelFactory, IIdentity> OpenPanel;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public IIdentity ReferenceObject => null;

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;

            LoadModel();
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
                    EditorType = typeof(GridLabelXEditControl),
                    AllowEdit = false
                });

                panel.Columns.Add(new GridColumn("SourceName")
                {
                    HeaderText = "Source Name",
                    AutoSizeMode = ColumnAutoSizeMode.AllCells,
                    DataType = typeof(string),
                    EditorType = typeof(GridLabelXEditControl),
                    AllowEdit = false
                });

                panel.Columns.Add(new GridColumn("Version")
                {
                    HeaderText = "Version",
                    AutoSizeMode = ColumnAutoSizeMode.AllCells,
                    DataType = typeof(string),
                    EditorType = typeof(GridLabelXEditControl),
                    AllowEdit = false
                });

                panel.Columns.Add(new GridColumn("Author")
                {
                    HeaderText = "Author",
                    AutoSizeMode = ColumnAutoSizeMode.AllCells,
                    DataType = typeof(string),
                    EditorType = typeof(GridLabelXEditControl),
                    AllowEdit = false
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

                var filter = _filter.Text;
                var filterSpecial = EnumExtensions.GetEnumValue<ImportedListFilter>((string)_specialFilter.SelectedItem);

                LoadItems(_model.Entities?.OfType<IExternalInteractor>(), filter, filterSpecial, panel);
                LoadItems(_model.Entities?.OfType<IProcess>(), filter, filterSpecial, panel);
                LoadItems(_model.Entities?.OfType<IDataStore>(), filter, filterSpecial, panel);
                LoadItems(_model.Groups, filter, filterSpecial, panel);
                LoadItems(_model.DataFlows, filter, filterSpecial, panel);
                LoadItems(_model.Diagrams, filter, filterSpecial, panel);
                LoadItems(_model.Schemas, filter, filterSpecial, panel);
                LoadItems(_model.EntityTemplates?.Where(x => x.EntityType == EntityType.ExternalInteractor), filter, filterSpecial, panel);
                LoadItems(_model.EntityTemplates?.Where(x => x.EntityType == EntityType.Process), filter, filterSpecial, panel);
                LoadItems(_model.EntityTemplates?.Where(x => x.EntityType == EntityType.DataStore), filter, filterSpecial, panel);
                LoadItems(_model.TrustBoundaryTemplates, filter, filterSpecial, panel);
                LoadItems(_model.FlowTemplates, filter, filterSpecial, panel);
                LoadItems(_model.ThreatTypes, filter, filterSpecial, panel);
                LoadItems(_model.Weaknesses, filter, filterSpecial, panel);
                LoadItems(_model.Mitigations, filter, filterSpecial, panel);
                LoadItems(_model.ThreatActors, filter, filterSpecial, panel);
            }
            finally
            {
                _loading = false;
                _currentRow = null;
                _grid.ResumeLayout(true);
            }
        }

        private void LoadItems(IEnumerable<IIdentity> items, 
            string filter, 
            ImportedListFilter filterSpecial, 
            GridPanel panel)
        {
            if (items?.Any() ?? false)
            {
                var sorted = items.OrderBy(x => x.Name).ToArray();

                foreach (var item in sorted)
                {
                    if (IsSelected(item, filter, filterSpecial))
                    {
                        AddGridRow(item, panel);
                    }
                }
            }
        }

        private void AddGridRow([NotNull] IIdentity identity, [NotNull] GridPanel panel)
        {
            var name = identity.Name;

            if (identity is ISourceInfo info)
            {
                var row = new GridRow(
                    name,
                    info.SourceTMName,
                    info.VersionId,
                    info.VersionAuthor);

                row.Tag = identity;
                row.Cells[0].CellStyles.Default.Image = identity.GetImage(ImageSize.Small);
                AddSuperTooltipProvider(identity, row.Cells[0]);

                panel.Rows.Add(row);
            }
        }

        private bool IsSelected([NotNull] IIdentity identity, string filter, ImportedListFilter filterSpecial)
        {
            bool result = !string.IsNullOrWhiteSpace((identity as ISourceInfo)?.SourceTMName);

            if (result)
            {
                if (string.IsNullOrWhiteSpace(filter))
                    result = true;
                else
                {
                    result = (!string.IsNullOrWhiteSpace(identity.Name) &&
                                  identity.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                 (!string.IsNullOrWhiteSpace(identity.Description) &&
                                  identity.Description.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);
                    if (!result && identity is IPropertiesContainer container &&
                        (container.Properties?.Any() ?? false))
                    {
                        var properties = container.Properties.ToArray();
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
            }
            
            if (result)
            {
                switch (filterSpecial)
                {
                    case ImportedListFilter.NotApplied:
                        result = !IsApplied(identity);
                        break;
                    case ImportedListFilter.Applied:
                        result = IsApplied(identity);
                        break;
                }
            }

            return result;
        }

        public bool IsApplied([NotNull] IIdentity identity)
        {
            bool result = false;

            if (identity is IEntity || identity is IGroup || identity is IDataFlow || identity is IDiagram)
                result = true;
            else if (identity is IPropertySchema schema)
            {
                result = _model.IsSchemaUsed(schema.Id);
            }
            else if (identity is IEntityTemplate entityTemplate)
            {
                result = _model.Entities?
                    .Where(x => x.Template != null)
                    .Any(x => x.Template.Id == entityTemplate.Id) ?? false;
            }
            else if (identity is ITrustBoundaryTemplate trustBoundaryTemplate)
            {
                result = _model.Groups?.OfType<ITrustBoundary>()
                    .Where(x => x.Template != null)
                    .Any(x => x.Template.Id == trustBoundaryTemplate.Id) ?? false;
            }
            else if (identity is IFlowTemplate flowTemplate)
            {
                result = _model.DataFlows?
                    .Where(x => x.Template != null)
                    .Any(x => x.Template.Id == flowTemplate.Id) ?? false;
            }
            else if (identity is IThreatType threatType)
            {
                result = _model.GetThreatEvents()?
                    .Any(x => x.ThreatTypeId == threatType.Id) ?? false;
            }
            else if (identity is IWeakness weakness)
            {
                result = _model.GetVulnerabilities()?
                    .Any(x => x.WeaknessId == weakness.Id) ?? false;
            }
            else if (identity is IMitigation mitigation)
            {
                result = _model.GetUniqueMitigations()?
                    .Any(x => x.Id == mitigation.Id) ?? false;
            }
            else if (identity is IThreatActor actor)
            {
                result = _model.GetThreatEvents()?
                    .Where(x => x.Scenarios?.Any() ?? false)
                    .Any(x => x.Scenarios.Any(y => y.ActorId == actor.Id)) ?? false;
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

        private void ShowCurrentRow()
        {
            if (_currentRow?.Tag is IIdentity identity)
            {
                _properties.Item = identity;
                ChangeCustomActionStatus?.Invoke("Remove", true);
                //ChangeCustomActionStatus?.Invoke("FindDataFlow", true);
                //ChangeActionsStatus(true);
            }
            else
            {
                ChangeCustomActionStatus?.Invoke("Remove", true);
                //ChangeCustomActionStatus?.Invoke("FindDataFlow", false);
                //ChangeActionsStatus(false);
            }
        }

        private void ChangeActionsStatus(bool newStatus)
        {
            //if (_commandsBarContextAwareActions?.Any() ?? false)
            //{
            //    foreach (var definitions in _commandsBarContextAwareActions.Values)
            //    {
            //        if (definitions.Any())
            //        {
            //            foreach (var definition in definitions)
            //            {
            //                var actions = definition.Commands?.ToArray();
            //                if (actions?.Any() ?? false)
            //                {
            //                    foreach (var action in actions)
            //                    {
            //                        if (action.Tag is IIdentitiesContextAwareAction identitiesContextAwareAction &&
            //                            (identitiesContextAwareAction.Scope & SupportedScopes) != 0)
            //                        {
            //                            ChangeCustomActionStatus?.Invoke(action.Name, newStatus);
            //                        }
            //                        else if (action.Tag is IPropertiesContainersContextAwareAction containersContextAwareAction &&
            //                            (containersContextAwareAction.Scope & SupportedScopes) != 0)
            //                        {
            //                            ChangeCustomActionStatus?.Invoke(action.Name, newStatus);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
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
