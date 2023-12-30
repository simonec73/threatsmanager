using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Entities;
using ThreatsManager.Utilities;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class DuplicateConvertDialog : Form
    {
        private IEntity _entity;

        public DuplicateConvertDialog()
        {
            InitializeComponent();

            _newEntityType.Items.AddRange(EnumExtensions.GetEnumLabels<EntityType>().ToArray());
        }

        public void Initialize([NotNull] IEntity entity)
        {
            _entity = entity;

            _entityName.Text = _entity.Name;
            _newEntityName.Text = $"{_entity.Name} (copy)";
            _entityName.Image = _entity.GetImage(ImageSize.Small);
            _entityType.Text = _entity.GetEntityType().GetEnumLabel();
            _entityType.Image = _entity.GetEntityType().GetEntityImage(ImageSize.Small);
            _newEntityType.SelectedItem = _entity.GetEntityType().GetEnumLabel();

            var entityTemplate = _entity.Template;
            if (entityTemplate != null)
            {
                _entityTemplate.Text = entityTemplate.Name;
                _entityTemplate.Image = entityTemplate.GetImage(ImageSize.Small);
            }
            else
            {
                _entityTemplate.Text = null;
                _entityTemplate.Image = null;
            }
        }

        public string NewEntityName => _newEntityName.Text;

        public bool Convert => _convert.Checked;

        public EntityType EntityType
        {
            get
            {
                EntityType result = _entity.GetEntityType();
                if (EnumExtensions.TryGetEnumValue<EntityType>(_newEntityType.SelectedItem.ToString(), out var entityType))
                {
                    result = entityType;
                }

                return result;
            }
        }

    public IEntityTemplate EntityTemplate => _newEntityTemplate.SelectedItem as IEntityTemplate;

        private void _duplicate_CheckedChanged(object sender, EventArgs e)
        {
            _newEntityType.Enabled = _convert.Checked;
            _newEntityTemplate.Enabled = _convert.Checked;
        }

        private void _newEntityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _newEntityTemplate.Items.Clear();
            _newEntityTemplate.Items.Add("<No Entity Template>");
            _newEntityTemplate.SelectedIndex = 0;
            if (EnumExtensions.TryGetEnumValue<EntityType>(_newEntityType.SelectedItem.ToString(), out var entityType))
            {
                var templates = _entity.Model?.GetEntityTemplates(entityType)?
                    .OrderBy(x => x.Name)
                    .ToArray();
                if (templates?.Any() ?? false)
                {
                    _newEntityTemplate.Items.AddRange(templates);
                }
            }
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
