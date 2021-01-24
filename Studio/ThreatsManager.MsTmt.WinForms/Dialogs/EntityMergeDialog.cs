using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.SuperGrid.Style;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.MsTmt.Dialogs
{
    public partial class EntityMergeDialog : Form
    {
        private bool _loading;

        public EntityMergeDialog()
        {
            InitializeComponent();

            // ReSharper disable once CoVariantArrayConversion
            _strategy.Items.AddRange(EnumExtensions.GetEnumLabels<ReplacementStrategy>().ToArray());
            _strategy.SelectedIndex = 0;

            InitializeGrid();
        }

        public void Initialize([NotNull] IEnumerable<IEntity> entities)
        {
            var list = entities.ToArray();
            if (list.Any())
            {
                try
                {
                    _loading = true;
                    foreach (var entity in list)
                        AddGridRow(entity);
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private void InitializeGrid()
        {
            GridPanel panel = _grid.PrimaryGrid;
            panel.ShowTreeButtons = false;
            panel.ShowTreeLines = false;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.AllowRowResize = true;
            panel.ShowRowDirtyMarker = false;
            panel.ShowRowHeaders = false;
            panel.InitialActiveRow = RelativeRow.None;

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
                Width = 400,
                DataType = typeof(string),
                AllowEdit = false
            });
            panel.Columns["Description"].CellStyles.Default.AllowMultiLine = Tbool.True;

            panel.Columns.Add(new GridColumn("Parent")
            {
                HeaderText = "Parent",
                DataType = typeof(string),
                AllowEdit = false,
                Width = 150
            });

            panel.Columns.Add(new GridColumn("Target")
            {
                HeaderText = "Is Target",
                DataType = typeof(bool),
                AllowEdit = true,
                Width = 75,
                EditorType = typeof(GridSwitchButtonEditControl),
            });

            panel.Columns.Add(new GridColumn("Source")
            {
                HeaderText = "Is Source",
                DataType = typeof(bool),
                AllowEdit = true,
                Width = 75,
                EditorType = typeof(GridSwitchButtonEditControl),
            });
        }

        public IEntity Target =>
            _grid.PrimaryGrid.Rows.OfType<GridRow>().FirstOrDefault(x => (bool) x.Cells["Target"].Value)
                ?.Tag as IEntity;

        public IEnumerable<IEntity> Sources =>
            _grid.PrimaryGrid.Rows.OfType<GridRow>().Where(x => (bool) x.Cells["Source"].Value)
                .Select(x => x.Tag).OfType<IEntity>();

        public ReplacementStrategy Strategy => ((string) _strategy.SelectedItem).GetEnumValue<ReplacementStrategy>();

        private void AddGridRow([NotNull] IEntity entity)
        {
            GridRow row = new GridRow(
                entity.Name,
                entity.Description,
                entity.Parent?.Name,
                false,
                false)
            {
                Tag = entity
            };
            row.Cells[0].CellStyles.Default.Image = entity.GetImage(ImageSize.Small);
            row.Cells[3].PropertyChanged += OnTargetChanged;
            row.Cells[4].PropertyChanged += OnSourceChanged;
            _grid.PrimaryGrid.Rows.Add(row);
        }

        private void OnTargetChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_loading && sender is GridCell cell && cell.GridRow is GridRow row && row.Tag is IEntity entity)
            {
                _loading = true;

                try
                {
                    if ((bool) cell.Value)
                    {
                        // New Target.
                        row.Cells["Source"].Value = false;
                        row.Cells["Source"].ReadOnly = true;
                        var targetType = entity.GetType();

                        var siblings = row.GridPanel.Rows.OfType<GridRow>().Where(x => x != row).ToArray();
                        if (siblings.Any())
                        {
                            foreach (var sibling in siblings)
                            {
                                sibling.Cells["Target"].Value = false;

                                if (sibling.Tag is IEntity siblingEntity)
                                {
                                    var siblingType = siblingEntity.GetType();
                                    sibling.Cells["Source"].Value = siblingType == targetType;
                                    sibling.Cells["Source"].ReadOnly = siblingType != targetType;
                                    _ok.Enabled = true;
                                }
                                else
                                {
                                    sibling.Cells["Source"].Value = false;
                                    sibling.Cells["Source"].ReadOnly = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        _ok.Enabled = false;
                        var rows = row.GridPanel.Rows.OfType<GridRow>().ToArray();
                        foreach (var r in rows)
                        {
                            r.Cells["Source"].ReadOnly = false;
                        }
                    }
                }
                finally
                {
                    _loading = false;
                }
            }
        }
 
        private void OnSourceChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_loading && sender is GridCell cell && cell.GridRow is GridRow row)
            {
                _ok.Enabled = row.GridPanel.Rows.OfType<GridRow>().Any(x => (bool) x.Cells["Source"].Value);
            }
        }
    }
}
