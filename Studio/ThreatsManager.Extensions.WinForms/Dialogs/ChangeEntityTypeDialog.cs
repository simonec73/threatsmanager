using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class ChangeEntityTypeDialog : Form
    {
        private readonly IEntity _entity;
        private readonly EntityType _option1EntityType;
        private readonly EntityType _option2EntityType;
        private readonly IEnumerable<IEntityTemplate> _option1Templates;
        private readonly IEnumerable<IEntityTemplate> _option2Templates;
        private byte _currentOption;

        private ChangeEntityTypeDialog()
        {
            InitializeComponent();

            _templates.ItemHeight = (int)(16 * Dpi.Factor.Height);
        }

        public ChangeEntityTypeDialog(IEntity entity) : this()
        {
            _entity = entity;
            _affectedItem.Text = entity.Name;
            _affectedItem.Image = entity.GetImage(ImageSize.Small);
            var entityType = entity.GetEntityType();
            _itemType.Text = entityType.GetEnumLabel();

            var template = entity.Template;
            if (template != null)
            {
                _currentTemplate.Text = template.Name;
                _currentTemplate.Image = template.GetImage(ImageSize.Small);
            }
            else
            {
                _currentTemplate.Text = string.Empty;
                _currentTemplate.Image = null;
            }

            switch (entityType)
            {
                case EntityType.ExternalInteractor:
                    _option1EntityType = EntityType.Process;
                    _option2EntityType = EntityType.DataStore;
                    break;
                case EntityType.Process:
                    _option1EntityType = EntityType.ExternalInteractor;
                    _option2EntityType = EntityType.DataStore;
                    break;
                case EntityType.DataStore:
                    _option1EntityType = EntityType.ExternalInteractor;
                    _option2EntityType = EntityType.Process;
                    break;
            }

            _option1.Text = _option1EntityType.GetEnumLabel();
            _option1Templates = GetTemplates(_option1EntityType);
            _option2.Text = _option2EntityType.GetEnumLabel();
            _option2Templates = GetTemplates(_option2EntityType);
        }

        private void _templates_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableOk();
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            var model = _entity.Model;
            IEntityTemplate template = null;
            if (_templates.SelectedItem is ComboBoxItem item)
            {
                if (item.Value is IEntityTemplate entityTemplate)
                {
                    template = entityTemplate;
                }
            }

            IEntity newEntity = null;
            if (_currentOption == 1)
            {
                switch (_option1EntityType)
                {
                    case EntityType.ExternalInteractor:
                        newEntity = model.AddEntity<IExternalInteractor>(_entity.Name, template);
                        break;
                    case EntityType.Process:
                        newEntity = model.AddEntity<IProcess>(_entity.Name, template);
                        break;
                    case EntityType.DataStore:
                        newEntity = model.AddEntity<IDataStore>(_entity.Name, template);
                        break;
                }
            }
            else if (_currentOption == 2)
            {
                switch (_option2EntityType)
                {
                    case EntityType.ExternalInteractor:
                        newEntity = model?.AddEntity<IExternalInteractor>(_entity.Name, template);
                        break;
                    case EntityType.Process:
                        newEntity = model?.AddEntity<IProcess>(_entity.Name, template);
                        break;
                    case EntityType.DataStore:
                        newEntity = model?.AddEntity<IDataStore>(_entity.Name, template);
                        break;
                }
            }
        }

        private void _option1_CheckedChanged(object sender, EventArgs e)
        {
            ChangeOption((byte)(_option1.Checked ? 1 : 2));

            EnableOk();
        }

        private void ChangeOption(byte option)
        {
            if (option != _currentOption)
            {
                _currentOption = option;
                _templates.Items.Clear();
                _templates.Items.Add(new ComboBoxItem("<No Template>", null));
                
                if (option == 1)
                {
                    if (_option1Templates?.Any() ?? false)
                    {
                        _templates.Items.AddRange(_option1Templates.Select(x => new ComboBoxItem(x, x.GetImage(ImageSize.Small))).ToArray());
                    }
                }
                else if (option == 2)
                {
                    if (_option2Templates?.Any() ?? false)
                    {
                        _templates.Items.AddRange(_option2Templates.Select(x => new ComboBoxItem(x, x.GetImage(ImageSize.Small))).ToArray());
                    }
                }
            }
        }

        private IEnumerable<IEntityTemplate> GetTemplates(EntityType entityType)
        {
            return _entity?.Model?.GetEntityTemplates(entityType);
        }

        private void EnableOk()
        {
            _ok.Enabled = _entity != null &&
                _entity.Model != null &&
                (_option1.Checked ||  _option2.Checked) &&
                _templates.SelectedIndex >= 0;
        }
    }
}
