using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Windows.Threading;
using ACT.FFXIVTranslate.translate;
using ACT.FoxCommon;

namespace ACT.FFXIVTranslate
{
    /// <summary>
    /// TranslateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TranslateWindow : Window, IPluginComponent
    {
        private MainController _controller;

        private IntPtr _handle;

        private bool _isClosed = false;

        private bool _showOverlay = false;
        private bool _autoHide = false;
        private string _activatedExePath = null;

        public TranslateWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _handle = new WindowInteropHelper(this).Handle;
            _isClosed = false;
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
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

            _isClosed = true;
        }

        private void ThumbResize_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var height = Height - e.VerticalChange;
            var width = Width + e.HorizontalChange;

            height = height.Clamp(MinHeight, MaxHeight);
            width = width.Clamp(MinWidth, MaxWidth);

            var dHeight = Height - height;

            Height = height;
            Width = width;

            Top += dHeight;
            _controller.NotifyOverlayMoved(true, (int)Left, (int)Top);
            _controller.NotifyOverlayResized(true, (int)width, (int)height);
        }

        private void ThumbMove_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Left += e.HorizontalChange;
            Top += e.VerticalChange;
            _controller.NotifyOverlayMoved(true, (int)Left, (int)Top);
        }

        private void ControllerOnOverlayMoved(bool fromView, int x, int y)
        {
            if (fromView)
            {
                return;
            }

            Top = y;
            Left = x;
        }

        private void ControllerOnOverlayResized(bool fromView, int w, int h)
        {
            if (fromView)
            {
                return;
            }

            Width = w;
            Height = h;
        }

        private void ControllerOnOpacityChanged(bool fromView, double value)
        {
            if (fromView)
            {
                return;
            }

            Background.Opacity = value;
        }

        private void ControllerOnClickthroughChanged(bool fromView, bool clickthrough)
        {
            if (fromView)
            {
                return;
            }
            Win32APIUtils.SetWS_EX_TRANSPARENT(_handle, clickthrough);

            var v = clickthrough ? Visibility.Collapsed : Visibility.Visible;
            ThumbResize.Visibility = v;
            ThumbMove.Visibility = v;
        }

        private void ControllerOnOverlayContentUpdated(bool fromView, string content)
        {
            if (RichTextBoxContent.Dispatcher.CheckAccess())
            {
                if (_isClosed)
                {
                    return;
                }
                var textRange = new TextRange(RichTextBoxContent.Document.ContentEnd,
                    RichTextBoxContent.Document.ContentEnd);
                textRange.Load(new MemoryStream(Encoding.UTF8.GetBytes(content)), DataFormats.Rtf);
                textRange.ApplyPropertyValue(TextElement.FontSizeProperty, RichTextBoxContent.FontSize);

                RichTextBoxContent.ScrollToEnd();
            }
            else
            {
                RichTextBoxContent.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
                {
                    ControllerOnOverlayContentUpdated(fromView, content);
                }));
            }
        }

        private void ControllerOnOverlayFontChanged(bool fromView, Font font)
        {
            var tf = Utils.NewTypeFaceFromFont(font);
            RichTextBoxContent.FontFamily = tf.FontFamily;
            RichTextBoxContent.FontStyle = tf.Style;
            RichTextBoxContent.FontWeight = tf.Weight;
            RichTextBoxContent.FontSize = font.Size * 96.0 / 72.0;
            var range = new TextRange(RichTextBoxContent.Document.ContentStart, RichTextBoxContent.Document.ContentEnd);
            range.ApplyPropertyValue(TextElement.FontSizeProperty, font.Size * 96.0 / 72.0);
        }

        private void ControllerOnLegalInfoChanged(bool fromView, ProviderLegalInfo info)
        {
            var label = info?.LabelResult;
            if (label != null)
            {
                LabelCopyRight.Visibility = Visibility.Visible;
                if (info.LabelResultLink != null)
                {
                    var hl = new Hyperlink(new Run(label))
                    {
                        Foreground = LabelCopyRight.Foreground,
                        NavigateUri = new Uri(info.LabelResultLink)
                    };
                    hl.RequestNavigate += RequestNavigate;
                    LabelCopyRight.Content = hl;
                }
                else
                {
                    LabelCopyRight.Content = label;
                }
            }
            else
            {
                LabelCopyRight.Visibility = Visibility.Collapsed;
            }
        }

        private static void RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
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

        private void ControllerOnActivatedProcessPathChanged(bool fromView, string path, uint pid)
        {
            _activatedExePath = path;
            CheckVisibility();
        }

        private void CheckVisibility()
        {
            if (Dispatcher.CheckAccess())
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
                        if (Utils.IsGameExePath(_activatedExePath) || Utils.IsActExePath(_activatedExePath))
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
            else
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(CheckVisibility));
            }
        }

        private void ApplyVisibility(bool visibility)
        {
            if (_isClosed)
            {
                return;
            }

            var t = visibility ? Visibility.Visible : Visibility.Hidden;
            if (Visibility != t)
            {
                Visibility = t;
            }
        }
    }
}
