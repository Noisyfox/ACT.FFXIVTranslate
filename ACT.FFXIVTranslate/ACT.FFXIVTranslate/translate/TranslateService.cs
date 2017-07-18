using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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

                        try
                        {
                            // read json
                            var ser = new DataContractJsonSerializer(typeof(YandexTranslateResult));
                            var result =
                                (YandexTranslateResult)
                                    ser.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(textResponse)));

                            if (result.Code != 200)
                            {
                                // Faild
                                context._controller.NotifyOverlayContentUpdated(false, textResponse);
                                continue;
                            }

                            var translatedLinesXml = result.Text[0];
                            // Parse xml
                            var doc = new XmlDocument();
                            doc.LoadXml(translatedLinesXml);
                            var nodes = doc.SelectNodes("lines/line");
                            if (nodes == null || nodes.Count != batchWorkingList.Count)
                            {
                                // Error
                                continue;
                            }

                            foreach (var p in batchWorkingList.Zip(nodes.Cast<XmlNode>(),
                                (a, b) => new KeyValuePair<ChattingLine, XmlNode>(a, b)))
                            {
                                p.Key.TranslatedContent = p.Value.InnerText;
                            }

                            var finalResultBuilder = new StringBuilder();
                            foreach (var line in batchWorkingList)
                            {
                                finalResultBuilder.Append($"{line.RawSender} says: {line.TranslatedContent}\n");
                            }

                            context._controller.NotifyOverlayContentUpdated(false, finalResultBuilder.ToString());
                        }
                        catch (Exception ex)
                        {
                            context._controller.NotifyOverlayContentUpdated(false, textResponse);
                            context._controller.NotifyOverlayContentUpdated(false, ex.ToString());
                        }
                        finally
                        {
                            batchWorkingList.Clear();
                        }
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

        [DataContract]
        internal class YandexTranslateResult
        {
            [DataContract]
            internal class DetectedLang
            {
                [DataMember(Name = "lang", IsRequired = true)]
                internal string Lang;
            }

            [DataMember(Name = "code", IsRequired = true)]
            internal int Code;

            [DataMember(Name = "message", IsRequired = false)]
            internal string Message;

            [DataMember(Name = "detected", IsRequired = false)]
            internal DetectedLang Detected;

            [DataMember(Name = "lang", IsRequired = false)]
            internal string Lang;

            [DataMember(Name = "text", IsRequired = false)]
            internal string[] Text;
        }
    }
}
