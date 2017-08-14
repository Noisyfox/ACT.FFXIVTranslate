using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ACT.FFXIVTranslate.translate;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    public class FFXIVTranslatePlugin : IActPluginV1
    {

        public MainController Controller { get; private set; }
        public PluginSettings Settings { get; private set; }
        public TabPage ParentTabPage { get; private set; }
        public Label StatusLabel { get; private set; }
        public FFXIVTranslateTabControl SettingsTab { get; private set; }
        public TranslateForm Overlay { get; private set; }
        internal TranslateService TranslateService { get; } = new TranslateService();

        private readonly LogReadThread _workingThread = new LogReadThread();
        private readonly ConcurrentDictionary<EventCode, ChannelSettings> _channelSettings = new ConcurrentDictionary<EventCode, ChannelSettings>();

        public string TranslateProvider { get; set; }

        public string TranslateApiKey { get; set; }

        public string TranslateLangFrom { get; set; }

        public string TranslateLangTo { get; set; }

        public string Language { get; set; }

        public string OverlayFont { get; set; }

        public FFXIVTranslatePlugin()
        {
            _workingThread.OnLogLineRead += OnLogLineRead;
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            ParentTabPage = pluginScreenSpace;
            StatusLabel = pluginStatusText;
            ParentTabPage.Text = "Chatting Translate";

            try
            {
                // Load FFXIV plugin's assembly if needed
                AppDomain.CurrentDomain.AssemblyResolve += (o, e) =>
                {
                    var simpleName = new string(e.Name.TakeWhile(x => x != ',').ToArray());
                    if (simpleName == "FFXIV_ACT_Plugin")
                    {
                        var query =
                            ActGlobals.oFormActMain.ActPlugins.Where(
                                x => x.lblPluginTitle.Text == "FFXIV_ACT_Plugin.dll");
                        var plugin = query.FirstOrDefault();

                        if (plugin != null)
                        {
                            return System.Reflection.Assembly.LoadFrom(plugin.pluginFile.FullName);
                        }
                    }

                    return null;
                };

                Controller = new MainController();

                Settings = new PluginSettings(this);

                Overlay = new TranslateForm();
                Overlay.AttachToAct(this);
                Overlay.Show();

                SettingsTab = new FFXIVTranslateTabControl();
                SettingsTab.AttachToAct(this);

                Controller.ChannelFilterChanged += ControllerOnChannelFilterChanged;
                Controller.ChannelColorChanged += ControllerOnChannelColorChanged;
                Controller.ChannelLabelChanged += ControllerOnChannelLabelChanged;
                Controller.LanguageChanged += ControllerOnLanguageChanged;
                Controller.OverlayFontChanged += ControllerOnOverlayFontChanged;

                Controller.TranslateProviderChanged += ControllerOnTranslateProviderChanged;
                TranslateService.AttachToAct(this);

                Overlay.PostAttachToAct(this);
                SettingsTab.PostAttachToAct(this);
                TranslateService.PostAttachToAct(this);

                Settings.Load();

                DoLocalization();

                ActGlobals.oFormActMain.LogFileChanged += OFormActMainOnLogFileChanged;

                _workingThread.StartWorkingThread(ActGlobals.oFormActMain.LogFilePath);

                try
                {
                    Controller.NotifyOverlayFontChanged(false,
                        (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(OverlayFont));
                }
                catch (Exception)
                {
                    Controller.NotifyOverlayFontChanged(true, new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular));
                }

                Controller.NotifyTranslateProviderChanged(false, TranslateProvider, TranslateApiKey, TranslateLangFrom, TranslateLangTo);

                StatusLabel.Text = "Init Success. >w<";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Init Failed: " + ex.ToString();
            }
        }

        private void DoLocalization()
        {
            Controller.NoitfyLanguageChanged(false, Language);

            localization.Localization.ConfigLocalization(Language);

            ParentTabPage.Text = localization.strings.actPanelTitle;
            SettingsTab.DoLocalization();
        }

        private void ControllerOnLanguageChanged(bool fromView, string lang)
        {
            if (!fromView)
            {
                return;
            }

            Language = lang;
        }

        private void ControllerOnOverlayFontChanged(bool fromView, Font font)
        {
            if (!fromView)
            {
                return;
            }

            OverlayFont = TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(font);
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

        private void ControllerOnTranslateProviderChanged(bool fromView, string provider, string apiKey, string langFrom, string langTo)
        {
            if (!fromView)
            {
                return;
            }

            TranslateProvider = provider;
            TranslateApiKey = apiKey;
            TranslateLangFrom = langFrom;
            TranslateLangTo = langTo;
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
            Overlay?.Close();

            ActGlobals.oFormActMain.LogFileChanged -= OFormActMainOnLogFileChanged;
            TranslateService.Stop();
            _workingThread.StopWorkingThread();

            Settings?.Save();

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

            int eventCode;
            if (!int.TryParse(data[2], NumberStyles.HexNumber, null, out eventCode))
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

    public interface PluginComponent
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
//            NPC = 0x3d,
    }

    public class ChattingLine
    {
        public byte RawEventCode;
        public string RawSender;
        public string RawContent;

        public DateTime Timestamp;

        public EventCode EventCode;
        public string FormattedContent;
        public string TranslatedContent;
    }

    public class ChannelSettings
    {
        public bool Show { get; set; } = true;
        public Color DisplayColor { get; set; } = Color.White;
        public bool ShowLabel { get; set; } = false;
    }
}
