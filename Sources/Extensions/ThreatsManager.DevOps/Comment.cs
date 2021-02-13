using System;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.DevOps
{
    /// <summary>
    /// A comment from the DevOps system.
    /// </summary>
    public class Comment
    {
        public Comment([Required] string text, [Required] string author, DateTime timestamp)
        {
            Text = text;
            Author = author;
            Timestamp = timestamp;
        }

        public string Text { get; }
        public string Author { get; }
        public DateTime Timestamp { get; }
    }
}