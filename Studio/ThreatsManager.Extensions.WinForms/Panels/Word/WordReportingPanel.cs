using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using Syncfusion.Compression.Zip;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using ThreatsManager.Extensions.Panels.Word.Engine;
using ThreatsManager.Extensions.Panels.Word.Engine.Fields;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Icons;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Panels.Word
{
    public partial class WordReportingPanel : UserControl, IShowThreatModelPanel<Form>, IDesktopAlertAwareExtension,
        ICustomRibbonExtension
    {
        #region Private member variables.
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private bool _loading;
        #endregion

        public WordReportingPanel()
        {
            InitializeComponent();
            InitializeGrid();
        }


        public event Action<string> ShowMessage;
        
        public event Action<string> ShowWarning;

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetThreatModel([NotNull] IThreatModel threatModel)
        {
            _model = threatModel;
            var wordFile = WordFile;
            if (!string.IsNullOrWhiteSpace(wordFile))
            {
                var file = GetDocumentPath(threatModel, wordFile);
                if (File.Exists(file))
                {
                    _wordFile.Text = wordFile;
                    LoadDocStructure(file);
                }
            }
        }
        #endregion

        #region Word file management.
        private void _browse_Click(object sender, EventArgs e)
        {
            if (_openFile.ShowDialog(this) == DialogResult.OK)
            {
                string path;
                var modelPath = _model?.GetLocation();
                if (modelPath == null)
                    path = _openFile.FileName;
                else
                    path = GetRelativePath(modelPath, _openFile.FileName);

                WordFile = _wordFile.Text = path;
                LoadDocStructure(_openFile.FileName);
            }
        }

        private static string GetRelativePath([Required] string relativeTo, [Required] string path)
        {
            var uri = new Uri(relativeTo);
            var rel = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (rel.Contains(Path.DirectorySeparatorChar.ToString()) == false)
            {
                rel = $".{ Path.DirectorySeparatorChar }{ rel }";
            }
            return rel;
        }

        private static string GetDocumentPath([NotNull] IThreatModel model, [Required] string relativePath)
        {
            string result = null;

            if (Path.IsPathRooted(relativePath))
                result = relativePath;
            else
            {
                var modelPath = model.GetLocation();
                result = modelPath != null ? 
                    GetAbsolutePath(Path.GetDirectoryName(modelPath), relativePath) : relativePath;
            }

            return result;
        }

        private static string GetAbsolutePath([Required] string reference, [Required] string relativePath)
        {
            string result;

            if (relativePath.StartsWith(@"..\"))
            {
                relativePath = relativePath.Substring(3);
                reference = Directory.GetParent(reference).FullName;
                result = GetAbsolutePath(reference, relativePath);
            }
            else
            {
                if (!reference.EndsWith(@"\") && (!relativePath.StartsWith(@"\") || !relativePath.StartsWith(@".\")))
                    reference = reference + @"\";

                result = Path.GetFullPath(reference + relativePath);
            }

            return result;
        }

        private string WordFile
        {
            get
            {
                string result = null;

                var schema = (new ReportingConfigPropertySchemaManager(_model)).GetSchema();
                var propertyType = schema?.GetPropertyType("WordDocumentPath");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);
                    if (property is IPropertySingleLineString propertyString)
                        result = propertyString.StringValue;
                }

                return result;
            }

            set
            {
                var schema = (new ReportingConfigPropertySchemaManager(_model)).GetSchema();
                var propertyType = schema?.GetPropertyType("WordDocumentPath");
                if (propertyType != null)
                {
                    var property = _model.GetProperty(propertyType);
                    if (property == null)
                        _model.AddProperty(propertyType, value);
                    else
                    {
                        property.StringValue = value;
                    }
                }
            }
        }
        #endregion

        #region Load document structure.
        private bool LoadDocStructure([Required] string fileName)
        {
            bool result = false;
            _loading = true;

            try
            {
                var doc = new WordDocument();
                doc.OpenReadOnly(fileName, FormatType.Automatic);

                var items = doc.FindAll(new Regex(@"\[ThreatsManagerPlatform:[\w]*\]"))?
                    .Where(x => !x.IsToc())
                    .Select(x => new Placeholder(x.SelectedText, _model))
                    .Distinct(new PlaceholderComparer())
                    .OrderBy(x => x.Name)
                    .ToArray();

                CreatePlaceholderPanel(PlaceholderSection.Model, items);
                CreatePlaceholderPanel(PlaceholderSection.Counter, items);
                CreatePlaceholderPanel(PlaceholderSection.Chart, items);
                CreatePlaceholderPanel(PlaceholderSection.List, items);
                CreatePlaceholderPanel(PlaceholderSection.Table, items);

                result = true;
            }
            catch (ZipException)
            {
                ShowWarning?.Invoke("Reference document may be corrupted.");
            }
            finally
            {
                _loading = false;
            }

            return result;
        }

        private void CreatePlaceholderPanel(PlaceholderSection section, IEnumerable<Placeholder> placeholders)
        {
            var sectionItems = placeholders?.Where(x => x.Section == section).ToArray();
            if (sectionItems?.Any() ?? false)
            {
                var row = new GridRow(section);
                switch (section)
                {
                    case PlaceholderSection.Model:
                        row.Cells[0].CellStyles.Default.Image = Resources.threat_model_small;
                        break;
                    case PlaceholderSection.Counter:
                        row.Cells[0].CellStyles.Default.Image = Properties.Resources.odometer_small;
                        break;
                    case PlaceholderSection.Chart:
                        row.Cells[0].CellStyles.Default.Image = Properties.Resources.presentation_pie_chart_small;
                        break;
                    case PlaceholderSection.List:
                        row.Cells[0].CellStyles.Default.Image = Properties.Resources.list_style_numbered_small;
                        break;
                    case PlaceholderSection.Table:
                        row.Cells[0].CellStyles.Default.Image = Properties.Resources.table_small;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(section), section, null);
                }
                _docStructure.PrimaryGrid.Rows.Add(row);
                var panel = CreateSimplePanel(section, sectionItems);
                if (section == PlaceholderSection.List)
                    UpgradeToListPanel(panel);
                else if (section == PlaceholderSection.Table)
                    UpgradeToTablePanel(panel);
                row.Rows.Add(panel);
            }
        }

        private void InitializeGrid()
        {
            GridPanel panel = _docStructure.PrimaryGrid;
            panel.ShowTreeButtons = true;
            panel.ShowTreeLines = true;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.AllowRowResize = true;
            panel.ShowRowDirtyMarker = false;
            panel.InitialActiveRow = RelativeRow.None;

            panel.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Placeholders Category",
                Width = 400,
                DataType = typeof(string),
                EditorType = typeof(GridTextBoxDropDownEditControl),
                AllowEdit = true
            });
        }

        private GridPanel CreateSimplePanel(PlaceholderSection section, [NotNull] IEnumerable<Placeholder> placeholders)
        {
            GridPanel result = null;

            result = new GridPanel
            {
                Name = section.ToString(),
                AllowRowDelete = false,
                AllowRowInsert = false,
                AllowRowResize = true,
                ShowRowDirtyMarker = false,
                ShowTreeButtons = true,
                ShowTreeLines = true,
                InitialSelection = RelativeSelection.None
            };

            result.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Placeholder Name",
                Width = 400,
                DataType = typeof(string),
                AllowEdit = false
            });

            foreach (var ph in placeholders)
            {
                var row = new GridRow(ph.Name) {Tag = ph};
                var image = ph.Image;
                if (image != null)
                    row.Cells[0].CellStyles.Default.Image = image;
                result.Rows.Add(row);
            }

            return result;
        }

        private void UpgradeToListPanel([NotNull] GridPanel simplePanel)
        {
            var rows = simplePanel.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                {
                    if (row.Tag is Placeholder placeholder)
                    {
                        var panel = CreateListPanel(placeholder);
                        if (panel != null)
                            row.Rows.Add(panel);
                    }
                }
            }
        }

        private void UpgradeToTablePanel([NotNull] GridPanel simplePanel)
        {
            var rows = simplePanel.Rows.OfType<GridRow>().ToArray();
            if (rows.Any())
            {
                foreach (var row in rows)
                {
                    if (row.Tag is Placeholder placeholder)
                    {
                        var panel = CreateTablePanel(placeholder);
                        if (panel != null)
                            row.Rows.Add(panel);
                    }
                }
            }
        }

        private GridPanel CreateListPanel([NotNull] Placeholder placeholder)
        {
            GridPanel result = null;

            var fields = placeholder.Fields?.ToArray();

            if (fields?.Any() ?? false)
            {
                result = new GridPanel
                {
                    Name = "ListProperties",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowTreeButtons = true,
                    ShowTreeLines = true,
                    CheckBoxes = true,
                    InitialSelection = RelativeSelection.None
                };

                result.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Property Name",
                    Width = 300,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                result.Columns.Add(new GridColumn("Namespace")
                {
                    HeaderText = "Property Schema Namespace",
                    Width = 300,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                result.Columns.Add(new GridColumn("Schema")
                {
                    HeaderText = "Property Schema Name",
                    Width = 300,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                var selected = placeholder.Selected?.ToArray();
                foreach (var field in fields)
                {
                    string schemaNs = null;
                    string schemaName = null;
                    if (field is PropertyTypeField ptField && 
                        ptField.PropertyType is IPropertyType propertyType)
                    {
                        var schema = _model.GetSchema(propertyType.SchemaId);
                        if (schema != null)
                        {
                            schemaNs = schema.Namespace;
                            schemaName = schema.Name;
                        }
                    }
                    else if (field is TablePropertyTypeField tptField && 
                             tptField.PropertyType is IPropertyType tPropertyType)
                    {
                        var schema = _model.GetSchema(tPropertyType.SchemaId);
                        if (schema != null)
                        {
                            schemaNs = schema.Namespace;
                            schemaName = schema.Name;
                        }
                    }

                    var row = new GridRow(field.ToString(), schemaNs, schemaName)
                    {
                        Tag = field,
                        Checked = selected?.Contains(field) ?? false
                    };
                    result.Rows.Add(row);
                }
            }

            return result;
        }

        private GridPanel CreateTablePanel([NotNull] Placeholder placeholder)
        {
            GridPanel result = null;

            var widths = placeholder.PropertyWidths?.ToArray();

            if (widths?.Any() ?? false)
            {
                result = new GridPanel
                {
                    Name = "TableProperties",
                    AllowRowDelete = false,
                    AllowRowInsert = false,
                    AllowRowResize = true,
                    ShowRowDirtyMarker = false,
                    ShowTreeButtons = true,
                    ShowTreeLines = true,
                    InitialSelection = RelativeSelection.None
                };

                result.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Property Name",
                    Width = 300,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                var index = result.Columns.Add(new GridColumn("Width")
                {
                    HeaderText = "Width (%)",
                    Width = 100,
                    DataType = typeof(float),
                    EditorType = typeof(GridDoubleInputEditControl),
                    AllowEdit = true
                });
                var control = (GridDoubleInputEditControl)result.Columns[index].EditControl;
                control.ShowUpDown = false;
                control.ButtonClear.Visible = true;
                control.ButtonClear.Enabled = true;

                foreach (var width in widths)
                {
                    var row = new GridRow(width.Key, width.Value);
                    row.Cells[1].PropertyChanged += OnWidthChanged;
                    result.Rows.Add(row);
                }
            }

            return result;
        }

        private void OnWidthChanged(object sender, PropertyChangedEventArgs e)
        {
            var cell = sender as GridCell;

            if (!_loading && cell != null)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    if (row.GridPanel is GridPanel panel && panel.Parent is GridRow parent &&
                        parent.Tag is Placeholder placeholder)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Width":
                                placeholder.Set(row.Cells[0].Value.ToString(), (float) cell.Value);
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

        private void _docStructure_AfterCheck(object sender, GridAfterCheckEventArgs e)
        {
            if (e.Item is GridRow row && row.Tag is Field field && 
                row.GridPanel.Parent is GridRow parent && parent.Tag is Placeholder placeholder)
            {
                placeholder.Set(field, row.Checked);
            }
        }
        #endregion
    }
}
