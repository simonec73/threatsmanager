using System;
using ThreatsManager.Interfaces.ObjectModel.Properties;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Interface describing a Mitigation Strength.
    /// </summary>
    public interface IStrength : IPropertiesContainer, IComparable<IStrength>, IComparable, IThreatModelChild, IDirty
    {
        /// <summary>
        /// Identifier of the Strength.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Name of the Strength.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Description of the Strength.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Visibility of the Strength in the UI.
        /// </summary>
        bool Visible { get; set; }
 
        /// <summary>
        /// Creates a duplicate of the current Strength and attaches it to the Container passed as argument.
        /// </summary>
        /// <param name="container">Destination container.</param>
        /// <returns>Freshly created Strength.</returns>
        IStrength Clone(IStrengthsContainer container);
    }
}