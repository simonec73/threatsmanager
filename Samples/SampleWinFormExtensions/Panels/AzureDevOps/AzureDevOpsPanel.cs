using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace SampleWinFormExtensions.Panels.AzureDevOps
{
    public partial class AzureDevOpsPanel : UserControl, IShowThreatModelPanel<Form>
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly Random _rand = new Random();


        public AzureDevOpsPanel()
        {
            InitializeComponent();
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetThreatModel(IThreatModel model)
        {
            if (model != null)
            {
                List<IThreatEventMitigation> mitigations = new List<IThreatEventMitigation>();
                AddMitigations(mitigations, model.Entities);
                AddMitigations(mitigations, model.DataFlows);

                var sorted = mitigations.OrderBy(x => x.Mitigation.Name);
                foreach (var item in sorted)
                {
                    _list.Items.Add(new ListViewItem(new[]
                        {item.Mitigation.Name, item.ThreatEvent.Name, item.ThreatEvent.Parent.Name, RandomState()})
                    {
                        Tag = item
                    });
                }
            }
        }

        private void AddMitigations(List<IThreatEventMitigation> mitigations,
            IEnumerable<IThreatEventsContainer> containers)
        {
            var cont = containers?.ToArray();
            if (cont?.Any() ?? false)
            {
                foreach (var container in cont)
                {
                    var tes = container.ThreatEvents?.ToArray();
                    if (tes?.Any() ?? false)
                    {
                        foreach (var te in tes)
                        {
                            var ms = te.Mitigations?.ToArray();
                            if (ms?.Any() ?? false)
                                mitigations.AddRange(ms);
                        }
                    }
                }
            }
        }

        private string RandomState()
        {
            var list = new[]
            {
                "Accepted", "Active", "Approved", "Closed", "Committed", "Completed", "Done", "In Planning",
                "In Progress", "Inactive", "New", "Open", "Ready", "Removed", "Requested", "ToDo"
            };
            return list[_rand.Next(list.Length - 1)];
        }
        #endregion

        public IActionDefinition ActionDefinition => new ActionDefinition(Guid.NewGuid(), "Azure DevOps", "Azure DevOps", 
            Properties.Resources.devops_big, Properties.Resources.devops);

        private void _list_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            _item.Item = e.Item.Tag;
        }
    }
}
