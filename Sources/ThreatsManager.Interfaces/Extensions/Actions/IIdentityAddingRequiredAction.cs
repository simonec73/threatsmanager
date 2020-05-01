using System;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel;
using ThreatsManager.Interfaces.ObjectModel.Diagrams;

namespace ThreatsManager.Interfaces.Extensions.Actions
{
    /// <summary>
    /// Interface implemented by Context Aware Actions that can require to add an Identity to the Diagram.
    /// </summary>
    public interface IIdentityAddingRequiredAction
    {
        /// <summary>
        /// Event generated when a freshly created identity needs to be added to the Diagram.
        /// </summary>
        event Action<IDiagram, IIdentity, PointF, SizeF> IdentityAddingRequired;
    }
}