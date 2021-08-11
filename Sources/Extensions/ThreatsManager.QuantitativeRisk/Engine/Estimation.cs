namespace ThreatsManager.QuantitativeRisk.Engine
{
    public class Estimation : Frequency
    {
        public double MagnitudeMin { get; set; }

        public double MagnitudeMostLikely { get; set; }

        public double MagnitudeMax { get; set; }

        public Confidence MagnitudeConfidence { get; set; }

        public override bool IsValid => base.IsValid && (MagnitudeMin <= MagnitudeMostLikely)
                               && (MagnitudeMostLikely <= MagnitudeMax)
                               && (MagnitudeMin < MagnitudeMax);
    }
}