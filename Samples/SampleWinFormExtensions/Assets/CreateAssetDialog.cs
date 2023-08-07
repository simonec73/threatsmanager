using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.SampleWinFormExtensions.Assets
{
    public partial class CreateAssetDialog : Form
    {
        private readonly IThreatModel _model;

        public CreateAssetDialog([NotNull] IThreatModel model)
        {
            InitializeComponent();

            _model = model;
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_name.Text))
            {
                using (var scope = UndoRedoManager.OpenScope("Add Asset"))
                {
                    var schemaManager = new AssetPropertySchemaManager(_model);
                    var propertyType = schemaManager.AssetPropertyType;
                    if (propertyType != null)
                    {
                        var property = _model.GetProperty(propertyType) ?? _model.AddProperty(propertyType, null);
                        if (property is IPropertyJsonSerializableObject jsonSerializableObject)
                        {
                            var assets = jsonSerializableObject.Value as Assets;
                            if (assets == null)
                            {
                                assets = new Assets();
                                jsonSerializableObject.Value = assets;
                            }

                            assets.AddAsset(new Asset(_name.Text)
                            {
                                Description = _description.Text,
                                Value = _value.Value
                            }, _model
                            );

                            scope?.Complete();
                        }
                    }
                }

                this.Close();
            }
        }

        private void _cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
