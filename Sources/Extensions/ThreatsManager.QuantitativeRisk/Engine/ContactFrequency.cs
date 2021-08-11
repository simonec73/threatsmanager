namespace ThreatsManager.QuantitativeRisk.Engine
{
    /// <summary>
    /// Contact Frequency. The probable frequency, within a given time-frame, that threat agents will come into contact with assets. 
    /// </summary>
    public class ContactFrequency : Frequency
    {
        /// <summary>
        /// Types of contact.
        /// </summary>
        public ContactType ContactType { get; set; }
    }
}