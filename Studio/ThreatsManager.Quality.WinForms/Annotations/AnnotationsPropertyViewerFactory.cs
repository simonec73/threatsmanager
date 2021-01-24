using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.Annotations
{
    [Extension("B3773AA1-8140-4986-B956-5254C7EA6A15", "Annotations Property Viewer", 10, ExecutionMode.Simplified)]
    public class AnnotationsPropertyViewerFactory : IPropertyViewerFactory
    {
        public IPropertyViewer CreatePropertyViewer([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            return new AnnotationsPropertyViewer(container, property);
        }
    }
}