using System;
using System.Drawing;
using ACT.FFXIVTranslate.translate;

namespace ACT.FFXIVTranslate
{
    internal class MainController
    {
        public event Action SettingsLoaded;

        public void NotifySettingsLoaded()
        {
            SettingsLoaded?.Invoke();
        }

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

        public delegate void OnChannelLabelChangedDelegate(bool fromView, EventCode code, bool show);

        public event OnChannelLabelChangedDelegate ChannelLabelChanged;

        public void NotifyChannelLabelChanged(bool fromView, EventCode code, bool show)
        {
            ChannelLabelChanged?.Invoke(fromView, code, show);
        }

        public delegate void OnChannelColorChangedDelegate(bool fromView, EventCode code, Color color);

        public event OnChannelColorChangedDelegate ChannelColorChanged;

        public void NotifyChannelColorChanged(bool fromView, EventCode code, Color color)
        {
            ChannelColorChanged?.Invoke(fromView, code, color);
        }

        public delegate void OnLanguageChangedDelegate(bool fromView, string lang);

        public event OnLanguageChangedDelegate LanguageChanged;

        public void NotifyLanguageChanged(bool fromView, string lang)
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

        public delegate void OnShowOverlayChangedDelegate(bool fromView, bool showOverlay);

        public event OnShowOverlayChangedDelegate ShowOverlayChanged;

        public void NotifyShowOverlayChanged(bool fromView, bool showOverlay)
        {
            ShowOverlayChanged?.Invoke(fromView, showOverlay);
        }

        public delegate void OnOverlayAutoHideChangedDelegate(bool fromView, bool autoHide);

        public event OnOverlayAutoHideChangedDelegate OverlayAutoHideChanged;

        public void NotifyOverlayAutoHideChanged(bool fromView, bool autoHide)
        {
            OverlayAutoHideChanged?.Invoke(fromView, autoHide);
        }

        public delegate void OnAddTimestampChangedDelegate(bool fromView, bool show);

        public event OnAddTimestampChangedDelegate AddTimestampChanged;

        public void NotifyAddTimestampChanged(bool fromView, bool show)
        {
            AddTimestampChanged?.Invoke(fromView, show);
        }

        public delegate void OnTimestampFormatChangedDelegate(bool fromView, bool is24Hour);

        public event OnTimestampFormatChangedDelegate TimestampFormatChanged;

        public void NotifyTimestampFormatChanged(bool fromView, bool is24Hour)
        {
            TimestampFormatChanged?.Invoke(fromView, is24Hour);
        }

        public delegate void OnActivatedProcessPathChangedDelegate(bool fromView, string path);

        public event OnActivatedProcessPathChangedDelegate ActivatedProcessPathChanged;

        public void NotifyActivatedProcessPathChanged(bool fromView, string path)
        {
            ActivatedProcessPathChanged?.Invoke(fromView, path);
        }

        public delegate void OnClipboardContentChangedDelegate(bool fromView, string newContent);

        public event OnClipboardContentChangedDelegate ClipboardContentChanged;

        public void NotifyClipboardContentChanged(bool fromView, string newContent)
        {
            ClipboardContentChanged?.Invoke(fromView, newContent);
        }

        public delegate void OnProxyChangedDelegate(bool fromView, string type, string server, int port,
            string user, string password, string domain);

        public event OnProxyChangedDelegate ProxyChanged;

        public void NotifyProxyChanged(bool fromView, string type, string server, int port,
            string user, string password, string domain)
        {
            ProxyChanged?.Invoke(fromView, type, server, port, user, password, domain);
        }

        public delegate void OnUpdateCheckingStarted(bool fromView);

        public event OnUpdateCheckingStarted UpdateCheckingStarted;

        public void NotifyUpdateCheckingStarted(bool fromView)
        {
            UpdateCheckingStarted?.Invoke(fromView);
        }

        public delegate void OnNewVersionIgnored(bool fromView, string ignoredVersion);

        public event OnNewVersionIgnored NewVersionIgnored;

        public void NotifyNewVersionIgnored(bool fromView, string ignoredVersion)
        {
            NewVersionIgnored?.Invoke(fromView, ignoredVersion);
        }

        public delegate void OnVersionChecked(bool fromView, UpdateChecker.VersionInfo versionInfo, bool forceNotify);

        public event OnVersionChecked VersionChecked;

        public void NotifyVersionChecked(bool fromView, UpdateChecker.VersionInfo versionInfo, bool forceNotify)
        {
            VersionChecked?.Invoke(fromView, versionInfo, forceNotify);
        }
    }
}
