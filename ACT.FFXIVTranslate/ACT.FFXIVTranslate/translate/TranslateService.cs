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
using ACT.FFXIVTranslate.localization;
using ACT.FFXIVTranslate.translate.bing;
using ACT.FFXIVTranslate.translate.yandax;

namespace ACT.FFXIVTranslate.translate
{

    class TranslateService
    {
        private MainController _controller;

        private readonly object _mainLock = new object();
        private readonly List<ChattingLine> _pendingLines = new List<ChattingLine>();
        private readonly TranslateThread _workingThread = new TranslateThread();

        private string TranslateProvider { get; set; }
        private string TranslateApiKey { get; set; }
        private string TranslateLangFrom { get; set; }
        private string TranslateLangTo { get; set; }

        public List<ITranslaterProviderFactory> AllProviders { get; } =
            new ITranslaterProviderFactory[] {new YandaxTranslateProviderFactory(), new BingTranslateProviderFactory(), }.ToList();

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _controller = plugin.Controller;
            _controller.TranslateProviderChanged += ControllerOnTranslateProviderChanged;
        }

        private void ControllerOnTranslateProviderChanged(bool fromView, string provider, string apiKey, string langFrom,
            string langTo)
        {
            if (!fromView)
            {
                return;
            }

            TranslateProvider = provider;
            TranslateApiKey = apiKey;
            TranslateLangFrom = langFrom;
            TranslateLangTo = langTo;

            var factory = AllProviders.First(it => it.ProviderName == provider);
            var lF = langFrom == LanguageDef.CodeAuto ? null : factory.SupportedSrcLanguages.First(it => it.LangCode == langFrom);
            var lT = factory.SupportedDestLanguages.First(it => it.LangCode == langTo);
            var context = new TranslateContext
            {
                Service = this,
                Provider = factory.CreateProvider(apiKey, lF, lT)
            };

            _workingThread.StartWorkingThread(context);
        }

        public void SubmitNewLine(ChattingLine line)
        {
            lock (_mainLock)
            {
                _pendingLines.Add(line);
                Monitor.PulseAll(_mainLock);
            }
        }

        public void Stop()
        {
            _workingThread.StopWorkingThread();
        }

        private class TranslateContext
        {
            public TranslateService Service;
            public ITranslateProvider Provider;
        }

        private class TranslateThread : BaseThreading<TranslateContext>
        {
            private const int BatchThreshold = 10;

            protected override void DoWork(TranslateContext context)
            {
                var service = context.Service;
                int batchWait = 0;
                var batchWorkingList = new List<ChattingLine>();

                while (!WorkingThreadStopping)
                {
                    if (batchWorkingList.Count > BatchThreshold || (batchWait > 1 && batchWorkingList.Count > 0))
                    {
                        batchWait = 0;
                        // Invoke translate service

                        try
                        {
                            context.Provider.Translate(batchWorkingList);

                            var finalResultBuilder = new StringBuilder();
                            foreach (var line in batchWorkingList)
                            {
                                finalResultBuilder.Append(
                                    $"{TextProcessor.BuildQuote(line)}{line.TranslatedContent}\n");
                            }

                            service._controller.NotifyOverlayContentUpdated(false, finalResultBuilder.ToString());
                        }
                        catch (Exception ex)
                        {
                            service._controller.NotifyLogMessageAppend(false, ex + "\n");
                        }
                        finally
                        {
                            batchWorkingList.Clear();
                        }
                    }
                    else
                    {
                        lock (service._mainLock)
                        {
                            if (service._pendingLines.Count > 0)
                            {
                                batchWait = 0;
                                batchWorkingList.AddRange(service._pendingLines);
                                service._pendingLines.Clear();
                            }
                            else
                            {
                                if (batchWorkingList.Count > 0)
                                {
                                    batchWait++;
                                }
                                Monitor.Wait(service._mainLock, 500);
                            }
                        }
                    }
                }
            }
        }
    }
}
