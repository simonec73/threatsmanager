using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Quality.Annotations;

namespace ThreatsManager.Quality.Dialogs
{
    public partial class AnnotationDialog : Form
    {
        private readonly bool _editMode;

        public AnnotationDialog()
        {
            InitializeComponent();
        }

        public AnnotationDialog([NotNull] IThreatModel model, [NotNull] object obj, [NotNull] Annotation annotation, bool editMode = false) : this()
        {
            _annotation.SetObject(model, obj);
            _annotation.Annotation = annotation;
            _editMode = editMode;

            if (annotation is TopicToBeClarified)
            {
                if (string.IsNullOrWhiteSpace(annotation?.ModifiedBy))
                    this.Text = "Add Topic to be Clarified";
                else
                    this.Text = "Edit Topic to be Clarified";
            }
            else if (annotation is Highlight)
            {
                if (string.IsNullOrWhiteSpace(annotation?.ModifiedBy))
                    this.Text = "Add Highlight";
                else
                    this.Text = "Edit Highlight";
            }
            else if (annotation is ReviewNote reviewNote)
            {
                if (string.IsNullOrWhiteSpace(annotation?.ModifiedBy))
                    this.Text = "Add Review Note";
                else
                    this.Text = "Edit Review Note";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(annotation?.ModifiedBy))
                    this.Text = "Add Notes";
                else
                    this.Text = "Edit Notes";
            }

            if (_editMode)
            {
                CancelButton = _ok;
                _cancel.DialogResult = DialogResult.None;
                _cancel.Text = "Delete";
                _ok.Text = "Close";
            }
        }

        public Annotation Annotation => _annotation.Annotation;

        private void _cancel_Click(object sender, System.EventArgs e)
        {
            if (_editMode && MessageBox.Show("You are about to delete the current Annotation. Do you confirm?",
                "Delete Annotation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                DialogResult = DialogResult.Abort;
            }
        }
    }
}
