using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using PostSharp.Patterns.Threading;

namespace ThreatsManager
{
    class ResourcesGuard
    {
        private const int cWarning = 9000;
        private static bool _shown;

        [DllImport("User32")]
        extern public static int GetGuiResources(IntPtr hProcess, int uiFlags);

        public ResourcesGuard()
        {
            WarningThreshold = cWarning;
        }

        public int WarningThreshold { get; set; }

        [Background(IsLongRunning = true)]
        public void StartChecking()
        {
            do
            {
                var current = GetGuiResources(Process.GetCurrentProcess().Handle, 1);
                if (current > WarningThreshold)
                {
                    _shown = true;
                    ShowMessage();
                }

                Thread.Sleep(10000);
            }
            while (!_shown);
        }

        public static void StopChecking()
        {
            _shown = true;
        }

        [Dispatched]
        private void ShowMessage()
        {
            MessageBox.Show(Form.ActiveForm, Properties.Resources.ResourcesGuard_ThresholdReached, 
                "Please restart Threats Manager Studio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
