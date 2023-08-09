using System.Drawing;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Dialogs;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Annotations
{
    public class AddToBeClarifiedButtonPropertyViewerBlock : IPropertyViewerBlock
    {
        private readonly IPropertiesContainer _container;
        private readonly IProperty _property;
        private readonly IThreatModel _model;

        public AddToBeClarifiedButtonPropertyViewerBlock([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            _container = container;
            _property = property;

            if (container is IThreatModelChild child)
            {
                _model = child.Model;
            }
        }

        public bool Execute()
        {
            bool result = false;

            var dialog = new AnnotationDialog(_model, _container, new TopicToBeClarified());
            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                using (var scope = UndoRedoManager.OpenScope("Add Topic to Be Clarified"))
                {
                    var schemaManager = new AnnotationsPropertySchemaManager(_model);
                    schemaManager.AddAnnotation(_container, dialog.Annotation);

                    scope?.Complete();
                    result = true;
                }
            }

            return result;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.Button;
        public string Label => "Add Topic to be Clarified";
        public string Text { get; set; }
        public Bitmap Image => null;
        public bool Printable => false;
    }
}
