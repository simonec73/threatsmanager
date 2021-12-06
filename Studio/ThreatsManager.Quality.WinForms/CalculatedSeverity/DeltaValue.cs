using ThreatsManager.Interfaces;

namespace ThreatsManager.Quality.CalculatedSeverity
{
    enum DeltaValue
    {
        [EnumLabel("Strong decrease")]
        DecreaseStrong = -50,
        [EnumLabel("Weak decrease")]
        DecreaseWeak = -25,
        [EnumLabel("Nominal")]
        Nominal = 0,
        [EnumLabel("Weak increase")]
        IncreaseWeak = 25,
        [EnumLabel("Strong increase")]
        IncreaseStrong = 50
    }
}