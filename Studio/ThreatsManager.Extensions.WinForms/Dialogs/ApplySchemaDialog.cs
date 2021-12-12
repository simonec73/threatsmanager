using System;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    public partial class ApplySchemaDialog : Form
    {
        public ApplySchemaDialog()
        {
            InitializeComponent();
            _schemas.Items.Add(string.Empty);
        }

        public void Initialize([NotNull] IPropertiesContainer container)
        {
            IThreatModel model = null;
            if (container is IThreatModelChild child)
            {
                model = child.Model;
            }

            if (model != null)
            {
                if (container is IIdentity identity)
                {
                    string text = identity.Name;
                    int height = (int)(21 * Dpi.Factor.Height);
                    var image = identity.GetImage(ImageSize.Small);
                    if (image != null)
                    {
                        height = image.Height + 10;
                        text = "      " + text;
                        _container.Image = image;
                    }
                    _container.Text = text;
                }
                else
                {
                    if (container is IThreatTypeMitigation ttm)
                    {
                        _container.Text = ttm.Mitigation.Name;
                    }
                    else if (container is IThreatEventMitigation tte)
                    {
                        _container.Text = tte.Mitigation.Name;
                    }
                    else if (container is IWeaknessMitigation wm)
                    {
                        _container.Text = wm.Mitigation.Name;
                    }
                    else if (container is IVulnerabilityMitigation vm)
                    {
                        _container.Text = vm.Mitigation.Name;
                    }
                }

                var schemas = model.Schemas?
                    .Where(x => x.Visible && x.AppliesTo.HasFlag(container.PropertiesScope) &&
                        !(container.Properties?.Any(y => (y.PropertyType?.SchemaId ?? Guid.Empty) == x.Id) ?? false))
                    .ToArray();

                if (schemas?.Any() ?? false)
                {
                    _schemas.Items.AddRange(schemas);
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
