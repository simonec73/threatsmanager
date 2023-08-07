using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Extensions.Diagrams
{
    /// <summary>
    /// Interface implemented by factories of Marker Providers.
    /// </summary>
    [ExtensionDescription("Marker Provider Factory")]
    public interface IMarkerProviderFactory : IExtension
    {
        /// <summary>
        /// Name of the Marker Provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates an instance of the marker associated to the object passed as argument.
        /// </summary>
        /// <param name="item">Reference object.</param>
        /// <returns>Marker associated with the reference object.</returns>
        /// <remarks>If the object is not compatible with the marker, the function returns null.</remarks>
        IMarkerProvider Create(object item);
    }

}
