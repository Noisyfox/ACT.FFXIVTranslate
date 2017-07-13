using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    public class FFXIVTranslatePlugin : IActPluginV1
    {
        public PluginSettings Settings { get; private set; }
        public TabPage ParentTabPage { get; private set; }
        public Label StatusLabel { get; private set; }

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

                StartWorkingThread(ActGlobals.oFormActMain.LogFilePath);

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
            StartWorkingThread(newLogFileName);
        }

        public void DeInitPlugin()
        {
            ActGlobals.oFormActMain.LogFileChanged -= OFormActMainOnLogFileChanged;
            StopWorkingThread();

            Settings?.Save();

            StatusLabel.Text = "Exited. Bye~";
        }


        #region Working Thread

        private readonly object _workingThreadLock = new object();
        private bool _workingThreadStopping = false;
        private Thread _workingThread;
        private string _logFilePath;

        private void StartWorkingThread(string logFilePath)
        {
            Monitor.Enter(_workingThreadLock);
            try
            {
                StopWorkingThread();

                _logFilePath = logFilePath;
                _workingThread = new Thread(WorkingThreadFunc);
                _workingThread.IsBackground = true;
                _workingThread.Name = "FFXIV Translate";
                _workingThread.Start();
            }
            finally
            {
                Monitor.Exit(_workingThreadLock);
            }
        }

        private void StopWorkingThread()
        {
            Monitor.Enter(_workingThreadLock);
            try
            {
                while (_workingThread != null && _workingThread.IsAlive)
                {
                    _workingThreadStopping = true;
                    Monitor.Wait(_workingThreadLock, 100);
                }
                _workingThread = null;
                _workingThreadStopping = false;
            }
            finally
            {
                Monitor.Exit(_workingThreadLock);
            }
        }

        private void WorkingThreadFunc()
        {
            try
            {
                DoWork();
            }
            finally
            {
                Monitor.Enter(_workingThreadLock);
                try
                {
                    _workingThread = null;
                    Monitor.PulseAll(_workingThreadLock);
                }
                finally
                {
                    Monitor.Exit(_workingThreadLock);
                }
            }
        }

        private void DoWork()
        {
            FileStream logStream = null;
            StreamReader logReader = null;
            try
            {
                lock (_workingThreadLock)
                {
                    // Open log file
                    logStream = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                logStream.Seek(0L, SeekOrigin.End);
                logReader = new StreamReader(logStream, ActGlobals.oFormActMain.LogEncoding);

                while (!_workingThreadStopping)
                {

                    var data = logReader.ReadToEnd();
                    if (string.IsNullOrEmpty(data))
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    Debug.Write(data);
                }
            }
            finally
            {
                logReader?.Dispose();
                logStream?.Dispose();
            }
        }

        #endregion
    }
}
