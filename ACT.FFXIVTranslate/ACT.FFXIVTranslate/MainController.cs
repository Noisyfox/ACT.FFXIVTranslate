using System.Drawing;
using ACT.FFXIVTranslate.translate;

namespace ACT.FFXIVTranslate
{
    public class MainController
    {
        public delegate void OnOverlayMovedDelegate(bool fromView, int x, int y);

        public event OnOverlayMovedDelegate OverlayMoved;

        public void NotifyOverlayMoved(bool fromView, int x, int y)
        {
            OverlayMoved?.Invoke(fromView, x, y);
        }

        public delegate void OnOverlayResizedDelegate(bool fromView, int w, int h);

        public event OnOverlayResizedDelegate OverlayResized;

        public void NotifyOverlayResized(bool fromView, int w, int h)
        {
            OverlayResized?.Invoke(fromView, w, h);
        }

        public delegate void OnOpacityChangedDelegate(bool fromView, double value);

        public event OnOpacityChangedDelegate OpacityChanged;

        public void NotifyOpacityChanged(bool fromView, double value)
        {
            OpacityChanged?.Invoke(fromView, value);
        }

        public delegate void OnClickthroughChangedDelegate(bool fromView, bool clickthrough);

        public event OnClickthroughChangedDelegate ClickthroughChanged;

        public void NotifyClickthroughChanged(bool fromView, bool clickthrough)
        {
            ClickthroughChanged?.Invoke(fromView, clickthrough);
        }

        public delegate void OnOverlayContentUpdatedDelegate(bool fromView, string content);

        public event OnOverlayContentUpdatedDelegate OverlayContentUpdated;

        public void NotifyOverlayContentUpdated(bool fromView, string content)
        {
            OverlayContentUpdated?.Invoke(fromView, content);
        }

        public delegate void OnProviderChangedDelegate(
            bool fromView, string provider, string apiKey, string langFrom, string langTo);

        public event OnProviderChangedDelegate TranslateProviderChanged;

        public void NotifyTranslateProviderChanged(bool fromView, string provider, string apiKey, string langFrom,
            string langTo)
        {
            TranslateProviderChanged?.Invoke(fromView, provider, apiKey, langFrom, langTo);
        }

        public delegate void OnChannelFilterChangedDelegate(bool fromView, EventCode code, bool show);

        public event OnChannelFilterChangedDelegate ChannelFilterChanged;

        public void NotifyChannelFilterChanged(bool fromView, EventCode code, bool show)
        {
            ChannelFilterChanged?.Invoke(fromView, code, show);
        }

        public delegate void OnLanguageChangedDelegate(bool fromView, string lang);

        public event OnLanguageChangedDelegate LanguageChanged;

        public void NoitfyLanguageChanged(bool fromView, string lang)
        {
            LanguageChanged?.Invoke(fromView, lang);
        }

        public delegate void OnLogMessageAppendDelegate(bool fromView, string log);

        public event OnLogMessageAppendDelegate LogMessageAppend;

        public void NotifyLogMessageAppend(bool fromView, string log)
        {
            LogMessageAppend?.Invoke(fromView, log);
        }

        public delegate void OnOverlayFontChangedDelegate(bool fromView, Font font);

        public event OnOverlayFontChangedDelegate OverlayFontChanged;

        public void NotifyOverlayFontChanged(bool fromView, Font font)
        {
            OverlayFontChanged?.Invoke(fromView, font);
        }

        public delegate void OnLegalInfoChangedDelegate(bool fromView, ProviderLegalInfo info);

        public event OnLegalInfoChangedDelegate LegalInfoChanged;

        public void NotifyLegalInfoChanged(bool fromView, ProviderLegalInfo info)
        {
            LegalInfoChanged?.Invoke(fromView, info);
        }
    }
}
