using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Patterns.Contracts;

namespace ThreatsManager.Utilities
{
    public static class StringExtensions
    {
        public static bool IsEqual(this string first, string second)
        {
            return string.Compare(first, second, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public static bool ContainsCaseInsensitive(this IEnumerable<string> list, [Required] string text)
        {
            return list?.Any(x => string.Compare(x, text, StringComparison.InvariantCultureIgnoreCase) == 0) ?? false;
        }

        public static IEnumerable<string> TagSplit(this string text)
        {
            return text.Split(';', null);
        }

        public static string TagConcat(this IEnumerable<string> list)
        {
            return list.Concat(';', null);
        }

        public static string Concat(this IEnumerable<string> list, char separator, string replacement)
        {
            string result = null;

            var items = list?.ToArray();

            if (items?.Any() ?? false)
            {
                if (replacement == null)
                    replacement = $"\\x{Convert.ToInt32(separator):X4}";
                var builder = new StringBuilder();
                foreach (var curr in items)
                {
                    string text = curr.Replace(separator.ToString(), replacement);
                    if (builder.Length > 0)
                        builder.Append(separator);
                    builder.Append(text);
                }

                result = builder.ToString();
            }

            return result;
        }

        public static IEnumerable<string> Split(this string text, char separator, string replacement)
        {
            if (replacement == null)
                replacement = $"\\x{Convert.ToInt32(separator):X4}";
            return text?.Split(separator).Select(x => x.Replace(replacement, separator.ToString()).Trim());
        }
    }
}