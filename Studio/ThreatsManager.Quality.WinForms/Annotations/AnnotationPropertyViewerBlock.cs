using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Dialogs;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities.Aspects;

namespace ThreatsManager.Quality.Annotations
{
    public class AnnotationPropertyViewerBlock : IPropertyViewerBlock, IInitializableObject
    {
        private readonly IPropertiesContainer _container;
        private readonly Annotation _annotation;
        private readonly IThreatModel _model;

        public AnnotationPropertyViewerBlock([NotNull] IPropertiesContainer container, [NotNull] Annotation annotation)
        {
            _annotation = annotation;
            _container = container;

            if (container is IThreatModelChild child)
            {
                _model = child.Model;
            }

            if (annotation is TopicToBeClarified topicToBeClarified)
            {
                Image = topicToBeClarified.Answered
                    ? Properties.Resources.speech_balloon_answer_big
                    : Properties.Resources.speech_balloon_question_big;
                BlockType = PropertyViewerBlockType.ImageButton;
            }
            else if (annotation is Highlight highlight)
            {
                Image = Properties.Resources.marker_big;
                BlockType = PropertyViewerBlockType.ImageButton;
            }
            else if (annotation is ReviewNote reviewNote)
            {
                Image = Properties.Resources.clipboard_check_edit_big;
                BlockType = PropertyViewerBlockType.ImageButton;
            }
            else
            {
                Image = Properties.Resources.note_text_big;
                BlockType = PropertyViewerBlockType.String;
            }
        }

        [InitializationRequired]
        public bool Execute()
        {
            var result = false;

            if (_annotation is TopicToBeClarified topicToBeClarified)
            {
                var dialog = new AnnotationDialog(_model, _container, topicToBeClarified, true);
                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.Abort)
                {
                    var schemaManager = new AnnotationsPropertySchemaManager(_model);
                    schemaManager.RemoveAnnotation(_container, topicToBeClarified);
                }

                result = true;
            } else if (_annotation is Highlight highlight)
            {
                var dialog = new AnnotationDialog(_model, _container, highlight, true);
                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.Abort)
                {
                    var schemaManager = new AnnotationsPropertySchemaManager(_model);
                    schemaManager.RemoveAnnotation(_container, highlight);
                }

                result = true;
            } else if (_annotation is ReviewNote reviewNote)
            {
                var dialog = new AnnotationDialog(_model, _container, reviewNote, true);
                if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.Abort)
                {
                    var schemaManager = new AnnotationsPropertySchemaManager(_model);
                    schemaManager.RemoveAnnotation(_container, reviewNote);
                }

                result = true;
            }

            return result;
        }

        public PropertyViewerBlockType BlockType { get; }

        public string Label
        {
            get
            {
                string result;

                if (_annotation is TopicToBeClarified topicToBeClarified)
                {
                    result = $"<b>{(topicToBeClarified.Answered ? "Question Answered" : "Needs Clarification")}</b><br/>{Text}";
                }
                else if (_annotation is Highlight highlight)
                {
                    result = $"<b>Highlight</b><br/>{Text}";
                }
                else if (_annotation is ReviewNote reviewNote)
                {
                    result = $"<b>Review Note</b><br/>{Text}";
                }
                else
                {
                    result = "Notes";
                }

                return result;
            }
        }

        public string Text
        {
            get => _annotation.Text;
            set => _annotation.Text = value;
        }

        public Bitmap Image { get; }
        public bool Printable => BlockType == PropertyViewerBlockType.String;
        public bool IsInitialized => _model != null;
    }
}