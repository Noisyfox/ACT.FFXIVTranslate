using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    public class FFXIVTranslatePlugin : IActPluginV1
    {
        public PluginSettings Settings { get; private set; }
        public TabPage ParentTabPage { get; private set; }
        public Label StatusLabel { get; private set; }

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

                Settings = new PluginSettings(this);

                var mainControl = new FFXIVTranslateTabControl();
                mainControl.AttachToAct(this);

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
            ActGlobals.oFormActMain.LogFileChanged -= OFormActMainOnLogFileChanged;
            _workingThread.StopWorkingThread();

            Settings?.Save();

            StatusLabel.Text = "Exited. Bye~";
        }

        private void OnLogLineRead(string line)
        {
            Debug.WriteLine(line);
        }
    }
}
