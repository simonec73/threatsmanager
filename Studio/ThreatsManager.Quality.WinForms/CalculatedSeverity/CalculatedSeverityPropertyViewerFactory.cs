using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    [Extension("0543D4C4-D06B-4F7A-9A44-CA3CE85A9B8A", "Calculated Severity Property Viewer", 10, ExecutionMode.Simplified)]
    public class CalculatedSeverityPropertyViewerFactory : IPropertyViewerFactory
    {
        public IPropertyViewer CreatePropertyViewer([NotNull] IPropertiesContainer container, [NotNull] IProperty property)
        {
            return new CalculatedSeverityPropertyViewer(container, property);
        }
    }
}