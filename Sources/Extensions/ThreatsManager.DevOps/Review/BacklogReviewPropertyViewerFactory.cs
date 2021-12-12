using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.DevOps.Review
{
    [Extension("4C829CF2-FE0A-4801-89A6-4594B8BC19AD", "Backlog Review Property Viewer", 10, ExecutionMode.Management)]
    public class BacklogReviewPropertyViewerFactory : IPropertyViewerFactory
    {
        public IPropertyViewer CreatePropertyViewer([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            return new BacklogReviewPropertyViewer(container, property);
        }
    }
}