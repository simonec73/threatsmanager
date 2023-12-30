using Microsoft.Win32;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Abstract Policy class.
    /// </summary>
    /// <remarks>This is the recommemded starting point to create your own Policies.<para/>
    /// It stores the policies configuration in the Registry, under a location that by default is
    /// "HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Simone Curzi\Threats Manager Studio\Policies".</remarks>
    public abstract class Policy : IPolicy
    {
        /// <summary>
        /// Get a Policy given its identifier.
        /// </summary>
        /// <param name="id">Extension Identifier of the Policy.</param>
        /// <returns>Instanec of the Policy, if found, otherwise null.</returns>
        public static Policy GetPolicy([Required] string id)
        {
            return ExtensionUtils.GetExtension<IPolicy>(id) as Policy;
        }

        /// <summary>
        /// Location of the Key in the Registry containing the Policies.
        /// </summary>
        protected virtual string Location => @"HKEY_LOCAL_MACHINE\SOFTWARE\Simone Curzi\Threats Manager Studio\Policies";

        /// <summary>
        /// Name of the Policy in the Registry.
        /// </summary>
        protected abstract string PolicyName { get; }

        /// <summary>
        /// Property returning true if the Policy is defined.
        /// </summary>
        public bool IsDefined => Value != null;

        /// <summary>
        /// Value of the Policy.
        /// </summary>
        public object Value => Registry.GetValue(Location, PolicyName, null);

        /// <summary>
        /// Provides the Value as boolean.
        /// </summary>
        protected bool? BoolValue
        {
            get
            {
                bool? result = null;

                var fromRegistry = (int?)Value;
                if (fromRegistry.HasValue)
                {
                    result = fromRegistry.Value != 0;
                }

                return result;
            }
        }

        /// <summary>
        /// Provides the Value as enumeration of strings.
        /// </summary>
        public IEnumerable<string> StringArrayValue
        {
            get
            {
                return Value as string[];
            }
        }
    }
}
