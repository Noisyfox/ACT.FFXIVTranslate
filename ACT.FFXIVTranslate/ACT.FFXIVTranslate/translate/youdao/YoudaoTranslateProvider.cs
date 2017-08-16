using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using ACT.FFXIVTranslate.localization;
using Newtonsoft.Json.Linq;

namespace ACT.FFXIVTranslate.translate.youdao
{
    internal class YoudaoTranslateProvider : ITranslateProvider
    {
        private readonly string _appKey;
        private readonly string _secret;
        private readonly string _langFrom;
        private readonly string _langTo;


        public YoudaoTranslateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            var kp = apiKey.Split(':');
            if (kp.Length < 2)
            {
                _appKey = "";
                _secret = "";
            }
            else
            {
                _appKey = kp[0];
                _secret = kp[1];
            }
            _langFrom = src?.LangCode ?? "auto";
            _langTo = dst.LangCode;
        }

        public void Translate(List<ChattingLine> chattingLines)
        {
            try
            {
                // Build text
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

                var query = string.Join("\n", availableLines.Select(it => it.CleanedContent));

                // 1. Generate a salt
                var salt = Utils.TimestampMillisFromDateTime(DateTime.Now).ToString();
                // 2. Calculate sign
                var md5Input = Encoding.UTF8.GetBytes($"{_appKey}{query}{salt}{_secret}");
                var md5 = System.Security.Cryptography.MD5.Create();
                var hash = md5.ComputeHash(md5Input);
                var sign = string.Join("", hash.Select(it => it.ToString("x2")));

                string textResponse;
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, "/api"))
                {
                    client.BaseAddress = new Uri("https://openapi.youdao.com");

                    var keyValues = new List<KeyValuePair<string, string>>();
                    keyValues.Add(new KeyValuePair<string, string>("q", query));
                    keyValues.Add(new KeyValuePair<string, string>("from", _langFrom));
                    keyValues.Add(new KeyValuePair<string, string>("to", _langTo));
                    keyValues.Add(new KeyValuePair<string, string>("appKey", _appKey));
                    keyValues.Add(new KeyValuePair<string, string>("salt", salt));
                    keyValues.Add(new KeyValuePair<string, string>("sign", sign));

                    request.Content = new FormUrlEncodedContent(keyValues);
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    textResponse = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(textResponse);
                }

                // read json
                var result = JObject.Parse(textResponse);
                var errorCode = (string) result["errorCode"];
                switch (errorCode)
                {
                    case "0": // This one is good
                        break;
                    case "102":
                        throw new TranslateException(TranslateException.ExceptionReason.DirectionNotSupported,
                            "Language Not Supported", null);
                    case "103":
                        throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                            "Query Content Too Long", null);
                    case "101":
                    case "104":
                    case "105":
                    case "106":
                    case "107":
                    case "109":
                    case "110":
                    case "201":
                        throw new TranslateException(TranslateException.ExceptionReason.InternalError,
                            $"Error {errorCode}.", null);
                    case "108":
                        throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                            "AppId Error", null);
                    case "111":
                    case "401":
                        throw new TranslateException(TranslateException.ExceptionReason.ApiLimitExceed,
                            "Insufficient Account Balance", null);
                    case "202":
                        throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                            "Secret Error", null);
                    case "203":
                        throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                            "Limited IP",
                            null);

                    case "301":
                    case "302":
                    case "303":
                        throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                            $"Error {errorCode}.", null);
                    default:
                        throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                            (string) result["error_msg"], null);
                }

                var translation = ((string) result["translation"][0]).Split('\n');
                if (translation.Length >= availableLines.Count)
                {
                    for (var i = 0; i < availableLines.Count; i++)
                    {
                        availableLines[i].TranslatedContent = translation[i];
                    }
                }
                else
                {
                    // TODO: Oops!
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
