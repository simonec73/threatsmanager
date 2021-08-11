using ThreatsManager.Interfaces;

namespace ThreatsManager.QuantitativeRisk.Engine
{
    public enum LossForm
    {
        [EnumLabel("Productivity")]
        Productivity,
        [EnumLabel("Response")]
        Response,
        [EnumLabel("Replacement")]
        Replacement,
        [EnumLabel("Fines and Judgements")]
        FinesJudgements,
        [EnumLabel("Competitive Advantage")]
        CompetitiveAdvantage,
        [EnumLabel("Reputation")]
        Reputation
    }
}