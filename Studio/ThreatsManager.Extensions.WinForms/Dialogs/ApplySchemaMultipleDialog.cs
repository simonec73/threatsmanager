using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class ApplySchemaMultipleDialog : Form
    {
        public ApplySchemaMultipleDialog()
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
                            var s = model.Schemas?
                                .Where(x => x.Visible && 
                                    x.AppliesTo.HasFlag(container.PropertiesScope) &&
                                    !schemas.Any(y => string.CompareOrdinal(x.Name, y.Name) == 0 && string.Compare(x.Namespace, y.Namespace) == 0))
                                .ToArray();

                            if (s?.Any() ?? false)
                                schemas.AddRange(s);
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
