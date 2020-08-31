using System;
using System.Linq;
using System.Windows.Forms;
using SampleWinFormExtensions.Schemas;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace SampleWinFormExtensions.Panels.Definitions
{
    public partial class DefinitionsPanel : UserControl, IShowThreatModelPanel<Form>
    {
        private readonly Guid _id = Guid.NewGuid();
        private DefinitionContainer _container;
        private bool _loading = true;

        public DefinitionsPanel()
        {
            InitializeComponent();
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public void SetThreatModel(IThreatModel model)
        {
            try
            {
                if (model != null)
                {
                    var schema = new DefinitionsPropertySchemaManager(model);
                    var propertyType = schema.DefinitionsPropertyType;
                    if (propertyType != null)
                    {
                        var property = model.GetProperty(propertyType) ?? model.AddProperty(propertyType, null);
                        if (property is IPropertyJsonSerializableObject jsonProperty)
                        {
                            if (jsonProperty.Value is DefinitionContainer container)
                            {
                                _container = container;

                                var definitions = container.Definitions?.ToArray();
                                if (definitions?.Any() ?? false)
                                {
                                    foreach (var definition in definitions)
                                        _data.Rows.Add(definition.Key, definition.Value);
                                }
                            }
                            else
                            {
                                _container = new DefinitionContainer();
                                jsonProperty.Value = _container;
                            }
                        }
                    }
                }
            }
            finally
            {
                _loading = false;
            }
        }
        #endregion

        public IActionDefinition ActionDefinition => new ActionDefinition(Guid.NewGuid(), "Definitions", "Definitions",
            Properties.Resources.book_open_big, Properties.Resources.book_open);

        private void _data_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_loading)
            {
                var row = _data.Rows[e.RowIndex];
                var name = row.Cells[0].Value as string;
                var value = row.Cells[1].Value as string;
                _container.SetDefinition(name, value);
            }
        }

        private void _data_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!_loading)
            {
                var name = e.Row.Cells[0].Value as string;
                _container.RemoveDefinition(name);
            }
        }
    }
}
