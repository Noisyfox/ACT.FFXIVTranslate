using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ACT.FFXIVTranslate.translate.baidu;
using ACT.FFXIVTranslate.translate.bing;
using ACT.FFXIVTranslate.translate.google_unofficial;
using ACT.FFXIVTranslate.translate.yandax;
using ACT.FFXIVTranslate.translate.youdao;
using ACT.FoxCommon.core;
using ACT.FoxCommon.localization;

namespace ACT.FFXIVTranslate.translate
{

    class TranslateService : IPluginComponent
    {
        private FFXIVTranslatePlugin _plugin;
        private MainController _controller;

        private readonly object _mainLock = new object();
        private readonly List<ChattingLine> _pendingLines = new List<ChattingLine>();
        private readonly TranslateThread _workingThread = new TranslateThread();

        private string TranslateProvider { get; set; }
        private string TranslateApiKey { get; set; }
        private string TranslateLangFrom { get; set; }
        private string TranslateLangTo { get; set; }
        private bool AddTimestamp { get; set; }
        private bool Timestamp24Hour { get; set; }

        public List<ITranslaterProviderFactory> AllProviders { get; } =
            new ITranslaterProviderFactory[]
            {
                new YandaxTranslateProviderFactory(),
                new BaiduTranslateProviderFactory(), 
                new BingTranslateProviderFactory(),
                new GoogleTranslateProviderFactory(),
                new YoudaoTranslateProviderFactory(), 
            }.ToList();

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _plugin = plugin;
            _controller = plugin.Controller;
            _controller.TranslateProviderChanged += ControllerOnTranslateProviderChanged;
            _controller.AddTimestampChanged += ControllerOnAddTimestampChanged;
            _controller.TimestampFormatChanged += ControllerOnTimestampFormatChanged;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
        }

        private void ControllerOnTranslateProviderChanged(bool fromView, string provider, string apiKey,
            string langFrom,
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

            var factory = AllProviders.First(it => it.ProviderId == provider);
            var lF = langFrom == LanguageDef.CodeAuto
                ? null
                : factory.SupportedSrcLanguages.First(it => it.LangCode == langFrom);
            var lT = factory.SupportedDestLanguages.First(it => it.LangCode == langTo);
            var context = new TranslateContext
            {
                Service = this,
                Provider = factory.CreateProvider(apiKey, lF, lT)
            };

            _controller.NotifyLegalInfoChanged(false, factory.LegalInfo);
            _workingThread.StartWorkingThread(context);
        }

        private void ControllerOnAddTimestampChanged(bool fromView, bool show)
        {
            AddTimestamp = show;
        }

        private void ControllerOnTimestampFormatChanged(bool fromView, bool is24Hour)
        {
            Timestamp24Hour = is24Hour;
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
                var provider = context.Provider;
                int batchWait = 0;
                var batchWorkingList = new List<ChattingLine>();
                var availableLines = new List<ChattingLine>();

                while (!WorkingThreadStopping)
                {
                    if (batchWorkingList.Count > BatchThreshold || (batchWait > 1 && batchWorkingList.Count > 0))
                    {
                        batchWait = 0;
                        // Invoke translate service

                        try
                        {
                            // Make sure we do have something to translate
                            batchWorkingList.ForEach(it =>
                            {
                                if (provider.PreprocessLine(it))
                                {
                                    availableLines.Add(it);
                                }
                                else
                                {
                                    it.TranslatedContent = string.Empty;
                                }
                            });
                            if (availableLines.Count != 0)
                            {
                                provider.Translate(availableLines);
                                availableLines.Clear();
                            }

                            service._controller.NotifyBatchTranslateCompleted(false, new List<ChattingLine>(batchWorkingList));

                            // Render overlay text
                            var overlayContentRendered = TextProcessor.BuildRTF(
                                service._plugin,
                                service.AddTimestamp,
                                service.Timestamp24Hour,
                                batchWorkingList.Where(it => it.EventCode <= EventCode.Clipboard)); // Don't display test channels

                            service._controller.NotifyOverlayContentUpdated(false, overlayContentRendered);
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
