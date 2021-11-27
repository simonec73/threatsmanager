using ThreatsManager.Interfaces;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    /// <summary>
    /// Types of contact.
    /// </summary>
    public enum ContactType
    {
        /// <summary>
        /// A random contact happens by chance. There is no intentionality.
        /// </summary>
        [EnumLabel("Random")]
        Random,
        
        /// <summary>
        /// A regular contact occurs as part of the normal behavior.
        /// </summary>
        [EnumLabel("Regular")]
        Regular,

        /// <summary>
        /// An intentional contact occurs due to some intention by the subject.
        /// </summary>
        [EnumLabel("Intentional")]
        Intentional
    }
}