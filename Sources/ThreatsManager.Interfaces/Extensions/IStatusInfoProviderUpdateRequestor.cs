using System;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by Extensions which can require to update Status Info Providers.
    /// </summary>
    /// <remarks>This interface can be applied to Extensions implementing interface IMainRibbonExtension.</remarks>
    public interface IStatusInfoProviderUpdateRequestor
    {
        /// <summary>
        /// Event to require updating all Status Info Providers
        /// </summary>
        event Action UpdateStatusInfoProviders;
    }
}