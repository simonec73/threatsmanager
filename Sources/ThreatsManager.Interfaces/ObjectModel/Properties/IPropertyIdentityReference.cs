using System;

namespace ThreatsManager.Interfaces.ObjectModel.Properties
{
    /// <summary>
    /// Property containing a reference to other Identities.
    /// </summary>
    public interface IPropertyIdentityReference : IProperty
    {
        /// <summary>
        /// Identifier associated to the Identity.
        /// </summary>
        Guid ValueId { get; }

            /// <summary>
        /// Value of the Property.
        /// </summary>
        IIdentity Value { get; set; }
    }
}