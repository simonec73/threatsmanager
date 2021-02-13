using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Dialogs;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;
using Shortcut = ThreatsManager.Interfaces.Extensions.Shortcut;

namespace ThreatsManager.Extensions.Panels.PropertySchemaList
{
#pragma warning disable CS0067
    public partial class PropertySchemaListPanel
    {
        public event Action<string, bool> ChangeCustomActionStatus;

        public string TabLabel => "Property Schema List";

        public IEnumerable<ICommandsBarDefinition> CommandBars
        {
            get
            {
                var result = new List<ICommandsBarDefinition>
                {
                    new CommandsBarDefinition("AddRemove", "Add/Remove", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "AddSchema", "Add Property Schema",
                            Properties.Resources.properties_big_new,
                            Properties.Resources.properties_new,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "AddProperty", "Add Property",
                            Properties.Resources.property_big_new,
                            Properties.Resources.property_new,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "RemoveSchema", "Remove Property Schema",
                            Properties.Resources.properties_big_delete,
                            Properties.Resources.properties_delete,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "RemoveProperty", "Remove Selected Properties",
                            Properties.Resources.property_big_delete,
                            Properties.Resources.property_delete,
                            true, Shortcut.None),
                    }),
                    new CommandsBarDefinition("Apply", "Apply", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "ApplySchema", "Apply Schema",
                            Properties.Resources.magic_wand_big,
                            Properties.Resources.magic_wand,
                            true, Shortcut.None),
                    }),
                    new CommandsBarDefinition("ImportExport", "Import/Export", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Import", "Import Schemas",
                            Properties.Resources.import_template_big,
                            Properties.Resources.import_template,
                            true, Shortcut.None),
                        new ActionDefinition(Id, "Export", "Export Schemas",
                            Properties.Resources.export_template_big,
                            Properties.Resources.export_template,
                            true, Shortcut.None),
                    }),
                    new CommandsBarDefinition("Promote", "Promote", new IActionDefinition[]
                    {
                        new ActionDefinition(Id, "Promote", "Get Full Rights",
                            Properties.Resources.caesar_big,
                            Properties.Resources.caesar,
                            true, Shortcut.None), 
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
                    case "AddSchema":
                        using (var dialogSchema = new PropertySchemaCreationDialog())
                        {
                            if (dialogSchema.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                var schema = _model.AddSchema(dialogSchema.SchemaName, dialogSchema.SchemaNamespace);
                                schema.Description = dialogSchema.Description;
                                text = "Add Property Schema";
                                _schemas.SelectedIndex = _schemas.Items.Add(schema);
                            }
                        }
                        break;
                    case "AddProperty":
                        if (_schemas.SelectedItem is IPropertySchema schema1 && (!schema1.System || _promoted))
                        {
                            using (var dialogProperty = new PropertyTypeCreationDialog(schema1))
                            {
                                if (dialogProperty.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                                {
                                    text = "Add Property";
                                    var propertyType = schema1.AddPropertyType(dialogProperty.PropertyName,
                                        dialogProperty.PropertyValueType);
                                    if (propertyType != null)
                                    {
                                        propertyType.Description = dialogProperty.Description;
                                        propertyType.Visible = true;
                                        if (propertyType is IListPropertyType listPt)
                                            listPt.SetListProvider(new ListProvider());
                                        else if (propertyType is IListMultiPropertyType listMultiPt)
                                            listMultiPt.SetListProvider(new ListProvider());
                                        AddGridRow(propertyType, _grid.PrimaryGrid);
                                    }
                                }
                            }
                        }
                        else
                        {
                            warning = true;
                            text = "The action is possible only on non-System Property Schemas.";
                        }
                        break;
                    case "RemoveSchema":
                        if (_schemas.SelectedItem is IPropertySchema schema2 && (!schema2.System || _promoted))
                        {
                            var found = false;

                            if (_updaters?.Any() ?? false)
                            {
                                foreach (var updater in _updaters)
                                {
                                    found = updater.HasPropertySchema(_model, schema2.Name, schema2.Namespace);
                                    if (found)
                                        break;
                                }
                            }

                            if (!found)
                            {
                                if (MessageBox.Show(Form.ActiveForm,
                                    "Are you sure that you want to remove the current Property Schema?",
                                    "Property Schema removal", MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                                {
                                    if (_model.RemoveSchema(schema2.Id))
                                    {
                                        text = "Remove Property Schema";
                                        _schemas.Items.Remove(schema2);
                                        Reset();
                                    }
                                    else
                                    {
                                        if (MessageBox.Show(Form.ActiveForm,
                                            "The Property Schema is in use. Do you want to force its removal?",
                                            "Property Schema Removal", MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Exclamation) == DialogResult.Yes)
                                        {
                                            if (_model.RemoveSchema(schema2.Id, true))
                                            {
                                                text = "Remove Property Schema";
                                                _schemas.Items.Remove(schema2);
                                                Reset();
                                            }
                                            else
                                            {
                                                warning = true;
                                                text = "Schema removal has failed.";
                                            }
                                        }
                                        else
                                        {
                                            warning = true;
                                            text = "Schema removal has failed.";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                warning = true;
                                text = "Schema cannot be removed because it is used somewhere.";
                            }
                        }
                        else
                        {
                            warning = true;
                            text = "The action is possible only on non-System Property Schemas.";
                        }
                        break;
                    case "RemoveProperty":
                        if (_schemas.SelectedItem is IPropertySchema schema3 && (!schema3.System || _promoted))
                        {
                            var rows = GetSelectedRows()?.ToArray();
                            if (rows?.Any() ?? false)
                            {
                                var found = false;

                                if (_updaters?.Any() ?? false)
                                {
                                    foreach (var row in rows)
                                    {
                                        if (row.Tag is IPropertyType propertyType)
                                        {
                                            foreach (var updater in _updaters)
                                            {
                                                found = updater.HasPropertyType(_model,
                                                    schema3.Name, schema3.Namespace, propertyType.Name);
                                                if (found)
                                                    break;
                                            }
                                        }

                                        if (found)
                                            break;
                                    }
                                }

                                if (!found)
                                {
                                    if (MessageBox.Show(Form.ActiveForm,
                                        "Are you sure that you want to remove the selected Properties?",
                                        "Properties removal", MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Exclamation) == DialogResult.Yes)
                                    {
                                        text = "Remove Properties";
                                        foreach (var row in rows)
                                        {
                                            if (row.Tag is IPropertyType propertyType)
                                            {
                                                if (schema3.RemovePropertyType(propertyType.Id))
                                                    _grid.PrimaryGrid.Rows.Remove(row);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    warning = true;
                                    text = "Selected properties cannot be removed because they are used somewhere.";
                                }
                            }
                        }
                        else
                        {
                            warning = true;
                            text = "The action is possible only on non-System Property Schemas.";
                        }
                        break;
                    case "ApplySchema":
                        if (_schemas.SelectedItem is IPropertySchema schema4)
                        {
                            text = "Apply current Property Schema";
                            _model.ApplySchema(schema4.Id);
                        }
                        break;
                    case "Import":
                        using (var dialogI = new ImportSchemaDialog(_model))
                        {
                            if (dialogI.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                text = "Import Schemas";

                                var schemas = _model.Schemas?.ToArray();
                                if (schemas?.Any() ?? false)
                                {
                                    foreach (var schema in schemas)
                                    {
                                        if (!_schemas.Items.Contains(schema))
                                            _schemas.Items.Add(schema);
                                    }
                                }
                            }
                        }
                        break;
                    case "Export":
                        using (var dialogE = new ExportSchemaDialog(_model))
                        {
                            if (dialogE.ShowDialog(Form.ActiveForm) == DialogResult.OK)
                            {
                                text = "Export Schemas";
                            }
                        }
                        break;
                    case "Promote":
                        if (MessageBox.Show(Form.ActiveForm, Properties.Resources.MessagePromote,
                                "ATTENTION", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            _promoted = true;
                            RefreshSchema();
                        }
                        break;
                    case "Refresh":
                        text = "Refresh Property Schema List";
                        LoadModel();
                        break;
                }

                if (warning)
                    ShowWarning?.Invoke(text);
                else if (text != null)
                    ShowMessage?.Invoke($"{text} has been executed successfully.");
            }
            catch
            {
                ShowWarning?.Invoke($"An error occurred during the execution of the action.");
                throw;
            }
        }

        private IEnumerable<GridRow> GetSelectedRows()
        {
            IEnumerable<GridRow> result = null;

            var selectedRows = _grid.PrimaryGrid.SelectedRows.OfType<GridRow>().ToArray();
            if (selectedRows.Any())
            {
                result = selectedRows;
            }
            else
            {
                var rowsFromSelectedCells = _grid.PrimaryGrid.SelectedCells.OfType<GridCell>().Select(x => x.GridRow).ToArray();
                if (rowsFromSelectedCells.Any())
                {
                    result = rowsFromSelectedCells;
                }
            }

            return result;
        }
    }
}