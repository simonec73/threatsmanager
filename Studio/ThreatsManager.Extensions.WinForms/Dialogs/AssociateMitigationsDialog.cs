using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class AssociateMitigationsDialog : Form
    {
        private IThreatModel _model;
        private IThreatType _threatType;
        private IThreatEvent _threatEvent;
        private IWeakness _weakness;
        private IVulnerability _vulnerability;

        public AssociateMitigationsDialog()
        {
            InitializeComponent();
        }

        public void Initialize([NotNull] IThreatType threatType)
        {
            _threatType = threatType;
            _model = threatType.Model;
            _standardMitigationsContainer.Visible = false;
            InitializeGrid(false);
            InitializeItem(threatType);

            var existingMitigations = threatType.Mitigations?.ToArray();
            var mitigations = _model?.Mitigations?.OrderBy(x => x.Name);
            if (mitigations != null)
            {
                var defaultStrength = _model.GetStrength((int) DefaultStrength.Average);

                foreach (var mitigation in mitigations)
                {
                    var existingMitigation = existingMitigations?.FirstOrDefault(x => x.MitigationId == mitigation.Id);
                    var row = new GridRow(mitigation.Name,
                        mitigation.ControlType.GetEnumLabel(),
                        existingMitigation?.Strength ?? defaultStrength);
                    row.Tag = mitigation;
                    row.Checked = existingMitigation != null;
                    _grid.PrimaryGrid.Rows.Add(row);
                }
            }
        }

        public void Initialize([NotNull] IThreatEvent threatEvent)
        {
            _threatEvent = threatEvent;
            _model = threatEvent.Model;
            _standardMitigationsContainer.Visible = true;
            InitializeGrid(true);
            InitializeItem(threatEvent);

            var existingMitigations = threatEvent.Mitigations?.ToArray();
            var mitigations = _model?.Mitigations?.OrderBy(x => x.Name);
            if (mitigations != null)
            {
                var defaultStrength = _model.GetStrength((int)DefaultStrength.Average);

                foreach (var mitigation in mitigations)
                {
                    var existingMitigation = existingMitigations?.FirstOrDefault(x => x.MitigationId == mitigation.Id);
                    var row = new GridRow(mitigation.Name,
                        mitigation.ControlType.GetEnumLabel(),
                        existingMitigation?.Strength ?? defaultStrength,
                        existingMitigation?.Status ?? MitigationStatus.Undefined);
                    row.Tag = mitigation;
                    row.Checked = existingMitigation != null;
                    _grid.PrimaryGrid.Rows.Add(row);
                }
            }
        }

        public void Initialize([NotNull] IWeakness weakness)
        {
            _weakness = weakness;
            _model = weakness.Model;
            _standardMitigationsContainer.Visible = false;
            InitializeGrid(false);
            InitializeItem(weakness);

            var existingMitigations = weakness.Mitigations?.ToArray();
            var mitigations = _model?.Mitigations?.OrderBy(x => x.Name);
            if (mitigations != null)
            {
                var defaultStrength = _model.GetStrength((int)DefaultStrength.Average);

                foreach (var mitigation in mitigations)
                {
                    var existingMitigation = existingMitigations?.FirstOrDefault(x => x.MitigationId == mitigation.Id);
                    var row = new GridRow(mitigation.Name,
                        mitigation.ControlType.GetEnumLabel(),
                        existingMitigation?.Strength ?? defaultStrength);
                    row.Tag = mitigation;
                    row.Checked = existingMitigation != null;
                    _grid.PrimaryGrid.Rows.Add(row);
                }
            }
        }

        public void Initialize([NotNull] IVulnerability vulnerability)
        {
            _vulnerability = vulnerability;
            _model = vulnerability.Model;
            _standardMitigationsContainer.Visible = true;
            InitializeGrid(true);
            InitializeItem(vulnerability);

            var existingMitigations = vulnerability.Mitigations?.ToArray();
            var mitigations = _model?.Mitigations?.OrderBy(x => x.Name);
            if (mitigations != null)
            {
                var defaultStrength = _model.GetStrength((int)DefaultStrength.Average);

                foreach (var mitigation in mitigations)
                {
                    var existingMitigation = existingMitigations?.FirstOrDefault(x => x.MitigationId == mitigation.Id);
                    var row = new GridRow(mitigation.Name,
                        mitigation.ControlType.GetEnumLabel(),
                        existingMitigation?.Strength ?? defaultStrength,
                        existingMitigation?.Status ?? MitigationStatus.Undefined);
                    row.Tag = mitigation;
                    row.Checked = existingMitigation != null;
                    _grid.PrimaryGrid.Rows.Add(row);
                }
            }
        }

        private void InitializeGrid(bool showStatus)
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
                panel.CheckBoxes = true;
                panel.ShowCheckBox = true;

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

                panel.Columns.Add(new GridColumn("Strength")
                {
                    HeaderText = "Strength",
                    DataType = typeof(IStrength),
                    EditorType = typeof(StrengthComboBox),
                    EditorParams = new object[] { _model.Strengths?.Where(x => x.Visible) },
                    AllowEdit = true,
                    Width = 75
                });

                if (showStatus)
                {
                    panel.Columns.Add(new GridColumn("Status")
                    {
                        HeaderText = "Status",
                        DataType = typeof(string),
                        EditorType = typeof(EnumComboBox),
                        EditorParams = new object[] {EnumExtensions.GetEnumLabels<MitigationStatus>()},
                        AllowEdit = true,
                        Width = 75
                    });
                }
            }
        }

        private void InitializeItem(IIdentity item)
        {
            string text = item.Name;
            int height = (int)(21 * Dpi.Factor.Height);
            var image = item.GetImage(ImageSize.Small);
            if (image != null)
            {
                height = image.Height + 10;
                text = "      " + text;
                _item.Image = image;
            }
            _item.Text = text;
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            var checkedRows = _grid.PrimaryGrid.Rows.OfType<GridRow>().Where(x => x.Checked).ToArray();
            if (checkedRows.Any())
            {
                foreach (var row in checkedRows)
                {
                    if (row.Tag is IMitigation mitigation && !IsAssigned(mitigation) && row["Strength"].Value is IStrength strength)
                    {
                        if (_grid.PrimaryGrid.Columns.Contains("Status") && 
                            Enum.TryParse<MitigationStatus>(row["Status"].Value as string, out var status))
                            Assign(mitigation, strength, status);
                        else
                            Assign(mitigation, strength);
                    }
                }
            }

            var uncheckedRows = _grid.PrimaryGrid.Rows.OfType<GridRow>().Where(x => !x.Checked).ToArray();
            if (uncheckedRows.Any())
            {
                foreach (var row in uncheckedRows)
                {
                    if (row.Tag is IMitigation mitigation && IsAssigned(mitigation))
                    {
                        Unassign(mitigation);
                    }
                }
            }
        }

        private bool IsAssigned([NotNull] IMitigation mitigation)
        {
            bool result = false;

            if (_threatType != null)
            {
                result = _threatType.Mitigations?.Any(x => x.MitigationId == mitigation.Id) ?? false;
            } else if (_threatEvent != null)
            {
                result = _threatEvent.Mitigations?.Any(x => x.MitigationId == mitigation.Id) ?? false;
            }
            else if (_weakness != null)
            {
                result = _weakness.Mitigations?.Any(x => x.MitigationId == mitigation.Id) ?? false;
            }
            else if (_vulnerability != null)
            {
                result = _vulnerability.Mitigations?.Any(x => x.MitigationId == mitigation.Id) ?? false;
            }

            return result;
        }

        private void Assign([NotNull] IMitigation mitigation, [NotNull] IStrength strength, MitigationStatus status = MitigationStatus.Undefined)
        {
            if (_threatType != null)
            {
                _threatType.AddMitigation(mitigation, strength);
            }
            else if (_threatEvent != null)
            {
                if (_standardMitigationsContainer.Visible && _standardMitigations.Checked &&
                    !(_threatEvent.ThreatType?.Mitigations?.Any(x => x.MitigationId == mitigation.Id) ?? false))
                {
                    _threatEvent.ThreatType?.AddMitigation(mitigation, strength);
                }

                _threatEvent.AddMitigation(mitigation, strength, status);
            }
            else if (_weakness != null)
            {
                _weakness.AddMitigation(mitigation, strength);
            }
            else if (_vulnerability != null)
            {
                if (_standardMitigationsContainer.Visible && _standardMitigations.Checked &&
                    !(_vulnerability.Weakness?.Mitigations?.Any(x => x.MitigationId == mitigation.Id) ?? false))
                {
                    _vulnerability.Weakness?.AddMitigation(mitigation, strength);
                }

                _vulnerability.AddMitigation(mitigation, strength, status);
            }
        }

        private void Unassign([NotNull] IMitigation mitigation)
        {
            if (_threatType != null)
            {
                _threatType.RemoveMitigation(mitigation.Id);
            }
            else if (_threatEvent != null)
            {
                _threatEvent.RemoveMitigation(mitigation.Id);
            }
            else if (_weakness != null)
            {
                _weakness.RemoveMitigation(mitigation.Id);
            }
            else if (_vulnerability != null)
            {
                _vulnerability.RemoveMitigation(mitigation.Id);
            }
        }
    }
}
