using PostSharp.Patterns.Contracts;

namespace ThreatsManager.QuantitativeRisk.Facts
{
    /// <summary>
    /// Parameter to configure the Fact Provider.
    /// </summary>
    public class FactProviderParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the Fact Provider.</param>
        /// <param name="description">Description of the Fact Provider.</param>
        /// <param name="secure">Flag stating if the parameter should be protected because it contains sensitive information.</param>
        public FactProviderParameter([Required] string name, string description, bool secure)
        {
            Name = name;
            Description = description;
            Secure = secure;
        }

        /// <summary>
        /// Name of the Fact Provider.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the Fact Provider.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Flag stating if the parameter should be protected because it contains sensitive information.
        /// </summary>
        public bool Secure { get; private set; }
    }
}
