using ThreatsManager.Interfaces;

namespace ThreatsManager.Extensions.Panels.Word.Engine
{
    internal enum PlaceholderSection
    {
        [EnumLabel("Model Placeholders")]
        Model,
        [EnumLabel("Counter Placeholders")]
        Counter,
        [EnumLabel("Chart Placeholders")]
        Chart,
        [EnumLabel("List Placeholders")]
        List,
        [EnumLabel("Table Placeholders")]
        Table
    }
}