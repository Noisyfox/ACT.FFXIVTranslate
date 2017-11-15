using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ACT.FoxCommon;

namespace ACT.FFXIVTranslate
{
    class WindowsMessagePump : WindowsMessagePumpBase<MainController, FFXIVTranslatePlugin>
    {

        private MessageOnlyWindow _window;

        public override void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            base.AttachToAct(plugin);

            _window = new MessageOnlyWindow(plugin);

            Controller.ChannelFilterChanged += ControllerOnChannelFilterChanged;
        }

        public override void Dispose()
        {
            Controller.ChannelFilterChanged -= ControllerOnChannelFilterChanged;
            
            _window?.Dispose();
            _window = null;

            base.Dispose();
        }

        private void ControllerOnChannelFilterChanged(bool fromView, EventCode code, bool show)
        {
            if (code != EventCode.Clipboard)
            {
                return;
            }

            _window.EnableClipboardMonitor(show);
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
