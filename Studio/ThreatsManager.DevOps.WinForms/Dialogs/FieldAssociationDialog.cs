using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.DevOps.Dialogs
{
    public partial class FieldAssociationDialog : Form
    {
        private IThreatModel _model;

        public FieldAssociationDialog()
        {
            InitializeComponent();
        }

        public void Initialize([NotNull] IThreatModel model, [NotNull] IEnumerable<IDevOpsField> fields)
        {
            _model = model;

            var fieldList = fields.ToArray();
            if (fieldList.Any())
            {
                _fields.Items.AddRange(fieldList);

                var schemas = _model.Schemas?
                    .Where(x => x.AppliesTo.HasFlag(Interfaces.Scope.Mitigation) && x.Visible)
                    .ToArray();
                if (schemas?.Any() ?? false)
                {
                    foreach (var schema in schemas)
                    {
                        var properties = schema.PropertyTypes?.Where(x => x.Visible).ToArray();
                        if (properties?.Any() ?? false)
                        {
                            _properties.Items.AddRange(properties);
                        }
                    }
                }
                else
                {
                    _property.Enabled = false;
                    _properties.Enabled = false;
                }
            }
        }

        public void SetField([NotNull] IDevOpsField field, [NotNull] IdentityField identityField)
        {
            if (_fields.Items.Contains(field))
                _fields.SelectedItem = field;
            switch (identityField.FieldType)
            {
                case IdentityFieldType.Id:
                    _id.Checked = true;
                    break;
                case IdentityFieldType.Name:
                    _name.Checked = true;
                    break;
                case IdentityFieldType.Description:
                    _description.Checked = true;
                    break;
                case IdentityFieldType.Priority:
                    _priority.Checked = true;
                    break;
                case IdentityFieldType.Property:
                    _property.Checked = true;
                    if (_properties.Items.OfType<IPropertyType>().Any(x => x == identityField.PropertyType))
                        _properties.SelectedItem = identityField.PropertyType;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IDevOpsField Field => _fields.SelectedItem as IDevOpsField;

        public IdentityField IdentityField
        {
            get
            {
                IdentityField result = null;

                if (_id.Checked)
                    result = new IdentityField(IdentityFieldType.Id);
                else if (_name.Checked)
                    result = new IdentityField(IdentityFieldType.Name);
                else if (_description.Checked)
                    result = new IdentityField(IdentityFieldType.Description);
                else if (_priority.Checked)
                    result = new IdentityField(IdentityFieldType.Priority);
                else if (_property.Checked && _properties.SelectedItem is IPropertyType propertyType)
                    result = new IdentityField(propertyType);

                return result;
            }
        }

        private void _fields_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _id_CheckedChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _name_CheckedChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _description_CheckedChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _priority_CheckedChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _property_CheckedChanged(object sender, EventArgs e)
        {
            _properties.Enabled = _property.Checked;
            _ok.Enabled = IsValid();
        }

        private void _properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();

            string text;
            if (_properties.SelectedItem is IPropertyType propertyType)
            {
                var schema = _model.GetSchema(propertyType.SchemaId);
                text = schema.Name;
            }
            else
            {
                text = null;
            }
            _schema.Text = text;
        }

        private bool IsValid()
        {
            return _fields.SelectedItem is IDevOpsField && (
                _id.Checked || _name.Checked || _description.Checked || _priority.Checked ||
                (_property.Checked && _properties.SelectedItem is IPropertyType));
        }
    }
}
