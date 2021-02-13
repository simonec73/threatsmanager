using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Exception raised if the creation of the Work Item fails.
    /// </summary>
    [Serializable]
    public class WorkItemCreationException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mitigation">Mitigation for which it has not been possible to create the Mitigation.</param>
        public WorkItemCreationException([NotNull] IMitigation mitigation) : 
            base("A failure occurred during creation of the Work Item for the Mitigation.")
        {
            Mitigation = mitigation;
        }

        /// <summary>
        /// Mitigation whose Work Item has not been created.
        /// </summary>
        public IMitigation Mitigation { get; }
    }
}