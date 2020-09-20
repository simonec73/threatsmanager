using System;
using System.Drawing;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface describing a Severity.
    /// </summary>
    /// <remarks>This is implemented as a way to represent different levels of Severity.</remarks>
    public interface ISeverity : IPropertiesContainer, IComparable<ISeverity>, IComparable, IThreatModelChild, IDirty
    {
        /// <summary>
        /// Identifier of the Severity.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Name of the Severity.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Description of the Severity.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Visibility of the Severity in the UI.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Text color associated to the Severity.
        /// </summary>
        KnownColor TextColor { get; set; }

        /// <summary>
        /// Background color associated to the Severity.
        /// </summary>
        KnownColor BackColor { get; set; }
 
        /// <summary>
        /// Creates a duplicate of the current Severity and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Severity.</returns>
        ISeverity Clone(ISeveritiesContainer container);
    }
}