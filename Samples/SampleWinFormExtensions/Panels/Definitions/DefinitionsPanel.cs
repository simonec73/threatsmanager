using System;
using System.Linq;
using System.Windows.Forms;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.Extensions.Panels;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.SampleWinFormExtensions.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.SampleWinFormExtensions.Panels.Definitions
{
    public partial class DefinitionsPanel : UserControl, IShowThreatModelPanel<Form>, IDesktopAlertAwareExtension
    {
        private readonly Guid _id = Guid.NewGuid();
        private DefinitionContainer _container;
        private bool _loading = true;

        public event Action<string> ShowMessage;
        public event Action<string> ShowWarning;

        public DefinitionsPanel()
        {
            InitializeComponent();
        }

        #region Implementation of interface IShowThreatModelPanel.
        public Guid Id => _id;

        public Form PanelContainer { get; set; }

        public IIdentity ReferenceObject => null;

        public void SetThreatModel(IThreatModel model)
        {
            try
            {
                if (model != null)
                {
                    using (var scope = UndoRedoManager.OpenScope("Prepare the Threat Model to support Definitions"))
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

                                scope?.Complete();
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
                try
                {
                    using (var scope = UndoRedoManager.OpenScope("Set Definition"))
                    {
                        var row = _data.Rows[e.RowIndex];
                        var name = row.Cells[0].Value as string;
                        var value = row.Cells[1].Value as string;

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                        {
                            _container.SetDefinition(name, value);
                        }

                        scope?.Complete();
                    }
                }
                catch (Exception)
                {
                    ShowWarning?.Invoke("Definition cannot be added");
                }
            }
        }

        private void _data_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (!_loading)
            {
                try
                {
                    using (var scope = UndoRedoManager.OpenScope("Remove Definition"))
                    {
                        var name = e.Row.Cells[0].Value as string;
                        if (!string.IsNullOrEmpty(name))
                            _container.RemoveDefinition(name);

                        scope?.Complete();
                    }
                }
                catch (Exception)
                {
                    ShowWarning?.Invoke("Definition cannot be removed");
                }
            }
        }
    }
}
