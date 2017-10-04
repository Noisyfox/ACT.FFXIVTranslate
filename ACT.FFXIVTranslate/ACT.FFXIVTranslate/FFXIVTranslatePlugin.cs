﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using ACT.FFXIVTranslate.localization;
using ACT.FFXIVTranslate.translate;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    public class FFXIVTranslatePlugin : IActPluginV1
    {
        private bool _settingsLoaded = false;

        internal MainController Controller { get; private set; }
        public SettingsHolder Settings { get; private set; }
        public TabPage ParentTabPage { get; private set; }
        public Label StatusLabel { get; private set; }
        public FFXIVTranslateTabControl SettingsTab { get; private set; }
        public TranslateWindow OverlayWPF { get; private set; }
        internal UpdateChecker UpdateChecker { get; } = new UpdateChecker();
        internal TranslateService TranslateService { get; } = new TranslateService();
        private readonly WindowsMessagePump _windowsMessagePump = new WindowsMessagePump();
        private ShortkeyManager _shortkeyManager;

        private readonly LogReadThread _workingThread = new LogReadThread();
        private readonly ConcurrentDictionary<EventCode, ChannelSettings> _channelSettings = new ConcurrentDictionary<EventCode, ChannelSettings>();

        private bool _isGameActivated = false;

        public FFXIVTranslatePlugin()
        {
            _workingThread.OnLogLineRead += OnLogLineRead;
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            _settingsLoaded = false;
            ParentTabPage = pluginScreenSpace;
            StatusLabel = pluginStatusText;
            ParentTabPage.Text = "Chatting Translate";

            try
            {
                Controller = new MainController();
                Settings = new SettingsHolder();

                Settings.AttachToAct(this);

                OverlayWPF = new TranslateWindow();
                ElementHost.EnableModelessKeyboardInterop(OverlayWPF);
                OverlayWPF.AttachToAct(this);
                OverlayWPF.Show();

                SettingsTab = new FFXIVTranslateTabControl();
                SettingsTab.AttachToAct(this);

                Controller.ChannelFilterChanged += ControllerOnChannelFilterChanged;
                Controller.ChannelColorChanged += ControllerOnChannelColorChanged;
                Controller.ChannelLabelChanged += ControllerOnChannelLabelChanged;
                Controller.ActivatedProcessPathChanged += ControllerOnActivatedProcessPathChanged;
                Controller.ClipboardContentChanged += ControllerOnClipboardContentChanged;

                ProxyFactory.Instance.AttachToAct(this);

                TranslateService.AttachToAct(this);
                _windowsMessagePump.AttachToAct(this);
                UpdateChecker.AttachToAct(this);

                _shortkeyManager = new ShortkeyManager();
                _shortkeyManager.AttachToAct(this);

                Settings.PostAttachToAct(this);
                OverlayWPF.PostAttachToAct(this);
                SettingsTab.PostAttachToAct(this);
                ProxyFactory.Instance.PostAttachToAct(this);
                TranslateService.PostAttachToAct(this);
                _windowsMessagePump.PostAttachToAct(this);
                UpdateChecker.PostAttachToAct(this);
                _shortkeyManager.PostAttachToAct(this);

                Settings.Load();
                _settingsLoaded = true;

                DoLocalization();

                ActGlobals.oFormActMain.LogFileChanged += OFormActMainOnLogFileChanged;

                _workingThread.StartWorkingThread(ActGlobals.oFormActMain.LogFilePath);

                Settings.NotifySettingsLoaded();

                StatusLabel.Text = "Init Success. >w<";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Init Failed: " + ex.ToString();
                if (_settingsLoaded)
                {
                    MessageBox.Show($"Init failed!\nCaused by:\n{ex}");
                }
                else
                {
                    MessageBox.Show($"Init failed before settings are loaded. Settings won't be saved until next successfully initialization to prevent settings lost!\nCaused by:\n{ex}");
                }
            }
        }

        private void DoLocalization()
        {
            Controller.NotifyLanguageChanged(false, Settings.Language);

            Localization.ConfigLocalization(Settings.Language);

            ParentTabPage.Text = strings.actPanelTitle;
            SettingsTab.DoLocalization();
        }

        private void ControllerOnChannelFilterChanged(bool fromView, EventCode code, bool show)
        {
            if (!fromView)
            {
                return;
            }

            _channelSettings.AddOrUpdate(code,
                eventCode => new ChannelSettings
                {
                    Show = show
                }, (eventCode, b) =>
                {
                    b.Show = show;
                    return b;
                });
        }

        private void ControllerOnChannelColorChanged(bool fromView, EventCode code, Color color)
        {
            if (!fromView)
            {
                return;
            }

            _channelSettings.AddOrUpdate(code,
                eventCode => new ChannelSettings
                {
                    DisplayColor = color
                }, (eventCode, b) =>
                {
                    b.DisplayColor = color;
                    return b;
                });
        }

        private void ControllerOnChannelLabelChanged(bool fromView, EventCode code, bool show)
        {
            if (!fromView)
            {
                return;
            }

            _channelSettings.AddOrUpdate(code,
                eventCode => new ChannelSettings
                {
                    ShowLabel = show
                }, (eventCode, b) =>
                {
                    b.ShowLabel = show;
                    return b;
                });
        }

        private void ControllerOnActivatedProcessPathChanged(bool fromView, string path)
        {
            _isGameActivated = Utils.IsGameExePath(path);
        }

        private void ControllerOnClipboardContentChanged(bool fromView, string newContent)
        {
            if (!_isGameActivated)
            {
                return;
            }

            // The channel filter has done in WindowsMessagePump.cs by enable / disable
            // clipboard monitoring, which means this function will be called only if
            // the filter for clipboard is set to show, so here we don't need to check 
            // the filter as we do in OnLogLineRead().

            var chat = new ChattingLine
            {
                RawEventCode = (byte)EventCode.Clipboard,
                EventCode = EventCode.Clipboard,
                RawSender = strings.checkBoxChannelFilterClipboard,
                RawContent = newContent,
                Timestamp = DateTime.Now,
            };

            TranslateService.SubmitNewLine(chat);
        }

        private void OFormActMainOnLogFileChanged(bool isImport, string newLogFileName)
        {
            if (isImport)
            {
                return;
            }

            // Read raw logs by ourself
            _workingThread.StartWorkingThread(newLogFileName);
        }

        public void DeInitPlugin()
        {
            _shortkeyManager?.Dispose();
            _shortkeyManager = null;

            _windowsMessagePump.Dispose();
            
            OverlayWPF?.Close();

            ActGlobals.oFormActMain.LogFileChanged -= OFormActMainOnLogFileChanged;
            TranslateService.Stop();
            _workingThread.StopWorkingThread();
            UpdateChecker.Stop();

            if (_settingsLoaded)
            {
                Settings?.Save();
            }

            StatusLabel.Text = "Exited. Bye~";
        }

        private void OnLogLineRead(string line)
        {
            // Parse log line

            // The legal talking log line has the following format:
            // 00|[timestamp]|[event code]|[name or to?]|[content]<|[MD5]>
            // eg:
            // 00|2017-07-13T22:26:50.0000000+08:00|0010|Pinoko Fox|哈呀|f94872989411d9f173c9e9b36e29c9d1
            // or
            // 00|2017-07-13T22:26:50.0000000+08:00|0010|Pinoko Fox|哈呀

            var data = line.Split('|');
            if (data.Length < 5)
            {
                return;
            }

            if (!"00".Equals(data[0]))
            {
                return;
            }

            if (string.IsNullOrEmpty(data[3]))
            {
                return;
            }

            if (!int.TryParse(data[2], NumberStyles.HexNumber, null, out var eventCode))
            {
                return;
            }

            var knownCode = Enum.IsDefined(typeof(EventCode), (byte) (eventCode & byte.MaxValue));

            var name = data[3];
            var content = data[4];

            Debug.WriteLine(line);
            Debug.WriteLine($"eventCode={data[2]}, known={knownCode}, {name} says: {content}");
//            Controller.NotifyOverlayContentUpdated(false,
//                $"eventCode={data[2]}, known={knownCode}, {name} says: {content}\n");

            if (!knownCode)
            {
                return;
            }

            var eventCodeKnown =(EventCode) (byte) (eventCode & byte.MaxValue);
            // Filter by event code
            if (!GetChannelSettings(eventCodeKnown).Show)
            {
                return;
            }

            var chat = new ChattingLine
            {
                RawEventCode = (byte) (eventCode & byte.MaxValue),
                EventCode = eventCodeKnown,
                RawSender = name,
                RawContent = content,
                Timestamp = DateTime.Parse(data[1]),
            };

            TranslateService.SubmitNewLine(chat);
        }

        public ChannelSettings GetChannelSettings(EventCode eventCode)
        {
            if (eventCode == EventCode.TellTo)
            {
                eventCode = EventCode.TellFrom;
            }
            return _channelSettings.GetOrAdd(eventCode, e => new ChannelSettings());
        }
    }

    public interface IPluginComponent
    {
        void AttachToAct(FFXIVTranslatePlugin plugin);
        void PostAttachToAct(FFXIVTranslatePlugin plugin);
    }

    /// <summary>
    /// All known event codes.
    /// The codes not treated as talk are commented.
    /// </summary>
    public enum EventCode : byte
    {
        Say = 0x0a,
        Shout = 0x0b,
        TellTo = 0x0c,
        TellFrom = 0x0d,
        Party = 0x0e,
        Alliance = 0x0f,
        LS1 = 0x10,
        LS2 = 0x11,
        LS3 = 0x12,
        LS4 = 0x13,
        LS5 = 0x14,
        LS6 = 0x15,
        LS7 = 0x16,
        LS8 = 0x17,
        FreeCompany = 0x18,
        Novice = 0x1b,
//            Emote = 0x1d,
        Yell = 0x1e,
        NPC = 0x3d,

        Clipboard = 0xf0,
    }

    public class ChattingLine
    {
        public byte RawEventCode;
        public string RawSender;
        public string RawContent;

        public DateTime Timestamp;

        public EventCode EventCode;
        public string CleanedContent;
        public string FormattedContent;
        public string TranslatedContent;
    }

    public class ChannelSettings
    {
        public bool Show { get; set; } = true;
        public Color DisplayColor { get; set; } = Color.White;
        public bool ShowLabel { get; set; } = false;
    }

    internal enum Shortcut
    {
        HideOverlay
    }
}
