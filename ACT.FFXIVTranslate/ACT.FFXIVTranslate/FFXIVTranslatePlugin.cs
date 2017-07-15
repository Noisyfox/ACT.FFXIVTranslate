using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
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

        private readonly LogReadThread _workingThread = new LogReadThread();

        public FFXIVTranslatePlugin()
        {
            _workingThread.OnLogLineRead += OnLogLineRead;
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            ParentTabPage = pluginScreenSpace;
            StatusLabel = pluginStatusText;
            ParentTabPage.Text = "Talk Translate";

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

                Settings.Load();
                
                ActGlobals.oFormActMain.LogFileChanged += OFormActMainOnLogFileChanged;

                _workingThread.StartWorkingThread(ActGlobals.oFormActMain.LogFilePath);

                StatusLabel.Text = "Init Success. >w<";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Init Failed: " + ex.Message;
            }
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

            var eventCode = data[2];
            var name = data[3];
            var content = data[4];

            Debug.WriteLine(line);
            Debug.WriteLine($"{name} says: {content}");
            Controller.NotifyOverlayContentUpdated(false, $"{name} says: {content}\n");
        }
    }
}
