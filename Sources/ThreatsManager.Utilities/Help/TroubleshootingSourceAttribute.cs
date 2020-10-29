using System;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Utilities.Help
{
    /// <summary>
    /// Attribute to be applied to the Assembly to specify where the Troubleshooting configuration file for the Extension library can be found. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class TroubleshootingSourceAttribute : Attribute
    {
        /// <summary>
        /// Constructor to specify the full URL of the Troubleshooting configuration file for the Extension library.
        /// </summary>
        /// <param name="url">Url containing the Troubleshooting configuration file.</param>
        /// <remarks>The default Priority is Medium.</remarks>
        public TroubleshootingSourceAttribute([RegularExpression(@"(https?|ftp)://[^\s/$.?#].[^\s]*\.json")] string url)
        {
            Url = url;
            Priority = Priority.Medium;
        }

        /// <summary>
        /// Constructor to specify the full URL of the Troubleshooting configuration file for the Extension library.
        /// </summary>
        /// <param name="url">Url containing the Troubleshooting configuration file.</param>
        /// <param name="priority">Priority of the source.</param>
        public TroubleshootingSourceAttribute([RegularExpression(@"(https?|ftp)://[^\s/$.?#].[^\s]*\.json")] string url,
            Priority priority)
        {
            Url = url;
            Priority = priority;
        }

        /// <summary>
        /// Url containing the full path of the Troubleshooting configuration file.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Priority of the source.
        /// </summary>
        public Priority Priority { get; set; }
    }
}