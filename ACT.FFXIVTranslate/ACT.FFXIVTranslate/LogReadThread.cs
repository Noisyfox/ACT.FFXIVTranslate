using System;
using System.IO;
using System.Linq;
using System.Threading;
using ACT.FoxCommon.core;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    class LogReadThread : BaseThreading<string>
    {
        public delegate void LogLineReadDelegate(string line);

        public event LogLineReadDelegate OnLogLineRead;

        private readonly char[] _newlineChars = { '\r', '\n' };

        protected override void DoWork(string logFilePath)
        {
            FileStream logStream = null;
            StreamReader logReader = null;
            try
            {
                lock (WorkingThreadLock)
                {
                    // Open log file
                    logStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                }
                logStream.Seek(0L, SeekOrigin.End);
                logReader = new StreamReader(logStream, ActGlobals.oFormActMain.LogEncoding);


                var lineBuffer = string.Empty;
                var lineUpdateCount = 0;

                while (!WorkingThreadStopping)
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
