namespace ThreatsManager.QuantitativeRisk.Engine
{
    public class Frequency : Definition
    {
        public double FrequencyMin { get; set; }

        public double FrequencyMostLikely { get; set; }

        public double FrequencyMax { get; set; }

        public Confidence FrequencyConfidence { get; set; }

        public virtual bool IsValid => (FrequencyMin <= FrequencyMostLikely)
                               && (FrequencyMostLikely <= FrequencyMax)
                               && (FrequencyMin < FrequencyMax);
    }
}