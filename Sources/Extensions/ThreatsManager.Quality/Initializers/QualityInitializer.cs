using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Quality.Annotations;
using ThreatsManager.Quality.Schemas;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Initializers
{
    [Extension("1069F3AA-BD5E-41C6-B681-0D37FCA12937", "Quality Initializer", 10, ExecutionMode.Business)]
    public class QualityInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(FalsePositiveList));
            KnownTypesBinder.AddKnownType(typeof(FalsePositiveInfo));
            KnownTypesBinder.AddKnownType(typeof(Annotation));
            KnownTypesBinder.AddKnownType(typeof(AnnotationAnswer));
            KnownTypesBinder.AddKnownType(typeof(TopicToBeClarified));
            KnownTypesBinder.AddKnownType(typeof(Question));
            KnownTypesBinder.AddKnownType(typeof(Questions));
            KnownTypesBinder.AddKnownType(typeof(Highlight));
            KnownTypesBinder.AddKnownType(typeof(ReviewNote));
        }
    }
}