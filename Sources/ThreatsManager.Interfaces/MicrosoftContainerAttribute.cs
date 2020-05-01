using System;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Interfaces
{
    /// <summary>
    /// Attribute to be applied to the Assembly to characterize it as a container reserved for Microsoft internal consumption. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class MicrosoftContainerAttribute : Attribute
    {
    }
}