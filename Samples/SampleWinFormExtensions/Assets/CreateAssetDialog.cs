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
        private readonly Asset _asset;

        public CreateAssetDialog([NotNull] IThreatModel model, Asset asset = null)
        {
            InitializeComponent();

            _model = model;

            if (asset != null)
            { 
                _asset = asset;
                Text = "Edit Asset";
                _name.Text = asset.Name;
                _description.Text = asset.Description;
                _value.Value = asset.Value;
            }
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_name.Text))
            {
                if (_asset == null)
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
                }
                else
                {
                    using (var scope = UndoRedoManager.OpenScope("Modify Asset"))
                    {
                        _asset.Name = _name.Text;
                        _asset.Description = _description.Text;
                        _asset.Value = _value.Value;

                        scope?.Complete();
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
