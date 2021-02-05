using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.DevOps.Schemas;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.DevOps.Dialogs
{
    public partial class DevOpsWorkItemAssociationDialog : Form
    {
        private readonly IMitigation _mitigation;

        public DevOpsWorkItemAssociationDialog()
        {
            InitializeComponent();
        }

        public DevOpsWorkItemAssociationDialog([NotNull] IMitigation mitigation) : this()
        {
            _mitigation = mitigation;
        }

        public IDevOpsItemInfo SelectedItemInfo { get; private set; }

        private async void _auto_Click(object sender, EventArgs e)
        {
            if (_mitigation?.Model != null)
            {
                var connector = DevOpsManager.GetConnector(_mitigation.Model);
                if (connector != null)
                {
                    var devOpsItemInfos = (await connector.GetItemsAsync(_mitigation.Name))?.ToArray();
                    if (devOpsItemInfos?.Any() ?? false)
                    {
                        _objects.Items.AddRange(devOpsItemInfos);
                        _objects.SelectedIndex = 0;
                    }
                }
            }
        }

        private void _objects_SelectedIndexChanged(object sender, EventArgs e)
        {
            _objectType.Text = (_objects.SelectedItem as IDevOpsItemInfo)?.WorkItemType;
            _ok.Enabled = (_objects.SelectedItem as IDevOpsItemInfo) != null;
        }

        private async void _ok_Click(object sender, EventArgs e)
        {
            if (_objects.SelectedItem is IDevOpsItemInfo devOpsItemInfo)
            {
                var connector = DevOpsManager.GetConnector(_mitigation.Model);
                if (connector != null)
                {
                    var workItemInfo = await connector.GetWorkItemInfoAsync(devOpsItemInfo.Id);
                    if (workItemInfo != null)
                    {
                        var schemaManager = new DevOpsPropertySchemaManager(_mitigation.Model);
                        schemaManager.SetDevOpsStatus(_mitigation, connector, devOpsItemInfo.Id, 
                            devOpsItemInfo.Url, devOpsItemInfo.AssignedTo, workItemInfo.Status);

                        SelectedItemInfo = devOpsItemInfo;
                        DialogResult = DialogResult.OK;
                    }
                }
            }
        }

        private async void _objects_TextUpdate(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && _mitigation.Model != null)
            {
                var connector = DevOpsManager.GetConnector(_mitigation.Model);

                if (connector != null)
                {
                    string filter = comboBox.Text;
                    comboBox.Items.Clear();

                    var itemsAsync = await connector.GetItemsAsync(filter);
                    var items = itemsAsync?.ToArray();
                    if (items?.Any() ?? false)
                        comboBox.Items.AddRange(items);

                    comboBox.DroppedDown = true;
                    comboBox.IntegralHeight = true;
                    comboBox.SelectedIndex = -1;
                    comboBox.Text = filter;
                    comboBox.SelectionStart = filter.Length;
                    comboBox.SelectionLength = 0;
                }
            }
        }

        private void _objects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (string.IsNullOrEmpty(_objects.Text))
                {
                    _objects.DroppedDown = false;
                }
                else
                {
                    e.SuppressKeyPress = true;
                    _objects.Text = null;
                    _objects_TextUpdate(_objects, null);
                }
            }
        }
    }
}
