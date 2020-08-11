using System;

namespace ThreatsManager.Utilities.Aspects
{
    /// <summary>
    /// Attribute to be used to decorate Properties that must not be considered by AutoDirty.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoDirtyIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutoDirtyIgnoreAttribute()
        {

        }
    }
}