using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class ImportSchemaDialog : Form, IInitializableObject
    {
        private readonly IThreatModel _model;
        private IThreatModel _kb;

        public ImportSchemaDialog()
        {
            InitializeComponent();
        }

        public ImportSchemaDialog([NotNull] IThreatModel model) : this()
        {
            _model = model;
        }

        public bool IsInitialized => _model != null;

        public bool IsValid()
        {
            return IsInitialized && _kb != null && _schemas.CheckedItems.Count > 0;
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            var list = _schemas.CheckedItems.OfType<IPropertySchema>().Select(x => x.Id).ToArray();
            if (list.Any())
            {
                var def = new DuplicationDefinition()
                {
                    PropertySchemas = list
                };

                using (var scope = UndoRedoManager.OpenScope("Import Schema"))
                {
                    if (_model.Merge(_kb, def) && _applySchemas.Checked)
                    {
                        foreach (var id in list)
                        {
                            _model.ApplySchema(id);
                        }
                    }

                    scope?.Complete();
                }
            }
        }

        private void _browse_Click(object sender, EventArgs e)
        {
            var kbManagers = ExtensionUtils.GetExtensions<IKnowledgeBaseManager>()?.ToArray();
            
            if (kbManagers?.Any() ?? false)
            {
                _openFile.Filter = kbManagers.GetFilter();

                if (_openFile.ShowDialog(ActiveForm) == DialogResult.OK)
                {
                    if (_kb != null)
                        ThreatModelManager.Remove(_kb.Id);
                    _schemas.Items.Clear();

                    var location = _openFile.FileName;
                    _fileName.Text = location;

                    IKnowledgeBaseManager selected = null;
                    int index = _openFile.FilterIndex;
                    if (index > 0 && index <= kbManagers.Count())
                    {
                        selected = kbManagers[index - 1];
                    }
                    else
                    {
                        selected = kbManagers.FirstOrDefault(x => x.CanHandle(LocationType.FileSystem, location));
                    }

                    _kb = selected?.Load(LocationType.FileSystem, location, false);

                    var schemas = _kb?.Schemas?.OrderBy(x => x.Name).ToArray();
                    if (schemas?.Any() ?? false)
                    {
                        _schemas.Items.AddRange(schemas);
                    }

                    _ok.Enabled = IsValid();
                }
            }
        }

        private void _schemas_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var count = _schemas.CheckedItems.Count;
            if (e.NewValue == CheckState.Checked)
                count++;
            else
                count--;

            _ok.Enabled = IsInitialized && _kb != null && count > 0;
        }

        private void _checkAll_Click(object sender, EventArgs e)
        {
            var count = _schemas.Items.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                    _schemas.SetItemChecked(i, true);
            }
        }

        private void _uncheckAll_Click(object sender, EventArgs e)
        {
            var count = _schemas.Items.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                    _schemas.SetItemChecked(i, false);
            }
        }

        private void ImportSchemaDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_kb != null)
                ThreatModelManager.Remove(_kb.Id);
        }
    }
}
