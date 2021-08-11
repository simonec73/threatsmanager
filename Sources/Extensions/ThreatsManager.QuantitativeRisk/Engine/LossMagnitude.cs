using System.Collections.Generic;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    /// <summary>
    /// Loss Magnitude. The probable magnitude of primary and secondary loss resulting from an event.
    /// </summary>
    public class LossMagnitude : Definition
    {
        private List<Loss> _losses = new List<Loss>();

        /// <summary>
        /// Primary Loss Magnitude. Primary stakeholder loss that materialize directly as a result of the event.
        /// </summary>
        public List<Loss> PrimaryLosses
        {
            get { return _losses; }
            set
            {
                if (value != null)
                    _losses = new List<Loss>(value);
                else
                    _losses = null;
            }
        }

        /// <summary>
        /// Secondary Loss Event Frequency. The percentage of primary events that have secondary effects.
        /// </summary>
        public Probability SecondaryLossEventFrequency { get; set; }

        /// <summary>
        /// Secondary Risk. Primary stakeholder loss-exposure that exists due to the potential for secondary stakeholder reactions to the primary event.
        /// </summary>
        public List<Loss> SecondaryLosses
        {
            get { return _losses; }
            set
            {
                if (value != null)
                    _losses = new List<Loss>(value);
                else
                    _losses = null;
            }
        }
    }
}