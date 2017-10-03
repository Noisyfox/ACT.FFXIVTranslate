using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ACT.FFXIVTranslate
{
    class WindowsMessagePump : IPluginComponent, IDisposable
    {
        private FFXIVTranslatePlugin _plugin;
        private MainController _controller;

        private MessageOnlyWindow _window;
        private WinEventDelegate _hookPtrDele;
        private IntPtr _hookPtrForeground = IntPtr.Zero;
        private string _lastActivatedProcessPath = null;

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _plugin = plugin;
            _controller = plugin.Controller;

            _window = new MessageOnlyWindow(plugin);

            _controller.ChannelFilterChanged += ControllerOnChannelFilterChanged;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
            _hookPtrDele = WinEventProc;
            _hookPtrForeground = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_MINIMIZEEND, IntPtr.Zero, _hookPtrDele, 0, 0, WINEVENT_OUTOFCONTEXT);
            _hookPtrDele(IntPtr.Zero, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, 0, 0, 0, 0);
        }

        public void Dispose()
        {
            _controller.ChannelFilterChanged -= ControllerOnChannelFilterChanged;

            UnhookWinEvent(_hookPtrForeground);
            _hookPtrForeground = IntPtr.Zero;
            _hookPtrDele = null;
            _window?.Dispose();
            _window = null;
            _plugin = null;
        }

        private void ControllerOnChannelFilterChanged(bool fromView, EventCode code, bool show)
        {
            if (code != EventCode.Clipboard)
            {
                return;
            }

            _window.EnableClipboardMonitor(show);
        }

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int UnhookWinEvent(IntPtr hWinEventHook);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 0x3;
        private const uint EVENT_SYSTEM_MENUPOPUPSTART = 0x6;
        private const uint EVENT_SYSTEM_MENUPOPUPEND = 0x7;
        private const uint EVENT_SYSTEM_MINIMIZEEND = 0x17;

//        [DllImport("user32.dll")]
//        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
//
//        private string GetActiveWindowTitle()
//        {
//            const int nChars = 256;
//            IntPtr handle = IntPtr.Zero;
//            StringBuilder Buff = new StringBuilder(nChars);
//            handle = Win32APIUtils.GetForegroundWindow();
//
//            if (GetWindowText(handle, Buff, nChars) > 0)
//            {
//                return Buff.ToString();
//            }
//            return null;
//        }

        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (eventType != EVENT_SYSTEM_FOREGROUND && eventType != EVENT_SYSTEM_MENUPOPUPSTART &&
                eventType != EVENT_SYSTEM_MENUPOPUPEND && eventType != EVENT_SYSTEM_MINIMIZEEND)
            {
                return;
            }
            string path = null;
            if (hwnd != IntPtr.Zero)
            {
                path = Win32APIUtils.GetProcessPathByWindow(hwnd);
            }
            if (path == null)
            {
                path = Win32APIUtils.GetForgegroundProcessPath();
            }
            if (path == null)
            {
                return;
            }

            if (_lastActivatedProcessPath != path)
            {
                _lastActivatedProcessPath = path;
                _controller.NotifyActivatedProcessPathChanged(false, path);
            }
            //            Log.Text += GetActiveWindowTitle() + "\r\n";
//            _controller.NotifyLogMessageAppend(false, path + "\r\n");

        }
    }

    sealed class MessageOnlyWindow : NativeWindow, IDisposable
    {

        // Only works on VISTA+, but since we use .net 4.5, so it's guaranteed to work 
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private const int WM_CLIPBOARDUPDATE = 0x031D;

        private const int WM_USER = 0x0400;
        private const int WM_USER_POSTCLIPBOARDUPDATE = WM_USER + 1;

        private readonly FFXIVTranslatePlugin _plugin;

        private bool _clipboardEnabled = false;

        public MessageOnlyWindow(FFXIVTranslatePlugin plugin)
        {
            _plugin = plugin;

            var cp = new CreateParams();
            var HWND_MESSAGE = new IntPtr(-3);
            cp.Parent = HWND_MESSAGE;
            CreateHandle(cp);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_CLIPBOARDUPDATE:
                    PostMessage(Handle, WM_USER_POSTCLIPBOARDUPDATE, IntPtr.Zero, IntPtr.Zero);
                    m.Result = IntPtr.Zero;
                    break;
                case WM_USER_POSTCLIPBOARDUPDATE:
                    var text = Clipboard.GetText(TextDataFormat.UnicodeText);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        _plugin.Controller.NotifyClipboardContentChanged(false, text);
//                        _plugin.Controller.NotifyLogMessageAppend(false, $"Clipboard: {text}\n");
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public void Dispose()
        {
            EnableClipboardMonitor(false);
            DestroyHandle();
        }

        public void EnableClipboardMonitor(bool enable)
        {
            if (_clipboardEnabled == enable)
            {
                return;
            }

            if (enable)
            {
                if (!AddClipboardFormatListener(Handle))
                {
                    _plugin.Controller.NotifyLogMessageAppend(false, "AddClipboardFormatListener() failed! \n");
                    return;
                }
            }
            else
            {
                if (!RemoveClipboardFormatListener(Handle))
                {
                    _plugin.Controller.NotifyLogMessageAppend(false, "RemoveClipboardFormatListener() failed! \n");
                    return;
                }
            }

            _clipboardEnabled = enable;
        }
    }
}
