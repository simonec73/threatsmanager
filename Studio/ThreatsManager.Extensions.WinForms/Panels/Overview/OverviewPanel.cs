using System;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Extensions.Panels.Roadmap;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Overview
{
    public partial class OverviewPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private bool _loading;
        private ExecutionMode _executionMode = ExecutionMode.Expert;

        public OverviewPanel()
        {
            InitializeComponent();
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
            try
            {
                _lowestRisks.SuspendLayout();
                _topRisks.SuspendLayout();
                _roadmapDetails.SuspendLayout();

                _loading = true;
                _roadmap.RefreshChart(_model);

                _threatTypesBySeverity.RefreshChart(_model);
                _mitigationsByStatus.RefreshChart(_model);
                _roadmap.RefreshChart(_model);

                var assignedThreatTypes = _model.AssignedThreatTypes;
                //var rowCount = assignedThreatTypes > 20 ? 10 : Math.Min(5, assignedThreatTypes / 2);
                LoadTopRisks(5);
                LoadLowestRisks(5);
                LoadRoadmapStatusDetails();

                _roadmapCharts.Initialize(_model);
            }
            finally
            {
                _loading = false;

                _lowestRisks.ResumeLayout(true);
                _topRisks.ResumeLayout(true);
                _roadmapDetails.ResumeLayout(true);
            }
        }
        #endregion

        private void LoadTopRisks(int rowCount)
        {
            _topRisks.PrimaryGrid.Rows.Clear();

            var threatTypes = _model.ThreatTypes?
                .Where(x => (x.GetTopSeverity()?.Id ?? 0) > 0)
                .OrderByDescending(x => x.GetTopSeverity()?.Id ?? 0)
                .ThenByDescending(x => _model.GetThreatEvents(x)?.Sum(y => y.SeverityId) ?? 0)
                .Take(rowCount)
                .ToArray();

            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    var weight = _model.GetThreatEvents(threatType)?
                        .Sum(x => x.SeverityId) ?? 0;
                    var topSeverity = threatType.GetTopSeverity();
                    _topRisks.PrimaryGrid.Rows.Add(new GridRow(threatType.Name, topSeverity?.Name, weight)
                    {
                        Tag = threatType
                    });
                }
            }
        }

        private void LoadLowestRisks(int rowCount)
        {
            _lowestRisks.PrimaryGrid.Rows.Clear();

            var threatTypes = _model.ThreatTypes?
                .Where(x => (x.GetTopSeverity()?.Id ?? 0) > 0)
                .OrderBy(x => x.GetTopSeverity()?.Id ?? 0)
                .ThenBy(x => _model.GetThreatEvents(x)?.Sum(y => y.SeverityId) ?? 0)
                .Take(rowCount)
                .ToArray();

            if (threatTypes?.Any() ?? false)
            {
                foreach (var threatType in threatTypes)
                {
                    var weight = _model.GetThreatEvents(threatType)?
                        .Sum(x => x.SeverityId) ?? 0;
                    var topSeverity = threatType.GetTopSeverity();
                    _lowestRisks.PrimaryGrid.Rows.Add(new GridRow(threatType.Name, topSeverity?.Name, weight)
                    {
                        Tag = threatType
                    });
                }
            }
        }

        private void LoadRoadmapStatusDetails()
        {
            _roadmapDetails.PrimaryGrid.Rows.Clear();

            LoadRoadmapStatusDetails(RoadmapStatus.ShortTerm);
            LoadRoadmapStatusDetails(RoadmapStatus.MidTerm);
            LoadRoadmapStatusDetails(RoadmapStatus.LongTerm);

            _roadmapDetails.PrimaryGrid.ExpandAll();
        }

        private void LoadRoadmapStatusDetails(RoadmapStatus status)
        {
            var mitigations = _model.Mitigations?
                .Where(x => x.GetStatus() == status)
                .OrderBy(x => x.Name)
                .ToArray();

            if (mitigations?.Any() ?? false)
            {
                var container = new GridRow(status.GetEnumLabel());
                _roadmapDetails.PrimaryGrid.Rows.Add(container);

                var panel = new GridPanel
                {
                    Name = status.GetEnumLabel(),
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowTreeButtons = false,
                    ShowTreeLines = false,
                    ShowRowHeaders = false,
                    InitialActiveRow = RelativeRow.None,
                    InitialSelection = RelativeSelection.None
                };
                panel.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Mitigation Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    AllowEdit = false
                });
                panel.Columns.Add(new GridColumn("ControlType")
                {
                    HeaderText = "Control Type",
                    DataType = typeof(string),
                    AllowEdit = false,
                    Width = 75
                });
                container.Rows.Add(panel);

                foreach (var mitigation in mitigations)
                {
                    panel.Rows.Add(new GridRow(mitigation.Name, mitigation.ControlType.GetEnumLabel())
                    {
                        Tag = mitigation
                    });
                }
            }
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
            _itemEditor.SetExecutionMode(mode);

            if (mode == ExecutionMode.Business || mode == ExecutionMode.Management)
            {
                _itemEditor.ReadOnly = true;
            }
        }

        private void OnCellActivated(object sender, GridCellActivatedEventArgs e)
        {
            if (!_loading)
                _itemEditor.Item = e.NewActiveCell.GridRow.Tag;
        }

        private void OnRowActivated(object sender, GridRowActivatedEventArgs e)
        {
            if (!_loading)
                _itemEditor.Item = e.NewActiveRow.Tag;
        }
    }
}
