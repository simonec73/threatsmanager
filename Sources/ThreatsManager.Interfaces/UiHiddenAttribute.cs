using System;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Attribute used to mark enumeration fields to hide them in the UI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class UiHiddenAttribute : Attribute
    {
    }
}