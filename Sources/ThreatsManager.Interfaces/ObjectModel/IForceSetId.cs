using System;

namespace ThreatsManager.Interfaces.ObjectModel
{
    /// <summary>
    /// Interface used to allow setting the identifier for some IIdentity objects.
    /// </summary>
    public interface IForceSetId
    {
        /// <summary>
        /// Identifier to be set.
        /// </summary>
        /// <param name="id">New identifier.</param>
        /// <remarks>This method sets the identifier on the object, without doing anything else.
        /// If the Id is used anywhere else, it will not be changed.
        /// It is never recommended to use this method, but it may be useful if you need to change
        /// the identified right after the creation of the object.</remarks>
        void SetId(Guid id);
    }
}
