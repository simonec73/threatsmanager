using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Reporting;
using ThreatsManager.Extensions.Schemas;
using ThreatsManager.Extensions.Word;
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
        private ReportGenerator _reportGenerator;
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
            if (_model != null)
            {
                _reportGenerator.ProgressUpdated -= ReportGeneratorOnProgressUpdated;
                _reportGenerator.ShowMessage -= ReportGeneratorOnShowMessage;
                _reportGenerator.ShowWarning -= ReportGeneratorOnShowWarning;
            }

            _model = threatModel;
            _reportGenerator = new ReportGenerator(_model);
            _reportGenerator.ProgressUpdated += ReportGeneratorOnProgressUpdated;
            _reportGenerator.ShowMessage += ReportGeneratorOnShowMessage;
            _reportGenerator.ShowWarning += ReportGeneratorOnShowWarning;
            
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

        private void ReportGeneratorOnShowWarning(string text)
        {
            ShowWarning?.Invoke(text);
        }

        private void ReportGeneratorOnShowMessage(string text)
        {
            ShowMessage?.Invoke(text);
        }

        private void ReportGeneratorOnProgressUpdated(int percentage)
        {
            UpdateProgress(percentage);
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
                _docStructure.PrimaryGrid.Rows.Clear();
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
                var structure = _reportGenerator.LoadStructure(fileName)?.ToArray();

                if (structure?.Any() ?? false)
                {
                    CreatePlaceholderPanel(PlaceholderSection.Model, structure);
                    CreatePlaceholderPanel(PlaceholderSection.Counter, structure);
                    CreatePlaceholderPanel(PlaceholderSection.Chart, structure);
                    CreatePlaceholderPanel(PlaceholderSection.List, structure);
                    CreatePlaceholderPanel(PlaceholderSection.Table, structure);

                    result = true;
                }
            }
            catch
            {
                ShowWarning?.Invoke("An error occurred while loading the reference document structure.");
            }
            finally
            {
                _loading = false;
            }

            return result;
        }

        private void CreatePlaceholderPanel(PlaceholderSection section, IEnumerable<IPlaceholder> placeholders)
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

        private GridPanel CreateSimplePanel(PlaceholderSection section, [NotNull] IEnumerable<IPlaceholder> placeholders)
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
                var row = new GridRow(ph.Label) {Tag = ph};
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
                    if (row.Tag is IListPlaceholder placeholder)
                    {
                        var panel = CreateListPanel(placeholder.Name, placeholder.GetProperties(_model));
                        if (panel != null)
                            row.Rows.Add(panel);
                    } else if (row.Tag is IGroupedListPlaceholder groupedListPlaceholder)
                    {
                        var panel = CreateListPanel(groupedListPlaceholder.Name, groupedListPlaceholder.GetProperties(_model));
                        if (panel != null)
                            row.Rows.Add(panel);
                    }
                }
            }
        }

        private GridPanel CreateListPanel([Required] string placeholderName, IEnumerable<KeyValuePair<string, IPropertyType>> fields)
        {
            GridPanel result = null;

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

                var ignored = GetIgnoredFields(placeholderName)?.ToArray();
                foreach (var field in fields)
                {
                    var schema = _model.GetSchema(field.Value.SchemaId);
                    if (schema != null)
                    {
                        var row = new GridRow(field.Key, schema.Namespace, schema.Name)
                        {
                            Tag = field.Value,
                            Checked = !(ignored?.Contains(field.Key) ?? false)
                        };
                        result.Rows.Add(row);
                    }
                }
            }

            return result;
        }

        public IEnumerable<string> GetPlaceholdersWithIgnoredFields()
        {
            IEnumerable<string> result = null;

            var schemaManager = new WordPropertySchemaManager(_model);
            var schema = schemaManager.GetSchema();
            var propertyType = schema?.GetPropertyType("IgnoredListFields");
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) as IPropertyArray;
                result = property?.Value?.Select(x => x.Split('#').FirstOrDefault())
                    .Distinct().Where(x => !string.IsNullOrWhiteSpace(x));
            }

            return result;
        }

        public IEnumerable<string> GetIgnoredFields(string placeholderName)
        {
            IEnumerable<string> result = null;

            var schemaManager = new WordPropertySchemaManager(_model);
            var schema = schemaManager.GetSchema();
            var propertyType = schema?.GetPropertyType("IgnoredListFields");
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) as IPropertyArray;
                var values = property?.Value?.Where(x => x.StartsWith($"{placeholderName}#")).ToArray();
                if (values?.Any() ?? false)
                {
                    result = values.Select(x => x.Replace($"{placeholderName}#", ""));
                }
            }

            return result;
        }

        public void SetIgnoredFields(string placeholderName, IEnumerable<string> fields)
        {
            var schemaManager = new WordPropertySchemaManager(_model);
            var schema = schemaManager.GetSchema();
            var propertyType = schema?.GetPropertyType("IgnoredListFields");
            if (propertyType != null)
            {
                var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                if (property is IPropertyArray propertyArray)
                {
                    var list = new List<string>();
                    var existing = propertyArray.Value?
                        .Where(x => !string.IsNullOrWhiteSpace(x) && !x.StartsWith($"{placeholderName}#"))
                        .ToArray();
                    if (existing?.Any() ?? false)
                    {
                        list.AddRange(existing);
                    }
                    if (fields?.Any() ?? false)
                    {
                        list.AddRange(fields.Select(x => $"{placeholderName}#{x}"));
                    }
                    propertyArray.Value = list;
                }
            }
        }

        private void _docStructure_AfterCheck(object sender, GridAfterCheckEventArgs e)
        {
            if (e.Item is GridRow row && row.Tag is IPropertyType propertyType &&
                row.GridPanel.Parent is GridRow parent && parent.Tag is IPlaceholder placeholder)
            {
                var field = row.Cells.FirstOrDefault()?.Value?.ToString();

                if (!string.IsNullOrWhiteSpace(field))
                {
                    var ignored = GetIgnoredFields(placeholder.Name)?.ToArray();
                    if (row.Checked && (ignored?.Contains(field) ?? false))
                    {
                        SetIgnoredFields(placeholder.Name, ignored.Where(x => string.CompareOrdinal(x, field) != 0));
                    }
                    else if (!row.Checked && !(ignored?.Contains(field) ?? false))
                    {
                        var list = new List<string>();
                        if (ignored?.Any() ?? false)
                            list.AddRange(ignored);
                        list.Add(field);
                        SetIgnoredFields(placeholder.Name, list);
                    }
                }
            }
        }
        #endregion
    }
}
