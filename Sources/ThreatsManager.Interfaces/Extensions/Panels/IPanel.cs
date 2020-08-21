using System;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Interface representing a Panel.
    /// </summary>
    public interface IPanel
    {
        /// <summary>
        /// Identifier of the Panel.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Form containing the Panel.
        /// </summary>
        /// <remarks>It may be a Windows Form, but not necessarily.</remarks>
        object ContainingForm { get; set; }
    }
}