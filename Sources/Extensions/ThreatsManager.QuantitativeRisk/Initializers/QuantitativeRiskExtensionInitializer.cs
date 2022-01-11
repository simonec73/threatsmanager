using ThreatsManager.Interfaces;
using ThreatsManager.Interfaces.Extensions;
using ThreatsManager.QuantitativeRisk.Engine;
using ThreatsManager.QuantitativeRisk.Facts;
using ThreatsManager.Utilities;

namespace ThreatsManager.Quality.Initializers
{
    [Extension("036D0E98-A88E-4327-A967-FC10C0499272", "Quantitative Risk Extension Initializer", 10, ExecutionMode.Business)]
    public class QuantitativeRiskExtensionInitializer : IExtensionInitializer
    {
        public void Initialize()
        {
            KnownTypesBinder.AddKnownType(typeof(Fact));
            KnownTypesBinder.AddKnownType(typeof(FactContainer));
            KnownTypesBinder.AddKnownType(typeof(FactHardNumber));
            KnownTypesBinder.AddKnownType(typeof(FactProbRange));
            KnownTypesBinder.AddKnownType(typeof(FactRange));
            KnownTypesBinder.AddKnownType(typeof(ContactFrequency));
            KnownTypesBinder.AddKnownType(typeof(Estimation));
            KnownTypesBinder.AddKnownType(typeof(FactBased));
            KnownTypesBinder.AddKnownType(typeof(Frequency));
            KnownTypesBinder.AddKnownType(typeof(Loss));
            KnownTypesBinder.AddKnownType(typeof(LossEventFrequency));
            KnownTypesBinder.AddKnownType(typeof(LossMagnitude));
            KnownTypesBinder.AddKnownType(typeof(Magnitude));
            KnownTypesBinder.AddKnownType(typeof(Probability));
            KnownTypesBinder.AddKnownType(typeof(Risk));
            KnownTypesBinder.AddKnownType(typeof(ThreatEventFrequency));
            KnownTypesBinder.AddKnownType(typeof(Vulnerability));
            KnownTypesBinder.AddKnownType(typeof(Threshold));
            KnownTypesBinder.AddKnownType(typeof(Thresholds));
        }
    }
}