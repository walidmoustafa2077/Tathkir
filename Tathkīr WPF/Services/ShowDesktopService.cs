using System.Text;
using System.Windows;
using Tathkīr_WPF.Helpers;

namespace Tathkīr_WPF.Services
{
    public static class ShowDesktopService
    {
        private const uint WINEVENT_OUTOFCONTEXT = 0u;
        private const uint EVENT_SYSTEM_FOREGROUND = 3u;

        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOACTIVATE = 0x0010;


        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        private const string WORKERW = "WorkerW";
        private const string PROGMAN = "Progman";
        private static IntPtr? _hookIntPtr { get; set; }
        private static WinEventDelegate? _delegate { get; set; }
        private static Window? _window { get; set; }

        public static bool IsHooked { get; private set; } = false;

        public static void AddHook(Window window)
        {
            if (IsHooked)
                return;

            IsHooked = true;

            _delegate = new WinEventDelegate(WinEventHook);

            _hookIntPtr = NativeMethods.SetWinEventHook(
                EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND,
                IntPtr.Zero, _delegate, 0, 0, WINEVENT_OUTOFCONTEXT);

            var helper = new System.Windows.Interop.WindowInteropHelper(window);
            IntPtr hwnd = helper.Handle;

            int exStyle = NativeMethods.GetWindowLong(hwnd, GWL_EXSTYLE);
            exStyle |= WS_EX_TOOLWINDOW;
            NativeMethods.SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);

            _window = window;
        }

        public static void RemoveHook()
        {
            if (!IsHooked || _hookIntPtr == null || _delegate == null || _window == null)
                return;

            IsHooked = false;
            NativeMethods.UnhookWinEvent(_hookIntPtr.Value);
            _delegate = null;
            _hookIntPtr = null;
            _window = null;
        }

        private static string GetWindowClass(IntPtr hwnd)
        {
            StringBuilder sb = new StringBuilder(32);
            NativeMethods.GetClassName(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        private static void WinEventHook(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (_window == null || _hookIntPtr == null || _delegate == null)
                return;

            if (eventType == EVENT_SYSTEM_FOREGROUND)
            {
                string className = GetWindowClass(hwnd);

                // Show Desktop triggers WorkerW or Progman to foreground
                if (string.Equals(className, WORKERW, StringComparison.Ordinal) ||
                    string.Equals(className, PROGMAN, StringComparison.Ordinal))
                {
                    // Send widget to bottom but visible on desktop
                    NativeMethods.SetWindowPos(hwnd, HWND_BOTTOM, 0, 0, 0, 0,
                        SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                }
                else
                {
                    // Stay behind normal windows
                    _window.Topmost = false;
                }
            }
        }

    }
}
