﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Actions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.ThreatEventList
{
    public partial class ThreatEventListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, 
        IDesktopAlertAwareExtension, IPanelOpenerExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private GridCell _lastMouseOverCell;
        private ExecutionMode _executionMode;

        public ThreatEventListPanel()
        {
            InitializeComponent();

            _properties.OpenDiagram += OpenDiagram;
 
            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<ThreatEventListFilter>().ToArray());
            _specialFilter.SelectedIndex = 0;

            UndoRedoManager.Undone += RefreshOnUndoRedo;
            UndoRedoManager.Redone += RefreshOnUndoRedo;
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
            _model.ChildRemoved += ModelChildRemoved;
            _model.ThreatEventAdded += OnThreatEventAdded;
            _model.ThreatEventRemoved += OnThreatEventRemoved;
            _model.ThreatEventAddedToEntity += OnThreatEventAdded;
            _model.ThreatEventRemovedFromEntity += OnThreatEventRemoved;
            _model.ThreatEventAddedToDataFlow += OnThreatEventAdded;
            _model.ThreatEventRemovedFromDataFlow += OnThreatEventRemoved;
            _model.VulnerabilityAdded += OnVulnerabilityAdded;
            _model.VulnerabilityRemoved += OnVulnerabilityRemoved;
            _model.VulnerabilityAddedToEntity += OnVulnerabilityAdded;
            _model.VulnerabilityRemovedFromEntity += OnVulnerabilityRemoved;

            if (_model is IUndoable undoable && undoable.IsUndoEnabled)
            {
                undoable.Undone += ModelUndone;
            }

            InitializeGrid();
            LoadModel();
        }

        [Dispatched]
        private void OnThreatEventRemoved([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            var row = GetRow(threatEvent);
            if (row != null)
            {
                RemoveEventSubscriptions(threatEvent, row);

                var panel = row.GridPanel;
                panel.Rows.Remove(row);

                UpdateThreatTypeSeverity(panel);
            }
        }

        [Dispatched]
        private void OnThreatEventAdded([NotNull] IThreatEventsContainer container, [NotNull] IThreatEvent threatEvent)
        {
            var filter = _filter.Text;
            var filterSpecial = EnumExtensions.GetEnumValue<ThreatEventListFilter>((string)_specialFilter.SelectedItem);
            if (IsSelected(threatEvent, filter, filterSpecial))
            {
                AddGridRow(threatEvent);
            }
        }

        [Dispatched]
        private void ModelChildRemoved(IIdentity identity)
        {
            if (identity is IThreatType threatType)
            {
                var row = GetRow(threatType);
                if (row != null)
                {
                    RemoveEventSubscriptions(threatType, row);
                    var panel = row.GridPanel;
                    panel.Rows.Remove(row);

                    if (panel.Rows.Count == 0)
                    {
                        _grid.PrimaryGrid.Rows.Remove(panel.Parent);
                    }
                }
            }
        }

        [Dispatched]
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
                    var filter = _filter.Text;
                    var filterSpecial = EnumExtensions.GetEnumValue<ThreatEventListFilter>((string)_specialFilter.SelectedItem);
                    var threatEvents = GetThreatEvents(filter, filterSpecial);
                    var list = new List<IThreatEvent>();
                    if (threatEvents?.Any() ?? false)
                    {
                        list.AddRange(threatEvents);
                    }

                    var grid = _grid.PrimaryGrid;
                    var threatTypeRows = grid.Rows.OfType<GridRow>().ToArray();
                    if (threatTypeRows.Any())
                    {
                        foreach (var ttRow in threatTypeRows)
                        {
                            if (ttRow.Tag is IThreatType threatType)
                            {
                                var panel = ttRow.Rows.OfType<GridPanel>()
                                    .FirstOrDefault(x => string.CompareOrdinal(x.Name, "ThreatEvents") == 0);
                                var threatEventRows = panel?.Rows.OfType<GridRow>().ToArray();
                                if (threatEventRows?.Any() ?? false)
                                {
                                    foreach (var teRow in threatEventRows)
                                    {
                                        if (teRow.Tag is IThreatEvent threatEvent)
                                        {
                                            if (list.Contains(threatEvent))
                                            {
                                                list.Remove(threatEvent);
                                            }
                                            else
                                            {
                                                RemoveEventSubscriptions(threatEvent, teRow);
                                                panel.Rows.Remove(teRow);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (list.Any())
                    {
                        foreach (var i in list)
                        {
                            var ttRow = GetRow(i.ThreatType);
                            if (ttRow == null)
                            {
                                ttRow = AddGridRow(i.ThreatType, new[] { i }, grid);
                            }
                            var panel = ttRow.Rows.OfType<GridPanel>()
                                .FirstOrDefault(x => string.CompareOrdinal(x.Name, "ThreatEvents") == 0);
                            if (panel != null)
                                AddGridRow(i, panel);
                        }
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
                panel.ReadOnly = _executionMode > ExecutionMode.Simplified;

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
                var filterSpecial = EnumExtensions.GetEnumValue<ThreatEventListFilter>((string)_specialFilter.SelectedItem);
                var threatEvents = GetThreatEvents(filter, filterSpecial);
                var threatTypes = threatEvents?
                    .Select(x => x.ThreatType)
                    .Distinct(new ThreatTypeComparer())
                    .OrderBy(x => x.Name);

                if (threatTypes?.Any() ?? false)
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
                }
            }
            catch (InvalidOperationException)
            {
                ShowWarning?.Invoke("An error occurred while loading the Threat Events. The Panel will be closed now. Please, try again.");
                this.PanelContainer?.Close();
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

        private void AddThreatEvents([NotNull] List<IThreatEvent> threatEvents, 
            IEnumerable<IThreatEvent>[] array,
            string filter,
            ThreatEventListFilter filterSpecial)
        {
            if (array?.Any() ?? false)
            {
                foreach (var threats in array)
                {
                    var list = threats?.Where(x => IsSelected(x, filter, filterSpecial)).ToArray();
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

        [Dispatched]
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
                    ReadOnly = _executionMode > ExecutionMode.Simplified
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

            threatEvent.ScenarioAdded += OnScenarioAdded;
            threatEvent.ScenarioRemoved += OnScenarioRemoved;
            threatEvent.ThreatEventMitigationAdded += OnThreatEventMitigationAdded;
            threatEvent.ThreatEventMitigationRemoved += OnThreatEventMitigationRemoved;
            threatEvent.VulnerabilityAdded += OnVulnerabilityAdded;
            threatEvent.VulnerabilityRemoved += OnVulnerabilityRemoved;
 
            var subPanel = CreateThreatEventScenariosPanel(threatEvent);
            if (subPanel != null)
                row.Rows.Add(subPanel);
   
            var subPanel2 = CreateThreatEventMitigationsPanel(threatEvent);
            if (subPanel2 != null)
                row.Rows.Add(subPanel2);

            var subPanel3 = CreateVulnerabilitiesPanel(threatEvent);
            if (subPanel3 != null)
                row.Rows.Add(subPanel3);

            UpdateThreatTypeSeverity(row);

            return row;
        }
        
        [Dispatched]
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
            try
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
            catch
            {
                // Ignore
            }
        }

        [Dispatched]
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

        [Dispatched]
        private void OnThreatEventParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IIdentity identity && string.CompareOrdinal(e.PropertyName, "Name") == 0)
            {
                var rows = GetRowsAssociatedTo(identity);
                if (rows?.Any() ?? false)
                {
                    foreach (var row in rows)
                    {
                        row["Parent"].Value = identity.Name;
                    }
                }
            }
        }

        [Dispatched]
        private void OnScenarioAdded(IThreatEventScenariosContainer container, IThreatEventScenario scenario)
        {
            if (container is IThreatEvent threatEvent)
            {
                var filterSpecial = EnumExtensions.GetEnumValue<ThreatEventListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(threatEvent, _filter.Text, filterSpecial))
                {
                    var row = GetRow(threatEvent);
                    if (row == null)
                    {
                        var typeRow = GetRow(threatEvent.ThreatType);
                        if (typeRow == null)
                            typeRow = AddGridRow(threatEvent.ThreatType, new IThreatEvent[] {threatEvent}, _grid.PrimaryGrid);
                        
                        var panel = typeRow.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Scenarios") == 0);
                        if (panel != null)
                        {
                            AddGridRow(threatEvent, panel);
                        }
                        else
                        {
                            // We should not come here.
                            panel = CreateThreatEventsPanel(threatEvent.ThreatType,
                                new IThreatEvent[] {threatEvent});
                            if (panel != null)
                            {
                                typeRow.Rows.Add(panel);
                            }
                        }
                    }
                    else
                    {
                        var scenarioPanel = row.Rows.OfType<GridPanel>()
                            .FirstOrDefault(x => string.CompareOrdinal(x.Name, "Scenarios") == 0);
                        if (scenarioPanel == null)
                        {
                            scenarioPanel = CreateThreatEventScenariosPanel(threatEvent);
                            if (scenarioPanel != null)
                                row.Rows.Add(scenarioPanel);
                        }
                        else
                        {
                            AddGridRow(scenario, scenarioPanel);
                        }
                    }
                }
            }
        }

        [Dispatched]
        private void OnScenarioRemoved(IThreatEventScenariosContainer container, IThreatEventScenario scenario)
        {
            if (container is IThreatEvent threatEvent)
            {
                var row = GetRow(threatEvent);

                var panel = row.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Scenarios") == 0);
                var scenarioRow = panel?.Rows.OfType<GridRow>()
                    .FirstOrDefault(x =>
                        (x.Tag is IThreatEventScenario eventScenario) && eventScenario.Id == scenario.Id);
                if (scenarioRow != null)
                {
                    RemoveEventSubscriptions(scenario, scenarioRow);
                    panel.Rows.Remove(scenarioRow);

                    if (panel.Rows.Count == 0)
                        row.Rows.Remove(panel);
                }
            }
        }

        [Dispatched]
        private void OnThreatEventMitigationAdded(IThreatEventMitigationsContainer container, IThreatEventMitigation mitigation)
        {
            if (container is IThreatEvent threatEvent)
            {
                var filterSpecial = EnumExtensions.GetEnumValue<ThreatEventListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(threatEvent, _filter.Text, filterSpecial))
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

        [Dispatched]
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
                    RemoveEventSubscriptions(mitigation, mitigationRow);

                    panel.Rows.Remove(mitigationRow);
  
                    if (panel.Rows.Count == 0)
                        row.Rows.Remove(panel);
                }

                UpdateMitigationLevel(threatEvent, row);
            }
        }

        [Dispatched]
        private void OnVulnerabilityAdded(IVulnerabilitiesContainer container, IVulnerability vulnerability)
        {
            if (container is IThreatEvent threatEvent)
            {
                var filterSpecial = EnumExtensions.GetEnumValue<ThreatEventListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(threatEvent, _filter.Text, filterSpecial))
                {
                    var row = GetRow(threatEvent);
                    if (row == null)
                    {
                        var typeRow = GetRow(threatEvent.ThreatType);
                        if (typeRow == null)
                            typeRow = AddGridRow(threatEvent.ThreatType, new IThreatEvent[] { threatEvent }, _grid.PrimaryGrid);

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
                                new IThreatEvent[] { threatEvent });
                            if (panel != null)
                            {
                                typeRow.Rows.Add(panel);
                                row = panel.Rows.OfType<GridRow>().FirstOrDefault();
                            }
                        }
                    }
                    else
                    {
                        var vulnerabilitiesPanel = row.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Vulnerabilities") == 0);
                        if (vulnerabilitiesPanel != null)
                        {
                            AddGridRow(vulnerability, vulnerabilitiesPanel);
                        }
                        else
                        {
                            vulnerabilitiesPanel = CreateVulnerabilitiesPanel(threatEvent);
                            if (vulnerabilitiesPanel != null)
                                row.Rows.Add(vulnerabilitiesPanel);
                        }
                    }

                    if (row != null)
                    {
                        UpdateMitigationLevel(threatEvent, row);
                    }
                }
            }
        }

        [Dispatched]
        private void OnVulnerabilityRemoved(IVulnerabilitiesContainer container, IVulnerability vulnerability)
        {
            if (container is IThreatEvent threatEvent)
            {
                var row = GetRow(threatEvent);

                var panel = row?.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Vulnerabilities") == 0);
                var vulnerabilityRow = panel?.Rows.OfType<GridRow>()
                    .FirstOrDefault(x =>
                        (x.Tag is IVulnerability eventVulnerability) && eventVulnerability.Id == vulnerability.Id);
                if (vulnerabilityRow != null)
                {
                    RemoveEventSubscriptions(vulnerability, vulnerabilityRow);

                    panel.Rows.Remove(vulnerabilityRow);

                    if (panel.Rows.Count == 0)
                        row.Rows.Remove(panel);
                }

                UpdateMitigationLevel(threatEvent, row);
            }
        }

        private void AddGridRow([NotNull] IThreatEvent threatEvent)
        {
            var filterSpecial = EnumExtensions.GetEnumValue<ThreatEventListFilter>((string)_specialFilter.SelectedItem);
            if (IsSelected(threatEvent, _filter.Text, filterSpecial) && GetRow(threatEvent) == null)
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

        #region Threat Event Scenario Level.
        private GridPanel CreateThreatEventScenariosPanel([NotNull] IThreatEvent threatEvent)
        {
            GridPanel result = null;

            var scenarios = threatEvent.Scenarios?
                .OrderBy(x => x.Name)
                .ToArray();
            if (!string.IsNullOrWhiteSpace(threatEvent.Name) && (scenarios?.Any() ?? false))
            {
                result = new GridPanel
                {
                    Name = "Scenarios",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowRowHeaders = false,
                    InitialSelection = RelativeSelection.None,
                    ReadOnly = _executionMode > ExecutionMode.Simplified
                };
                result.DefaultVisualStyles.CellStyles.ReadOnly.TextColor = Color.Black;

                result.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Scenario Name",
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

                result.Columns.Add(new GridColumn("Severity")
                {
                    HeaderText = "Severity",
                    DataType = typeof(ISeverity),
                    EditorType = typeof(SeverityComboBox),
                    EditorParams = new object[] { _model.Severities },
                    AllowEdit = true,
                    Width = 75
                });

                result.Columns.Add(new GridColumn("Actor")
                {
                    HeaderText = "Threat Actor",
                    DataType = typeof(IThreatActor),
                    EditorType = typeof(ThreatActorComboBox),
                    EditorParams = new object[] { _model.ThreatActors },
                    AllowEdit = true,
                    Width = 150
                });

                result.Columns.Add(new GridColumn("Motivation")
                {
                    HeaderText = "Motivation",
                    Width = 300,
                    DataType = typeof(string),
                    EditorType = typeof(GridTextBoxDropDownEditControl),
                    AllowEdit = true
                });
                result.Columns["Motivation"].CellStyles.Default.AllowMultiLine = Tbool.True;
                ddc = result.Columns["Motivation"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }

                foreach (var scenario in scenarios)
                {
                    AddGridRow(scenario, result);
                }
            }

            return result;
        }

        private void AddGridRow([NotNull] IThreatEventScenario scenario, [NotNull] GridPanel panel)
        {
            GridRow row = new GridRow(
                scenario.Name,
                scenario.Severity,
                scenario.Actor,
                scenario.Motivation)
            {
                Tag = scenario
            };
            row.Cells[0].CellStyles.Default.Image = scenario.GetImage(ImageSize.Small);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnScenarioCellChanged;
            panel.Rows.Add(row);
            ((INotifyPropertyChanged) scenario).PropertyChanged += OnScenarioPropertyChanged;
        }

        [Dispatched]
        private void OnScenarioPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEventScenario scenario)
            {
                var row = GetRow(scenario);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = scenario.Name;
                            break;
                        case "Severity":
                            row["Severity"].Value = scenario.Severity;
                            break;
                        case "Actor":
                            row["Actor"].Value = scenario.Actor;
                            break;
                        case "Motivation":
                            row["Motivation"].Value = scenario.Motivation;
                            break;
                    }
                }
            }
        }

        private void OnScenarioCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var cell = sender as GridCell;
            var propertyName = propertyChangedEventArgs.PropertyName;

            if (!_loading && cell != null)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    var scenario = row.Tag as IThreatEventScenario;
                    if (scenario != null)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                if (cell.Value is string name)
                                    scenario.Name = name;
                                break;
                            case "Severity":
                                if (cell.Value is ISeverity severity)
                                    scenario.Severity = severity;
                                break;
                            case "Actor":
                                if (cell.Value is IThreatActor actor)
                                    scenario.Actor = actor;
                                break;
                            case "Motivation":
                                if (cell.Value is string motivation)
                                    scenario.Motivation = motivation;
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
                    ReadOnly = _executionMode > ExecutionMode.Simplified
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
        
        [Dispatched]
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
       
        [Dispatched]
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

        #region Vulnerabilities Level.
        private GridPanel CreateVulnerabilitiesPanel([NotNull] IThreatEvent threatEvent)
        {
            GridPanel result = null;

            var vulnerabilities = threatEvent.Vulnerabilities?
                .OrderBy(x => x.Name)
                .ToArray();
            if (!string.IsNullOrWhiteSpace(threatEvent.Name) && (vulnerabilities?.Any() ?? false))
            {
                result = new GridPanel
                {
                    Name = "Vulnerabilities",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowRowHeaders = false,
                    InitialSelection = RelativeSelection.None,
                    ReadOnly = _executionMode > ExecutionMode.Simplified
                };
                result.DefaultVisualStyles.CellStyles.ReadOnly.TextColor = Color.Black;

                result.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Vulnerability Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    AllowEdit = false
                });
                GridTextBoxDropDownEditControl ddc = result.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }

                result.Columns.Add(new GridColumn("Severity")
                {
                    HeaderText = "Severity",
                    DataType = typeof(ISeverity),
                    EditorType = typeof(SeverityComboBox),
                    EditorParams = new object[] { _model.Severities?.Where(x => x.Visible) },
                    AllowEdit = true,
                    Width = 75
                });

                foreach (var vulnerability in vulnerabilities)
                {
                    AddGridRow(vulnerability, result);
                }
            }

            return result;
        }

        private void AddGridRow([NotNull] IVulnerability vulnerability, [NotNull] GridPanel panel)
        {
            GridRow row = new GridRow(
                vulnerability.Name,
                vulnerability.Severity)
            {
                Tag = vulnerability
            };
            UpdateMitigationLevel(vulnerability, row);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnVulnerabilityCellChanged;
            panel.Rows.Add(row);
            ((INotifyPropertyChanged)vulnerability).PropertyChanged += OnVulnerabilityPropertyChanged;

            vulnerability.VulnerabilityMitigationAdded += OnVulnerabilityMitigationAdded;
            vulnerability.VulnerabilityMitigationRemoved += OnVulnerabilityMitigationRemoved;

            var subPanel = CreateVulnerabilityMitigationsPanel(vulnerability);
            if (subPanel != null)
                row.Rows.Add(subPanel);
        }

        private static void UpdateMitigationLevel([NotNull] IVulnerability vulnerability, [NotNull] GridRow row)
        {
            try
            {
                switch (vulnerability.GetMitigationLevel())
                {
                    case MitigationLevel.NotMitigated:
                        row.Cells[0].CellStyles.Default.Image = Resources.weakness_circle_red_small;
                        break;
                    case MitigationLevel.Partial:
                        row.Cells[0].CellStyles.Default.Image = Resources.weakness_circle_orange_small;
                        break;
                    case MitigationLevel.Complete:
                        row.Cells[0].CellStyles.Default.Image = Resources.weakness_circle_green_small;
                        break;
                }
            }
            catch
            {
                // Ignore
            }
        }

        [Dispatched]
        private void OnVulnerabilityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IVulnerability vulnerability)
            {
                var row = GetRow(vulnerability);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Severity":
                            row["Severity"].Value = vulnerability.Severity;
                            break;
                    }
                }
            }
        }

        private void OnVulnerabilityCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var cell = sender as GridCell;
            var propertyName = propertyChangedEventArgs.PropertyName;

            if (!_loading && cell != null)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    var vulnerability = row.Tag as IVulnerability;
                    if (vulnerability != null)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Severity":
                                if (cell.Value is ISeverity severity)
                                    vulnerability.Severity = severity;
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
        
        [Dispatched]
        private void OnVulnerabilityMitigationAdded(IVulnerabilityMitigationsContainer container, IVulnerabilityMitigation mitigation)
        {
            if (container is IVulnerability vulnerability && vulnerability.Parent is IThreatEvent threatEvent)
            {
                var filterSpecial = EnumExtensions.GetEnumValue<ThreatEventListFilter>((string)_specialFilter.SelectedItem);
                if (IsSelected(threatEvent, _filter.Text, filterSpecial))
                {
                    var row = GetRow(vulnerability);
                    if (row == null)
                    {
                        OnVulnerabilityAdded(threatEvent, vulnerability);
                        row = GetRow(vulnerability);
                    }

                    if (row != null)
                    {
                        var mitigationPanel = row.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Mitigations") == 0);
                        if (mitigationPanel != null)
                        {
                            AddGridRow(mitigation, mitigationPanel);
                        }
                        else
                        {
                            mitigationPanel = CreateVulnerabilityMitigationsPanel(vulnerability);
                            if (mitigationPanel != null)
                                row.Rows.Add(mitigationPanel);
                        }
                    }

                    if (row != null)
                    {
                        UpdateMitigationLevel(vulnerability, row);
                    }
                }
            }
        }

        [Dispatched]
        private void OnVulnerabilityMitigationRemoved(IVulnerabilityMitigationsContainer container, IVulnerabilityMitigation mitigation)
        {
            if (container is IVulnerability vulnerability)
            {
                var row = GetRow(vulnerability);

                var panel = row?.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Mitigations") == 0);
                var mitigationRow = panel?.Rows.OfType<GridRow>()
                    .FirstOrDefault(x =>
                        (x.Tag is IVulnerabilityMitigation vulnMitigation) && vulnMitigation.MitigationId == mitigation.MitigationId);
                if (mitigationRow != null)
                {
                    RemoveEventSubscriptions(mitigation, mitigationRow);

                    panel.Rows.Remove(mitigationRow);

                    if (panel.Rows.Count == 0)
                        row.Rows.Remove(panel);
                }

                UpdateMitigationLevel(vulnerability, row);
            }
        }
        #endregion

        #region Vulnerability Mitigation Level.
        private GridPanel CreateVulnerabilityMitigationsPanel([NotNull] IVulnerability vulnerability)
        {
            GridPanel result = null;

            var mitigations = vulnerability.Mitigations?
                .OrderBy(x => x.Mitigation.Name)
                .ToArray();
            if (!string.IsNullOrWhiteSpace(vulnerability.Name) && (mitigations?.Any() ?? false))
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
                    ReadOnly = _executionMode > ExecutionMode.Simplified
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

        private void AddGridRow([NotNull] IVulnerabilityMitigation mitigation, [NotNull] GridPanel panel)
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
                row.Cells[i].PropertyChanged += OnVulnerabilityMitigationCellChanged;
            panel.Rows.Add(row);
            ((INotifyPropertyChanged)mitigation).PropertyChanged += OnVulnerabilityMitigationPropertyChanged;
            ((INotifyPropertyChanged)mitigation.Mitigation).PropertyChanged += OnMitigationPropertyChanged;
        }

        [Dispatched]
        private void OnVulnerabilityMitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IVulnerabilityMitigation mitigation)
            {
                var row = GetRow(mitigation);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Strength":
                            row["Strength"].Value = mitigation.Strength;
                            if (mitigation.Vulnerability is IVulnerability vulnerability)
                            {
                                var vulnRow = GetRow(vulnerability);
                                UpdateMitigationLevel(vulnerability, vulnRow);
                            }
                            break;
                        case "Status":
                            row["Status"].Value = mitigation.Status.ToString();
                            break;
                    }
                }
            }
        }

        private void OnVulnerabilityMitigationCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var cell = sender as GridCell;
            var propertyName = propertyChangedEventArgs.PropertyName;

            if (!_loading && cell != null)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    var scenario = row.Tag as IVulnerabilityMitigation;
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

        #region Remove events.
        private void RemoveEventSubscriptions()
        {
            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
                RemoveEventSubscriptions(row);

            var ddc = _grid.PrimaryGrid.Rows.OfType<GridPanel>().FirstOrDefault()?.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
            if (ddc != null)
            {
                ddc.ButtonClearClick -= DdcButtonClearClick;
            }
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IThreatType threatType)
            {
                RemoveEventSubscriptions(threatType, row);
            }
            else if (row?.Tag is IThreatEvent threatEvent)
            {
                RemoveEventSubscriptions(threatEvent, row);
            }
            else if (row?.Tag is IThreatEventScenario scenario)
            {
                RemoveEventSubscriptions(scenario, row);
            }
            else if (row?.Tag is IThreatEventMitigation mitigation)
            {
                RemoveEventSubscriptions(mitigation, row);
            }
            else if (row?.Tag is IVulnerability vulnerability)
            {
                RemoveEventSubscriptions(vulnerability, row);
            }
            else if (row?.Tag is IVulnerabilityMitigation vulnMitigation)
            {
                RemoveEventSubscriptions(vulnMitigation, row);
            }
        }

        private void RemoveEventSubscriptions([NotNull] IThreatType threatType, [NotNull] GridRow row)
        {
            if (row == null)
                row = GetRow(threatType);

            if (row != null)
            {
                ((INotifyPropertyChanged)threatType).PropertyChanged -= OnThreatTypePropertyChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnThreatTypeCellChanged;

                var panel = row.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "ThreatEvents") == 0);
                var ddc = panel?.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClearClick -= DdcButtonClearClick;
                }

                RemoveEventSubscriptionsFromChildren(row);
            }
        }

        private void RemoveEventSubscriptions([NotNull] IThreatEvent threatEvent, [NotNull] GridRow row)
        {
            if (row == null)
                row = GetRow(threatEvent);

            if (row != null)
            {
                if (threatEvent.Parent != null)
                {
                    ((INotifyPropertyChanged)threatEvent.Parent).PropertyChanged -= OnThreatEventParentPropertyChanged;
                    if (threatEvent.Parent is IEntity entity)
                        entity.ImageChanged -= OnImageChanged;
                }
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnThreatEventCellChanged;
                ((INotifyPropertyChanged)threatEvent).PropertyChanged -= OnThreatEventPropertyChanged;

                threatEvent.ScenarioAdded -= OnScenarioAdded;
                threatEvent.ScenarioRemoved -= OnScenarioRemoved;
                threatEvent.ThreatEventMitigationAdded -= OnThreatEventMitigationAdded;
                threatEvent.ThreatEventMitigationRemoved -= OnThreatEventMitigationRemoved;
                threatEvent.VulnerabilityAdded -= OnVulnerabilityAdded;
                threatEvent.VulnerabilityRemoved -= OnVulnerabilityRemoved;

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

                panel = row.Rows.OfType<GridPanel>().FirstOrDefault(x => string.CompareOrdinal(x.Name, "Vulnerabilities") == 0);
                ddc = panel?.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClearClick -= DdcButtonClearClick;
                }

                RemoveSuperTooltipProvider(row.Cells["Parent"]);

                RemoveEventSubscriptionsFromChildren(row);
            }
        }

        private void RemoveEventSubscriptions([NotNull] IThreatEventScenario scenario, [NotNull] GridRow row)
        {
            if (row == null)
                row = GetRow(scenario);

            if (row != null)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnScenarioCellChanged;
                ((INotifyPropertyChanged)scenario).PropertyChanged -= OnScenarioPropertyChanged;
            }
        }

        private void RemoveEventSubscriptions([NotNull] IThreatEventMitigation mitigation, [NotNull] GridRow row)
        {
            if (row == null)
                row = GetRow(mitigation);

            if (row != null)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnMitigationCellChanged;
                ((INotifyPropertyChanged)mitigation).PropertyChanged -= OnThreatEventMitigationPropertyChanged;
                ((INotifyPropertyChanged)mitigation.Mitigation).PropertyChanged -= OnMitigationPropertyChanged;
            }

        }

        private void RemoveEventSubscriptions([NotNull] IVulnerability vulnerability, [NotNull] GridRow row)
        {
            if (row == null)
                row = GetRow(vulnerability);

            if (row != null)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnVulnerabilityCellChanged;
                ((INotifyPropertyChanged)vulnerability).PropertyChanged -= OnVulnerabilityPropertyChanged;

                vulnerability.VulnerabilityMitigationAdded -= OnVulnerabilityMitigationAdded;
                vulnerability.VulnerabilityMitigationRemoved -= OnVulnerabilityMitigationRemoved;

                RemoveEventSubscriptionsFromChildren(row);
            }

        }

        private void RemoveEventSubscriptions([NotNull] IVulnerabilityMitigation vulnMitigation, [NotNull] GridRow row)
        {
            if (row == null)
                row = GetRow(vulnMitigation);

            if (row != null)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnVulnerabilityMitigationCellChanged;
                ((INotifyPropertyChanged)vulnMitigation).PropertyChanged -= OnVulnerabilityMitigationPropertyChanged;
                ((INotifyPropertyChanged)vulnMitigation.Mitigation).PropertyChanged -= OnVulnerabilityMitigationPropertyChanged;
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
        #endregion

        #region Auxiliary private members.
        private IEnumerable<IThreatEvent> GetThreatEvents(string filter, ThreatEventListFilter filterSpecial)
        {
            IEnumerable<IThreatEvent> result = null;

            List<IThreatEvent> threatEvents = new List<IThreatEvent>();
            AddThreatEvents(threatEvents,
                _model.Entities?.Select(x => x.ThreatEvents).ToArray(),
                filter, filterSpecial);
            AddThreatEvents(threatEvents, _model.DataFlows?.Select(x => x.ThreatEvents).ToArray(),
                filter, filterSpecial);
            var modelThreats = _model.ThreatEvents?
                .Where(x => IsSelected(x, filter, filterSpecial)).ToArray();
            if (modelThreats?.Any() ?? false)
                threatEvents.AddRange(modelThreats);

            if (threatEvents.Any())
                result = threatEvents.AsEnumerable();

            return result;
        }

        private void DdcButtonClearClick(object sender, CancelEventArgs e)
        {
            GridTextBoxDropDownEditControl ddc =
                sender as GridTextBoxDropDownEditControl;

            if (ddc != null)
            {
                ddc.Text = null;
                e.Cancel = true;
            }
        }

        private bool IsSelected([NotNull] IThreatEvent item, string filter, ThreatEventListFilter filterSpecial)
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
                    case ThreatEventListFilter.NoMitigations:
                        result = !(item.Mitigations?.Any() ?? false);
                        break;
                    case ThreatEventListFilter.DifferentNameDescription:
                        result = (string.CompareOrdinal(item.Name, item.ThreatType.Name) != 0) ||
                                 (string.CompareOrdinal(item.Description, item.ThreatType.Description) != 0);
                        break;
                    case ThreatEventListFilter.DifferentSeverity:
                        result = (item.SeverityId != item.ThreatType.SeverityId);
                        break;
                    case ThreatEventListFilter.SameSeverity:
                        result = (item.SeverityId == item.ThreatType.SeverityId);
                        break;
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

        private GridRow GetRow([NotNull] IThreatEventScenario scenario)
        {
            GridRow result = null;

            var parentRow = GetRow(scenario.ThreatEvent);
            if (parentRow != null)
            {
                var panels = parentRow.Rows.OfType<GridPanel>().ToArray();
                if (panels.Any())
                {
                    foreach (var panel in panels)
                    {
                        result = panel.Rows.OfType<GridRow>().FirstOrDefault(x => x.Tag == scenario);
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

        private GridRow GetRow([NotNull] IVulnerability vulnerability)
        {
            GridRow result = null;

            if (vulnerability.Parent is IThreatEvent threatEvent)
            {
                var parentRow = GetRow(threatEvent);
                if (parentRow != null)
                {
                    var panels = parentRow.Rows.OfType<GridPanel>().ToArray();
                    if (panels.Any())
                    {
                        foreach (var panel in panels)
                        {
                            result = panel.Rows.OfType<GridRow>().FirstOrDefault(x => x.Tag == vulnerability);
                            if (result != null)
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private GridRow GetRow([NotNull] IVulnerabilityMitigation mitigation)
        {
            GridRow result = null;

            var parentRow = GetRow(mitigation.Vulnerability);
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
                                if ((leaf.Tag is IThreatEventMitigation tem && tem.MitigationId == mitigation.Id) ||
                                    (leaf.Tag is IVulnerabilityMitigation vm && vm.MitigationId == mitigation.Id))
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

            bool isThreatType = _currentRow?.Tag is IThreatType;
            bool isMitigation = _currentRow?.Tag is IThreatEventMitigation;
            bool isThreatEvent = _currentRow?.Tag is IThreatEvent;
            bool isScenario = _currentRow?.Tag is IThreatEventScenario;
            bool isVulnerability = _currentRow?.Tag is IVulnerability;
            var isVMitigation = _currentRow?.Tag is IVulnerabilityMitigation;

            ChangeCustomActionStatus?.Invoke("AddScenario", isThreatEvent);
            ChangeCustomActionStatus?.Invoke("AddMitigation", isThreatEvent || isVulnerability);
            ChangeCustomActionStatus?.Invoke("AddVulnerability", isThreatEvent);
            ChangeCustomActionStatus?.Invoke("RemoveThreatEvent", isThreatEvent);
            ChangeCustomActionStatus?.Invoke("RemoveScenario", isScenario);
            ChangeCustomActionStatus?.Invoke("RemoveMitigation", isMitigation || isVMitigation);
            ChangeCustomActionStatus?.Invoke("RemoveVulnerability", isVulnerability);
            ChangeCustomActionStatus?.Invoke("ChangeSeverities", isThreatEvent || isScenario || isVulnerability);
            ChangeActionsStatus();
        }

        private void ChangeActionsStatus()
        {
            if (_commandsBarContextAwareActions?.Any() ?? false)
            {
                var isThreatType = _currentRow?.Tag is IThreatType;
                var isThreatEvent = _currentRow?.Tag is IThreatEvent;
                var isTEMitigation = _currentRow?.Tag is IThreatEventMitigation;
                var isScenario = _currentRow?.Tag is IThreatEventScenario;
                var isVulnerability = _currentRow?.Tag is IVulnerability;
                var isVMitigation = _currentRow?.Tag is IVulnerabilityMitigation;

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
                                    if (action.Tag is IIdentitiesContextAwareAction identitiesContextAwareAction)
                                    {
                                        if (isThreatType &&
                                            (identitiesContextAwareAction.Scope & Scope.ThreatType) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else if (isThreatEvent &&
                                            (identitiesContextAwareAction.Scope & Scope.ThreatEvent) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else if (isScenario &&
                                            (identitiesContextAwareAction.Scope & Scope.ThreatEventScenario) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else if (isVulnerability &&
                                            (identitiesContextAwareAction.Scope & Scope.Vulnerability) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, false);
                                        }
                                    }
                                    else if (action.Tag is IPropertiesContainersContextAwareAction pcContextAwareAction)
                                    {
                                        if (isThreatType &&
                                            (pcContextAwareAction.Scope & Scope.ThreatType) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else if (isThreatEvent &&
                                            (pcContextAwareAction.Scope & Scope.ThreatEvent) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else if (isTEMitigation &&
                                            (pcContextAwareAction.Scope & Scope.ThreatEventMitigation) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else if (isScenario &&
                                            (pcContextAwareAction.Scope & Scope.ThreatEventScenario) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else if (isVulnerability &&
                                            (pcContextAwareAction.Scope & Scope.Vulnerability) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else if (isVMitigation &&
                                            (pcContextAwareAction.Scope & Scope.VulnerabilityMitigation) != 0)
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, true);
                                        }
                                        else
                                        {
                                            ChangeCustomActionStatus?.Invoke(action.Name, false);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
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
                if (row?.Tag is IThreatEventScenario)
                {
                    MenuDefinition.UpdateVisibility(_scenarioMenu, row.Tag);
                    _scenarioMenu?.Show(_grid.PointToScreen(e.Location));
                }
                if (row?.Tag is IThreatEventMitigation)
                {
                    MenuDefinition.UpdateVisibility(_threatEventMitigationMenu, row.Tag);
                    _threatEventMitigationMenu?.Show(_grid.PointToScreen(e.Location));
                }
                if (row?.Tag is IVulnerability)
                {
                    MenuDefinition.UpdateVisibility(_vulnerabilityMenu, row.Tag);
                    _vulnerabilityMenu?.Show(_grid.PointToScreen(e.Location));
                }
                if (row?.Tag is IVulnerabilityMitigation)
                {
                    MenuDefinition.UpdateVisibility(_vulnerabilityMitigationMenu, row.Tag);
                    _vulnerabilityMitigationMenu?.Show(_grid.PointToScreen(e.Location));
                }
            }
        }
        #endregion

        #region Other Form Events.
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
        #endregion

        #region Execution Mode.
        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
            _properties.SetExecutionMode(mode);
 
            if (mode == ExecutionMode.Management)
            {
                _properties.ReadOnly = true;
            }
        }
        #endregion
    }
}
