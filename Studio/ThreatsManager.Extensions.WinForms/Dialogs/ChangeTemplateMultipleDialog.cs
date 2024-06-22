using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class ChangeTemplateMultipleDialog : Form
    {
        private readonly IEnumerable<IIdentity> _items;

        private ChangeTemplateMultipleDialog()
        {
            InitializeComponent();

            _templates.ItemHeight = 16;
        }

        public ChangeTemplateMultipleDialog(IEnumerable<IExternalInteractor> items) : this()
        {
            _items = items?.Where(x => x != null).ToArray();
            ProcessItems();
        }

        public ChangeTemplateMultipleDialog(IEnumerable<IProcess> items) : this()
        {
            _items = items?.Where(x => x != null).ToArray();
            ProcessItems();
        }

        public ChangeTemplateMultipleDialog(IEnumerable<IDataStore> items) : this()
        {
            _items = items?.Where(x => x != null).ToArray();
            ProcessItems();
        }

        public ChangeTemplateMultipleDialog(IEnumerable<IDataFlow> items) : this()
        {
            _items = items?.Where(x => x != null).ToArray();
            ProcessItems();
        }

        public ChangeTemplateMultipleDialog(IEnumerable<ITrustBoundary> items) : this()
        {
            _items = items?.Where(x => x != null).ToArray();
            ProcessItems();
        }

        private void ProcessItems()
        {
            IEnumerable<IItemTemplate> templates = null;

            _templates.Items.Add(new ComboBoxItem("<No Template>", null));

            if (_items?.Any() ?? false)
            {
                var first = _items.First();
                IThreatModel model = null;
                if (first is IThreatModelChild child)
                {
                    model = child.Model;
                }

                if (model != null)
                {
                    _affectedItemsCount.Text = _items.Count().ToString();

                    if (first is IEntity entity)
                    {
                        var entityType = entity.GetEntityType();
                        _itemType.Text = entityType.GetEnumLabel();
                        templates = model?.EntityTemplates?
                            .Where(x => x.EntityType == entityType)
                            .OrderBy(x => x.Name);
                    }
                    else if (first is IDataFlow flow)
                    {
                        _itemType.Text = "Flow";
                        templates = model?.FlowTemplates?
                            .OrderBy(x => x.Name);
                    }
                    else if (first is ITrustBoundary boundary)
                    {
                        _itemType.Text = "Trust Boundary";
                        templates = model?.TrustBoundaryTemplates?
                            .OrderBy(x => x.Name);
                    }
                }

            }

            if (templates?.Any() ?? false)
            {
                _templates.Items.AddRange(templates.Select(x => new ComboBoxItem(x, x.GetImage(ImageSize.Small))).ToArray());
            }
        }

        private void _templates_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = _templates.SelectedIndex >= 0;
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (_templates.SelectedItem is ComboBoxItem template)
            {
                if (template.Value is IEntityTemplate entityTemplate)
                {
                    var items = _items?.OfType<IEntity>().ToArray();
                    if (items?.Any() ?? false) 
                    {
                        foreach (var item in items)
                        {
                            entityTemplate.ApplyTo(item);
                        }
                    }
                }
                else if (template.Value is IFlowTemplate flowTemplate)
                {
                    var items = _items?.OfType<IDataFlow>().ToArray();
                    if (items?.Any() ?? false)
                    {
                        foreach (var item in items)
                        {
                            flowTemplate.ApplyTo(item);
                        }
                    }
                }
                else if (template.Value is ITrustBoundaryTemplate trustBoundaryTemplate)
                {
                    var items = _items?.OfType<ITrustBoundary>().ToArray();
                    if (items?.Any() ?? false)
                    {
                        foreach (var item in items)
                        {
                            trustBoundaryTemplate.ApplyTo(item);
                        }
                    }
                }
                else
                {
                    var entities = _items?.OfType<IEntity>().ToArray();
                    if (entities?.Any() ?? false)
                    {
                        foreach (var item in entities)
                        {
                            item.ResetTemplate();
                        }
                    }
                    var flows = _items?.OfType<IDataFlow>().ToArray();
                    if (flows?.Any() ?? false)
                    {
                        foreach (var item in flows)
                        {
                            item.ResetTemplate();
                        }
                    }
                    var boundaries = _items?.OfType<ITrustBoundary>().ToArray();
                    if (boundaries?.Any() ?? false)
                    {
                        foreach (var item in boundaries)
                        {
                            item.ResetTemplate();
                        }
                    }
                }
            }
        }
    }
}
