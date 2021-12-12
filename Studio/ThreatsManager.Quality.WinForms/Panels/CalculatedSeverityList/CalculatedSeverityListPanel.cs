using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Quality.CalculatedSeverity;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Quality.Panels.CalculatedSeverityList
{
    public partial class CalculatedSeverityListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, 
        IDesktopAlertAwareExtension, IPanelOpenerExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private GridCell _lastMouseOverCell;
        private ExecutionMode _executionMode;

        public CalculatedSeverityListPanel()
        {
            InitializeComponent();

            _properties.OpenDiagram += OpenDiagram;
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
            _model.ChildRemoved += ModelChildRemoved;
            _model.ThreatEventAdded += OnThreatEventAdded;
            _model.ThreatEventRemoved += OnThreatEventRemoved;
            _model.ThreatEventAddedToEntity += OnThreatEventAdded;
            _model.ThreatEventRemovedFromEntity += OnThreatEventRemoved;
            _model.ThreatEventAddedToDataFlow += OnThreatEventAdded;
            _model.ThreatEventRemovedFromDataFlow += OnThreatEventRemoved;

            InitializeGrid();
            LoadModel();
        }

        private void OnThreatEventRemoved([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            var row = GetRow(threatEvent);
            if (row != null)
            {
                RemoveEventSubscriptions(row);

                var panel = row.GridPanel;
                panel.Rows.Remove(row);

                UpdateThreatTypeSeverity(panel);
            }
        }

        private void OnThreatEventAdded([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            var filter = _filter.Text;
            if (IsSelected(threatEvent, filter))
            {
                AddGridRow(threatEvent);
            }
        }

        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is IThreatType threatType)
            {
                var row = GetRow(threatType);
                if (row != null)
                {
                    RemoveEventSubscriptions(row);
                    var panel = row.GridPanel;
                    panel.Rows.Remove(row);

                    if (panel.Rows.Count == 0)
                    {
                        _grid.PrimaryGrid.Rows.Remove(panel.Parent);
                    }
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

        #region Threat Type Level.
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
                panel.DefaultVisualStyles.CellStyles.ReadOnly.TextColor = Color.Black;
                panel.ReadOnly = _executionMode == ExecutionMode.Management;

                panel.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Threat Type Name",
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

                panel.Columns.Add(new GridColumn("Severity")
                {
                    HeaderText = "Max Severity",
                    DataType = typeof(ISeverity),
                    EditorType = typeof(SeverityComboBox),
                    EditorParams = new object[] { _model.Severities?.Where(x => x.Visible) },
                    AllowEdit = false,
                    Width = 100
                });
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

                var filter = _filter.Text;

                var schemaManager = new CalculatedSeverityPropertySchemaManager(_model);

                List<IThreatEvent> threatEvents = new List<IThreatEvent>();
                AddThreatEvents(threatEvents, 
                    _model.Entities?.Select(x => x.ThreatEvents?
                        .Where(y => SeverityDoesNotMatchCalculated(y, schemaManager))).ToArray(), filter);
                AddThreatEvents(threatEvents, _model.DataFlows?.Select(x => x.ThreatEvents?
                    .Where(y => SeverityDoesNotMatchCalculated(y, schemaManager))).ToArray(), filter);
                var modelThreats = _model.ThreatEvents?
                    .Where(x => IsSelected(x, filter) && SeverityDoesNotMatchCalculated(x, schemaManager)).ToArray();
                if (modelThreats?.Any() ?? false)
                    threatEvents.AddRange(modelThreats);

                var threatTypes = threatEvents
                    .Select(x => x.ThreatType)
                    .Distinct(new ThreatTypeComparer())
                    .OrderBy(x => x.Name);


                if (threatTypes != null)
                {
                    foreach (var item in threatTypes)
                    {
                        var selectedEvents = threatEvents
                            .Where(x => x.ThreatTypeId == item.Id)
                            .OrderBy(x => x.Parent.Name)
                            .ToArray();

                        if (selectedEvents.Any())
                        {
                            AddGridRow(item, selectedEvents, panel);
                        }
                    }

                    _currentRow = _grid.PrimaryGrid.Rows.OfType<GridRow>().FirstOrDefault();
                }
            }
            finally
            {
                _loading = false;
                _grid.ResumeLayout(true);
            }
        }

        private bool SeverityDoesNotMatchCalculated([NotNull] IThreatEvent threatEvent, [NotNull] CalculatedSeverityPropertySchemaManager schemaManager)
        {
            var result = false;

            if (schemaManager.IsCalculatedSeverityEnabled)
            {
                var configDelta = schemaManager.GetSeverityCalculationConfig(threatEvent)?.Delta ?? 0;
                var calculatedSeverity = threatEvent.GetCalculatedSeverity(configDelta);
                result = calculatedSeverity != null && calculatedSeverity.Id != threatEvent.SeverityId;
            }

            return result;
        }

        private void AddThreatEvents([NotNull] List<IThreatEvent> threatEvents, 
            IEnumerable<IThreatEvent>[] array,
            string filter)
        {
            if (array?.Any() ?? false)
            {
                foreach (var threats in array)
                {
                    var list = threats?.Where(x => IsSelected(x, filter)).ToArray();
                    if (list?.Any() ?? false)
                    {
                        threatEvents.AddRange(list);
                    }
                }
            }
        }

        private GridRow AddGridRow([NotNull] IThreatType threatType, 
            [NotNull] IEnumerable<IThreatEvent> threatEvents, [NotNull] GridPanel panel)
        {
            var row = new GridRow(
                threatType.Name,
                threatEvents
                    .Select(x => x.Severity)
                    .OrderByDescending(x => x, new SeverityComparer())
                    .FirstOrDefault());
            ((INotifyPropertyChanged) threatType).PropertyChanged += OnThreatTypePropertyChanged;
            row.Tag = threatType;
            row.Cells[0].CellStyles.Default.Image = threatType.GetImage(ImageSize.Small);
            panel.Rows.Add(row);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnThreatTypeCellChanged;

            var subPanel = CreateThreatEventsPanel(threatType, threatEvents);
            if (subPanel != null)
                row.Rows.Add(subPanel);

            return row;
        }

        private void RemoveEventSubscriptions()
        {
            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
                RemoveEventSubscriptions(row);
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IThreatType threatType)
            {
                ((INotifyPropertyChanged) threatType).PropertyChanged -= OnThreatTypePropertyChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnThreatTypeCellChanged;

                var ddc = row.Rows.OfType<GridPanel>().FirstOrDefault()?.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClearClick -= DdcButtonClearClick;
                }

                RemoveEventSubscriptionsFromChildren(row);
            }
            else if (row?.Tag is IThreatEvent threatEvent)
            {
                if (threatEvent.Parent != null)
                {
                    ((INotifyPropertyChanged) threatEvent.Parent).PropertyChanged -= OnThreatEventParentPropertyChanged;
                    if (threatEvent.Parent is IEntity entity)
                        entity.ImageChanged -= OnImageChanged;
                }
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnThreatEventCellChanged;
                ((INotifyPropertyChanged) threatEvent).PropertyChanged -= OnThreatEventPropertyChanged;

                threatEvent.ThreatEventMitigationAdded -= OnThreatEventMitigationAdded;
                threatEvent.ThreatEventMitigationRemoved -= OnThreatEventMitigationRemoved;

                var panel = row.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Scenarios") == 0);
                var ddc = panel?.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClearClick -= DdcButtonClearClick;
                }
                ddc = panel?.Columns["Motivation"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClearClick -= DdcButtonClearClick;
                }

                RemoveSuperTooltipProvider(row.Cells["Parent"]);

                RemoveEventSubscriptionsFromChildren(row);
            }
            else if (row?.Tag is IThreatEventMitigation mitigation)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnMitigationCellChanged;
                ((INotifyPropertyChanged) mitigation).PropertyChanged -= OnThreatEventMitigationPropertyChanged;
                ((INotifyPropertyChanged) mitigation.Mitigation).PropertyChanged -= OnMitigationPropertyChanged;
            }
        }

        private void RemoveEventSubscriptionsFromChildren(GridRow row)
        {
            var panels = row?.Rows.OfType<GridPanel>().ToArray();
            if (panels?.Any() ?? false)
            {
                foreach (var panel in panels)
                {
                    var children = panel.Rows.OfType<GridRow>().ToArray();
                    if (children.Any())
                    {
                        foreach (var child in children)
                            RemoveEventSubscriptions(child);
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
                    var row = cell.GridRow;
                    if (row.Tag is IThreatType threatType)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                threatType.Name = (string) cell.Value;
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

        private void OnThreatTypePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatType threatType)
            {
                var row = GetRow(threatType);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = threatType.Name;
                            break;
                    }
                }
            }
        }
        #endregion

        #region Threat Event Level.
        private GridPanel CreateThreatEventsPanel([NotNull] IThreatType threatType, 
            [NotNull] IEnumerable<IThreatEvent> threatEvents)
        {
            GridPanel result = null;

            if (!string.IsNullOrWhiteSpace(threatType.Name))
            {
                result = new GridPanel
                {
                    Name = "ThreatEvents",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowTreeButtons = true,
                    ShowTreeLines = true,
                    ShowRowHeaders = false,
                    InitialSelection = RelativeSelection.None,
                    ReadOnly = _executionMode == ExecutionMode.Management
                };
                result.DefaultVisualStyles.CellStyles.ReadOnly.TextColor = Color.Black;

                result.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Threat Event Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    EditorType = typeof(GridTextBoxDropDownEditControl),
                    AllowEdit = true
                });
                GridTextBoxDropDownEditControl ddc = result.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }

                result.Columns.Add(new GridColumn("Parent")
                {
                    HeaderText = "Associated To",
                    DataType = typeof(string),
                    AllowEdit = false,
                    Width = 250
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

                foreach (var threatEvent in threatEvents)
                {
                    AddGridRow(threatEvent, result);
                }
            }

            return result;
        }

        private GridRow AddGridRow([NotNull] IThreatEvent threatEvent, [NotNull] GridPanel panel)
        {
            GridRow row = new GridRow(
                threatEvent.Name,
                threatEvent.Parent?.Name ?? string.Empty,
                threatEvent.Severity)
            {
                Tag = threatEvent
            };
            UpdateMitigationLevel(threatEvent, row);
            if (threatEvent.Parent != null)
            {
                row.Cells[1].CellStyles.Default.Image = threatEvent.Parent.GetImage(ImageSize.Small);
                ((INotifyPropertyChanged) threatEvent.Parent).PropertyChanged += OnThreatEventParentPropertyChanged;
                if (threatEvent.Parent is IEntity entity)
                    entity.ImageChanged += OnImageChanged;
                AddSuperTooltipProvider(threatEvent.Parent, row.Cells[1]);
            }
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnThreatEventCellChanged;
            panel.Rows.Add(row);
            ((INotifyPropertyChanged) threatEvent).PropertyChanged += OnThreatEventPropertyChanged;

            threatEvent.ThreatEventMitigationAdded += OnThreatEventMitigationAdded;
            threatEvent.ThreatEventMitigationRemoved += OnThreatEventMitigationRemoved;
   
            var subPanel2 = CreateThreatEventMitigationsPanel(threatEvent);
            if (subPanel2 != null)
                row.Rows.Add(subPanel2);

            UpdateThreatTypeSeverity(row);

            return row;
        }
        
        private void OnImageChanged([NotNull] IEntity entity, ImageSize size)
        {
            var sourceRows = GetRowsForEntity(entity)?.ToArray();
            if (sourceRows?.Any() ?? false)
            {
                foreach (var row in sourceRows)
                {
                    row["Parent"].CellStyles.Default.Image = entity.GetImage(ImageSize.Small);
                }
            }
        }

        private IEnumerable<GridRow> GetRowsForEntity([NotNull] IEntity entity)
        {
            List<GridRow> result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var panels = row.Rows.OfType<GridPanel>();
                foreach (var panel in panels)
                {
                    var children = panel.Rows.OfType<GridRow>();
                    foreach (var child in children)
                    {
                        if (child.Tag is IThreatEvent threatEvent && threatEvent.ParentId == entity.Id)
                        {
                            if (result == null)
                                result = new List<GridRow>();
                            result.Add(child);
                        }
                    }
                }
            }

            return result;
        }

        private static void UpdateMitigationLevel([NotNull] IThreatEvent threatEvent, [NotNull] GridRow row)
        {
            switch (threatEvent.GetMitigationLevel())
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

        private void OnThreatEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEvent threatEvent)
            {
                var row = GetRow(threatEvent);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = threatEvent.Name;
                            break;
                        case "Severity":
                            row["Severity"].Value = threatEvent.Severity;
                            UpdateThreatTypeSeverity(row);
                            break;
                    }
                }
            }
        }

        private static void UpdateThreatTypeSeverity([NotNull] GridElement reference)
        {
            if (!(reference is GridPanel panel))
            {
                panel = reference.GridPanel;
            }
            GridRow parent = panel.Parent as GridRow;

            if (parent != null)
            {
                var siblings = panel.Rows.OfType<GridRow>().ToArray();
                parent[1].Value = siblings
                    .Select(x => x.Tag)
                    .OfType<IThreatEvent>()
                    .Select(x => x.Severity)
                    .OrderByDescending(x => x, new SeverityComparer())
                    .FirstOrDefault();
            }
        }

        private void OnThreatEventParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IIdentity identity && string.CompareOrdinal(e.PropertyName, "Name") == 0)
            {
                var rows = GetRowsAssociatedTo(identity)?.ToArray();
                if (rows?.Any() ?? false)
                {
                    foreach (var row in rows)
                    {
                        row["Parent"].Value = identity.Name;
                    }
                }
            }
        }

        private void OnThreatEventMitigationAdded(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            if (container is IThreatEvent threatEvent)
            {
                if (IsSelected(threatEvent, _filter.Text))
                {
                    var row = GetRow(threatEvent);
                    if (row == null)
                    {
                        var typeRow = GetRow(threatEvent.ThreatType);
                        if (typeRow == null)
                            typeRow = AddGridRow(threatEvent.ThreatType, new IThreatEvent[] {threatEvent}, _grid.PrimaryGrid);

                        var panel = typeRow.Rows.OfType<GridPanel>()
                                .FirstOrDefault(x => string.CompareOrdinal(x.Name, "Mitigations") == 0);
                        if (panel != null)
                        {
                            row = AddGridRow(threatEvent, panel);
                        }
                        else
                        {
                            // We should not come here.
                            panel = CreateThreatEventsPanel(threatEvent.ThreatType,
                                new IThreatEvent[] {threatEvent});
                            if (panel != null)
                            {
                                typeRow.Rows.Add(panel);
                                row = panel.Rows.OfType<GridRow>().FirstOrDefault();
                            }
                        }
                    }
                    else
                    {
                        var mitigationPanel = row.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Mitigations") == 0);
                        if (mitigationPanel != null)
                        {
                            AddGridRow(mitigation, mitigationPanel);
                        }
                        else
                        {
                            mitigationPanel = CreateThreatEventMitigationsPanel(threatEvent);
                            if (mitigationPanel != null)
                                row.Rows.Add(mitigationPanel);
                        }
                    }

                    if (row != null)
                    {
                        UpdateMitigationLevel(threatEvent, row);
                    }
                }
            }
        }

        private void OnThreatEventMitigationRemoved(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            if (container is IThreatEvent threatEvent)
            {
                var row = GetRow(threatEvent);

                var panel = row?.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Mitigations") == 0);
                var mitigationRow = panel?.Rows.OfType<GridRow>()
                    .FirstOrDefault(x =>
                        (x.Tag is IThreatEventMitigation eventMitigation) && eventMitigation.MitigationId == mitigation.MitigationId);
                if (mitigationRow != null)
                {
                    panel.Rows.Remove(mitigationRow);
  
                    if (panel.Rows.Count == 0)
                        row.Rows.Remove(panel);
                }

                UpdateMitigationLevel(threatEvent, row);
            }
        }

        private void AddGridRow([NotNull] IThreatEvent threatEvent)
        {
            if (IsSelected(threatEvent, _filter.Text) && GetRow(threatEvent) == null)
            {
                var threatType = threatEvent.ThreatType;
                var threatTypeRow = GetRow(threatType);
                var panel = threatTypeRow?.Rows.OfType<GridPanel>().FirstOrDefault();
                if (panel != null)
                {
                    AddGridRow(threatEvent, panel);
                }
                else
                {
                    AddGridRow(threatType, new[] {threatEvent}, _grid.PrimaryGrid);
                }
            }
        }

        private void OnThreatEventCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_loading && sender is GridCell cell)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    if (row.Tag is IThreatEvent threatEvent)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                threatEvent.Name = (string) cell.Value;
                                break;
                            case "Severity":
                                if (cell.Value is ISeverity severity)
                                    threatEvent.Severity = severity;
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
        #endregion

        #region Threat Event Mitigation Level.
        private GridPanel CreateThreatEventMitigationsPanel([NotNull] IThreatEvent threatEvent)
        {
            GridPanel result = null;

            var mitigations = threatEvent.Mitigations?
                .OrderBy(x => x.Mitigation.Name)
                .ToArray();
            if (!string.IsNullOrWhiteSpace(threatEvent.Name) && (mitigations?.Any() ?? false))
            {
                result = new GridPanel
                {
                    Name = "Mitigations",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowRowHeaders = false,
                    InitialSelection = RelativeSelection.None,
                    ReadOnly = _executionMode == ExecutionMode.Management
                };
                result.DefaultVisualStyles.CellStyles.ReadOnly.TextColor = Color.Black;

                result.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Mitigation Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                result.Columns.Add(new GridColumn("ControlType")
                {
                    HeaderText = "Control Type",
                    DataType = typeof(string),
                    EditorType = typeof(EnumComboBox),
                    EditorParams = new object[] { EnumExtensions.GetEnumLabels<SecurityControlType>() },
                    AllowEdit = false,
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

                result.Columns.Add(new GridColumn("Status")
                {
                    HeaderText = "Status",
                    DataType = typeof(string),
                    EditorType = typeof(EnumComboBox),
                    EditorParams = new object[] { EnumExtensions.GetEnumLabels<MitigationStatus>() },
                    AllowEdit = true,
                    Width = 75
                });

                foreach (var mitigation in mitigations)
                {
                    AddGridRow(mitigation, result);
                }
            }

            return result;
        }

        private void AddGridRow([NotNull] IThreatEventMitigation mitigation, [NotNull] GridPanel panel)
        {
            GridRow row = new GridRow(
                mitigation.Mitigation.Name,
                mitigation.Mitigation.ControlType.ToString(),
                mitigation.Strength,
                mitigation.Status.ToString())
            {
                Tag = mitigation
            };
            row.Cells[0].CellStyles.Default.Image = mitigation.Mitigation.GetImage(ImageSize.Small);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnMitigationCellChanged;
            panel.Rows.Add(row);
            ((INotifyPropertyChanged) mitigation).PropertyChanged += OnThreatEventMitigationPropertyChanged;
            ((INotifyPropertyChanged) mitigation.Mitigation).PropertyChanged += OnMitigationPropertyChanged;
        }
        
        private void OnThreatEventMitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEventMitigation mitigation)
            {
                var row = GetRow(mitigation);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Strength":
                            row["Strength"].Value = mitigation.Strength;
                            if (mitigation.ThreatEvent is IThreatEvent threatEvent)
                            {
                                var threatEventRow = GetRow(threatEvent);
                                UpdateMitigationLevel(threatEvent, threatEventRow);
                            }
                            break;
                        case "Status":
                            row["Status"].Value = mitigation.Status.ToString();
                            break;
                    }
                }
            }
        }
       
        private void OnMitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IMitigation mitigation)
            {
                var rows = GetRows(mitigation)?.ToArray();
                if (rows?.Any() ?? false)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            foreach (var row in rows)
                                row["Name"].Value = mitigation.Name;
                            break;
                        case "ControlType":
                            foreach (var row in rows)
                                row["ControlType"].Value = mitigation.ControlType.ToString();
                            break;
                    }
                }
            }
        }

        private void OnMitigationCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var cell = sender as GridCell;
            var propertyName = propertyChangedEventArgs.PropertyName;

            if (!_loading && cell != null)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    var scenario = row.Tag as IThreatEventMitigation;
                    if (scenario != null)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Strength":
                                if (cell.Value is IStrength strength)
                                    scenario.Strength = strength;
                                break;
                            case "Status":
                                scenario.Status = ((string)cell.Value).GetEnumValue<MitigationStatus>();
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
        #endregion

        #region Auxiliary private members.
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

        private bool IsSelected([NotNull] IThreatEvent item, string filter)
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

            return result;
        }

        private GridRow GetRow([NotNull] IThreatType threatType)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == threatType)
                {
                    result = row;
                    break;
                }
            }

            return result;
        }

        private GridRow GetRow([NotNull] IThreatEvent threatEvent)
        {
            GridRow result = null;

            var parentRow = GetRow(threatEvent.ThreatType);
            if (parentRow != null)
            {
                var panels = parentRow.Rows.OfType<GridPanel>().ToArray();
                if (panels.Any())
                {
                    foreach (var panel in panels)
                    {
                        result = panel.Rows.OfType<GridRow>().FirstOrDefault(x => x.Tag == threatEvent);
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        private GridRow GetRow([NotNull] IThreatEventMitigation mitigation)
        {
            GridRow result = null;

            var parentRow = GetRow(mitigation.ThreatEvent);
            if (parentRow != null)
            {
                var panels = parentRow.Rows.OfType<GridPanel>().ToArray();
                if (panels.Any())
                {
                    foreach (var panel in panels)
                    {
                        result = panel.Rows.OfType<GridRow>().FirstOrDefault(x => x.Tag == mitigation);
                        if (result != null)
                            break;
                    }
                }
            }

            return result;
        }

        private IEnumerable<GridRow> GetRows([NotNull] IMitigation mitigation)
        {
            List<GridRow> result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var panels = row.Rows.OfType<GridPanel>().ToArray();
                foreach (var panel in panels)
                {
                    var subRows = panel.Rows.OfType<GridRow>().ToArray();
                    foreach (var subRow in subRows)
                    {
                        var subPanels = subRow.Rows.OfType<GridPanel>().ToArray();
                        foreach (var subPanel in subPanels)
                        {
                            var leaves = subPanel.Rows.OfType<GridRow>().ToArray();
                            foreach (var leaf in leaves)
                            {
                                if (leaf.Tag is IThreatEventMitigation tem && tem.MitigationId == mitigation.Id)
                                {
                                    if (result == null)
                                        result = new List<GridRow>();
                                    result.Add(leaf);
                                }
                            }
                        }
                    }
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

        private IEnumerable<GridRow> GetRowsAssociatedTo([NotNull] IIdentity identity)
        {
            List<GridRow> result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var children = row.Rows.OfType<GridPanel>().FirstOrDefault()?.Rows.OfType<GridRow>();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                    {
                        if (child.Tag is IThreatEvent threatEvent)
                        {
                            if (threatEvent.Parent == identity)
                            {
                                if (result == null)
                                    result = new List<GridRow>();
                                result.Add(child);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private void ShowCurrentRow()
        {
            _properties.Item = _currentRow?.Tag;

            bool isMitigation = _currentRow?.Tag is IThreatEventMitigation;
            bool isThreatEvent = _currentRow?.Tag is IThreatEvent;
            ChangeCustomActionStatus?.Invoke("AddMitigation", isThreatEvent);
            ChangeCustomActionStatus?.Invoke("RemoveMitigation", isMitigation);
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

                if (row?.Tag is IThreatEvent)
                {
                    MenuDefinition.UpdateVisibility(_threatEventMenu, row.Tag);
                    _threatEventMenu?.Show(_grid.PointToScreen(e.Location));
                }
                if (row?.Tag is IThreatType)
                {
                    MenuDefinition.UpdateVisibility(_threatTypeMenu, row.Tag);
                    _threatTypeMenu?.Show(_grid.PointToScreen(e.Location));
                }
                if (row?.Tag is IThreatEventMitigation)
                {
                    MenuDefinition.UpdateVisibility(_threatEventMitigationMenu, row.Tag);
                    _threatEventMitigationMenu?.Show(_grid.PointToScreen(e.Location));
                }
            }
        }
        #endregion

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
                OpenDiagram(id);
            }
        }
        #endregion

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
            _properties.SetExecutionMode(mode);
 
            if (mode == ExecutionMode.Management)
            {
                _properties.ReadOnly = true;
            }
        }
    }
}
