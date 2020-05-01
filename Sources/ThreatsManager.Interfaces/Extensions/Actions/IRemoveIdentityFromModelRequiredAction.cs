using System;
using ThreatsManager.Interfaces.ObjectModel;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface implemented by Context Aware Actions that can require to perform a full removal of an Identity from the Model.
    /// </summary>
    public interface IRemoveIdentityFromModelRequiredAction
    {
        /// <summary>
        /// Event generated when an identity needs to be removed from the Model.
        /// </summary>
        event Action<IIdentity> IdentityRemovingRequired;
    }
}