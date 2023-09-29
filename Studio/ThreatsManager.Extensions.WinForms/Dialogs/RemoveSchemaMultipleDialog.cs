using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class RemoveSchemaMultipleDialog : Form
    {
        public RemoveSchemaMultipleDialog()
        {
            InitializeComponent();
            _schemas.Items.Add(string.Empty);
        }

        public void Initialize(IEnumerable<IPropertiesContainer> containers)
        {
            if (containers?.Any() ?? false)
            {
                _containersCount.Text = containers.Count().ToString();

                if (containers.FirstOrDefault() is IThreatModelChild child)
                {
                    var model = child.Model;

                    if (model != null)
                    {
                        var schemas = new List<IPropertySchema>();

                        foreach (var container in containers)
                        {
                            var properties = container.Properties?.ToArray();
                            foreach (var property in properties)
                            {
                                var schema = model.GetSchema(property.PropertyType?.SchemaId ?? Guid.Empty);
                                if (schema != null && 
                                    !schemas.Any(x => string.CompareOrdinal(x.Name, schema.Name) == 0 && string.Compare(x.Namespace, schema.Namespace) == 0))
                                {
                                    schemas.Add(schema);
                                }
                            }
                        }

                        if (schemas?.Any() ?? false)
                        {
                            _schemas.Items.AddRange(schemas.OrderBy(x => x.Name).ToArray());
                        }
                    }
                }
            }
        }

        public IPropertySchema SelectedSchema { get; private set; }

        private void _schemas_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedSchema = _schemas.SelectedItem as IPropertySchema;
            _description.Text = SelectedSchema?.Description;
        }
    }
}
