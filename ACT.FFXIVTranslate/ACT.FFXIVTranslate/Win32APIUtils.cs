using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ACT.FFXIVTranslate
{
    internal static class Win32APIUtils
    {
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;

        const int WS_EX_TRANSPARENT = 0x00000020;
        const int WS_EX_NOACTIVATE = 0x08000000;
        const int GWL_EXSTYLE = (-20);

        static readonly IntPtr HWND_TOP = new IntPtr(0);
        const int SWP_NOMOVE = 2;

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public static void DragMove(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        public static void SetWS_EX_TRANSPARENT(IntPtr handle, bool value)
        {
            int origStyle = GetWindowLong(handle, GWL_EXSTYLE);

            int style;
            if (value)
                style = origStyle | WS_EX_TRANSPARENT;
            else
                style = origStyle & ~WS_EX_TRANSPARENT;

            SetWindowLong(handle, GWL_EXSTYLE, style);
        }

        public static void SetWS_EX_NOACTIVATE(IntPtr handle, bool value)
        {
            int origStyle = GetWindowLong(handle, GWL_EXSTYLE);

            int style;
            if (value)
                style = origStyle | WS_EX_NOACTIVATE;
            else
                style = origStyle & ~WS_EX_NOACTIVATE;

            SetWindowLong(handle, GWL_EXSTYLE, style);
        }

        public static void SetWindowSize(IntPtr handle, int w, int h)
        {
            SetWindowPos(handle, HWND_TOP, 0, 0, w, h, SWP_NOMOVE);
        }

        public static string GetForgegroundProcessPath()
        {
            var hWndFg = GetForegroundWindow();
            if (hWndFg == IntPtr.Zero)
            {
                return null;
            }
            return GetProcessPathByWindow(hWndFg);
        }

        public static string GetProcessPathByWindow(IntPtr hWnd)
        {
            GetWindowThreadProcessId(hWnd, out var pid);
            if (pid == 0)
            {
                return null;
            }
            return Process.GetProcessById((int)pid).MainModule.FileName;
        }
    }
}
