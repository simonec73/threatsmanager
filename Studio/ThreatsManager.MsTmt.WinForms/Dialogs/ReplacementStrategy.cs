using ThreatsManager.Interfaces;

namespace ThreatsManager.MsTmt.Dialogs
{
    public enum ReplacementStrategy
    {
        [EnumLabel("Stop it at least a Flow already exists")]
        Stop,
        [EnumLabel("Replace existing Flows")]
        Replace,
        [EnumLabel("Skip existing Flows")]
        Skip
    }
}