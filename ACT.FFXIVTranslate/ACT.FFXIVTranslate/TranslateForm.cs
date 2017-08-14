using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ACT.FFXIVTranslate.translate;

namespace ACT.FFXIVTranslate
{
    public partial class TranslateForm : Form, PluginComponent
    {
        private MainController _controller;

        private double _targetOpacity = 1;
        private bool _targetClickthrough = false;

        private bool _showOverlay = false;
        private bool _autoHide = false;

        public TranslateForm()
        {
            InitializeComponent();

            tableLayoutPanelContent.Size = Size;
            tableLayoutPanelContent.Location = new Point(0, 0);
        }

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _controller = plugin.Controller;

            _controller.OverlayMoved += ControllerOnOverlayMoved;
            _controller.OverlayResized += ControllerOnOverlayResized;
            _controller.OpacityChanged += ControllerOnOpacityChanged;
            _controller.ClickthroughChanged += ControllerOnClickthroughChanged;
            _controller.OverlayContentUpdated += ControllerOnOverlayContentUpdated;
            _controller.OverlayFontChanged += ControllerOnOverlayFontChanged;
            _controller.LegalInfoChanged += ControllerOnLegalInfoChanged;
            _controller.OverlayAutoHideChanged += ControllerOnOverlayAutoHideChanged;
            _controller.ShowOverlayChanged += ControllerOnShowOverlayChanged;

            Move += OnMove;
            SizeChanged += OnSizeChanged;

            timerAutoHide.Enabled = true;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
            
        }

        private void ControllerOnOverlayAutoHideChanged(bool fromView, bool autoHide)
        {
            _autoHide = autoHide;
        }

        private void ControllerOnShowOverlayChanged(bool fromView, bool showOverlay)
        {
            _showOverlay = showOverlay;
        }

        private void ControllerOnOpacityChanged(bool fromView, double value)
        {
            if (fromView)
            {
                return;
            }

            _targetOpacity = value;
            ApplyOpacityAndClickthrough();
        }

        private void ControllerOnOverlayMoved(bool fromView, int x, int y)
        {
            if (fromView)
            {
                return;
            }

            Move -= OnMove;

            Location = new Point(x, y);

            Move += OnMove;
        }

        private void ControllerOnOverlayResized(bool fromView, int w, int h)
        {
            if (fromView)
            {
                return;
            }

            SizeChanged -= OnSizeChanged;

            Size = new Size(w, h);

            SizeChanged += OnSizeChanged;
        }

        private void ControllerOnClickthroughChanged(bool fromView, bool clickthrough)
        {
            if (fromView)
            {
                return;
            }
            _targetClickthrough = clickthrough;
            ApplyOpacityAndClickthrough();
        }

        private void ApplyOpacityAndClickthrough()
        {
            var op = _targetOpacity;
            if (_targetClickthrough && op >= 1)
            {
                op = 0.99;
            }
            Opacity = op;
            Win32APIUtils.SetWS_EX_TRANSPARENT(Handle, _targetClickthrough);
        }

        private void ControllerOnOverlayContentUpdated(bool fromView, string content)
        {
            if (fromView)
            {
                return;
            }

            ThreadInvokesExt.RichTextBoxAppendRtf(this, richTextBoxContent, content);
        }

        private void ControllerOnOverlayFontChanged(bool fromView, Font font)
        {
            richTextBoxContent.Font = font;
        }

        private void ControllerOnLegalInfoChanged(bool fromView, ProviderLegalInfo info)
        {
            var label = info?.LabelResult;
            if (label != null)
            {
                linkLabelLegalInfo.Visible = true;
                linkLabelLegalInfo.Text = label;
                linkLabelLegalInfo.Tag = info.LabelResultLink;
            }
            else
            {
                linkLabelLegalInfo.Visible = false;
                linkLabelLegalInfo.Tag = null;
            }
        }

        private void OnMove(object sender, EventArgs e)
        {
            _controller.NotifyOverlayMoved(true, Left, Top);
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            _controller.NotifyOverlayResized(true, Size.Width, Size.Height);
        }

        private void TranslateForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Win32APIUtils.DragMove(Handle);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabelLegalInfo.Tag is string link)
            {
                Process.Start(link);
            }
        }

        private void timerAutoHide_Tick(object sender, EventArgs e)
        {
            var targetVisible = false;
            try
            {
                if (_showOverlay && _autoHide)
                {
                    var hWndFg = Win32APIUtils.GetForegroundWindow();
                    if (hWndFg == IntPtr.Zero)
                    {
                        return;
                    }
                    Win32APIUtils.GetWindowThreadProcessId(hWndFg, out uint pid);
                    var exePath = Process.GetProcessById((int) pid).MainModule.FileName;

                    if (Path.GetFileName(exePath) == "ffxiv.exe" ||
                        Path.GetFileName(exePath) == "ffxiv_dx11.exe" ||
                        exePath == Process.GetCurrentProcess().MainModule.FileName)
                    {
                        targetVisible = true;
                    }
                }
                else
                {
                    targetVisible = _showOverlay;
                }
            }
            catch (Exception ex)
            {
                _controller.NotifyLogMessageAppend(true, ex.ToString());
                targetVisible = true; // Force show overlay if something goes wrong.
            }

            if (Visible != targetVisible)
            {
                Visible = targetVisible;
            }
        }

        private void TranslateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerAutoHide.Enabled = false;
        }
    }
}
