using System;
using System.IO;
using System.Linq;
using System.Threading;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    class LogReadThread
    {
        public delegate void LogLineReadDelegate(string line);

        public event LogLineReadDelegate OnLogLineRead;

        private readonly object _workingThreadLock = new object();
        private bool _workingThreadStopping = false;
        private Thread _workingThread;
        private string _logFilePath;
        private readonly char[] _newlineChars = { '\r', '\n' };

        public void StartWorkingThread(string logFilePath)
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

        public void StopWorkingThread()
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


                var lineBuffer = string.Empty;
                var lineUpdateCount = 0;

                while (!_workingThreadStopping)
                {

                    var data = logReader.ReadToEnd();
                    if (string.IsNullOrEmpty(data))
                    {
                        Thread.Sleep(100);
                        if (!string.IsNullOrEmpty(lineBuffer))
                        {
                            lineUpdateCount++;
                            if (lineUpdateCount > 5)
                            {
                                OnLogLineRead?.Invoke(lineBuffer);

                                lineBuffer = string.Empty;
                            }
                        }
                        continue;
                    }
                    lineUpdateCount = 0;

                    lineBuffer += data;
                    var lines = lineBuffer.Split(_newlineChars, StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length == 0)
                    {
                        continue;
                    }

                    if (lineBuffer.EndsWith("\n") || lineBuffer.EndsWith("\r"))
                    {
                        lineBuffer = string.Empty;
                    }
                    else
                    {
                        lineBuffer = lines.Last(); // In case the last line is not complete
                        lines = lines.Take(lineBuffer.Length - 1).ToArray();
                    }
                    if (lines.Length == 0)
                    {
                        continue;
                    }

                    foreach (var line in lines)
                    {
                        OnLogLineRead?.Invoke(line);
                    }
                }
            }
            finally
            {
                logReader?.Dispose();
                logStream?.Dispose();
            }
        }
    }
}
