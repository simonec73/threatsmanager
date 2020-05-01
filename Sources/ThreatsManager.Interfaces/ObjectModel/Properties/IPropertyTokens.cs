using System.Collections.Generic;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property representing a list of Tokens.
    /// </summary>
    public interface IPropertyTokens : IProperty
    {
        /// <summary>
        /// List of Tokens associated to the Property.
        /// </summary>
        IEnumerable<string> Value { get; set; }
    }
}