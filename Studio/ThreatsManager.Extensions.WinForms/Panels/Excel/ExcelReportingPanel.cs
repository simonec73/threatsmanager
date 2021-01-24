using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Excel
{
    public partial class ExcelReportingPanel : UserControl, IShowThreatModelPanel<Form>, IDesktopAlertAwareExtension, ICustomRibbonExtension
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;

        public ExcelReportingPanel()
        {
            InitializeComponent();
        }

        
        public event Action<string> ShowMessage;
        
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;

            InitializeEntityGrid(_externalInteractors);
            InitializeEntityGrid(_processes);
            InitializeEntityGrid(_dataStores);
            InitializeDataFlowsGrid();
            InitializeThreatEventsGrid();
            InitializeMitigationsGrid();

            LoadModel(_model);
        }
        #endregion

        private void InitializeEntityGrid([NotNull] SuperGridControl grid)
        {
            GridPanel panel = grid.PrimaryGrid;
            panel.ShowTreeButtons = false;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.ShowRowDirtyMarker = false;
            panel.ShowCheckBox = true;
            panel.CheckBoxes = true;
            panel.InitialActiveRow = RelativeRow.None;
            panel.ShowRowHeaders = false;

            panel.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Name",
                Width = 250,
                DataType = typeof(string),
                AllowEdit = false
            });

            panel.Columns.Add(new GridColumn("Description")
            {
                HeaderText = "Description",
                Width = 500,
                DataType = typeof(string),
                AllowEdit = false,
                Visible = false
            });
            panel.Columns["Description"].CellStyles.Default.AllowMultiLine = Tbool.True;

            panel.Columns.Add(new GridColumn("Parent")
            {
                HeaderText = "Parent",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 150
            });
        }

        private void InitializeDataFlowsGrid()
        {
            GridPanel panel = _dataFlows.PrimaryGrid;
            panel.ShowTreeButtons = false;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.ShowRowDirtyMarker = false;
            panel.ShowCheckBox = true;
            panel.CheckBoxes = true;
            panel.InitialActiveRow = RelativeRow.None;
            panel.ShowRowHeaders = false;

            panel.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Name",
                Width = 250,
                DataType = typeof(string),
                AllowEdit = false
            });

            panel.Columns.Add(new GridColumn("Description")
            {
                HeaderText = "Description",
                Width = 500,
                DataType = typeof(string),
                AllowEdit = false,
                Visible = false
            });
            panel.Columns["Description"].CellStyles.Default.AllowMultiLine = Tbool.True;

            panel.Columns.Add(new GridColumn("Source")
            {
                HeaderText = "Source",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 250
            });

            panel.Columns.Add(new GridColumn("Target")
            {
                HeaderText = "Target",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 250
            });
 
            panel.Columns.Add(new GridColumn("FlowType")
            {
                HeaderText = "Flow Type",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 150
            });
       }

        private void InitializeThreatEventsGrid()
        {
            GridPanel panel = _threatEvents.PrimaryGrid;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.AllowRowResize = true;
            panel.ShowRowDirtyMarker = false;
            panel.ShowTreeButtons = false;
            panel.ShowTreeLines = false;
            panel.InitialSelection = RelativeSelection.None;
            panel.ShowCheckBox = true;
            panel.CheckBoxes = true;
            panel.ShowRowHeaders = false;

            panel.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Name",
                Width = 250,
                DataType = typeof(string),
                AllowEdit = false
            });

            panel.Columns.Add(new GridColumn("Description")
            {
                HeaderText = "Description",
                Width = 500,
                DataType = typeof(string),
                AllowEdit = false,
                Visible = false
            });
            panel.Columns["Description"].CellStyles.Default.AllowMultiLine = Tbool.True;

            panel.Columns.Add(new GridColumn("ParentType")
            {
                HeaderText = "Associated To Type",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 75,
                Visible = false
            });

            panel.Columns.Add(new GridColumn("Parent")
            {
                HeaderText = "Associated To",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 150
            });
            panel.Columns.Add(new GridColumn("Severity")
            {
                HeaderText = "Severity",
                DataType = typeof(ISeverity),
                AllowEdit = false,
                Width = 75
            });
        }

        private void InitializeMitigationsGrid()
        {
            GridPanel panel = _mitigations.PrimaryGrid;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.AllowRowResize = true;
            panel.ShowRowDirtyMarker = false;
            panel.ShowTreeButtons = false;
            panel.ShowTreeLines = false;
            panel.InitialSelection = RelativeSelection.None;
            panel.ShowCheckBox = true;
            panel.CheckBoxes = true;
            panel.ShowRowHeaders = false;

            panel.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Name",
                Width = 250,
                DataType = typeof(string),
                AllowEdit = false
            });

            panel.Columns.Add(new GridColumn("Description")
            {
                HeaderText = "Description",
                Width = 500,
                DataType = typeof(string),
                AllowEdit = false,
                Visible = false
            });
            panel.Columns["Description"].CellStyles.Default.AllowMultiLine = Tbool.True;

            panel.Columns.Add(new GridColumn("ControlType")
            {
                HeaderText = "Control Type",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 75
            });

            panel.Columns.Add(new GridColumn("ThreatEvent")
            {
                HeaderText = "Threat Event",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 150
            });

            panel.Columns.Add(new GridColumn("AssociatedToType")
            {
                HeaderText = "Associated To Type",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 75,
                Visible = false
            });

            panel.Columns.Add(new GridColumn("AssociatedTo")
            {
                HeaderText = "Associated To",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 150
            });

            panel.Columns.Add(new GridColumn("Strength")
            {
                HeaderText = "Strength",
                DataType = typeof(IStrength),
                AllowEdit = false,
                Width = 75
            });

            panel.Columns.Add(new GridColumn("Status")
            {
                HeaderText = "Status",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 75
            });

            panel.Columns.Add(new GridColumn("Directives")
            {
                HeaderText = "Directives",
                Width = 400,
                DataType = typeof(string),
                AllowEdit = false,
                Visible = false
            });
            panel.Columns["Directives"].CellStyles.Default.AllowMultiLine = Tbool.True;
        }

        private void LoadModel([NotNull] IThreatModel model)
        {
            List<IThreatEvent> threatEvents = new List<IThreatEvent>();
            List<IThreatEventMitigation> mitigations = new List<IThreatEventMitigation>();

            var externalInteractors = model.Entities?.OfType<IExternalInteractor>().OrderBy(x => x.Name).ToArray();
            if (externalInteractors?.Any() ?? false)
            {
                LoadItems(externalInteractors, threatEvents, mitigations);
            }

            var processes = model.Entities?.OfType<IProcess>().OrderBy(x => x.Name).ToArray();
            if (processes?.Any() ?? false)
            {
                LoadItems(processes, threatEvents, mitigations);
            }

            var dataStores = model.Entities?.OfType<IDataStore>().OrderBy(x => x.Name).ToArray();
            if (dataStores?.Any() ?? false)
            {
                LoadItems(dataStores, threatEvents, mitigations);
            }

            var dataFlows = model.DataFlows?.OrderBy(x => x.Name).ToArray();
            if (dataFlows?.Any() ?? false)
            {
                LoadItems(dataFlows, threatEvents, mitigations);
            }

            var threatModelTE = model.ThreatEvents?.ToArray();
            if (threatModelTE?.Any() ?? false)
            {
                threatEvents.AddRange(threatModelTE);
                foreach (var te in threatModelTE)
                {
                    var mTe = te.Mitigations?.ToArray();
                    if (mTe?.Any() ?? false)
                    {
                        mitigations.AddRange(mTe);
                    }
                }
            }

            if (threatEvents.Any())
            {
                LoadItems(threatEvents.OrderByDescending(x => x.Severity, new SeverityComparer()));
            }

            if (mitigations.Any())
            {
                LoadItems(mitigations.OrderBy(x => x.Mitigation.Name));
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void LoadItems([NotNull] IEnumerable<IExternalInteractor> entities, 
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            _externalInteractors.PrimaryGrid.Rows.Clear();
            _fieldsExternalInteractors.Items.Clear();

            if (entities.Any())
            {
                var selectedProperties = GetSelectedProperties();

                foreach (var entity in entities)
                {
                    var row = new GridRow(
                        entity.Name,
                        entity.Description,
                        entity.Parent?.Name ?? string.Empty)
                    {
                        Checked = true,
                        Tag = entity
                    };
                    _externalInteractors.PrimaryGrid.Rows.Add(row);

                    var teArray = entity.ThreatEvents?.ToArray();
                    if (teArray?.Any() ?? false)
                    {
                        threatEvents.AddRange(teArray);

                        foreach (var teItem in teArray)
                        {
                            var mArray = teItem.Mitigations?.ToArray();
                            if (mArray?.Any() ?? false)
                            {
                                mitigations.AddRange(mArray);
                            }
                        }
                    }

                    var properties = entity.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && 
                                    (_model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false)).ToArray();
                    if (properties?.Any() ?? false)
                    {
                        foreach (var property in properties)
                        {
                            var propertyType = property.PropertyType;
                            if (!_fieldsExternalInteractors.Items.Contains(propertyType))
                                _fieldsExternalInteractors.Items.Add(propertyType, 
                                    selectedProperties?.Contains(propertyType.Id.ToString()) ?? false);
                        }
                    }
                }
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void LoadItems([NotNull] IEnumerable<IProcess> entities, 
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            _processes.PrimaryGrid.Rows.Clear();
            _fieldsProcesses.Items.Clear();

            if (entities.Any())
            {
                var selectedProperties = GetSelectedProperties();

                foreach (var entity in entities)
                {
                    var row = new GridRow(
                        entity.Name,
                        entity.Description,
                        entity.Parent?.Name ?? string.Empty)
                    {
                        Checked = true,
                        Tag = entity
                    };
                    _processes.PrimaryGrid.Rows.Add(row);

                    var teArray = entity.ThreatEvents?.ToArray();
                    if (teArray?.Any() ?? false)
                    {
                        threatEvents.AddRange(teArray);

                        foreach (var teItem in teArray)
                        {
                            var mArray = teItem.Mitigations?.ToArray();
                            if (mArray?.Any() ?? false)
                            {
                                mitigations.AddRange(mArray);
                            }
                        }
                    }
    
                    var properties = entity.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && 
                                    (_model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false)).ToArray();
                    if (properties?.Any() ?? false)
                    {
                        foreach (var property in properties)
                        {
                            var propertyType = property.PropertyType;
                            if (!_fieldsProcesses.Items.Contains(propertyType))
                                _fieldsProcesses.Items.Add(propertyType, 
                                    selectedProperties?.Contains(propertyType.Id.ToString()) ?? false);
                        }
                    }
                }
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void LoadItems([NotNull] IEnumerable<IDataStore> entities, 
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            _dataStores.PrimaryGrid.Rows.Clear();
            _fieldsDataStores.Items.Clear();

            if (entities.Any())
            {
                var selectedProperties = GetSelectedProperties();

                foreach (var entity in entities)
                {
                    var row = new GridRow(
                        entity.Name,
                        entity.Description,
                        entity.Parent?.Name ?? string.Empty)
                    {
                        Checked = true,
                        Tag = entity
                    };
                    _dataStores.PrimaryGrid.Rows.Add(row);

                    var teArray = entity.ThreatEvents?.ToArray();
                    if (teArray?.Any() ?? false)
                    {
                        threatEvents.AddRange(teArray);

                        foreach (var teItem in teArray)
                        {
                            var mArray = teItem.Mitigations?.ToArray();
                            if (mArray?.Any() ?? false)
                            {
                                mitigations.AddRange(mArray);
                            }
                        }
                    }
 
                    var properties = entity.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && 
                                    (_model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false)).ToArray();
                    if (properties?.Any() ?? false)
                    {
                        foreach (var property in properties)
                        {
                            var propertyType = property.PropertyType;
                            if (!_fieldsDataStores.Items.Contains(propertyType))
                                _fieldsDataStores.Items.Add(propertyType, 
                                    selectedProperties?.Contains(propertyType.Id.ToString()) ?? false);
                        }
                    }
                }
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void LoadItems([NotNull] IEnumerable<IDataFlow> dataFlows, 
            [NotNull] List<IThreatEvent> threatEvents,
            [NotNull] List<IThreatEventMitigation> mitigations)
        {
            _dataFlows.PrimaryGrid.Rows.Clear();
            _fieldsDataFlows.Items.Clear();

            if (dataFlows.Any())
            {
                var selectedProperties = GetSelectedProperties();

                foreach (var dataFlow in dataFlows)
                {
                    var row = new GridRow(
                        dataFlow.Name,
                        dataFlow.Description,
                        dataFlow.Source?.Name ?? string.Empty,
                        dataFlow.Target?.Name ?? string.Empty,
                        dataFlow.FlowType.GetEnumLabel())
                    {
                        Checked = true,
                        Tag = dataFlow
                    };
                    row.Cells[2].CellStyles.Default.Image = dataFlow.Source?.GetImage(ImageSize.Small);
                    row.Cells[3].CellStyles.Default.Image = dataFlow.Target?.GetImage(ImageSize.Small);
                    _dataFlows.PrimaryGrid.Rows.Add(row);

                    var teArray = dataFlow.ThreatEvents?.ToArray();
                    if (teArray?.Any() ?? false)
                    {
                        threatEvents.AddRange(teArray);

                        foreach (var teItem in teArray)
                        {
                            var mArray = teItem.Mitigations?.ToArray();
                            if (mArray?.Any() ?? false)
                            {
                                mitigations.AddRange(mArray);
                            }
                        }
                    }
 
                    var properties = dataFlow.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && 
                                    (_model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false)).ToArray();
                    if (properties?.Any() ?? false)
                    {
                        foreach (var property in properties)
                        {
                            var propertyType = property.PropertyType;
                            if (!_fieldsDataFlows.Items.Contains(propertyType))
                                _fieldsDataFlows.Items.Add(propertyType, 
                                    selectedProperties?.Contains(propertyType.Id.ToString()) ?? false);
                        }
                    }
                }
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void LoadItems([NotNull] IEnumerable<IThreatEvent> threatEvents)
        {
            _threatEvents.PrimaryGrid.Rows.Clear();
            _fieldsThreatEvents.Items.Clear();

            if (threatEvents.Any())
            {
                var selectedProperties = GetSelectedProperties();

                foreach (var threatEvent in threatEvents)
                {
                    var row = new GridRow(
                        threatEvent.Name,
                        threatEvent.Description,
                        threatEvent.Parent != null ? _model.GetIdentityTypeName(threatEvent.Parent) : string.Empty,
                        threatEvent.Parent?.Name ?? string.Empty,
                        threatEvent.Severity)
                    {
                        Checked = true,
                        Tag = threatEvent
                    };
                    row.Cells[3].CellStyles.Default.Image = threatEvent.Parent.GetImage(ImageSize.Small);
                    _threatEvents.PrimaryGrid.Rows.Add(row);
   
                    var properties = threatEvent.Properties?
                        .Where(x => x.PropertyType != null && x.PropertyType.Visible && 
                                    (_model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false)).ToArray();
                    if (properties?.Any() ?? false)
                    {
                        foreach (var property in properties)
                        {
                            var propertyType = property.PropertyType;
                            if (!_fieldsThreatEvents.Items.Contains(propertyType))
                                _fieldsThreatEvents.Items.Add(propertyType, 
                                    selectedProperties?.Contains(propertyType.Id.ToString()) ?? false);
                        }
                    }
                }
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private void LoadItems([NotNull] IEnumerable<IThreatEventMitigation> mitigations)
        {
            _mitigations.PrimaryGrid.Rows.Clear();
            _fieldsMitigations.Items.Clear();

            if (mitigations.Any())
            {
                var selectedProperties = GetSelectedProperties();
                foreach (var mitigation in mitigations)
                {
                    var row = new GridRow(
                        mitigation.Mitigation.Name,
                        mitigation.Mitigation.Description,
                        mitigation.Mitigation.ControlType.ToString(),
                        mitigation.ThreatEvent.Name,
                        mitigation.ThreatEvent.Parent != null ? _model.GetIdentityTypeName(mitigation.ThreatEvent.Parent) : string.Empty,
                        mitigation.ThreatEvent.Parent?.Name ?? string.Empty,
                        mitigation.Strength,
                        mitigation.Status.ToString(),
                        mitigation.Directives)
                    {
                        Checked = true,
                        Tag = mitigation.Mitigation
                    };
                    row.Cells[5].CellStyles.Default.Image = mitigation.ThreatEvent.Parent?.GetImage(ImageSize.Small);
                    _mitigations.PrimaryGrid.Rows.Add(row);
    
                    var properties = mitigation.Mitigation.Properties?
                        .Where(x =>  x.PropertyType != null && x.PropertyType.Visible && 
                                    (_model.GetSchema(x.PropertyType.SchemaId)?.Visible ?? false)).ToArray();
                    if (properties?.Any() ?? false)
                    {
                        foreach (var property in properties)
                        {
                            var propertyType = property.PropertyType;
                            if (!_fieldsMitigations.Items.Contains(propertyType))
                                _fieldsMitigations.Items.Add(propertyType, 
                                    selectedProperties?.Contains(propertyType.Id.ToString()) ?? false);
                        }
                    }
                }
            }
        }

        private void _includeExternalInteractors_CheckedChanged(object sender, EventArgs e)
        {
            _externalInteractors.Enabled = _includeExternalInteractors.Checked;
            _fieldsExternalInteractors.Enabled = _includeExternalInteractors.Checked;
        }

        private void _includeProcesses_CheckedChanged(object sender, EventArgs e)
        {
            _processes.Enabled = _includeProcesses.Checked;
            _fieldsProcesses.Enabled = _includeProcesses.Checked;
        }

        private void _includeDataStores_CheckedChanged(object sender, EventArgs e)
        {
            _dataStores.Enabled = _includeDataStores.Checked;
            _fieldsDataStores.Enabled = _includeDataStores.Checked;
        }

        private void _includeDataFlows_CheckedChanged(object sender, EventArgs e)
        {
            _dataFlows.Enabled = _includeDataFlows.Checked;
            _fieldsDataFlows.Enabled = _includeDataFlows.Checked;
        }

        private void _includeThreatEvents_CheckedChanged(object sender, EventArgs e)
        {
            _threatEvents.Enabled = _includeThreatEvents.Checked;
            _fieldsThreatEvents.Enabled = _includeThreatEvents.Checked;
        }

        private void _includeMitigations_CheckedChanged(object sender, EventArgs e)
        {
            _mitigations.Enabled = _includeMitigations.Checked;
            _fieldsMitigations.Enabled = _includeMitigations.Checked;
        }

        private IEnumerable<string> GetSelectedProperties()
        {
            IEnumerable<string> result = null;

            var schema = (new ReportingConfigPropertySchemaManager(_model)).GetSchema();
            var propertyType = schema?.GetPropertyType("ExcelSelectedFields");
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType);
                if (property is IPropertyArray propertyArray)
                    result = propertyArray.Value;
            }

            return result;
        }
    }
}
