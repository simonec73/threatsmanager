using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Panels.SeverityList
{
    public partial class SeverityListPanel : UserControl, IShowThreatModelPanel<Form>, ICustomRibbonExtension, 
        IInitializableObject, IDesktopAlertAwareExtension
    {
        private readonly Guid _id = Guid.NewGuid();
        private IThreatModel _model;
        private bool _promoted;
        private GridRow _currentRow;
        private bool _loading;

        public SeverityListPanel()
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
            _model.SeverityCreated += ModelChildCreated;
            _model.SeverityRemoved += ModelChildRemoved;

            LoadModel();
        }

        private void ModelChildRemoved([NotNull] ISeverity severity)
        {
            var row = GetRow(severity);
            if (row != null)
            {
                RemoveEventSubscriptions(row);
                _grid.PrimaryGrid.Rows.Remove(row);
            }
        }

        private void ModelChildCreated([NotNull] ISeverity severity)
        {
            if (severity.Visible)
                AddGridRow(severity, _grid.PrimaryGrid);
        }

        #endregion

        public bool IsInitialized => _model != null;
        
        private void InitializeGrid()
        {
            lock (_grid)
            {
                // TODO: Add here the new columns.

                GridPanel panel = _grid.PrimaryGrid;
                panel.ShowTreeButtons = false;
                panel.AllowRowDelete = false;
                panel.AllowRowInsert = false;
                panel.ShowRowDirtyMarker = false;
                panel.ShowRowHeaders = false;
                panel.InitialActiveRow = RelativeRow.None;

                panel.Columns.Add(new GridColumn("Id")
                {
                    HeaderText = "Id",
                    Width = 50,
                    DataType = typeof(int),
                    EditorType = typeof(GridIntegerInputEditControl),
                    AllowEdit = false
                });

                panel.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Name",
                    Width = 200,
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
 
                panel.Columns.Add(new GridColumn("Description")
                {
                    HeaderText = "Description",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    EditorType = typeof(GridTextBoxDropDownEditControl),
                    AllowEdit = true
                });
                panel.Columns["Description"].CellStyles.Default.AllowMultiLine = Tbool.True;
                ddc = panel.Columns["Description"].EditControl as GridTextBoxDropDownEditControl;
                if (ddc != null)
                {
                    ddc.ButtonClear.Visible = true;
                    ddc.ButtonClearClick += DdcButtonClearClick;
                }
 
                panel.Columns.Add(new GridColumn("Visible")
                {
                    HeaderText = "Visible",
                    Width = 100,
                    DataType = typeof(bool),
                    EditorType = typeof(GridSwitchButtonEditControl),
                    AllowEdit = true
                });

                panel.Columns.Add(new GridColumn("TextColor")
                {
                    HeaderText = "Text",
                    Width = 120,
                    DataType = typeof(string),
                    EditorType = typeof(GridColorCombo),
                    AllowEdit = true
                });
 
                panel.Columns.Add(new GridColumn("BackColor")
                {
                    HeaderText = "Background",
                    Width = 120,
                    DataType = typeof(string),
                    EditorType = typeof(GridColorCombo),
                    AllowEdit = true
                });
            }
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

        private void LoadModel()
        {
            try
            {
                _grid.SuspendLayout();
                _loading = true;
                var panel = _grid.PrimaryGrid;
                panel.Rows.Clear();

                var items = _model.Severities?.OrderByDescending(x => x.Id).ToArray();
                var filter = _filter.Text;

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (string.IsNullOrWhiteSpace(filter) || IsSelected(item, filter))
                        {
                            AddGridRow(item, panel);
                        }
                    }
                }
            }
            finally
            {
                _loading = false;
                _grid.ResumeLayout(true);
            }
        }

        private void AddGridRow([NotNull] ISeverity severity, [NotNull] GridPanel panel)
        {
            var row = new GridRow(
                severity.Id,
                severity.Name,
                severity.Description,
                severity.Visible,
                severity.TextColor.ToString(),
                severity.BackColor.ToString());
            ((INotifyPropertyChanged) severity).PropertyChanged += OnSeverityPropertyChanged;
            row.Tag = severity;
            for (int i = 0; i < row.Cells.Count; i++)
                row.Cells[i].PropertyChanged += OnPropertyChanged;
            panel.Rows.Add(row);
        }

        private void RemoveEventSubscriptions(GridRow row)
        {
            if (row?.Tag is ISeverity severity)
            {
                ((INotifyPropertyChanged) severity).PropertyChanged -= OnSeverityPropertyChanged;
                for (int i = 0; i < row.Cells.Count; i++)
                    row.Cells[i].PropertyChanged -= OnPropertyChanged;
            }
        }

        private void OnSeverityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ISeverity severity)
            {
                var row = GetRow(severity);
                if (row != null)
                {
                    switch (e.PropertyName)
                    {
                        case "Id":
                            row["Id"].Value = severity.Id;
                            break;
                        case "Name":
                            row["Name"].Value = severity.Name;
                            break;
                        case "Description":
                            row["Description"].Value = severity.Description;
                            break;
                        case "Visible":
                            row["Visible"].Value = severity.Visible;
                            break;
                        case "TextColor":
                            row["TextColor"].Value = severity.TextColor.ToString();
                            break;
                        case "BackColor":
                            row["BackColor"].Value = severity.BackColor.ToString();
                            break;
                    }
                }
            }
        }

        private GridRow GetRow([NotNull] ISeverity severity)
        {
            GridRow result = null;

            var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row.Tag == severity)
                {
                    result = row;
                    break;
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

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var cell = sender as GridCell;
            var propertyName = propertyChangedEventArgs.PropertyName;

            if (!_loading && cell != null)
            {
                try
                {
                    _loading = true;
                    var row = cell.GridRow;
                    var severity = row.Tag as ISeverity;
                    if (severity != null)
                    {
                        switch (cell.GridColumn.Name)
                        {
                            case "Name":
                                severity.Name = (string) cell.Value;
                                break;
                            case "Description":
                                severity.Description = (string) cell.Value;
                                break;
                            case "Visible":
                                severity.Visible = (bool) cell.Value;
                                break;
                            case "TextColor":
                                if (Enum.TryParse<KnownColor>((string) cell.Value, out var textColor))
                                    severity.TextColor = textColor;
                                break;
                            case "BackColor":
                                if (Enum.TryParse<KnownColor>((string) cell.Value, out var backColor))
                                    severity.BackColor = backColor;
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

        private bool IsSelected([NotNull] ISeverity item, [Required] string filter)
        {
            var result = (!string.IsNullOrWhiteSpace(item.Name) &&
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

                if (row?.Tag != null)
                {
                    MenuDefinition.UpdateVisibility(_contextMenu, row.Tag);
                    _contextMenu?.Show(_grid.PointToScreen(e.Location));
                }
            }
        }

        private void _grid_CellClick(object sender, GridCellClickEventArgs e)
        {
            if (!_loading)
            {
                if (e.GridCell != null)
                {
                    _currentRow = e.GridCell.GridRow;
                    ShowCurrentRow();
                }
            }
        }

        private void _grid_RowClick(object sender, GridRowClickEventArgs e)
        {
            if (!_loading)
            {
                if (e.GridRow is GridRow gridRow)
                {
                    _currentRow = gridRow;
                    ShowCurrentRow();
                }
            }
        }

        private void ShowCurrentRow()
        {
            if (_promoted)
                ChangeCustomActionStatus?.Invoke("RemoveSeverity", _currentRow.Tag is ISeverity);
        }

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
    }
}
