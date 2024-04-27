using System;
using System.Linq;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Dialogs
{
    public partial class SetOpenTopicsDialog : Form
    {
        private IThreatModel _model;

        public SetOpenTopicsDialog()
        {
            InitializeComponent();
        }

        public SetOpenTopicsDialog([NotNull] IThreatModel model) : this()
        {
            _model = model;
        }

        private void _ok_Click(object sender, EventArgs e)
        {
            var containers = _model.GetContainersWithOpenTopics()?.ToArray();
            if (containers?.Any() ?? false)
            {
                if (MessageBox.Show(Form.ActiveForm,
                    $"There are {containers.Count()} objects with Open Topics. You are about to set details for every Open Topic missing them. Are you sure?",
                    "Set Open Topics", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                    == DialogResult.Yes)
                {
                    using (var scope = UndoRedoManager.OpenScope("Set Open Topics"))
                    {
                        foreach (var container in containers)
                        {
                            var openTopics = container.GetOpenTopics()?.ToArray();
                            if (openTopics?.Any() ?? false)
                            {
                                foreach (var openTopic in openTopics)
                                {
                                    if (string.IsNullOrWhiteSpace(openTopic.AskedBy))
                                        openTopic.AskedBy = _askedBy.Text;
                                    if (openTopic.AskedOn == DateTime.MinValue)
                                        openTopic.AskedOn = _askedOn.Value;
                                    if (string.IsNullOrWhiteSpace(openTopic.AskedVia))
                                        openTopic.AskedVia = _askedVia.Text;
                                }
                            }
                        }

                        scope?.Complete();
                    }
                }
            }
        }

        private void _askedBy_ButtonCustomClick(object sender, EventArgs e)
        {
            _askedBy.Text = UserName.GetDisplayName();
        }

        private void _askedOn_ButtonCustomClick(object sender, EventArgs e)
        {
            _askedOn.Value = DateTime.Now;
        }

        private void _askedVia_ButtonCustomClick(object sender, EventArgs e)
        {
            _askedVia.Text = "Email";
        }

        private void _askedVia_ButtonCustom2Click(object sender, EventArgs e)
        {
            _askedVia.Text = "Excel";
        }
    }
}
