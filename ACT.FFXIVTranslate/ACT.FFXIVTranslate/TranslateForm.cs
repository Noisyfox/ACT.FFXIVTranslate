using System;
using System.Diagnostics;
using System.Drawing;
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
        private string _activatedExePath = null;

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
            _controller.ActivatedProcessPathChanged += ControllerOnActivatedProcessPathChanged;

            Move += OnMove;
            SizeChanged += OnSizeChanged;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
            
        }

        private void ControllerOnOverlayAutoHideChanged(bool fromView, bool autoHide)
        {
            _autoHide = autoHide;
            CheckVisibility();
        }

        private void ControllerOnShowOverlayChanged(bool fromView, bool showOverlay)
        {
            _showOverlay = showOverlay;
            CheckVisibility();
        }

        private void ControllerOnActivatedProcessPathChanged(bool fromView, string path)
        {
            _activatedExePath = path;
            CheckVisibility();
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

        private void TranslateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _controller.OverlayMoved -= ControllerOnOverlayMoved;
            _controller.OverlayResized -= ControllerOnOverlayResized;
            _controller.OpacityChanged -= ControllerOnOpacityChanged;
            _controller.ClickthroughChanged -= ControllerOnClickthroughChanged;
            _controller.OverlayContentUpdated -= ControllerOnOverlayContentUpdated;
            _controller.OverlayFontChanged -= ControllerOnOverlayFontChanged;
            _controller.LegalInfoChanged -= ControllerOnLegalInfoChanged;
            _controller.OverlayAutoHideChanged -= ControllerOnOverlayAutoHideChanged;
            _controller.ShowOverlayChanged -= ControllerOnShowOverlayChanged;
            _controller.ActivatedProcessPathChanged -= ControllerOnActivatedProcessPathChanged;
        }

        private void CheckVisibility()
        {
            var targetVisible = false;
            if (_showOverlay && _autoHide)
            {
                if (_activatedExePath == null)
                {
                    targetVisible = true;
                }
                else
                {
                    if (Utils.IsGameExePath(_activatedExePath) || Utils.IsActExePaht(_activatedExePath))
                    {
                        targetVisible = true;
                    }
                }
            }
            else
            {
                targetVisible = _showOverlay;
            }

            ApplyVisibility(targetVisible);
        }

        private delegate void ApplyVisibilityCallback(bool visibility);

        private void ApplyVisibility(bool visibility)
        {
            if (InvokeRequired)
            {
                ApplyVisibilityCallback applyVisibilityCallback = ApplyVisibility;
                Invoke(applyVisibilityCallback, visibility);
            }
            else
            {

                if (Visible != visibility)
                {
                    Visible = visibility;
                }
            }
        }
    }
}
