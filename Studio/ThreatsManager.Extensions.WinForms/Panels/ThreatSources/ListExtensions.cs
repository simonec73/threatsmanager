using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreatsManager.Extensions.Panels.ThreatSources
{
    public static class ListExtensions
    {
        public static string ConcatenateString(this IEnumerable<string> list)
        {
            string result = null;

            var items = list?.ToArray();

            if (items?.Any() ?? false)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in items)
                {
                    if (builder.Length > 0)
                        builder.Append(", ");
                    builder.Append(item);
                }
                result = builder.ToString();
            }

            return result;
        }
    }
}