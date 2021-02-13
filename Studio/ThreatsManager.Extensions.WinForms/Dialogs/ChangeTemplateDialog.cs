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
    public partial class ChangeTemplateDialog : Form
    {
        private readonly IIdentity _item;

        public ChangeTemplateDialog(IIdentity item)
        {
            InitializeComponent();

            _templates.ItemHeight = (int) (16 * Dpi.Factor.Height);

            _item = item;

            IEnumerable<IItemTemplate> templates = null;

            if (item is IEntity entity)
            {
                _affectedItem.Text = entity.Name;
                _affectedItem.Image = entity.GetImage(ImageSize.Small);
                var entityType = entity.GetEntityType();
                _itemType.Text = entityType.GetEnumLabel();
                var template = entity.Template;
                if (template != null)
                {
                    _currentTemplate.Text = template.Name;
                    _currentTemplate.Image = template.GetImage(ImageSize.Small);

                    _templates.Items.Add(new ComboBoxItem("<No Template>", null));
                }
                else
                {
                    _currentTemplate.Text = string.Empty;
                    _currentTemplate.Image = null;
                }

                templates = entity.Model?.EntityTemplates?
                    .Where(x => x.EntityType == entityType && x.Id != (template?.Id ?? Guid.Empty))
                    .OrderBy(x => x.Name);
            } else if (item is IDataFlow flow)
            {
                _affectedItem.Text = flow.Name;
                _affectedItem.Image = flow.GetImage(ImageSize.Small);
                _itemType.Text = "Flow";
                var template = flow.Template;
                if (template != null)
                {
                    _currentTemplate.Text = template.Name;
                    _currentTemplate.Image = template.GetImage(ImageSize.Small);
                    _templates.Items.Add(new ComboBoxItem("<No Template>", null));
                }
                else
                {
                    _currentTemplate.Text = string.Empty;
                    _currentTemplate.Image = null;
                }

                templates = flow.Model?.FlowTemplates?
                    .Where(x => x.Id != (template?.Id ?? Guid.Empty))
                    .OrderBy(x => x.Name);
            } else if (item is ITrustBoundary boundary)
            {
                _affectedItem.Text = boundary.Name;
                _affectedItem.Image = boundary.GetImage(ImageSize.Small);
                _itemType.Text = "Trust Boundary";
                var template = boundary.Template;
                if (template != null)
                {
                    _currentTemplate.Text = template.Name;
                    _currentTemplate.Image = template.GetImage(ImageSize.Small);
                    _templates.Items.Add(new ComboBoxItem("<No Template>", null));
                }
                else
                {
                    _currentTemplate.Text = string.Empty;
                    _currentTemplate.Image = null;
                }

                templates = boundary.Model?.TrustBoundaryTemplates?
                    .Where(x => x.Id != (template?.Id ?? Guid.Empty))
                    .OrderBy(x => x.Name);
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
            if (_templates.SelectedItem is ComboBoxItem item)
            {
                if (item.Value is IEntityTemplate entityTemplate && _item is IEntity entity)
                {
                    entityTemplate.ApplyTo(entity);
                }
                else if (item.Value is IFlowTemplate flowTemplate && _item is IDataFlow flow)
                {
                    flowTemplate.ApplyTo(flow);
                }
                else if (item.Value is ITrustBoundaryTemplate trustBoundaryTemplate && _item is ITrustBoundary trustBoundary)
                {
                    trustBoundaryTemplate.ApplyTo(trustBoundary);
                }
                else
                {
                    if (_item is IEntity entityItem)
                    {
                        entityItem.ResetTemplate();
                    }
                    else if (_item is IDataFlow flowItem)
                    {
                        flowItem.ResetTemplate();
                    }
                    else if (_item is ITrustBoundary trustBoundaryItem)
                    {
                        trustBoundaryItem.ResetTemplate();
                    }
                }
            }
        }
    }
}
