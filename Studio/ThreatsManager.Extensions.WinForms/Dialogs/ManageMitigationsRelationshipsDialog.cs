using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Relationships;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.WinForms;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class ManageMitigationsRelationshipsDialog : Form
    {
        private IMitigation _mitigation;
        private RelationshipsManager _manager;
        private AssociationType _associationType;
        private Guid _mainId;
        private bool _loading;

        public ManageMitigationsRelationshipsDialog()
        {
            InitializeComponent();
            InitializeGrid();
            _grid.VScrollBar.Width = 20;
        }

        public ManageMitigationsRelationshipsDialog([NotNull] IMitigation mitigation,
            AssociationType associationType) : this()
        {
            _loading = true;
            _mitigation = mitigation;
            _mitigationName.Text = mitigation.Name;
            _associationType = associationType;

            _manager = new RelationshipsManager(mitigation);

            Text = $"Manage {associationType.GetEnumLabel()} Mitigations";

            _grid.PrimaryGrid.Rows.Clear();

            IEnumerable<Guid> associatedIds;
            IEnumerable<Guid> notAvailableIds;
            Guid mainId;
            switch (associationType)
            {
                case AssociationType.Alternative:
                    associatedIds = _manager.GetAlternativeIDs()?.ToArray();
                    notAvailableIds = _manager.GetComplementaryIDs()?.ToArray();
                    mainId = _manager.GetMainAlternativeId();
                    break;
                case AssociationType.Complementary:
                    associatedIds = _manager.GetComplementaryIDs()?.ToArray();
                    notAvailableIds = _manager.GetAlternativeIDs()?.ToArray();
                    mainId = _manager.GetMainComplementaryId();
                    break;
                default:
                    return;
            }

            // Get all mitigations that are not the current one and not associated to the other relationship.
            var mitigations = _mitigation.Model?.Mitigations?
                .Where(x => x.Id != _mitigation.Id &&
                            !(notAvailableIds?.Any(y => y == x.Id) ?? false))
            .OrderBy(x => x.Name)
            .ToArray();

            if (mitigations?.Any() ?? false)
            {
                foreach (var m in mitigations)
                {
                    var row = new GridRow(m.Name,
                        m.ControlType.GetEnumLabel(),
                        m.Id == mainId);
                    row.Tag = m;
                    if (associatedIds?.Any(x => x == m.Id) ?? false)
                    {
                        row.Checked = true;
                    }
                    _grid.PrimaryGrid.Rows.Add(row);
                }
            }

            _main.Checked = _mitigation.Id == mainId;

            _loading = false;
        }

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
                panel.CheckBoxes = true;
                panel.ShowCheckBox = true;

                panel.Columns.Add(new GridColumn("Name")
                {
                    HeaderText = "Mitigation Name",
                    AutoSizeMode = ColumnAutoSizeMode.Fill,
                    DataType = typeof(string),
                    AllowEdit = false
                });
                panel.Columns["Name"].CellStyles.Default.Padding = new DevComponents.DotNetBar.SuperGrid.Style.Padding(8, 0, 0, 0);

                panel.Columns.Add(new GridColumn("ControlType")
                {
                    HeaderText = "Control Type",
                    AutoSizeMode = ColumnAutoSizeMode.AllCells,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                panel.Columns.Add(new GridColumn("Main")
                {
                    HeaderText = "Main",
                    AutoSizeMode = ColumnAutoSizeMode.AllCells,
                    DataType = typeof(bool),
                    EditorType = typeof(GridCheckBoxEditControl),
                    AllowEdit = true
                });
                panel.Columns["Main"].CellStyles.Default.Alignment = DevComponents.DotNetBar.SuperGrid.Style.Alignment.MiddleCenter;
                var cb = panel.Columns["Main"].EditControl as GridCheckBoxEditControl;
                if (cb != null)
                {
                    cb.CheckedChanged += OnMainChanged;
                    panel.DefaultRowHeight = cb.Height + 8;
                }
            }
        }

        private void OnMainChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                var cb = sender as GridCheckBoxEditControl;
                var row = cb?.EditorCell.GridRow;

                if (row?.Tag is IMitigation associatedM)
                {
                    var c = cb.Checked;
                    if (c)
                    {
                        if (!row.Checked)
                            row.Checked = true;
                        _mainId = associatedM.Id;
                        ClearMainFlag();
                        _main.Checked = false;
                    }
                }
            }
        }

        private void ClearMainFlag()
        {
            var cb = _grid.PrimaryGrid.Columns["Main"].EditControl as GridCheckBoxEditControl;
            if (cb != null)
            {
                cb.CheckedChanged -= OnMainChanged;

                try
                {
                    var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
                    foreach (var row in rows)
                    {
                        if (row.Tag is IMitigation m && m.Id != _mainId)
                        {
                            row["Main"].Value = false;
                        }
                    }
                }
                finally
                {
                    cb.CheckedChanged += OnMainChanged;
                }
            }
        }

        private void _clear_Click(object sender, EventArgs e)
        {
            try
            { 
                _loading = true;
                _mainId = Guid.Empty;
                ClearMainFlag();
                _main.Checked = false;

                var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
                foreach (var row in rows)
                {
                    row.Checked = false;
                }
            }
            finally
            {
                _loading = false;
            }
        }

        private void _main_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loading)
            {
                if (_main.Checked)
                {
                    _mainId = _mitigation.Id;
                    ClearMainFlag();
                }
                else
                {
                    if (_mainId == _mitigation.Id)
                        _mainId = Guid.Empty;
                }
            }
        }

        private void _grid_AfterCheck(object sender, GridAfterCheckEventArgs e)
        {
            if (!_loading && e.Item is GridRow row && !row.Checked)
            {
                row["Main"].Value = false;
            }
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            using (var scope = UndoRedoManager.OpenScope("Change Mitigation Relationships"))
            {
                var relationshipIds = _grid.PrimaryGrid.Rows.OfType<GridRow>()
                    .Where(x => x.Checked)
                    .Select(x => ((IMitigation)x.Tag).Id)
                    .ToArray();

                IEnumerable<Guid> current = null;
                switch (_associationType)
                {
                    case AssociationType.Alternative:
                        current = _manager.GetAlternativeIDs()?.ToArray();
                        break;
                    case AssociationType.Complementary:
                        current = _manager.GetComplementaryIDs()?.ToArray();
                        break;
                }

                if (relationshipIds?.Any() ?? false)
                {
                    foreach (var relationship in relationshipIds)
                    {
                        if (!(current?.Any(x => x == relationship) ?? false))
                        {
                            switch (_associationType)
                            {
                                case AssociationType.Alternative:
                                    _manager.AddAlternative(relationship);
                                    break;
                                case AssociationType.Complementary:
                                    _manager.AddComplementary(relationship);
                                    break;
                            }
                        }
                    }
                }

                if (current?.Any() ?? false)
                {
                    foreach (var relationship in current)
                    {
                        if (!relationshipIds.Any(x => x == relationship))
                        {
                            switch (_associationType)
                            {
                                case AssociationType.Alternative:
                                    _manager.RemoveAlternative(relationship);
                                    break;
                                case AssociationType.Complementary:
                                    _manager.RemoveComplementary(relationship);
                                    break;
                            }
                        }
                    }
                }

                switch (_associationType)
                {
                    case AssociationType.Alternative:
                        _manager.SetMainAlternative(_mainId);
                        break;
                    case AssociationType.Complementary:
                        _manager.SetMainComplementary(_mainId);
                        break;
                }

                scope?.Complete();
            }
        }
    }
}
