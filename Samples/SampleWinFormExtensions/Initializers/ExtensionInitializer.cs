using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.Utilities;

namespace SampleWinFormExtensions.Initializers
{
    /// <summary>
    /// Initializer of the Extensions.
    /// </summary>
    /// <remarks>It is called only once, when the Extension is loaded.
    /// It is typically required to make classes known to support Json deserialization.</remarks>
    [Extension("FD73F28E-5D76-4CBD-98E5-0F0793CA5A24",
        "Sample WinForms Extensions Initializer", 10, ExecutionMode.Business)]
    public class ExtensionInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(DefinitionContainer));
        }
    }
}