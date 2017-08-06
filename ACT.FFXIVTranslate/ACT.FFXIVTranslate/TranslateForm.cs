using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACT.FFXIVTranslate.translate;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    public partial class TranslateForm : Form
    {
        private MainController _controller;

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

            Move += OnMove;
            SizeChanged += OnSizeChanged;
        }

        private void ControllerOnOpacityChanged(bool fromView, double value)
        {
            if (fromView)
            {
                return;
            }

            Opacity = value;
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
            Win32APIUtils.SetWS_EX_TRANSPARENT(Handle, clickthrough);
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
                System.Diagnostics.Process.Start(link);
            }
        }
    }
}
