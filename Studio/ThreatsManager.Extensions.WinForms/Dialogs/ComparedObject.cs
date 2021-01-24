using System.Collections.Generic;
using PostSharp.Patterns.Contracts;
using ThreatsManager.Utilities;

namespace ThreatsManager.Extensions.Dialogs
{
    internal class ComparedObject
    {
        private readonly List<string> _differences = new List<string>();

        public ComparedObject(object source, object target)
        {
            Source = source;
            Target = target;
        }

        public object Source { get; private set; }

        public object Target { get; private set; }

        public ActionType Action
        {
            get
            {
                ActionType result = ActionType.Unknown;

                if (Source != null)
                {
                    result = Target != null ? ActionType.Change : ActionType.Add;
                }
                else if (Target != null)
                {
                    result = ActionType.Remove;
                }

                return result;
            }
        }

        public string Differences => _differences.TagConcat();

        public void AddDifference([Required] string difference)
        {
            _differences.Add(difference);
        }

        public override string ToString()
        {
            return Target?.ToString() ?? Source?.ToString() ?? "<unknown>";
        }
    }
}