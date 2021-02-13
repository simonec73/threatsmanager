using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Annotations
{
    public class AnnotationsPropertyViewer : IPropertyViewer
    {
        private readonly IPropertiesContainer _container;
        private readonly IProperty _property;
        private Annotations _annotations;

        public AnnotationsPropertyViewer([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            _container = container;
            _property = property;
        }

        public IEnumerable<IPropertyViewerBlock> Blocks
        {
            get
            {
                List<IPropertyViewerBlock> result = new List<IPropertyViewerBlock>();

                var annotations = Annotations?.Items?
                    .Where(x => !(x is TopicToBeClarified || x is Highlight || x is ReviewNote))
                    .ToArray();
                if (annotations?.Any() ?? false)
                {
                    var notes = annotations.FirstOrDefault();
                    if (notes != null)
                        result.Add(new AnnotationPropertyViewerBlock(_container, notes));
                }

                if (result.Count == 0)
                {
                    result.Add(new AddNotesButtonPropertyViewerBlock(_container, _property));
                }

                var topicsToBeClarified = Annotations?.Items?.Where(x => x is TopicToBeClarified).ToArray();
                if (topicsToBeClarified?.Any() ?? false)
                {
                    result.AddRange(topicsToBeClarified.Select(x => new AnnotationPropertyViewerBlock(_container, x)));
                }
                result.Add(new AddToBeClarifiedButtonPropertyViewerBlock(_container, _property));

                var highlights = Annotations?.Items?.Where(x => x is Highlight).ToArray();
                if (highlights?.Any() ?? false)
                {
                    result.AddRange(highlights.Select(x => new AnnotationPropertyViewerBlock(_container, x)));
                }
                result.Add(new AddHighlightButtonPropertyViewerBlock(_container, _property));

                var reviewNotes = Annotations?.Items?.Where(x => x is ReviewNote).ToArray();
                if (reviewNotes?.Any() ?? false)
                {
                    result.AddRange(reviewNotes.Select(x => new AnnotationPropertyViewerBlock(_container, x)));
                }
                result.Add(new AddReviewNoteButtonPropertyViewerBlock(_container, _property));

                return result.AsReadOnly();
            }
        }

        private Annotations Annotations
        {
            get
            {
                if (_annotations == null && 
                    _property is IPropertyJsonSerializableObject jsonSerializableObject &&
                    jsonSerializableObject.Value is Annotations annotations)
                {
                    _annotations = annotations;
                }

                return _annotations;
            }
        }
    }
}
