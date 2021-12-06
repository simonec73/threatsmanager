using System;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.Roadmap
{
    public partial class RoadmapPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IPanelOpenerExtension, IExecutionModeSupport, IContextAwareExtension, IDesktopAlertAwareExtension
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridCell _lastMouseOverCell;
        private bool _loading;
        private ExecutionMode _executionMode = ExecutionMode.Expert;
        private RoadmapFilter _filter;

        public RoadmapPanel()
        {
            InitializeComponent();

            InitializeGrid();

            _notAssessedPalette.MitigationDropped += SetNotAssessed;
            _shortTermPalette.MitigationDropped += SetShortTerm;
            _midTermPalette.MitigationDropped += SetMidTerm;
            _longTermPalette.MitigationDropped += SetLongTerm;
            _noActionRequiredPalette.MitigationDropped += SetNoActionRequired;
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;
        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;

            LoadModel();
        }

        private void LoadModel()
        {
            _loading = true;
            RemoveEventSubscriptions();

            _notAssessedPalette.Clear();
            _shortTermPalette.Clear();
            _midTermPalette.Clear();
            _longTermPalette.Clear();
            _noActionRequiredPalette.Clear();

            var mitigations = _model?.GetUniqueMitigations()?.OrderBy(x => x.Name).ToArray();
            if ((mitigations?.Any() ?? false) && _filter != null)
            {
                mitigations = _filter.Filter(mitigations)?.ToArray();
            }

            if (mitigations?.Any() ?? false)
            {
                var schemaManager = new ResidualRiskEstimatorPropertySchemaManager(_model);
                var effectivenessDict = 
                    schemaManager.SelectedEstimator?.CategorizeMitigations(_model);

                foreach (var mitigation in mitigations)
                {
                    Effectiveness effectiveness = Effectiveness.Unknown;
                    if (effectivenessDict?.ContainsKey(mitigation.Id) ?? false)
                        effectiveness = effectivenessDict[mitigation.Id];

                    var status = mitigation.GetStatus(out var automatedCalculation);
                    var node = new RoadmapItem(mitigation, effectiveness);
                    if (_actions != null)
                        node.SetContextAwareActions(_actions);

                    switch (status)
                    {
                        case RoadmapStatus.NotAssessed:
                            _notAssessedPalette.AddNode(node);
                            break;
                        case RoadmapStatus.ShortTerm:
                            _shortTermPalette.AddNode(node);
                            break;
                        case RoadmapStatus.MidTerm:
                            _midTermPalette.AddNode(node);
                            break;
                        case RoadmapStatus.LongTerm:
                            _longTermPalette.AddNode(node);
                            break;
                        case RoadmapStatus.NoActionRequired:
                            _noActionRequiredPalette.AddNode(node);
                            if (automatedCalculation)
                                mitigation.SetStatus(RoadmapStatus.NoActionRequired);
                            break;
                    }
                }

                _notAssessedPalette.RefreshNodes();
                _shortTermPalette.RefreshNodes();
                _midTermPalette.RefreshNodes();
                _longTermPalette.RefreshNodes();
                _noActionRequiredPalette.RefreshNodes();

                _chart.RefreshChart(_model);
            }

            _loading = false;
        }
        #endregion

        public event Action<IPanelFactory, IIdentity> OpenPanel;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        #region Control Events.
        private void _notAssessed_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[0].Width = 20;
                _container.ColumnStyles[0].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[0].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[0].SizeType = SizeType.Absolute;
            }
        }

        private void _shortTerm_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[1].Width = 20;
                _container.ColumnStyles[1].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[1].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[1].SizeType = SizeType.Absolute;
            }
        }

        private void _midTerm_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[2].Width = 20;
                _container.ColumnStyles[2].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[2].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[2].SizeType = SizeType.Absolute;
            }
        }

        private void _longTerm_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[3].Width = 20;
                _container.ColumnStyles[3].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[3].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[3].SizeType = SizeType.Absolute;
            }
        }

        private void _noActionRequired_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[4].Width = 20;
                _container.ColumnStyles[4].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[4].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[4].SizeType = SizeType.Absolute;
            }
        }

        private void ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            if (e.GoObject.ParentNode is RoadmapItem mitigation)
            {
                _itemDetails.Item = mitigation.Mitigation;
                LoadGrid(mitigation.Mitigation);
            }
        }
        #endregion

        #region Load Grid.
        private void InitializeGrid()
        {
            var grid = _grid.PrimaryGrid;

            grid.Name = "ThreatEventMitigations";
            grid.AllowRowDelete = false;
            grid.AllowRowInsert = false;
            grid.AllowRowResize = false;
            grid.ShowRowDirtyMarker = false;
            grid.ShowTreeButtons = false;
            grid.ShowTreeLines = false;
            grid.ShowRowHeaders = false;
            grid.InitialSelection = RelativeSelection.None;

            grid.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Threat Event Name",
                Width = 200,
                DataType = typeof(string),
                AllowEdit = false
            });

            grid.Columns.Add(new GridColumn("Parent")
            {
                HeaderText = "Associated To",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 200
            });

            grid.Columns.Add(new GridColumn("Severity")
            {
                HeaderText = "Severity",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 75
            });

            grid.Columns.Add(new GridColumn("Strength")
            {
                HeaderText = "Strength",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 75
            });

            grid.Columns.Add(new GridColumn("Status")
            {
                HeaderText = "Status",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 75
            });
        }

        private void LoadGrid([NotNull] IMitigation mitigation)
        {
            _grid.PrimaryGrid.Rows.Clear();
            _gridItemDetails.Item = null;

            var tems = mitigation.GetThreatEventMitigations();

            foreach (var tem in tems)
            {
                AddGridRow(tem);
            }
        }

        private void AddGridRow([NotNull] IThreatEventMitigation mitigation)
        {
            GridRow row = new GridRow(
                mitigation.ThreatEvent?.Name ?? string.Empty,
                mitigation.ThreatEvent?.Parent?.Name ?? string.Empty,
                mitigation.ThreatEvent?.Severity.ToString(),
                mitigation.Strength.ToString(),
                mitigation.Status.ToString())
            {
                Tag = mitigation
            };
            switch (mitigation.ThreatEvent.GetMitigationLevel())
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
            if (mitigation.ThreatEvent?.Parent != null)
            {
                row.Cells[1].CellStyles.Default.Image = mitigation.ThreatEvent.Parent.GetImage(ImageSize.Small);
                AddSuperTooltipProvider(mitigation.ThreatEvent.Parent, row.Cells[1]);
            }

            _grid.PrimaryGrid.Rows.Add(row);
        }

        private void RemoveEventSubscriptions()
        {
            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
                RemoveEventSubscriptions(row);
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IThreatEventMitigation mitigation)
            {
                RemoveSuperTooltipProvider(row.Cells["Parent"]);
            }
        }
        #endregion
        
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
                OpenDiagram(id);
            }
        }

        private void OpenDiagram(Guid diagramId)
        {
            var diagram = _model.GetDiagram(diagramId);
            var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
            if (factory != null && diagram != null)
                OpenPanel?.Invoke(factory, diagram);
        }
        #endregion

        #region Status Management.
        private void SetNotAssessed(IMitigation mitigation)
        {
            mitigation.SetStatus(RoadmapStatus.NotAssessed);
            if (!_loading)
                _chart.RefreshChart(_model);
        }

        private void SetShortTerm(IMitigation mitigation)
        {
            mitigation.SetStatus(RoadmapStatus.ShortTerm);
            if (!_loading)
                _chart.RefreshChart(_model);
        }

        private void SetMidTerm(IMitigation mitigation)
        {
            mitigation.SetStatus(RoadmapStatus.MidTerm);
            if (!_loading)
                _chart.RefreshChart(_model);
        }

        private void SetLongTerm(IMitigation mitigation)
        {
            mitigation.SetStatus(RoadmapStatus.LongTerm);
            if (!_loading)
                _chart.RefreshChart(_model);
        }

        private void SetNoActionRequired(IMitigation mitigation)
        {
            mitigation.SetStatus(RoadmapStatus.NoActionRequired);
            if (!_loading)
                _chart.RefreshChart(_model);
        }
        #endregion

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
            _itemDetails.SetExecutionMode(mode);

            if (mode == ExecutionMode.Business)
            {
                _itemDetails.ReadOnly = true;
                _notAssessedPalette.SetModifiable(false);
                _notAssessedPalette.AllowDragOut = false;
                _notAssessedPalette.AllowDrop = false;
                _shortTermPalette.SetModifiable(false);
                _shortTermPalette.AllowDragOut = false;
                _shortTermPalette.AllowDrop = false;
                _midTermPalette.SetModifiable(false);
                _midTermPalette.AllowDragOut = false;
                _midTermPalette.AllowDrop = false;
                _longTermPalette.SetModifiable(false);
                _longTermPalette.AllowDragOut = false;
                _longTermPalette.AllowDrop = false;
                _noActionRequiredPalette.SetModifiable(false);
                _noActionRequiredPalette.AllowDragOut = false;
                _noActionRequiredPalette.AllowDrop = false;
            }
        }

        private void _grid_CellClick(object sender, GridCellClickEventArgs e)
        {
            _gridItemDetails.Item = e.GridCell?.GridRow.Tag;
        }
    }
}
