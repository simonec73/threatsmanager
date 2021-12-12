using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using Northwoods.Go;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.DevOps.Panels
{
    public partial class KanbanPanel : UserControl, IPanelOpenerExtension
    {
        protected IThreatModel _model;
        private GridCell _lastMouseOverCell;
        private readonly List<string> _captions = new List<string>();

        public event Action<IPanelFactory, IIdentity> OpenPanel;

        public KanbanPanel()
        {
            InitializeComponent();
            InitializeGrid();

            _firstPalette.ItemDropped += SetFirst;
            _secondPalette.ItemDropped += SetSecond;
            _thirdPalette.ItemDropped += SetThird;
            _fourthPalette.ItemDropped += SetFourth;
            _fifthPalette.ItemDropped += SetFifth;
            _sixthPalette.ItemDropped += SetSixth;
        }

        protected void Initialize(IEnumerable<string> values)
        {
            var captions = values?.ToArray();

            if (captions?.Any() ?? false)
            {
                var count = Math.Min(captions.Length, 6);
                _container.ColumnCount = count;

                if (count > 5)
                    _sixth.TitleText = captions[5];
                else
                    _sixth.Visible = false;

                if (count > 4)
                    _fifth.TitleText = captions[4];
                else
                    _fifth.Visible = false;

                if (count > 3)
                    _fourth.TitleText = captions[3];
                else
                    _fourth.Visible = false;

                if (count > 2)
                    _third.TitleText = captions[2];
                else
                    _third.Visible = false;

                if (count > 1)
                    _second.TitleText = captions[1];
                else
                    _second.Visible = false;

                _first.TitleText = captions[0];

                for (int i = 0; i < count; i++)
                {
                    _captions.Add(captions[i]);
                }
            }
        }

        #region Control Events.
        private void _first_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[0].Width = 100f / _captions.Count;
                _container.ColumnStyles[0].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[0].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[0].SizeType = SizeType.Absolute;
            }
        }

        private void _second_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[1].Width = 100f / _captions.Count;
                _container.ColumnStyles[1].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[1].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[1].SizeType = SizeType.Absolute;
            }
        }

        private void _third_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[2].Width = 100f / _captions.Count;
                _container.ColumnStyles[2].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[2].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[2].SizeType = SizeType.Absolute;
            }
        }

        private void _fourth_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[3].Width = 100f / _captions.Count;
                _container.ColumnStyles[3].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[3].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[3].SizeType = SizeType.Absolute;
            }
        }

        private void _fifth_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[4].Width = 100f / _captions.Count;
                _container.ColumnStyles[4].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[4].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[4].SizeType = SizeType.Absolute;
            }
        }

        private void _sixth_ExpandedChanged(object sender, ExpandedChangeEventArgs e)
        {
            if (e.NewExpandedValue)
            {
                _container.ColumnStyles[5].Width = 100f / _captions.Count;
                _container.ColumnStyles[5].SizeType = SizeType.Percent;
            }
            else
            {
                _container.ColumnStyles[5].Width = 30 * Dpi.Factor.Width;
                _container.ColumnStyles[5].SizeType = SizeType.Absolute;
            }
        }

        private async void ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            if (e.GoObject.ParentNode is KanbanItem item)
            {
                _itemDetails.Item = item.Item;

                _comments.Clear();

                if (item.Item is IThreatModelChild child && child.Model != null && item.Item is IMitigation mitigation)
                {
                    var connector = DevOpsManager.GetConnector(child.Model);
                    IEnumerable<Comment> comments = null;
                    var itemId = item.DevOpsId;
                    if (itemId > 0)
                    {
                        comments = (await connector.GetWorkItemCommentsAsync(itemId))?.ToArray();
                    }
                    else
                    {
                        comments = (await connector.GetWorkItemCommentsAsync(mitigation))?.ToArray();
                    }

                    if (comments?.Any() ?? false)
                    {
                        var myself = UserName.GetDisplayName();

                        foreach (var comment in comments)
                        {
                            _comments.AddComment(comment.Author, comment.Text, comment.Timestamp, 
                                string.CompareOrdinal(myself, comment.Author) == 0);

                        }
                    }

                    LoadGrid(mitigation);
                }

                _comments.RefreshNodes();
            }
        }
        #endregion

        #region Status Management.
        protected virtual bool SetFirst(object item)
        {
            return false;
        }

        protected virtual bool SetSecond(object item)
        {
            return false;
        }

        protected virtual bool SetThird(object item)
        {
            return false;
        }

        protected virtual bool SetFourth(object item)
        {
            return false;
        }

        protected virtual bool SetFifth(object item)
        {
            return false;
        }

        protected virtual bool SetSixth(object item)
        {
            return false;
        }
        #endregion

        #region Access to Palettes.
        protected WorkItemStatus GetPaletteWorkItemStatus(int index)
        {
            var caption = _captions[index];

            return EnumExtensions.GetEnumValue<WorkItemStatus>(caption);
        }

        protected void ClearPalettes()
        {
            _firstPalette.Clear();
            _secondPalette.Clear();
            _thirdPalette.Clear();
            _fourthPalette.Clear();
            _fifthPalette.Clear();
            _sixthPalette.Clear();
        }
        
        protected void RefreshNodes()
        {
            _firstPalette.RefreshNodes();
            _secondPalette.RefreshNodes();
            _thirdPalette.RefreshNodes();
            _fourthPalette.RefreshNodes();
            _fifthPalette.RefreshNodes();
            _sixthPalette.RefreshNodes();
        }

        protected void AddItem(IMitigation mitigation, WorkItemStatus status, string assignedTo)
        {
            var index = _captions.IndexOf(status.GetEnumLabel());
            if (mitigation != null && index >= 0)
            {
                var item = new KanbanItem(mitigation, index);
                if (!string.IsNullOrWhiteSpace(assignedTo))
                    item.SetInfo("Assigned To", assignedTo);
                item.MoveRequired += ItemOnMoveRequired;
                Add(item, index);
            }
        }

        private void ItemOnMoveRequired(KanbanItem item)
        {
            if (item?.Item is IMitigation mitigation)
            {
                var connector = DevOpsManager.GetConnector(_model);
                if (connector != null)
                {
                    var indexInitial = GetPosition(item);
                    var schemaManager = new DevOpsPropertySchemaManager(_model);
                    var devOpsInfo = schemaManager.GetDevOpsInfo(mitigation, connector);
                    if (devOpsInfo != null)
                    {
                        var indexFinal = _captions.IndexOf(devOpsInfo.Status.GetEnumLabel());
                        Remove(item, indexInitial);
                        Add(item, indexFinal);

                        RefreshPalette(indexInitial);
                        RefreshPalette(indexFinal);
                    }
                }
            }
        }

        protected void MoveItem(IMitigation mitigation, WorkItemStatus initialStatus, WorkItemStatus finalStatus)
        {
            var indexInitial = _captions.IndexOf(initialStatus.GetEnumLabel());
            var indexFinal = _captions.IndexOf(finalStatus.GetEnumLabel());
            var item = GetItem(mitigation, indexInitial);
            if (item != null && indexInitial >= 0 && indexFinal >= 0)
            {
                Remove(item, indexInitial);
                Add(item, indexFinal);

                RefreshPalette(indexInitial);
                RefreshPalette(indexFinal);
            }
        }

        private void Add([NotNull] KanbanItem item, [Positive] int index)
        {
            switch (index)
            {
                case 0:
                    _firstPalette.AddNode(item);
                    break;
                case 1:
                    _secondPalette.AddNode(item);
                    break;
                case 2:
                    _thirdPalette.AddNode(item);
                    break;
                case 3:
                    _fourthPalette.AddNode(item);
                    break;
                case 4:
                    _fifthPalette.AddNode(item);
                    break;
                case 5:
                    _sixthPalette.AddNode(item);
                    break;
            }
        }

        private void Remove([NotNull] KanbanItem item, [Positive] int index)
        {
            switch (index)
            {
                case 0:
                    _firstPalette.RemoveNode(item);
                    break;
                case 1:
                    _secondPalette.RemoveNode(item);
                    break;
                case 2:
                    _thirdPalette.RemoveNode(item);
                    break;
                case 3:
                    _fourthPalette.RemoveNode(item);
                    break;
                case 4:
                    _fifthPalette.RemoveNode(item);
                    break;
                case 5:
                    _sixthPalette.RemoveNode(item);
                    break;
            }
        }

        private KanbanItem GetItem([NotNull] IMitigation mitigation, [Positive] int index)
        {
            KanbanItem result = null;

            var palette = GetPalette(index);
            if (palette != null)
                result = palette.GetNode(mitigation);

            return result;
        }

        private int GetPosition([NotNull] KanbanItem item)
        {
            int result = -1;

            if (_firstPalette.HasNode(item))
                result = 0;
            else if (_secondPalette.HasNode(item))
                result = 1;
            else if (_thirdPalette.HasNode(item))
                result = 2;
            else if (_fourthPalette.HasNode(item))
                result = 3;
            else if (_fifthPalette.HasNode(item))
                result = 4;
            else if (_sixthPalette.HasNode(item))
                result = 5;

            return result;
        }

        private KanbanPalette GetPalette(int index)
        {
            KanbanPalette result = null;

            switch (index)
            {
                case 0:
                    result = _firstPalette;
                    break;
                case 1:
                    result = _secondPalette;
                    break;                 
                case 2:
                    result = _thirdPalette;
                    break;                 
                case 3:
                    result = _fourthPalette;
                    break;                 
                case 4:
                    result = _fifthPalette;
                    break;                 
                case 5:
                    result = _sixthPalette;
                    break;
            }

            return result;
        }

        private void RefreshPalette(int index)
        {
            var palette = GetPalette(index);
            palette?.RefreshNodes();
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

            if (_model == null)
            {
                _model = mitigation.Model;
            }

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
                    row.Cells[0].CellStyles.Default.Image = Icons.Resources.threat_circle_small;
                    break;
                case MitigationLevel.Partial:
                    row.Cells[0].CellStyles.Default.Image = Icons.Resources.threat_circle_orange_small;
                    break;
                case MitigationLevel.Complete:
                    row.Cells[0].CellStyles.Default.Image = Icons.Resources.threat_circle_green_small;
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

        }

        private void OpenDiagram(Guid diagramId)
        {
            var diagram = _model.GetDiagram(diagramId);
            var factory = ExtensionUtils.GetExtensionByLabel<IPanelFactory>("Diagram");
            if (factory != null && diagram != null)
                OpenPanel?.Invoke(factory, diagram);
        }
        #endregion
    }
}
