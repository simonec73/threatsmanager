using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreatsManager.Interfaces.Extensions;

namespace ThreatsManager.Utilities
{
    /// <summary>
    /// Utilities for File Managers.
    /// </summary>
    public static class FileManagerUtils
    {
        /// <summary>
        /// Get a filter to be used with OpenFile and SaveFile dialogs.
        /// </summary>
        /// <param name="managers">Managers to be used.</param>
        /// <returns>String containing the filter.</returns>
        public static string GetFilter(this IEnumerable<IFileManager> managers)
        {
            string result = null;

            if (managers?.Any() ?? false)
            {
                var builder = new StringBuilder();
                foreach (var manager in managers)
                {
                    var extensions = manager.Extensions?.ToArray();
                    if (extensions?.Any() ?? false)
                    {
                        if (builder.Length > 0)
                            builder.Append("|");

                        bool first = true;
                        builder.Append($"{manager.PackageName} (");
                        foreach (var extension in extensions)
                        {
                            if (first)
                                first = false;
                            else
                                builder.Append(";");
                            builder.Append(extension);
                        }
                        builder.Append(")|");

                        first = true;
                        foreach (var extension in extensions)
                        {
                            if (first)
                                first = false;
                            else
                                builder.Append(";");
                            builder.Append($"*{extension}");
                        }
                    }
                }

                if (builder.Length > 0)
                    result = builder.ToString();
            }

            return result;
        }
    }
}
