using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.Excel
{
#pragma warning disable CS0067
    public partial class ExcelReportingPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;
        private string _lastDocument;

        public string TabLabel => "Excel Reporting";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("Export", "Export", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "CreateFile", "Create File",
                            Properties.Resources.xlsx_big,
                            Properties.Resources.xlsx, true),
                    }),
                    new CommandsBarDefinition("Open", "Open", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Open", "Open Last Document",
                            Properties.Resources.note_text_big,
                            Properties.Resources.note_text, true),
                    }),
                    new CommandsBarDefinition("Check", "Check/Uncheck", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Check", "Check All Fields",
                            Properties.Resources.checkbox_big,
                            Properties.Resources.checkbox, true),
                        new ActionDefinition(Id, "Uncheck", "Uncheck All Fields",
                            Properties.Resources.checkbox_unchecked_big,
                            Properties.Resources.checkbox_unchecked, true),
                    }),
                    new CommandsBarDefinition("Refresh", "Refresh", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Refresh", "Refresh List",
                            Resources.refresh_big,
                            Resources.refresh,
                            true, Shortcut.F5),
                    }),
                };

                return result;
            }
        }

        [InitializationRequired]
        public void ExecuteCustomAction([NotNull] IActionDefinition action)
        {
            string text = null;
            bool warning = false;

            try
            {
                switch (action.Name)
                {
                    case "CreateFile":
                        _saveFile.FileName = _model.Name;
                        if (_saveFile.ShowDialog(Form.ActiveForm) == DialogResult.OK) {

                            if (SaveExcelFile(_saveFile.FileName))
                            {
                                text = "Excel file creation";
                                SaveSelectedProperties();

                                _lastDocument = _saveFile.FileName;
                            }
                        }
                        break;
                    case "Open":
                        if (!string.IsNullOrWhiteSpace(_lastDocument) && File.Exists(_lastDocument))
                            Process.Start(_lastDocument);
                        break;
                    case "Check":
                        ChangeStatus(_fieldsExternalInteractors, true);
                        ChangeStatus(_fieldsProcesses, true);
                        ChangeStatus(_fieldsDataStores, true);
                        ChangeStatus(_fieldsDataFlows, true);
                        ChangeStatus(_fieldsThreatEvents, true);
                        ChangeStatus(_fieldsMitigations, true);
                        break;
                    case "Uncheck":
                        ChangeStatus(_fieldsExternalInteractors, false);
                        ChangeStatus(_fieldsProcesses, false);
                        ChangeStatus(_fieldsDataStores, false);
                        ChangeStatus(_fieldsDataFlows, false);
                        ChangeStatus(_fieldsThreatEvents, false);
                        ChangeStatus(_fieldsMitigations, false);
                        break;
                    case "Refresh":
                        LoadModel(_model);
                        break;
                }

                if (warning)
                    ShowWarning?.Invoke(text);
                else if (text != null)
                    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch (Exception)
            {
                ShowWarning?.Invoke($"An error occurred during the execution of the action.");
                throw;
            }
        }

        private void ChangeStatus([NotNull] CheckedListBox listBox, bool check)
        {
            var count = listBox.Items.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    listBox.SetItemChecked(i, check);
                }
            }
        }

        private bool SaveExcelFile([Required] string fileName)
        {
            var result = false;

            using (ExcelReportEngine engine = new ExcelReportEngine())
            {
                if (_includeExternalInteractors.Checked)
                {
                    var rows = _externalInteractors.PrimaryGrid.FlatCheckedRows.OfType<GridRow>().ToArray();

                    if (rows.Any())
                    {
                        var p = engine.AddPage("External Interactors");
                        List<string> fields = new List<string> {"Name", "Description", "Parent"};
                        var checkedItems = _fieldsExternalInteractors.CheckedItems.OfType<IPropertyType>().ToArray();
                        ProcessRows(engine, rows, p, fields, checkedItems);
                    }
                }

                if (_includeProcesses.Checked)
                {
                    var rows = _processes.PrimaryGrid.FlatCheckedRows.OfType<GridRow>().ToArray();

                    if (rows.Any())
                    {
                        var p = engine.AddPage("Processes");
                        List<string> fields = new List<string> {"Name", "Description", "Parent"};
                        var checkedItems = _fieldsProcesses.CheckedItems.OfType<IPropertyType>().ToArray();
                        ProcessRows(engine, rows, p, fields, checkedItems);
                    }
                }

                if (_includeDataStores.Checked)
                {
                    var rows = _dataStores.PrimaryGrid.FlatCheckedRows.OfType<GridRow>().ToArray();

                    if (rows.Any())
                    {
                        var p = engine.AddPage("Data Stores");
                        List<string> fields = new List<string> {"Name", "Description", "Parent"};
                        var checkedItems = _fieldsDataStores.CheckedItems.OfType<IPropertyType>().ToArray();
                        ProcessRows(engine, rows, p, fields, checkedItems);
                    }
                }

                if (_includeDataFlows.Checked)
                {
                    var rows = _dataFlows.PrimaryGrid.FlatCheckedRows.OfType<GridRow>().ToArray();

                    if (rows.Any())
                    {
                        var p = engine.AddPage("Flows");
                        List<string> fields = new List<string> {"Name", "Description", "Source", "Target", "Flow Type"};
                        var checkedItems = _fieldsDataFlows.CheckedItems.OfType<IPropertyType>().ToArray();
                        ProcessRows(engine, rows, p, fields, checkedItems);
                    }
                }

                if (_includeThreatEvents.Checked)
                {
                    var rows = _threatEvents.PrimaryGrid.FlatCheckedRows.OfType<GridRow>().ToArray();

                    if (rows.Any())
                    {
                        var p = engine.AddPage("Threat Events");
                        List<string> fields = new List<string>
                            {"Name", "Description", "Associated To Type", "Associated To", "Severity"};
                        var checkedItems = _fieldsThreatEvents.CheckedItems.OfType<IPropertyType>().ToArray();
                        ProcessRows(engine, rows, p, fields, checkedItems, 5);
                    }
                }

                if (_includeMitigations.Checked)
                {
                    var rows = _mitigations.PrimaryGrid.FlatCheckedRows.OfType<GridRow>().ToArray();

                    if (rows.Any())
                    {
                        var p = engine.AddPage("Mitigations");
                        List<string> fields = new List<string>
                        {
                            "Name", "Description", "Control Type", "Threat Event",
                            "Associated To Type", "Associated To", "Strength", "Status", "Directives"
                        };
                        var checkedItems = _fieldsMitigations.CheckedItems.OfType<IPropertyType>().ToArray();
                        ProcessRows(engine, rows, p, fields, checkedItems);
                    }
                }

                try
                {
                    engine.Save(fileName);
                    result = true;
                }
                catch (System.IO.IOException e)
                {
                    ShowWarning?.Invoke(e.Message);
                }
            }

            return result;
        }

        private void ProcessRows(ExcelReportEngine engine, GridRow[] rows, 
            int p, List<string> fields, IPropertyType[] checkedItems,
            int severityCol = 0)
        {
            if (checkedItems.Any())
            {
                foreach (var item in checkedItems)
                {
                    fields.Add(item.Name);
                }
            }
            engine.AddHeader(p, fields.ToArray());

            List<object> values = new List<object>();
            foreach (var row in rows)
            {
                values.Clear();
                values.AddRange(row.Cells.Select(x => x.Value));
                foreach (var item in checkedItems)
                {
                    if (row.Tag is IPropertiesContainer container &&
                        container.GetProperty(item) is IProperty property)
                    {
                        if (property is IPropertyIdentityReference reference)
                        {
                            values.Add(reference.Value?.Name);
                        }
                        else
                        {
                            values.Add(property.StringValue);
                        }
                    }
                }

                var r = engine.AddRow(p, values.ToArray());
                if (severityCol > 0)
                {
                    var severity = _model.Severities.FirstOrDefault(x =>
                        string.CompareOrdinal(x.Name, values[severityCol - 1].ToString()) == 0);
                    if (severity != null)
                        engine.ColorCell(p, r, severityCol, 
                            Color.FromKnownColor(severity.TextColor), 
                            Color.FromKnownColor(severity.BackColor));
                }
            }
        }

        private void SaveSelectedProperties()
        {
            var schema = (new ReportingConfigPropertySchemaManager(_model)).GetSchema();
            var propertyType = schema?.GetPropertyType("ExcelSelectedFields");
            if (propertyType != null)
            {
                var values = GetSelectedPropertyIds();
                var property = _model.GetProperty(propertyType);
                if (property == null)
                    _model.AddProperty(propertyType, values?.TagConcat());
                else
                {
                    property.StringValue = values?.TagConcat();
                }
            }
        }

        private IEnumerable<string> GetSelectedPropertyIds()
        {
            IEnumerable<string> result = null;

            List<string> propertyIds = new List<string>();
            var ei = _fieldsExternalInteractors.CheckedItems.OfType<IPropertyType>().ToArray();
            if (ei.Any())
                propertyIds.AddRange(ei.Select(x => x.Id.ToString()));
            var p = _fieldsProcesses.CheckedItems.OfType<IPropertyType>().ToArray();
            if (p.Any())
                propertyIds.AddRange(p.Select(x => x.Id.ToString()));
            var ds = _fieldsDataStores.CheckedItems.OfType<IPropertyType>().ToArray();
            if (ds.Any())
                propertyIds.AddRange(ds.Select(x => x.Id.ToString()));
            var df = _fieldsDataFlows.CheckedItems.OfType<IPropertyType>().ToArray();
            if (df.Any())
                propertyIds.AddRange(df.Select(x => x.Id.ToString()));
            var te = _fieldsThreatEvents.CheckedItems.OfType<IPropertyType>().ToArray();
            if (te.Any())
                propertyIds.AddRange(te.Select(x => x.Id.ToString()));
            var m = _fieldsMitigations.CheckedItems.OfType<IPropertyType>().ToArray();
            if (m.Any())
                propertyIds.AddRange(m.Select(x => x.Id.ToString()));

            if (propertyIds.Any())
                result = propertyIds.AsReadOnly();

            return result;
        }
    }
}