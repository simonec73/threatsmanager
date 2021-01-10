using System;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations;

namespace ThreatsManager.DevOps
{
    /// <summary>
    /// Exception raised if the change of the Work Item status fails.
    /// </summary>
    public class WorkItemStateChangeException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mitigation">Mitigation for which it has not been possible to create the Mitigation.</param>
        public WorkItemStateChangeException([NotNull] IMitigation mitigation, WorkItemStatus initialStatus, WorkItemStatus finalStatus) : 
            base("A failure occurred during update of the Work Item status.")
        {
            Mitigation = mitigation;
            InitialStatus = initialStatus;
            FinalStatus = finalStatus;
        }

        /// <summary>
        /// Mitigation whose Work Item status change has been attempted.
        /// </summary>
        public IMitigation Mitigation { get; }

        /// <summary>
        /// Initial status.
        /// </summary>
        public WorkItemStatus InitialStatus { get; }

        /// <summary>
        /// The attempted final status.
        /// </summary>
        public WorkItemStatus FinalStatus { get; }
    }
}