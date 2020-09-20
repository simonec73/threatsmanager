using System;

namespace ThreatsManager.Interfaces.Extensions.Panels
{
    /// <summary>
    /// Base interface representing a Panel.
    /// </summary>
    public interface IPanel
    {
        /// <summary>
        /// Identifier of the Panel.
        /// </summary>
        Guid Id { get; }
    }

    /// <summary>
    /// Interface representing a Panel.
    /// </summary>
    /// <typeparam name="T">Type representing the Form containing the Panel.</typeparam>
    public interface IPanel<T> : IPanel
    {
        /// <summary>
        /// Container of the Panel.
        /// </summary>
        /// <remarks>For Windows Forms-based solutions like Threats Manager Studio, it would be Windows Form.
        /// For Web-based solutions, it would be something else.</remarks>
        T PanelContainer { get; set; }
    }
}