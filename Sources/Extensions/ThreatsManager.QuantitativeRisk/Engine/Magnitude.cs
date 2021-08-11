namespace ThreatsManager.QuantitativeRisk.Engine
{
    public class Magnitude : Definition
    {
        public double MagnitudeMin { get; set; }

        public double MagnitudeMostLikely { get; set; }

        public double MagnitudeMax { get; set; }

        public Confidence MagnitudeConfidence { get; set; }

        public virtual bool IsValid => (MagnitudeMin <= MagnitudeMostLikely)
                               && (MagnitudeMostLikely <= MagnitudeMax)
                               && (MagnitudeMin < MagnitudeMax);
    }
}