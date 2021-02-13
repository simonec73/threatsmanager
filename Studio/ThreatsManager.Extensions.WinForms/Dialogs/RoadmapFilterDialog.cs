using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Extensions.Panels.Roadmap;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class RoadmapFilterDialog : Form
    {
        private readonly RoadmapFilter _roadmapFilter;
        private bool _loading;

        public RoadmapFilterDialog()
        {
            InitializeComponent();
        }

        public RoadmapFilterDialog([NotNull] IThreatModel model) : this()
        {
            _roadmapFilter = new RoadmapFilter(model);
            var filters = _roadmapFilter.Filters?.ToArray();
            if (filters?.Any() ?? false)
            {
                _filters.Items.AddRange(filters);
            }
        }

        public RoadmapFilter Filter
        {
            get => _roadmapFilter;

            set
            {
                try
                {
                    _loading = true;

                    var enabled = value?.Enabled?.ToArray();
                    if (enabled?.Any() ?? false)
                    {
                        foreach (var item in enabled)
                        {
                            _roadmapFilter.Enable(item);
                            var index = _filters.Items.IndexOf(item);
                            if (index >= 0)
                                _filters.SetItemChecked(index, true);
                        }
                    }

                    _roadmapFilter.Or = value?.Or ?? false;
                    _operator.Value = _roadmapFilter.Or;
                }
                finally
                {
                    _loading = false;
                }
            }
        }

        private void _filters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_loading && _filters.Items.Count > e.Index)
            {
                var name = _filters.Items[e.Index] as string;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (e.NewValue == CheckState.Checked)
                    {
                        _roadmapFilter.Enable(name);
                    }
                    else
                    {
                        _roadmapFilter.Disable(name);
                    }
                }
            }
        }

        private void _operator_ValueChanged(object sender, System.EventArgs e)
        {
            if (!_loading)
                _roadmapFilter.Or = _operator.Value;
        }
    }
}
