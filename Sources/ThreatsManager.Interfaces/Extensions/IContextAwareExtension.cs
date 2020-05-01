using System.Collections.Generic;
using ThreatsManager.Interfaces.Extensions.Actions;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Interface implemented by the Extensions that can extend their behavior based on Context Aware Actions.
    /// </summary>
    public interface IContextAwareExtension
    {
        /// <summary>
        /// Supported scopes by the Context Aware Extension.
        /// </summary>
        Scope SupportedScopes { get; }      

        /// <summary>
        /// Configuration of the actions that shall be shown by the extension.
        /// </summary>
        /// <param name="actions">Enumeration of the actions.</param>
        void SetContextAwareActions(IEnumerable<IContextAwareAction> actions);
    }
}