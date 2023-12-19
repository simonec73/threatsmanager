using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatsManager.Interfaces.Extensions
{
    /// <summary>
    /// Policies are used to enforce settings defined by the organization.
    /// </summary>
    /// <remarks>The actual value of the setting is specific to the Policy itself, and must be read-only
    /// from some protected source. Policies will typically use values in the Registry
    /// under "HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Simone Curzi\Threats Manager Studio\Policies".<para/>
    /// The recommended way to create a Policy is to create a class derived from ThreatsManager.Utilities.Policy.</remarks>
    [ExtensionDescription("Extension Initializer")]
    public interface IPolicy : IExtension
    {
        /// <summary>
        /// Property returning true if the Policy is defined.
        /// </summary>
        bool IsDefined { get; }

        /// <summary>
        /// Value of the Policy.
        /// </summary>
        object Value { get; }
    }
}
