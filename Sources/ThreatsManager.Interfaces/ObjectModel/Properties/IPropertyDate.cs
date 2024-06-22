using System;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing a date.
    /// </summary>
    public interface IPropertyDate : IProperty
    {
        /// <summary>
        /// Value of the Property.
        /// </summary>
        DateTime Value { get; set; }
    }
}