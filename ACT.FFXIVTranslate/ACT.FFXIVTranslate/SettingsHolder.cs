using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    /// <summary>
    /// https://github.com/anoyetta/act_timeline/blob/origin/src/PluginSettings.cs
    /// </summary>
    internal class PluginSettings : SettingsSerializer
    {
        private readonly string _settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACT.FFXIVTranslate.config.xml");

        public PluginSettings(object ParentSettingsClass) : base(ParentSettingsClass)
        {
        }

        public void Load()
        {
            if (File.Exists(_settingsFile))
            {
                FileStream fs = new FileStream(_settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                XmlTextReader reader = new XmlTextReader(fs);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "SettingsSerializer")
                        ImportFromXml(reader);
                }
                reader.Close();
            }
        }

        public void Save()
        {
            FileStream stream = new FileStream(_settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 1;
            writer.IndentChar = '\t';
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Config");
            writer.WriteStartElement("SettingsSerializer");
            ExportToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
    }

    public class SettingsHolder : PluginComponent
    {
        #region Proxy methods

        private PluginSettings Settings { get; }

        public SettingsHolder()
        {
            Settings = new PluginSettings(this);
        }

        public void AddControlSetting(Control controlToSerialize)
        {
            Settings.AddControlSetting(controlToSerialize.Name, controlToSerialize);
        }

        public void Load()
        {
            Settings.Load();
        }

        public void Save()
        {
            Settings.Save();
        }

        #endregion

        #region Settings

        public string TranslateProvider { get; set; }

        public string TranslateApiKey { get; set; }

        public string TranslateLangFrom { get; set; }

        public string TranslateLangTo { get; set; }

        public string Language { get; set; }

        public string OverlayFont { get; set; }

        public string ProxyType { get; set; }

        public string ProxyServer { get; set; }

        public int ProxyPort { get; set; }

        public string ProxyUser { get; set; }

        public string ProxyPassword { get; set; }

        public string ProxyDomain { get; set; }

        public string VersionIgnored { get; set; }

        #endregion

        #region Controller notify
        
        private MainController _controller;

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            Settings.AddStringSetting(nameof(TranslateProvider));
            Settings.AddStringSetting(nameof(TranslateApiKey));
            Settings.AddStringSetting(nameof(TranslateLangFrom));
            Settings.AddStringSetting(nameof(TranslateLangTo));
            Settings.AddStringSetting(nameof(Language));
            Settings.AddStringSetting(nameof(OverlayFont));
            Settings.AddStringSetting(nameof(ProxyType));
            Settings.AddStringSetting(nameof(ProxyServer));
            Settings.AddIntSetting(nameof(ProxyPort));
            Settings.AddStringSetting(nameof(ProxyUser));
            Settings.AddStringSetting(nameof(ProxyPassword));
            Settings.AddStringSetting(nameof(ProxyDomain));
            Settings.AddStringSetting(nameof(VersionIgnored));

            _controller = plugin.Controller;

            _controller.LanguageChanged += ControllerOnLanguageChanged;
            _controller.OverlayFontChanged += ControllerOnOverlayFontChanged;
            _controller.TranslateProviderChanged += ControllerOnTranslateProviderChanged;
            _controller.ProxyChanged += ControllerOnProxyChanged;
            _controller.NewVersionIgnored += ControllerOnNewVersionIgnored;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {

        }

        public void NotifySettingsLoaded()
        {
            try
            {
                _controller.NotifyOverlayFontChanged(false,
                    (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(OverlayFont));
            }
            catch (Exception)
            {
                _controller.NotifyOverlayFontChanged(true, new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular));
            }

            _controller.NotifyTranslateProviderChanged(false, TranslateProvider, TranslateApiKey, TranslateLangFrom, TranslateLangTo);
            _controller.NotifyProxyChanged(false, ProxyType, ProxyServer, ProxyPort, ProxyUser, ProxyPassword, ProxyDomain);

            _controller.NotifySettingsLoaded();
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

        private void ControllerOnProxyChanged(bool fromView, string type, string server, int port, string user,
            string password, string domain)
        {
            if (!fromView)
            {
                return;
            }

            ProxyType = type;
            ProxyServer = server;
            ProxyPort = port;
            ProxyUser = user;
            ProxyPassword = password;
            ProxyDomain = domain;
        }

        private void ControllerOnNewVersionIgnored(bool fromView, string ignoredVersion)
        {
            VersionIgnored = ignoredVersion;
        }

        #endregion
    }
}
