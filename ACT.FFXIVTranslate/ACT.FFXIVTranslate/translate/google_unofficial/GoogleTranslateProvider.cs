using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using ACT.FFXIVTranslate.localization;
using Newtonsoft.Json.Linq;

namespace ACT.FFXIVTranslate.translate.google_unofficial
{
    internal class GoogleTranslateProvider : ITranslateProvider
    {
        private const int MaxContentLength = 2048 - 50;

        private readonly string _langFrom;
        private readonly string _langTo;

        public GoogleTranslateProvider(LanguageDef src, LanguageDef dst)
        {
            _langFrom = src?.LangCode ?? "auto";
            _langTo = dst.LangCode;
        }

        public void Translate(List<ChattingLine> chattingLines)
        {
            try
            {
                var availableLines = new List<ChattingLine>();
                chattingLines.ForEach(it =>
                {
                    it.CleanedContent = TextProcessor.NaiveCleanText(it.RawContent);
                    if (!string.IsNullOrEmpty(it.CleanedContent))
                    {
                        availableLines.Add(it);
                    }
                    else
                    {
                        it.TranslatedContent = string.Empty;
                    }
                });

                // Make sure we do have something to translate
                if (availableLines.Count == 0)
                {
                    return;
                }

                var current = 0;
                while (current < availableLines.Count)
                {
                    var start = current;

                    if (start > 0)
                    {
                        // Second call, make a short delay so google might like us :)
                        Thread.Sleep(500);
                    }

                    // Build text
                    var vaildText = string.Empty;
                    var textBuilder = new StringBuilder();
                    for (current = start; current < availableLines.Count; current++)
                    {
                        if (textBuilder.Length > 0)
                        {
                            textBuilder.Append('\n');
                        }
                        textBuilder.Append(availableLines[current].CleanedContent);
                        var newText = HttpUtility.UrlEncode(textBuilder.ToString(), Encoding.UTF8);
                        if (newText.Length > MaxContentLength)
                        {
                            break;
                        }
                        vaildText = newText;
                    }
                    if (current == start)
                    {
                        // The single line exceeds the limit, skip
                        availableLines[current].TranslatedContent = "Content too long for translating.";
                        current++;
                        continue;
                    }

                    // Send request
                    var url =
                        $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={_langFrom}&tl={_langTo}&dt=t&ie=UTF-8&oe=UTF-8&q={vaildText}";
                    string responseBody;
                    using (var client = new HttpClient())
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Get;
                        request.RequestUri = new Uri(url);
                        var response = client.SendAsync(request).Result;
                        response.EnsureSuccessStatusCode();
                        responseBody = response.Content.ReadAsStringAsync().Result;
                    }
                    // Parse result json
                    var results = (JArray) JArray.Parse(responseBody)[0];
                    var full = string.Join(string.Empty, results.Select(it => (string) it[0]));
                    var finalResults = full.Split('\n');
                    if (finalResults.Length >= current - start)
                    {
                        for (var i = start; i < current; i++)
                        {
                            availableLines[i].TranslatedContent = finalResults[i - start];
                        }
                    }
                    else
                    {
                        // TODO: Oops!
                    }
                }
            }
            catch (TranslateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TranslateException(TranslateException.ExceptionReason.UnknownError, null, ex);
            }
        }
    }
}
