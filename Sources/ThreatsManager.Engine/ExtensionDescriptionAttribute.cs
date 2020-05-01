using System;

namespace ThreatsManager.Engine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ExtensionDescriptionAttribute : Attribute
    {
        public ExtensionDescriptionAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
