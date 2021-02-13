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
    public partial class FlowMergeDialog : Form
    {
        private class Matrix
        {
            private IDictionary<string, IList<IDataFlow>> _items = new Dictionary<string, IList<IDataFlow>>();

            private string _current;

            public void Add([NotNull] IDataFlow dataFlow)
            {
                var key = GetKey(dataFlow);

                IList<IDataFlow> flows;
                if (_items.ContainsKey(key))
                {
                    flows = _items[key];
                }
                else
                {
                    flows = new List<IDataFlow>();
                    _items.Add(key, flows);
                }

                if (!flows.Contains(dataFlow))
                    flows.Add(dataFlow);
            }

            public IEntity GetCurrentSource()
            {
                IEntity result = null;

                var current = GetCurrent();

                if (current != null && _items.ContainsKey(current))
                {
                    var flows = _items[current];
                    if (flows?.Any() ?? false)
                    {
                        result = flows[0].Source;
                    }
                }

                return result;
            }

            public IEntity GetCurrentTarget()
            {
                IEntity result = null;

                var current = GetCurrent();

                if (current != null && _items.ContainsKey(current))
                {
                    var flows = _items[current];
                    if (flows?.Any() ?? false)
                    {
                        result = flows[0].Target;
                    }
                }

                return result;
            }

            public IEnumerable<IDataFlow> GetCurrentDataFlows()
            {
                IEnumerable<IDataFlow> result = null;

                var current = GetCurrent();

                if (current != null && _items.ContainsKey(current))
                {
                    var flows = _items[current];
                    if (flows?.Any() ?? false)
                    {
                        result = flows;
                    }
                }

                return result;
            }

            public bool Next()
            {
                _current = GetNext();

                return _current != null;
            }
 
            private string GetKey([NotNull] IDataFlow dataFlow)
            {
                var first = dataFlow.SourceId.ToString("N");
                var second = dataFlow.TargetId.ToString("N");

                return (string.CompareOrdinal(first, second) > 0) ? $"{first}{second}" : $"{second}{first}";
            }
 
            private string GetCurrent()
            {
                return _current ?? (_current = _items.Where(x => x.Value.Count > 1)
                           .Select(x => x.Key).FirstOrDefault());
            }

            private string GetNext()
            {
                var current = GetCurrent();

                return _items.Where(x => x.Value.Count > 1).Select(x => x.Key)
                    .FirstOrDefault(x => string.CompareOrdinal(x, current) > 0);
            }
       }

        private bool _loading;

        public FlowMergeDialog()
        {
            InitializeComponent();

            // ReSharper disable once CoVariantArrayConversion
            InitializeGrid();
        }

        public bool Initialize([NotNull] IEnumerable<IEntity> entities, [NotNull] IEnumerable<IDataFlow> flows)
        {
            bool result = false;

            _loading = true;

            try
            {
                var matrix = new Matrix();

                foreach (var entity in entities)
                {
                    var entityFlows = entity.Model?.DataFlows?
                        .Where(x => x.SourceId == entity.Id || x.TargetId == entity.Id).ToArray();

                    if (entityFlows?.Any() ?? false)
                    {
                        foreach (var flow in entityFlows)
                        {
                            matrix.Add(flow);
                        }
                    }
                }

                foreach (var flow in flows)
                {
                    matrix.Add(flow);
                }

                do
                {
                    var source = matrix.GetCurrentSource();
                    var target = matrix.GetCurrentTarget();
                    var dataFlows = matrix.GetCurrentDataFlows()?.ToArray();
                    if (source != null && target != null && (dataFlows?.Any() ?? false))
                    {
                        AddGridRow(source, target, dataFlows);
                        result = true;
                    }
                } while (matrix.Next());
            }
            finally
            {
                _loading = false;
            }

            return result;
        }

        public IEnumerable<FlowMergeInfo> Merges
        {
            get
            {
                List<FlowMergeInfo> result = null;

                var rows = _grid.PrimaryGrid.Rows.OfType<GridRow>().ToArray();
                foreach (var row in rows)
                {
                    var mergeInfo = GetFlowMergeInfo(row);
                    if (mergeInfo != null)
                    {
                        if (result == null)
                            result = new List<FlowMergeInfo>();
                        result.Add(mergeInfo);
                    }
                }

                return result;
            }
        }

        private void InitializeGrid()
        {
            GridPanel panel = _grid.PrimaryGrid;
            panel.ShowTreeButtons = true;
            panel.ShowTreeLines = true;
            panel.AllowRowDelete = false;
            panel.AllowRowInsert = false;
            panel.AllowRowResize = true;
            panel.ShowRowDirtyMarker = false;
            panel.InitialActiveRow = RelativeRow.None;

            panel.Columns.Add(new GridColumn("First")
            {
                HeaderText = "First Entity",
                Width = 200,
                DataType = typeof(string),
                AllowEdit = false
            });

            panel.Columns.Add(new GridColumn("Second")
            {
                HeaderText = "Second Entity",
                Width = 200,
                DataType = typeof(string),
                AllowEdit = false
            });

            panel.Columns.Add(new GridColumn("Name")
            {
                HeaderText = "Flow Name",
                Width = 350,
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

            panel.Columns.Add(new GridColumn("Ignore")
            {
                HeaderText = "Ignore",
                DataType = typeof(bool),
                AllowEdit = true,
                Width = 75,
                EditorType = typeof(GridSwitchButtonEditControl),
            });
        }

        private GridPanel InitializeSubGrid([NotNull] GridRow parent)
        {
            GridPanel panel = parent.Rows.OfType<GridPanel>().FirstOrDefault();
            if (panel == null)
            {
                panel = new GridPanel();
                parent.Rows.Add(panel);

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
                    Width = 200,
                    DataType = typeof(string),
                    AllowEdit = false
                });

                panel.Columns.Add(new GridColumn("Description")
                {
                    HeaderText = "Description",
                    Width = 200,
                    DataType = typeof(string),
                    AllowEdit = false
                });
                panel.Columns["Description"].CellStyles.Default.AllowMultiLine = Tbool.True;

                panel.Columns.Add(new GridColumn("Source")
                {
                    HeaderText = "Source",
                    DataType = typeof(string),
                    AllowEdit = false,
                    Width = 150
                });

                panel.Columns.Add(new GridColumn("Target")
                {
                    HeaderText = "Target",
                    DataType = typeof(string),
                    AllowEdit = false,
                    Width = 150
                });

                panel.Columns.Add(new GridColumn("FlowType")
                {
                    HeaderText = "Flow Type",
                    DataType = typeof(string),
                    AllowEdit = false,
                    Width = 125
                });

                panel.Columns.Add(new GridColumn("Master")
                {
                    HeaderText = "Is Master",
                    DataType = typeof(bool),
                    AllowEdit = true,
                    Width = 75,
                    EditorType = typeof(GridSwitchButtonEditControl),
                });
            }

            return panel;
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

        private void AddGridRow([NotNull] IEntity source, [NotNull] IEntity target, [NotNull] IEnumerable<IDataFlow> dataFlows)
        {
            GridRow row = new GridRow(
                source.Name,
                target.Name,
                null,
                false);
            row.Cells[0].CellStyles.Default.Image = source.GetImage(ImageSize.Small);
            row.Cells[1].CellStyles.Default.Image = target.GetImage(ImageSize.Small);
            row.Cells[3].PropertyChanged += OnIgnoreChanged;
            _grid.PrimaryGrid.Rows.Add(row);

            var panel = InitializeSubGrid(row);

            foreach (var flow in dataFlows)
            {
                AddGridRow(panel, flow);
            }
        }

        private void AddGridRow([NotNull] GridPanel panel, [NotNull] IDataFlow dataFlow)
        {
            GridRow row = new GridRow(
                dataFlow.Name,
                dataFlow.Description,
                dataFlow.Source.Name,
                dataFlow.Target.Name,
                dataFlow.FlowType.GetEnumLabel(),
                false)
            {
                Tag = dataFlow
            };
            row.Cells[2].CellStyles.Default.Image = dataFlow.Source.GetImage(ImageSize.Small);
            row.Cells[3].CellStyles.Default.Image = dataFlow.Target.GetImage(ImageSize.Small);
            row.Cells[5].PropertyChanged += OnMasterChanged;
            panel.Rows.Add(row);
        }

        private void OnIgnoreChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_loading && sender is GridCell cell && cell.GridRow is GridRow row)
            {
                _loading = true;

                try
                {
                    GridPanel panel = row.Rows.OfType<GridPanel>().FirstOrDefault();

                    panel.Visible = !((bool) cell.Value);
                }
                finally
                {
                    _loading = false;
                }
            }
        }
 
        private void OnMasterChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!_loading && sender is GridCell cell && cell.GridRow is GridRow row
                && row.Tag is IDataFlow flow)
            {
                _loading = true;

                try
                {
                    if ((bool) cell.Value)
                    {
                        var parent = row.GridPanel.Parent as GridRow;
                        if (parent != null)
                            parent.Cells["Name"].Value = flow.Name;

                        var siblings = row.GridPanel.Rows.OfType<GridRow>().Where(x => x != row);

                        foreach (var sibling in siblings)
                        {
                            sibling.Cells["Master"].Value = false;
                        }
                    }
                }
                finally
                {
                    _loading = false;
                }
           }
        }

        private FlowMergeInfo GetFlowMergeInfo([NotNull] GridRow row)
        {
            FlowMergeInfo result = null;

            if (!((bool) row.Cells["Ignore"].Value))
            {
                var name = (string) row.Cells["Name"].Value;
                var rows = row.Rows.OfType<GridPanel>().FirstOrDefault()?.Rows.OfType<GridRow>().ToArray();
                if (rows?.Any() ?? false)
                {
                    IDataFlow master = null;
                    List<IDataFlow> slaves = null;

                    foreach (var curr in rows)
                    {
                        if (curr.Tag is IDataFlow dataFlow)
                        {
                            if ((bool) curr.Cells["Master"].Value)
                            {
                                master = dataFlow;
                                if (string.IsNullOrWhiteSpace(name))
                                    name = dataFlow.Name;
                            }
                            else
                            {
                                if (slaves == null)
                                    slaves = new List<IDataFlow>();
                                slaves.Add(dataFlow);
                            }
                        }
                    }

                    if (master != null && slaves != null)
                    {
                        result = new FlowMergeInfo(name, master, slaves);
                    }
                }
            }

            return result;
        }
    }

    public class FlowMergeInfo
    {
        internal FlowMergeInfo([Required] string name, [NotNull] IDataFlow master, [NotNull] IEnumerable<IDataFlow> slaves)
        {
            Name = name;
            Master = master;
            Slaves = slaves;
        }

        public string Name { get; }

        public IDataFlow Master { get; }

        public IEnumerable<IDataFlow> Slaves { get; }
    }
}
