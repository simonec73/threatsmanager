using System;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Utilities.Training
{
    /// <summary>
    /// Attribute to be applied to the Assembly to specify where Training Pills for the implemented Extensions can be found. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class TrainingPillsAttribute : Attribute
    {
        /// <summary>
        /// Constructor to specify where the Training Pills for the implemented Extensions are uploaded.
        /// </summary>
        /// <param name="baseUrl">Url containing the Training Pills.</param>
        public TrainingPillsAttribute([Required] string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        /// <summary>
        /// Url containing the Training Pills.
        /// </summary>
        public string BaseUrl { get; set; }
    }
}