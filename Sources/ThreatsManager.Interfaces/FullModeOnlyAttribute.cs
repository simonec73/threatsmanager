using System;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Attribute used to mark Extensions that shall be loaded only when Full Mode is enabled. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FullModeOnlyAttribute : Attribute
    {
    }
}
