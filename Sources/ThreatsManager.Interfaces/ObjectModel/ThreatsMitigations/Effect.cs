using System;

namespace ThreatsManager.Interfaces.ObjectModel.ThreatsMitigations
{
    /// <summary>
    /// Enumeration of the main security concerns. 
    /// </summary>
    [Flags]
    public enum Effect
    {
        /// <summary>
        /// In information security, confidentiality "is the property, that information is not made available or
        /// disclosed to unauthorized individuals, entities, or processes."
        /// (source: Beckers, K. (2015). Pattern and Security Requirements:
        /// Engineering-Based Establishment of Security Standards. Springer. p. 100. ISBN 9783319166643)
        /// </summary>
        [EnumDescription("In information security, confidentiality 'is the property, that information is not made available or disclosed to unauthorized individuals, entities, or processes'. (source: Beckers, K. (2015). Pattern and Security Requirements: Engineering-Based Establishment of Security Standards. Springer. p. 100. ISBN 9783319166643)")]
        Confidentiality = 1,

        /// <summary>
        /// In information security, data integrity means maintaining and assuring the accuracy and completeness
        /// of data over its entire lifecycle (Boritz, J. Efrim. "IS Practitioners' Views on Core Concepts
        /// of Information Integrity". International Journal of Accounting Information Systems.
        /// Elsevier. 6 (4): 260–279. doi:10.1016/j.accinf.2005.07.001. Retrieved 12 August 2011).
        /// </summary>
        [EnumDescription("In information security, data integrity means maintaining and assuring the accuracy and completeness of data over its entire lifecycle (Boritz, J. Efrim. 'IS Practitioners' Views on Core Concepts of Information Integrity'. International Journal of Accounting Information Systems. Elsevier. 6 (4): 260–279. doi:10.1016/j.accinf.2005.07.001. Retrieved 12 August 2011).")]
        Integrity = 2,

        /// <summary>
        /// For any information system to serve its purpose, the information must be available when it is needed.
        /// This means the computing systems used to store and process the information, the security controls
        /// used to protect it, and the communication channels used to access it must be functioning correctly
        /// (Loukas, G.; Oke, G. (September 2010) [August 2009]. "Protection Against Denial of Service Attacks: A Survey"
        /// Comput. J. 53 (7): 1020–1037)
        /// </summary>
        [EnumDescription("For any information system to serve its purpose, the information must be available when it is needed. This means the computing systems used to store and process the information, the security controls used to protect it, and the communication channels used to access it must be functioning correctly (Loukas, G.; Oke, G. (September 2010) [August 2009]. 'Protection Against Denial of Service Attacks: A Survey' Comput. J. 53 (7): 1020–1037)")]
        Availability = 4
    }
}