using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Factory to create Property Viewers.
    /// </summary>
    [ExtensionDescription("Property Viewer")]
    public interface IPropertyViewerFactory : IExtension
    {
        /// <summary>
        /// Create a new instance of the Property Viewer.
        /// </summary>
        /// <param name="container">Container of the Property.</param>
        /// <param name="property">Property to be shown.</param>
        /// <returns>New Property Viewer.</returns>
        IPropertyViewer CreatePropertyViewer(IPropertiesContainer container, IProperty property);
    }
}