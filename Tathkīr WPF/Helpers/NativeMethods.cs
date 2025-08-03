using System.Runtime.InteropServices;
using System.Text;
using Tathkīr_WPF.Services;

namespace Tathkīr_WPF.Helpers
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern nint SetWinEventHook(uint eventMin, uint eventMax, nint hmodWinEventProc, ShowDesktopService.WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWinEvent(nint hWinEventHook);

        [DllImport("user32.dll")]
        internal static extern int GetClassName(nint hwnd, StringBuilder name, int count);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(nint hWnd, nint hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetWindowLong(nint hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);
    }

}
