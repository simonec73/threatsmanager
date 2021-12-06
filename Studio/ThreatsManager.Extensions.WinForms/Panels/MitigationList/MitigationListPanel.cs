using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.MitigationList
{
    public partial class MitigationListPanel : UserControl, IShowThreatModelPanel<Form>, 
        ICustomRibbonExtension, IInitializableObject, IContextAwareExtension, 
        IDesktopAlertAwareExtension, IPanelOpenerExtension, IExecutionModeSupport
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private GridRow _currentRow;
        private bool _loading;
        private GridCell _lastMouseOverCell;
        private ExecutionMode _executionMode = ExecutionMode.Expert;

        public MitigationListPanel()
        {
            InitializeComponent();

            _properties.OpenDiagram += OpenDiagram;

            _specialFilter.Items.AddRange(EnumExtensions.GetEnumLabels<MitigationListFilter>().ToArray());
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

            InitializeGrid();
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

        #region Mitigations level.
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
                    HeaderText = "Mitigation Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                panel.Columns.Add(new GridColumn("ControlType")
                {
                    HeaderText = "Control Type",
                    DataType = typeof(string),
                    EditorType = typeof(EnumComboBox),
                    EditorParams = new object[] { EnumExtensions.GetEnumLabels<SecurityControlType>() },
                    AllowEdit = false,
                    Width = 75
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

                List<IThreatEvent> threatEvents = new List<IThreatEvent>();
                AddThreatEvents(threatEvents, _model.Entities?.Select(x => x.ThreatEvents).ToArray());
                AddThreatEvents(threatEvents, _model.DataFlows?.Select(x => x.ThreatEvents).ToArray());
                var modelThreats = _model.ThreatEvents?.ToArray();
                if (modelThreats?.Any() ?? false)
                    threatEvents.AddRange(modelThreats);

                foreach (var threatEvent in threatEvents)
                {
                    ((INotifyPropertyChanged) threatEvent).PropertyChanged += OnThreatEventPropertyChanged;
                }

                AddThreatEventEvents(_model.Entities);
                AddThreatEventEvents(_model.DataFlows);
                AddThreatEventEvents(_model);

                AddMitigations(threatEvents, panel);
            }
            finally
            {
                _loading = false;
                _grid.ResumeLayout(true);
            }
        }

        private void AddMitigations(IEnumerable<IThreatEvent> threatEvents, GridPanel panel)
        {
            var mitigations = new Dictionary<IMitigation, List<IThreatEventMitigation>>();

            var eventsMitigations = threatEvents.Select(x => x.Mitigations).ToArray();
            foreach (var em in eventsMitigations)
            {
                var emArray = em?.ToArray();
                if (emArray?.Any() ?? false)
                {
                    foreach (var m in emArray)
                    {
                        List<IThreatEventMitigation> mList;
                        if (mitigations.ContainsKey(m.Mitigation))
                            mList = mitigations[m.Mitigation];
                        else
                        {
                            mList = new List<IThreatEventMitigation>();
                            mitigations.Add(m.Mitigation, mList);
                        }

                        mList.Add(m);
                    }
                }
            }

            var filter = _filter.Text;
            var filterSpecial = EnumExtensions.GetEnumValue<MitigationListFilter>((string)_specialFilter.SelectedItem);

            if (mitigations.Any())
            {
                var sorted = mitigations.Keys.OrderBy(x => x.Name).ToArray();
                foreach (var mitigation in sorted)
                {
                    var sortedMitigations = mitigations[mitigation]?
                        .OrderBy(x => x.ThreatEvent.Name)
                        .ThenBy(x => x.ThreatEvent.Parent.Name)
                        .ToArray();
                    if (IsSelected(mitigation, sortedMitigations, filter, filterSpecial))
                    {
                        var row = GetRow(mitigation);
                        if (row == null)
                            AddGridRow(mitigation, sortedMitigations, panel);
                        else
                        {
                            var subPanel = row.Rows.OfType<GridPanel>().FirstOrDefault(x =>
                                string.CompareOrdinal(x.Name, "ThreatEventMitigations") == 0);
                            if (subPanel != null)
                            {
                                foreach (var m in sortedMitigations)
                                {
                                    if (GetRow(m) == null)
                                        AddGridRow(m, subPanel);
                                }
                            }
                            else
                            {
                                subPanel = CreateThreatEventsPanel(mitigation, sortedMitigations);
                                if (subPanel != null)
                                    row.Rows.Add(subPanel);
                            }
                        }
                    }
                }

                _currentRow = panel.Rows.OfType<GridRow>().FirstOrDefault();
            }
        }

        private void AddThreatEventEvents(IEnumerable<IThreatEventsContainer> containers)
        {
            if (containers?.Any() ?? false)
            {
                foreach (var container in containers)
                    AddThreatEventEvents(container);
            }
        }

        private void AddThreatEventEvents(IThreatEventsContainer container)
        {
            if (container != null)
            {
                container.ThreatEventAdded += OnThreatEventAdded;
                container.ThreatEventRemoved += OnThreatEventRemoved;
            }
        }

        private void RemoveThreatEventEvents(IEnumerable<IThreatEventsContainer> containers)
        {
            if (containers?.Any() ?? false)
            {
                foreach (var container in containers)
                    RemoveThreatEventEvents(container);
            }
        }

        private void RemoveThreatEventEvents(IThreatEventsContainer container)
        {
            if (container != null)
            {
                container.ThreatEventAdded -= OnThreatEventAdded;
                container.ThreatEventRemoved -= OnThreatEventRemoved;
            }
        }

        private void OnThreatEventAdded(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            ((INotifyPropertyChanged) threatEvent).PropertyChanged += OnThreatEventPropertyChanged;
        }

        private void OnThreatEventRemoved(IThreatEventsContainer container, IThreatEvent threatEvent)
        {
            ((INotifyPropertyChanged) threatEvent).PropertyChanged -= OnThreatEventPropertyChanged;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var panel = row.Rows.OfType<GridPanel>()
                    .FirstOrDefault(x => string.CompareOrdinal("ThreatEventMitigations", x.Name) == 0);
                if (panel != null)
                {
                    var toBeRemoved = panel.Rows.OfType<GridRow>().Where(x =>
                        (x.Tag is IThreatEventMitigation threatEventMitigation) &&
                        threatEventMitigation.ThreatEvent.Id == threatEvent.Id).ToArray();
                    if (toBeRemoved?.Any() ?? false)
                    {
                        foreach (var tbr in toBeRemoved)
                        {
                            RemoveEventSubscriptions(tbr);
                            panel.Rows.Remove(tbr);
                        }
                    }
                }
            }
        }

        [Dispatched]
        private void OnThreatEventPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEvent threatEvent)
            {
                var rows = GetRows(threatEvent)?.ToArray();

                if (rows?.Any() ?? false)
                {
                    foreach (var row in rows)
                    {
                        switch (e.PropertyName)
                        {
                            case "Name":
                                row["Name"].Value = threatEvent.Name;
                                break;
                            case "Severity":
                                row["Severity"].Value = threatEvent.Severity;
                                break;
                        }
                    }
                }
            }
        }

        private IEnumerable<GridRow> GetRows([NotNull] IThreatEvent threatEvent)
        {
            List<GridRow> result = new List<GridRow>();

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                {
                    var panel = row.Rows.OfType<GridPanel>()
                        .FirstOrDefault(x => string.CompareOrdinal(x.Name, "ThreatEventMitigations") == 0);
                    if (panel != null)
                    {
                        var temRows = panel.Rows.OfType<GridRow>().ToArray();
                        if (temRows.Any())
                        {
                            foreach (var temRow in temRows)
                            {
                                if (temRow.Tag is IThreatEventMitigation threatEventMitigation &&
                                    threatEventMitigation.ThreatEvent == threatEvent)
                                {
                                    result.Add(temRow);
                                }
                            }
                        }
                    }
                }
            }

            return result.AsReadOnly();
        }

        private void AddThreatEvents([NotNull] List<IThreatEvent> threatEvents, IEnumerable<IThreatEvent>[] array)
        {
            if (array?.Any() ?? false)
            {
                foreach (var threats in array)
                {
                    var list = threats?.Where(x => x.Mitigations?.Any() ?? false).ToArray();
                    if (list?.Any() ?? false)
                    {
                        threatEvents.AddRange(list);
                    }
                }
            }
        }

        private void AddGridRow([NotNull] IMitigation mitigation, [NotNull] IList<IThreatEventMitigation> threatEventMitigations, [NotNull] GridPanel panel)
        {
            var row = new GridRow(
                mitigation.Name,
                mitigation.ControlType.ToString());
            ((INotifyPropertyChanged) mitigation).PropertyChanged += OnMitigationPropertyChanged;
            row.Tag = mitigation;
            row.Cells[0].CellStyles.Default.Image = mitigation.GetImage(ImageSize.Small);
            panel.Rows.Add(row);
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnMitigationCellChanged;

            var subPanel = CreateThreatEventsPanel(mitigation, threatEventMitigations);
            if (subPanel != null)
                row.Rows.Add(subPanel);
        }

        private void RemoveEventSubscriptions()
        {
            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
                RemoveEventSubscriptions(row);

            List<IThreatEvent> threatEvents = new List<IThreatEvent>();
            AddThreatEvents(threatEvents, _model.Entities?.Select(x => x.ThreatEvents).ToArray());
            AddThreatEvents(threatEvents, _model.DataFlows?.Select(x => x.ThreatEvents).ToArray());
            var modelThreats = _model.ThreatEvents?.ToArray();
            if (modelThreats?.Any() ?? false)
                threatEvents.AddRange(modelThreats);

            foreach (var threatEvent in threatEvents)
            {
                ((INotifyPropertyChanged) threatEvent).PropertyChanged -= OnThreatEventPropertyChanged;
            }

            RemoveThreatEventEvents(_model.Entities);
            RemoveThreatEventEvents(_model.DataFlows);
            RemoveThreatEventEvents(_model);
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is IMitigation mitigation)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnMitigationCellChanged;
                ((INotifyPropertyChanged) mitigation).PropertyChanged -= OnMitigationPropertyChanged;
 
                var panel = row.Rows.OfType<GridPanel>().FirstOrDefault();
                var children = panel?.Rows.OfType<GridRow>().ToArray();
                if (children?.Any() ?? false)
                {
                    foreach (var child in children)
                        RemoveEventSubscriptions(child);
                }

                GridTextBoxDropDownEditControl ddc = panel.Columns["Name"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                    ddc.ButtonClearClick -= DdcButtonClearClick;
            }
            else if (row?.Tag is IThreatEventMitigation tem)
            {
                if (tem.ThreatEvent?.Parent != null)
                {
                    ((INotifyPropertyChanged) tem.ThreatEvent.Parent).PropertyChanged -=
                        OnThreatEventMitigationParentPropertyChanged;
                    if (tem.ThreatEvent.Parent is IEntity entity)
                        entity.ImageChanged -= OnImageChanged;
                }
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnThreatEventMitigationCellChanged;

                ((INotifyPropertyChanged) tem).PropertyChanged -= OnThreatEventMitigationPropertyChanged;
                tem.ThreatEvent.ThreatEventMitigationAdded -= OnThreatEventMitigationAdded;
                tem.ThreatEvent.ThreatEventMitigationRemoved -= OnThreatEventMitigationRemoved;

                RemoveSuperTooltipProvider(row.Cells["Parent"]);
            }
        }

        private void OnMitigationCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_loading && sender is GridCell cell)
            {
                try
                {
                    _loading = true;
                    if (cell.GridRow.Tag is IMitigation mitigation)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                mitigation.Name = (string) cell.Value;
                                break;
                            case "ControlType":
                                mitigation.ControlType = ((string) cell.Value).GetEnumValue<SecurityControlType>();
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
        private void OnMitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IMitigation mitigation)
            {
                var row = GetRow(mitigation);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Name":
                            row["Name"].Value = mitigation.Name;
                            break;
                        case "ControlType":
                            row["ControlType"].Value = mitigation.ControlType.ToString();
                            break;
                    }
                }
            }
        }

        private GridRow GetRow([NotNull] IMitigation mitigation)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == mitigation)
                {
                    result = row;
                    break;
                }
            }

            return result;
        }
        #endregion

        #region Threat Event Mitigation level.
        private GridPanel CreateThreatEventsPanel([NotNull] IMitigation mitigation, [NotNull] IList<IThreatEventMitigation> threatEventMitigations)
        {
            GridPanel result = null;

            if (!string.IsNullOrWhiteSpace(mitigation.Name))
            {
                result = new GridPanel
                {
                    Name = "ThreatEventMitigations",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowTreeButtons = false,
                    ShowTreeLines = false,
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

                foreach (var tem in threatEventMitigations)
                {
                    AddGridRow(tem, result);
                }
            }

            return result;
        }

        private void AddGridRow([NotNull] IThreatEventMitigation mitigation, [NotNull] GridPanel panel)
        {
            GridRow row = new GridRow(
                mitigation.ThreatEvent?.Name ?? string.Empty,
                mitigation.ThreatEvent?.Parent?.Name ?? string.Empty,
                mitigation.ThreatEvent?.Severity,
                mitigation.Strength,
                mitigation.Status.ToString())
            {
                Tag = mitigation
            };
            UpdateMitigationLevel(mitigation.ThreatEvent, row);
            if (mitigation.ThreatEvent?.Parent != null)
            {
                row.Cells[1].CellStyles.Default.Image = mitigation.ThreatEvent.Parent.GetImage(ImageSize.Small);
                ((INotifyPropertyChanged) mitigation.ThreatEvent.Parent).PropertyChanged += OnThreatEventMitigationParentPropertyChanged;
                if (mitigation.ThreatEvent.Parent is IEntity entity)
                    entity.ImageChanged += OnImageChanged;
                AddSuperTooltipProvider(mitigation.ThreatEvent.Parent, row.Cells[1]);
            }

            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnThreatEventMitigationCellChanged;
            panel.Rows.Add(row);

            ((INotifyPropertyChanged) mitigation).PropertyChanged += OnThreatEventMitigationPropertyChanged;
            mitigation.ThreatEvent.ThreatEventMitigationAdded += OnThreatEventMitigationAdded;
            mitigation.ThreatEvent.ThreatEventMitigationRemoved += OnThreatEventMitigationRemoved;
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
                        if (child.Tag is IThreatEventMitigation mitigation && mitigation.ThreatEvent != null && 
                            mitigation.ThreatEvent.ParentId == entity.Id)
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

        [Dispatched]
        private void OnThreatEventMitigationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IThreatEventMitigation threatEventMitigation)
            {
                var row = GetRow(threatEventMitigation);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Strength":
                            row["Strength"].Value = threatEventMitigation.Strength;

                            if (threatEventMitigation.ThreatEvent is IThreatEvent threatEvent)
                            {
                                UpdateMitigationLevel(threatEvent, row);
                            }
                            break;
                        case "Status":
                            row["Status"].Value = threatEventMitigation.Status.ToString();
                            break;
                    }
                }
            }
        }

        [Dispatched]
        private void OnThreatEventMitigationParentPropertyChanged(object sender, PropertyChangedEventArgs e)
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

        private void OnThreatEventMitigationCellChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!_loading && sender is GridCell cell)
            {
                try
                {
                    _loading = true;
                    if (cell.GridRow.Tag is IThreatEventMitigation mitigation)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Severity":
                                if (cell.Value is ISeverity severity)
                                    mitigation.ThreatEvent.Severity = severity;
                                break;
                            case "Strength":
                                if (cell.Value is IStrength strength)
                                    mitigation.Strength = strength;
                                break;
                            case "Status":
                                mitigation.Status = ((string) cell.Value).GetEnumValue<MitigationStatus>();
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
        private void OnThreatEventMitigationAdded([NotNull] IThreatEventMitigationsContainer container, [NotNull] IThreatEventMitigation mitigation)
        {
            if (!_loading)
            {
                try
                {
                    _loading = true;
                    var row = GetRow(mitigation.Mitigation);
                    if (row != null)
                    {
                        var panel = row.Rows.OfType<GridPanel>().FirstOrDefault();
                        if (panel != null)
                            AddGridRow(mitigation, panel);
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        [Dispatched]
        private void OnThreatEventMitigationRemoved([NotNull] IThreatEventMitigationsContainer container, [NotNull] IThreatEventMitigation mitigation)
        {
            if (!_loading)
            {
                try
                {
                    _loading = true;
                    var row = GetRow(mitigation);
                    if (row != null)
                    {
                        row.GridPanel.Rows.Remove(row);
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private GridRow GetRow([NotNull] IThreatEventMitigation mitigation)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                var panel = row.Rows.OfType<GridPanel>()
                    .FirstOrDefault(x => string.CompareOrdinal("ThreatEventMitigations", x.Name) == 0);
                if (panel != null)
                {
                    result = panel.Rows.OfType<GridRow>().FirstOrDefault(x =>
                        (x.Tag is IThreatEventMitigation threatEventMitigation) &&
                        threatEventMitigation.MitigationId == mitigation.MitigationId &&
                        threatEventMitigation.ThreatEvent.Id == mitigation.ThreatEvent.Id);
                    if (result != null)
                        break;
                }
            }

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
                        if (child.Tag is IThreatEventMitigation mitigation)
                        {
                            if (mitigation.ThreatEvent.Parent == identity)
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
        #endregion

        #region Auxiliary private members.
        private GridRow GetRow(Point position)
        {
            GridRow result = null;

            GridElement item = _grid.GetElementAt(position);

            if (item is GridCell cell)
                result = cell.GridRow;

            return result;
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
    
        private bool IsSelected([NotNull] IMitigation item, IEnumerable<IThreatEventMitigation> mitigations,
            string filter, MitigationListFilter filterSpecial)
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
                    case MitigationListFilter.UndefinedMitigations:
                        result = mitigations?.Any(x => x.Status == MitigationStatus.Undefined) ?? false;
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

                if (row?.Tag is IMitigation)
                {
                    MenuDefinition.UpdateVisibility(_mitigationMenu, row.Tag);
                    _mitigationMenu?.Show(_grid.PointToScreen(e.Location));
                }
                if (row?.Tag is IThreatEventMitigation)
                {
                    MenuDefinition.UpdateVisibility(_threatEventMitigationMenu, row.Tag);
                    _threatEventMitigationMenu?.Show(_grid.PointToScreen(e.Location));
                }
            }
        }

        private void ShowCurrentRow()
        {
            _properties.Item = _currentRow?.Tag;

            ChangeCustomActionStatus?.Invoke("RemoveThreatEvent", _currentRow.Tag is IThreatEventMitigation);
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
