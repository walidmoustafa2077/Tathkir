using Microsoft.Win32;

namespace Tathkīr_WPF.Managers
{
    public static class AppLifecycleManager
    {
        const string RegistryName = "Tathkir";

        /// <summary>
        /// Check if the application is already running, and if so, exit the current instance.
        /// </summary>
        public static void AlreadyRunning()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var runningProcesses = System.Diagnostics.Process.GetProcessesByName(currentProcess.ProcessName);
            if (runningProcesses.Length > 1)
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Add the application to Windows startup if it is not already registered.
        /// </summary>
        public static void AddToStartup()
        {
            string? appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;

            if (string.IsNullOrWhiteSpace(appPath))
                return;

            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                // Check If Already Registered
                var currentValue = key?.GetValue(RegistryName)?.ToString();
                if (currentValue == null || !string.Equals(currentValue, $"\"{appPath}\"", StringComparison.OrdinalIgnoreCase))
                {
                    key?.SetValue(RegistryName, $"\"{appPath}\"");
                }
            }
        }

        /// <summary>
        /// Remove the application from Windows startup.
        /// </summary>
        public static void RemoveFromStartup()
        {

            using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                key?.DeleteValue(RegistryName, false);
            }
        }
    }
}
