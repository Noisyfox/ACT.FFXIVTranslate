using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Xml;

namespace ACT.FFXIVTranslate.translate
{

    class TranslateService
    {
        private MainController _controller;

        private readonly object _mainLock = new object();
        private readonly List<ChattingLine> _pendingLines = new List<ChattingLine>();
        private readonly TranslateThread _workingThread = new TranslateThread();

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _controller = plugin.Controller;
        }

        public void SubmitNewLine(ChattingLine line)
        {
            lock (_mainLock)
            {
                _pendingLines.Add(line);
                Monitor.PulseAll(_mainLock);
            }
        }

        public void Start()
        {
            _workingThread.StartWorkingThread(this);
        }

        public void Stop()
        {
            _workingThread.StopWorkingThread();
        }

        private class TranslateThread : BaseThreading<TranslateService>
        {
            private const int BatchThreshold = 10;

            protected override void DoWork(TranslateService context)
            {
                int batchWait = 0;
                var batchWorkingList = new List<ChattingLine>();

                while (!WorkingThreadStopping)
                {
                    if (batchWorkingList.Count > BatchThreshold || (batchWait > 1 && batchWorkingList.Count > 0))
                    {
                        batchWait = 0;
                        // Invoke translate service
                        // Build text
                        var textBuilder = new StringBuilder();
                        var settings = new XmlWriterSettings {OmitXmlDeclaration = true};
                        var textWriter = XmlWriter.Create(textBuilder, settings);
                        textWriter.WriteStartElement("lines");
                        foreach (var line in batchWorkingList)
                        {
                            textWriter.WriteStartElement("line");
                            textWriter.WriteString(line.RawContent);
                            textWriter.WriteEndElement();
                        }
                        textWriter.WriteEndElement();
                        textWriter.Flush();
                        textWriter.Close();

                        var text = textBuilder.ToString();
//                        context._controller.NotifyOverlayContentUpdated(false, text);

                        var client = new HttpClient();
                        client.BaseAddress = new Uri("https://translate.yandex.net");
                        var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1.5/tr.json/translate");

                        var keyValues = new List<KeyValuePair<string, string>>();
                        keyValues.Add(new KeyValuePair<string, string>("key", "trnsl.1.1.20170716T025951Z.13c73247084b012d.3404189299f91adf7792235bc7cf7fb7f3bd26a2"));
                        keyValues.Add(new KeyValuePair<string, string>("text", text));
                        keyValues.Add(new KeyValuePair<string, string>("lang", "zh"));
                        keyValues.Add(new KeyValuePair<string, string>("options", "1"));
                        keyValues.Add(new KeyValuePair<string, string>("format", "html"));

                        request.Content = new FormUrlEncodedContent(keyValues);
                        var response = client.SendAsync(request).Result;
                        var textResponse = response.Content.ReadAsStringAsync().Result;
                        context._controller.NotifyOverlayContentUpdated(false, textResponse);
                        batchWorkingList.Clear();
                    }
                    else
                    {
                        lock (context._mainLock)
                        {
                            if (context._pendingLines.Count > 0)
                            {
                                batchWait = 0;
                                batchWorkingList.AddRange(context._pendingLines);
                                context._pendingLines.Clear();
                            }
                            else
                            {
                                if (batchWorkingList.Count > 0)
                                {
                                    batchWait++;
                                }
                                Monitor.Wait(context._mainLock, 500);
                            }
                        }
                    }
                }
            }
        }
    }
}
