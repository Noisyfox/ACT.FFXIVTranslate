using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using ACT.FFXIVTranslate.translate;
using FontFamily = System.Windows.Media.FontFamily;

namespace ACT.FFXIVTranslate
{
    /// <summary>
    /// TranslateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TranslateWindow : Window, PluginComponent
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
        }

        private void ControllerOnOverlayContentUpdated(bool fromView, string content)
        {
            RichTextBoxContent.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
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
            }));
        }

        private void ControllerOnOverlayFontChanged(bool fromView, Font font)
        {
            var tf = NewTypeFaceFromFont(font);
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

        private void ControllerOnActivatedProcessPathChanged(bool fromView, string path)
        {
            _activatedExePath = path;
            CheckVisibility();
        }

        private static Typeface NewTypeFaceFromFont(Font f)
        {
            var ff = new FontFamily(f.Name);

            var typeface = new Typeface(ff,
                f.Style.HasFlag(System.Drawing.FontStyle.Italic) ? FontStyles.Italic : FontStyles.Normal,
                f.Style.HasFlag(System.Drawing.FontStyle.Bold) ? FontWeights.Bold : FontWeights.Normal,
                FontStretches.Normal);

            return typeface;
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

        private void ApplyVisibility(bool visibility)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
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
            }));
        }
    }
}
