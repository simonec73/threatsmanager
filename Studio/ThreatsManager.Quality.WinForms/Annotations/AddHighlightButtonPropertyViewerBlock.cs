using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Properties;
using ThreatsManager.Quality.Dialogs;
using ThreatsManager.Quality.Schemas;

namespace ThreatsManager.Quality.Annotations
{
    public class AddHighlightButtonPropertyViewerBlock : IPropertyViewerBlock
    {
        private readonly IPropertiesContainer _container;
        private readonly IProperty _property;
        private readonly IThreatModel _model;

        public AddHighlightButtonPropertyViewerBlock([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
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

            var dialog = new AnnotationDialog(_model, _container, new Highlight());
            if (dialog.ShowDialog(Form.ActiveForm) == DialogResult.OK)
            {
                var schemaManager = new AnnotationsPropertySchemaManager(_model);
                schemaManager.AddAnnotation(_container, dialog.Annotation);
                result = true;
            }

            return result;
        }

        public PropertyViewerBlockType BlockType => PropertyViewerBlockType.Button;
        public string Label => "Add Highlight";
        public string Text { get; set; }
        public Bitmap Image => null;
        public bool Printable => false;
    }
}
