using System;
using System.IO;
using System.Windows.Forms;
using PostSharp.Patterns.Diagnostics;
using ThreatsManager.Properties;
using ThreatsManager.Utilities;

namespace ThreatsManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [Log(AttributeExclude = true)]
        static void Main()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("PUT_YOUR_LICENSE_HERE");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Folder = 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Resources.SolutionTitle);
            try
            {
                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);
            }
            catch
            {
                // Ignore: this would happen if the Ransomware Protection is on.
            }

#if LOGGING
            var nlogConfig = new LoggingConfiguration();

            var fileTarget = new FileTarget("file")
            {
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                FileName = Path.Combine(Folder, "tms.log"),
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
                KeepFileOpen = true,
                ConcurrentWrites = false,
                ArchiveOldFileOnStartup = true,
                MaxArchiveFiles = 5
            };

            nlogConfig.AddTarget(fileTarget);
            nlogConfig.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Debug, fileTarget));

            LogManager.Configuration = nlogConfig;
            LogManager.EnableLogging();

            LoggingServices.DefaultBackend = new NLogLoggingBackend();
#endif

            Application.Run(new MainForm());
        }

        internal static string Folder {get; private set;}
    }
}
