namespace ThreatsManager.QuantitativeRisk.Engine
{
    public class Probability : Definition
    {
        public double Minimum { get; set; }

        public double MostLikely { get; set; }

        public double Maximum { get; set; }

        public Confidence Confidence { get; set; }

        public virtual bool IsValid => (Minimum <= MostLikely)
                                       && (MostLikely <= Maximum)
                                       && (Minimum < Maximum);
    }
}