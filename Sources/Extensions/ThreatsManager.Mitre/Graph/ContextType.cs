using ThreatsManager.Interfaces;

namespace ThreatsManager.Mitre.Graph
{
    public enum ContextType
    {
        Architecture,
        Language,
        [EnumLabel("Operating System")]
        OperatingSystem,
        Technology
    }
}