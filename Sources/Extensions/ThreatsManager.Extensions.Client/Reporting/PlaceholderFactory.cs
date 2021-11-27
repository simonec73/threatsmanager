using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Reporting
{
    /// <summary>
    /// A factory for Placeholders.
    /// </summary>
    public static class PlaceholderFactory
    {
        /// <summary>
        /// Creates a placeholder from a specified full qualifier.
        /// </summary>
        /// <param name="fullQualifier">Full qualifier.
        /// <para>It is in the format of [ThreatsManagerPlatform:ModelTest:parameter1#parameter2], where
        /// <list type="bullet">
        /// <item><description>ThreatsManagerPlatform is fixed.</description></item>
        /// <item><description>ModelTest represents the Qualifier which identifies the specific Placeholder.</description></item>
        /// <item><description>parameter1#parameter2 represent the parameters for the Placeholder, separated by #.</description></item>
        /// </list>
        /// </para></param>
        /// <param name="priority">[out] Integer representing the priority of the placeholder.</param>
        /// <returns>Generated Placeholder. It may be null, if no suitable placeholder has been found.</returns>
        public static IPlaceholder Create([Required] string fullQualifier, out int priority)
        {
            IPlaceholder result = null;
            priority = 100;

            var factories = ExtensionUtils.GetExtensions<IPlaceholderFactory>()?.ToArray();
            if (factories?.Any() ?? false)
            {
                var regex = new Regex(
                    @"\[ThreatsManagerPlatform:(?<qualifier>[\w]+):?(?<params>[\S ]*)?\]");
                var match = regex.Match(fullQualifier);
                if (match.Success)
                {
                    var factory = factories.FirstOrDefault(x =>
                        string.CompareOrdinal(x.Qualifier, match.Groups["qualifier"]?.Value) == 0);

                    if (factory != null)
                    {
                        priority = factory.GetExtensionPriority();
                        result = factory.Create(match.Groups["params"]?.Value);
                    }
                }
            }

            return result;
        }
    }
}
