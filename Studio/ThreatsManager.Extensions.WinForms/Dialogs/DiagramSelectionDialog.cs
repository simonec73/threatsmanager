using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;
using ThreatsManager.Interfaces.ObjectModel.Entities;

namespace ThreatsManager.Extensions.Dialogs
{
    // ReSharper disable CoVariantArrayConversion
    public partial class DiagramSelectionDialog : Form, IInitializableObject
    {
        private readonly IThreatModel _model;

        // ReSharper disable once MemberCanBePrivate.Global
        public DiagramSelectionDialog()
        {
            InitializeComponent();
        }

        public DiagramSelectionDialog([NotNull] IEntity entity) : this()
        {
            if (entity.Model is IThreatModel model)
            {
                _model = model;
                _diagramName.Text = entity.Name;

                var diagrams = model.Diagrams?
                    .Where(x => x.GetEntityShape(entity.Id) == null)
                    .ToArray();
                if (diagrams?.Any() ?? false)
                {
                    _assignExisting.Checked = true;
                    _diagrams.Items.AddRange(diagrams);
                }
                else
                {
                    _createNew.Checked = true;
                    _assignExisting.Enabled = false;
                }

                RefreshControls();
                _ok.Enabled = IsValid();
            }
            else
                throw new ArgumentException();
        }

        public bool IsInitialized => _model != null;

        public IDiagram Diagram => _assignExisting.Checked ? _diagrams.SelectedItem as IDiagram : null;

        public string DiagramName => _diagramName.Text;

        private bool IsValid()
        {
            return IsInitialized &&
                   ((_assignExisting.Checked && _diagrams.SelectedItem is IDiagram) ||
                    (_createNew.Checked && !string.IsNullOrWhiteSpace(_diagramName.Text)));
        }

        private void RefreshControls()
        {
            _diagrams.Enabled = _assignExisting.Checked;
            _diagramName.Enabled = !_assignExisting.Checked;
            _ok.Enabled = IsValid();
        }

        private void _assignExisting_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControls();
        }

        private void _createNew_CheckedChanged(object sender, EventArgs e)
        {
            RefreshControls();
        }

        private void _diagrams_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }

        private void _diagramName_TextChanged(object sender, EventArgs e)
        {
            _ok.Enabled = IsValid();
        }
    }
}
